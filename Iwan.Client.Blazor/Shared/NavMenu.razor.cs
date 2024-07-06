using Iwan.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Iwan.Shared.Extensions;
using Iwan.Shared.Dtos.Accounts;
using Iwan.Client.Blazor.Services.Accounts;
using System.Threading.Tasks;
using AKSoftware.Blazor.Utilities;
using Iwan.Client.Blazor.Constants;
using Iwan.Client.Blazor.Components.Accounts;

namespace Iwan.Client.Blazor.Shared
{
    public partial class NavMenu : ComponentBase
    {
        [Inject] protected IStringLocalizer<Localization> Localizer { get; set; }

        private UserDto _currentUser = new();
        [Inject] IAccountService AccoutService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try { _currentUser = await AccoutService.GetCurrentUserAsync(); } catch { }
            MessagingCenter.Subscribe<Profile, UserDto>(this, Messages.ProfileUpdated, (_, user) =>
            {
                _currentUser = user;
                StateHasChanged();
            });

            await base.OnInitializedAsync();
        }

        protected string Localize(string key, params object[] values) => Localizer.Localize(key, values);
    }
}
