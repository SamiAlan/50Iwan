using Iwan.Server.Constants;
using Iwan.Server.DataAccess;
using Iwan.Server.Extensions;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Server.Services.Accounts;
using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Sales;
using Iwan.Shared.Exceptions;
using Iwan.Shared.Extensions;
using Iwan.Shared.Options.Sales;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Sales
{
    [Injected(ServiceLifetime.Scoped, typeof(IQueryBillService))]
    public class QueryBillService : IQueryBillService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly ILoggedInUserProvider _loggedInUserProvider;

        public QueryBillService(IUnitOfWork unitOfWork,  ILoggedInUserProvider loggedInUserProvider)
        {
            _unitOfWork = unitOfWork;
            _loggedInUserProvider = loggedInUserProvider;
        }

        public async Task<BillDto> GetBillDetailsAsync(string billId, CancellationToken
            cancellationToken = default)
        {
            var bill = await _unitOfWork.BillsRepository.GetAsync(billId, true, true, cancellationToken);

            if (bill == null)
                throw new NotFoundException(Responses.Bills.BillNotFound);

            var isCultureArabic = _loggedInUserProvider.Culture.IsArabic();

            return new BillDto
            {
                Id = bill.Id,
                Number = bill.Number,
                CreatedDate = bill.CreatedDateUtc,
                UpdatedDate = bill.UpdatedDateUtc,
                CustomerName = bill.CustomerName,
                CustomerPhone = bill.CustomerPhone,
                Total = bill.Total,
                BillItems = bill.BillItems.Select(i => new BillItemDto
                {
                    Id = i.Id,
                    Price = i.Price,
                    Total = i.Quantity * i.Price,
                    Quantity = i.Quantity,
                    ProductName = isCultureArabic ? i.Product.ArabicName : i.Product.EnglishName
                }).ToList()
            };
        }

        public async Task<BillItemDto> GetBillItemDetailsAsync(string billItemId, CancellationToken cancellationToken =
            default)
        {
            var billItem = await _unitOfWork.BillItemsRepository.GetAsync(billItemId, false, true, cancellationToken);

            if (billItem == null)
                throw new NotFoundException(Responses.Bills.BillItemNotFound);

            return new BillItemDto
            {
                Id = billItem.Id,
                Price = billItem.Price,
                Quantity = billItem.Quantity,
                Total = billItem.Quantity * billItem.Price,
                ProductName = _loggedInUserProvider.IsCultureArabic ? billItem.Product.ArabicName : billItem.Product.EnglishName
            };
        }

        public async Task<PagedDto<BillDto>> GetBillsAsync(GetBillsOptions options, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.BillsRepository.Table;

            if (options.Number.HasValue)
                query = query.Where(b => b.Number == options.Number.Value);

            if (!options.CustomerName.IsNullOrEmptyOrWhiteSpaceSafe())
                query = query.Where(b => b.CustomerName.ToLower().Contains(options.CustomerName.ToLower()));

            if (!options.CustomerPhone.IsNullOrEmptyOrWhiteSpaceSafe())
                query = query.Where(b => b.CustomerPhone.ToLower().Contains(options.CustomerPhone.ToLower()));

            var billsTotal = await query.CountAsync(cancellationToken);
            var bills = await query.Skip((options.CurrentPage - 1) * options.PageSize).Take(options.PageSize)
                .ToListAsync(cancellationToken);

            return bills.AsPaged(options.CurrentPage, options.PageSize, billsTotal, b => new BillDto
            {
                Id = b.Id,
                Number = b.Number,
                Total = b.Total,
                CustomerName = b.CustomerName,
                CustomerPhone = b.CustomerPhone,
                CreatedDate = b.CreatedDateUtc,
                UpdatedDate = b.UpdatedDateUtc
            });
        }
    }
}
