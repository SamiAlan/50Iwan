using Iwan.Client.Blazor.Services.Pages;
using Iwan.Shared.Dtos.Pages;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Iwan.Client.Blazor.Pages.Pages
{
    public partial class HomePage
    {
        protected HomePageContentDto _homePageContent = new();

        [Inject] IPagesService PagesService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _homePageContent = await PagesService.GetHomePageContentAsync();
            await base.OnInitializedAsync();
        }
    }
}