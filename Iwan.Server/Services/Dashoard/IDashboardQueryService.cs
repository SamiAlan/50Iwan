using Iwan.Server.DataAccess;
using Iwan.Shared.Dtos.Dashboard;
using Iwan.Shared.Infrastructure.DI.Attributes;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Iwan.Server.Services.Accounts;
using Iwan.Shared.Constants;

namespace Iwan.Server.Services.Dashoard
{
    public interface IDashboardQueryService
    {
        Task<DashboardDto> GetDashboardDataAsync(CancellationToken cancellationToken = default);
    }

    [Injected(ServiceLifetime.Scoped, typeof(IDashboardQueryService))]
    public class DashboardService : IDashboardQueryService
    {
        protected readonly IUnitOfWork _context;
        protected readonly ILoggedInUserProvider _loggedInUser;

        public DashboardService(IUnitOfWork context, ILoggedInUserProvider loggedInUserProvider)
        {
            _context = context;
            _loggedInUser = loggedInUserProvider;
        }

        public async Task<DashboardDto> GetDashboardDataAsync(CancellationToken cancellationToken = default)
        {
            var generalReportLine = new GeneralReportLineDto
            {
                TotalMuseumValue = _loggedInUser.IsInRole(Roles.SuperAdmin) ?
                    await _context.ProductsRepository.Table.SumAsync(p => p.Price * p.StockQuantity, cancellationToken)
                    : 0,
            };

            var categoriesReportLine = new CategoriesReportLineDto
            {
                Count = await _context.CategoriesRepository.Table.CountAsync(cancellationToken),
                ParentCategoriesCount = await _context.CategoriesRepository.CountAsync(c => !c.IsSubCategory),
                SubCategoriesCount = await _context.CategoriesRepository.CountAsync(c => c.IsSubCategory),
            };

            var productsReportLine = new ProductsReportLineDto
            {
                Count = await _context.ProductsRepository.Table.CountAsync(cancellationToken),
                NotVisibleCount = await _context.ProductsRepository.CountAsync(p => p!.IsVisible, cancellationToken)
            };
            
            var vendors = await _context.VendorsRepository.Table.Where(v => v.Products.Any()).OrderByDescending(v => v.Products.Count).Take(3).ToListAsync(cancellationToken);

            var vendorsReportLine = new VendorsReportLineDto
            {
                Count = await _context.VendorsRepository.Table.CountAsync(cancellationToken),
                TopVendors = vendors
                .Select(v => new ReportLineVendorDto
                {
                    Name = v.Name,
                    BenefitPercent = v.BenefitPercent,
                    RelatedProductsCount = v.Products.Count
                }).ToList()
            };

            return new DashboardDto
            {
                GeneralReportLine = generalReportLine,
                CategoriesReportLine = categoriesReportLine,
                ProductsReportLine = productsReportLine,
                VendorsReportLine = vendorsReportLine
            };
        }
    }
}
