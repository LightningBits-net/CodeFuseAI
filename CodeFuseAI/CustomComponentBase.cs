using Microsoft.AspNetCore.Components;
using CodeFuseAI.Service;
using System.Threading.Tasks;
using CodeFuseAI.Service.IService;
using MudBlazor;

namespace CodeFuseAI
{
    public class CustomComponentBase : ComponentBase
    {
        [Inject]
        protected IClientService ClientService { get; set; }

        protected int ClientId { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            ClientId = await ClientService.GetClientIdAsync();
        }
    }
}
