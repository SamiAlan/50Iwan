using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Domain.Sales;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Server.Services.Accounts;
using Iwan.Shared.Dtos.Sales;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Sales
{
    [Injected(ServiceLifetime.Scoped, typeof(IBillService))]
    public class BillService : IBillService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly ILoggedInUserProvider _loggedInUserProvider;

        public BillService(IUnitOfWork unitOfWork, ILoggedInUserProvider loggedInUserProvider)
        {
            _unitOfWork = unitOfWork;
            _loggedInUserProvider = loggedInUserProvider;
        }

        public async Task<Bill> AddBillAsync(AddBillDto billToAdd, CancellationToken cancellationToken = default)
        {
            // Verify that there are any bill items
            if (!billToAdd.BillItems?.Any() ?? true)
                throw new BadRequestException(Responses.Bills.CantAddEmptyBill);

            // Verify that all products in the bill exist
            var productsIds = billToAdd.BillItems.Select(i => i.ProductId).ToList();

            var productsExist = await _unitOfWork.ProductsRepository.ExistAsync(productsIds, cancellationToken);

            if (!productsExist)
                throw new NotFoundException(Responses.Bills.NotAllProductsExist);

            var productsById = await _unitOfWork.ProductsRepository.GetGroupedByIdAsync(productsIds, cancellationToken);

            var bill = new Bill
            {
                CustomerName = billToAdd.CustomerName,
                CustomerPhone = billToAdd.CustomerPhone
            };

            await _unitOfWork.BillsRepository.AddAsync(bill, cancellationToken);

            var total = 0m;

            var billItemsToAdd = new List<BillItem>();

            foreach (var billItem in billToAdd.BillItems)
            {
                var product = productsById[billItem.ProductId];

                var productName = _loggedInUserProvider.IsCultureArabic ? product.ArabicName : product.EnglishName;

                if (product.StockQuantity < billItem.Quantity)
                    throw new BadRequestException(Responses.Bills.ProductNotEnoughStock, productName);

                billItemsToAdd.Add(new BillItem
                {
                    BillId = bill.Id,
                    Price = billItem.Price,
                    Quantity = billItem.Quantity,
                    ProductId = product.Id
                });

                product.StockQuantity -= billItem.Quantity;

                total += billItem.Quantity * billItem.Price;
            }

            bill.Total = total;

            await _unitOfWork.BillItemsRepository.AddRangeAsync(billItemsToAdd, cancellationToken);

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return bill;
        }

        public async Task<BillItem> AddBillItemAsync(string billId, AddBillItemDto itemToAdd, CancellationToken 
            cancellationToken = default)
        {
            var bill = await _unitOfWork.BillsRepository.GetAsync(billId, true, false, cancellationToken);

            if (bill == null)
                throw new NotFoundException(Responses.Bills.BillNotFound);

            var product = await _unitOfWork.ProductsRepository.FindAsync(itemToAdd.ProductId, cancellationToken);

            if (product == null)
                throw new NotFoundException(Responses.Products.ProductNotFound);

            var productName = _loggedInUserProvider.IsCultureArabic ? product.ArabicName : product.EnglishName;

            if (product.StockQuantity < itemToAdd.Quantity)
                throw new BadRequestException(Responses.Bills.ProductNotEnoughStock, productName);

            var itemExistingInBill = bill.BillItems.FirstOrDefault(i => i.ProductId == product.Id);

            product.StockQuantity -= itemToAdd.Quantity;

            // It is a new bill item
            if (itemExistingInBill == null)
            {
                itemExistingInBill = new BillItem
                {
                    BillId = bill.Id,
                    Price = itemToAdd.Price,
                    Quantity = itemToAdd.Quantity,
                    ProductId = itemToAdd.ProductId
                };

                await _unitOfWork.BillItemsRepository.AddAsync(itemExistingInBill, cancellationToken);
            }
            else
            {
                itemExistingInBill.Quantity += itemToAdd.Quantity;
            }

            bill.Total += itemToAdd.Quantity * itemExistingInBill.Price;

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return itemExistingInBill;
        }

        public async Task<Bill> EditBillAsync(EditBillDto editedBill, CancellationToken cancellationToken = default)
        {
            var bill = await _unitOfWork.BillsRepository.GetAsync(editedBill.Id, false, false, cancellationToken);

            if (bill == null)
                throw new NotFoundException(Responses.Bills.BillNotFound);

            bill.SetIfNotEqual(b => b.CustomerName, editedBill.CustomerName);
            bill.SetIfNotEqual(b => b.CustomerPhone, editedBill.CustomerPhone);
            bill.SetIfNotEqual(b => b.Total, editedBill.Total);

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return bill;
        }

        public async Task<BillItem> EditBillItemAsync(EditBillItemDto editedBillItem, CancellationToken cancellationToken = 
            default)
        {
            var billItem = await _unitOfWork.BillItemsRepository.GetAsync(editedBillItem.Id, true, true, cancellationToken);

            if (billItem == null)
                throw new NotFoundException(Responses.Bills.BillItemNotFound);

            var product = billItem.Product;
            var bill = billItem.Bill;

            var productName = _loggedInUserProvider.IsCultureArabic ? product.ArabicName : product.EnglishName;

            if (product.StockQuantity + billItem.Quantity < editedBillItem.NewQuantity)
                throw new BadRequestException(Responses.Bills.ProductNotEnoughStock, productName);

            var oldQuantity = billItem.Quantity;
            var oldPrice = billItem.Price;

            var newQuantity = editedBillItem.NewQuantity;
            var newPrice = editedBillItem.NewPrice;

            // Was 4 and became 3 -> 1, was 3 and became 4 -> -1, was 3 and became 3 -> 0
            var quantityDifference = oldQuantity - newQuantity;
            var totalDifference = (oldQuantity * oldPrice) - (newQuantity * newPrice);

            billItem.Quantity = newQuantity;
            billItem.Price = newPrice;

            // This is affected by changing either the quantity or the price of the billitem
            if (totalDifference != 0)
                bill.Total += totalDifference;

            // if there was actually a difference in the quantity
            if (quantityDifference != 0)
                product.StockQuantity += quantityDifference;

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);

            return billItem;
        }

        public async Task RemoveBillItemAsync(string billItemId, CancellationToken cancellationToken = default)
        {
            var billItem = await _unitOfWork.BillItemsRepository.GetAsync(billItemId, true, true, cancellationToken);

            if (billItem == null)
                throw new NotFoundException(Responses.Bills.BillItemNotFound);

            billItem.Product.StockQuantity += billItem.Quantity;
            billItem.Bill.Total -= billItem.Quantity * billItem.Price;

            _unitOfWork.BillItemsRepository.Delete(billItem);
            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
        }

        public async Task DeleteBillAsync(string billId, CancellationToken cancellationToken = default)
        {
            var bill = await _unitOfWork.BillsRepository.GetAsync(billId, true, true, cancellationToken);

            if (bill == null)
                throw new NotFoundException(Responses.Bills.BillNotFound);

            foreach (var billItem in bill.BillItems)
                billItem.Product.StockQuantity += billItem.Quantity;

            _unitOfWork.BillItemsRepository.DeleteRange(bill.BillItems);
            _unitOfWork.BillsRepository.Delete(bill);

            await _unitOfWork.SaveChangesAsync(_loggedInUserProvider.UserId, cancellationToken);
        }
    }
}
