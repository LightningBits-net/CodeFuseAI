using Microsoft.AspNetCore.Components;
using CodeFuseAI_Apps.Service;
using System.Threading.Tasks;
using CodeFuseAI_Apps.Service.IService;
using MudBlazor;

namespace CodeFuseAI_Apps
{
    public class CustomComponentBase : ComponentBase
    {
        [Inject]
        protected IClientService? _clientService { get; set; }

        [Inject]
        public NavigationManager? _navigationManager { get; set; }

        protected int ClientId { get; private set; }

        public bool PageIsLoading { get; set; } = true;

        protected override async Task OnInitializedAsync()
        {
            ClientId = await _clientService.GetClientIdAsync();
        }
    }
}
