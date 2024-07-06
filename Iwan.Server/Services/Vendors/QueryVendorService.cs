using Iwan.Server.DataAccess;
using Iwan.Server.Extensions;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Iwan.Shared.Dtos;
using Iwan.Shared.Dtos.Common;
using Iwan.Shared.Dtos.Vendors;
using Iwan.Shared.Extensions;
using Iwan.Shared.Options.Vendors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Iwan.Server.Services.Vendors
{
    [Injected(ServiceLifetime.Scoped, typeof(IQueryVendorService))]
    public class QueryVendorService : IQueryVendorService
    {
        protected readonly IUnitOfWork _unitOfWork;

        public QueryVendorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<VendorDto>> GetAllVendorsAsync(bool includeAddresses = false, CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.VendorsRepository.Table;

            if (includeAddresses)
                query = query.Include(v => v.Address);

            var vendors = await query.ToListAsync(cancellationToken);

            if (includeAddresses)
            {
                return vendors.Select(v => new VendorDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    ShowOwnProducts = v.ShowOwnProducts,
                    BenefitPercent = v.BenefitPercent
                });
            }

            return vendors.Select(v => new VendorDto
            {
                Id = v.Id,
                Name = v.Name,
                ShowOwnProducts = v.ShowOwnProducts,
                BenefitPercent = v.BenefitPercent,
                Address = new AddressDto
                {
                    Id = v.AddressId,
                    Country = v.Address.Country,
                    City = v.Address.City,
                    Company = v.Address.Company,
                    Email = v.Address.Company,
                    PhoneNumber = v.Address.PhoneNumber,
                    Address1 = v.Address.Address1,
                    Address2 = v.Address.Address2
                }
            });
        }

        public async Task<VendorDto> GetVendorDetailsAsync(string vendorId, bool includeAddress = true, CancellationToken cancellationToken = default)
        {
            var vendor = await _unitOfWork.VendorsRepository.GetAsync(vendorId, includeAddress, cancellationToken);

            var vendorDto = new VendorDto
            {
                Id = vendor.Id,
                BenefitPercent = vendor.BenefitPercent,
                Name = vendor.Name,
                ShowOwnProducts = vendor.ShowOwnProducts,
            };

            if (includeAddress)
            {
                var address = vendor.Address;

                vendorDto.Address = new AddressDto
                {
                    Id = address.Id,
                    City = address.City,
                    Company = address.Company,
                    Country = address.Country,
                    Address1 = address.Address1,
                    Address2 = address.Address2,
                    Email = address.Email,
                    PhoneNumber = address.PhoneNumber
                };
            }

            return vendorDto;
        }

        public async Task<PagedDto<VendorDto>> GetVendorsAsync(GetVendorsOptions options, CancellationToken cancellationToken = default)
        {
            var query = _unitOfWork.VendorsRepository.Table;

            if (!options.Name.IsNullOrEmptyOrWhiteSpaceSafe())
                query = query.Where(v => v.Name.ToLower().Contains(options.Name.ToLower()));

            if (options.OnlyVendorsShowingTheirProducts.HasValue && options.OnlyVendorsShowingTheirProducts.Value)
                query = query.Where(v => v.ShowOwnProducts == true);

            if (options.MinBenefitPercentage.HasValue)
                query = query.Where(v => v.BenefitPercent >= options.MinBenefitPercentage.Value);

            if (options.MaxBenefitPercentage.HasValue)
                query = query.Where(v => v.BenefitPercent <= options.MaxBenefitPercentage.Value);

            var totalCount = await query.CountAsync(cancellationToken);
            var vendors = await query.Skip(options.CurrentPage * options.PageSize).Take(options.PageSize)
                .ToListAsync(cancellationToken);

            return vendors.AsPaged(options.CurrentPage, options.PageSize, totalCount, v => new VendorDto
            {
                Id = v.Id,
                Name = v.Name,
                BenefitPercent = v.BenefitPercent,
                ShowOwnProducts = v.ShowOwnProducts
            });
        }
    }
}
