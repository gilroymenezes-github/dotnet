using Business.Abstractions;
using Business.Competencies.Abstractions.Clients;
using Business.Competencies.Abstractions.Extensions;
using Business.Competencies.Abstractions.Models;
using Business.WebApp.Abstractions;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Competencies.BlazorComponents
{
    public partial class CompetenciesComponent : BaseComponent
    {
        [Inject] CompetenciesWebApiClient CompetenciesApiClient { get; set; }

        protected IEnumerable<Competency> competencies;
        protected Competency currentCompetency;
        protected string name;
        protected string desc;
       
        protected override async Task OnInitializedAsync()
        {
            await Load();
            await StartHubConnection("BroadcastMessageOnCompetencies");
        }

        protected async Task Load()
        {
            competencies = await CompetenciesApiClient.GetAsync();
        }

        protected void Add()
        {
            Clear();
            Mode = CommandEnum.Add;
        }

        protected async Task AddCompetency()
        {
            var competency = new Competency { Name = name, Description = desc };
            var email = await AuthenticatedUserService.GetUserEmailFromIdentity();
            competency = competency.CreateFromCompetency(email);
            await CompetenciesApiClient.AddItemAsync(competency);
            Clear();
            await Load();
            NotificationService.Notify(Radzen.NotificationSeverity.Success, "Competency Added");
        }

        protected async Task EditCompetency()
        {
            var email = await AuthenticatedUserService.GetUserEmailFromIdentity();
            var competency = currentCompetency.UpdateFromCompetency(email);
            await CompetenciesApiClient.EditItemAsync(competency);
            Clear();
            await Load();
            NotificationService.Notify(Radzen.NotificationSeverity.Success, "Competency Edited");
        }

        protected async Task DeleteCompetency()
        {

            await CompetenciesApiClient.DeleteItemAsync(currentCompetency.Id);
            Clear();
            NotificationService.Notify(Radzen.NotificationSeverity.Warning, "Competency Deleted");
            await Load();
        }

        protected async Task GetCompetency(string id)
        {
            currentCompetency = await CompetenciesApiClient.GetFromIdAsync(id);
            Mode = CommandEnum.Edit;
        }

        protected override void Clear()
        {
            name = string.Empty;
            desc = string.Empty;
            currentCompetency = default(Competency);
            base.Clear();
        }
    }
}
