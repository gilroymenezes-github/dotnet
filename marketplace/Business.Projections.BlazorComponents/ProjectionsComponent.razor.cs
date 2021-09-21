using Business.Abstractions;
using Business.Projections.Abstractions.Clients;
using Business.Projections.Abstractions.Extensions;
using Business.Projections.Abstractions.Models;
using Business.WebApp.Abstractions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Projections.BlazorComponents
{
    public partial class ProjectionsComponent : BaseComponent
    {
        [Inject] ProjectionsWebApiClient ProjectionsWebApiClient { get; set; }

        protected Projection SelectedProjection = new Projection();
        protected IEnumerable<Projection> Projections;
        protected DateTime? SelectedStartDate;
        protected DateTime? SelectedEndDate;
        protected string SelectedProjectionName;
        
        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender) await StartHubConnection("BroadcastMessageOnProjections");
        }

        protected async Task Load()
        {
            var tasks = new List<Task>();
            tasks.Add(GetProjectionsFromWebApi());
            await Task.WhenAll(tasks);
        }

        protected void Add()
        {
            Clear();
            Mode = CommandEnum.Add;
        }

       
        protected async Task AddProjection()
        {
            var isAuth = await HasWritePermission(SelectedProjection);
            if (!isAuth) return;
            var projection = new Projection { Name = SelectedProjectionName };
            var idUserName = await AuthenticatedUserService.GetIdentityUserName();
            if (string.IsNullOrEmpty(idUserName)) return;
            projection.ProjectionBeginDateTimeUtc = SelectedStartDate.Value.ToUniversalTime();
            projection.ProjectionEndDateTimeUtc = SelectedEndDate.Value.ToUniversalTime();
            var email = await AuthenticatedUserService.GetUserEmailFromIdentity();
            projection = projection.CreateFromProjection(email);
            await ProjectionsWebApiClient.AddItemAsync(projection);
            Clear();
            await Load();
            NotificationService.Notify(Radzen.NotificationSeverity.Success, "Projection Added");
        }

        protected async Task EditProjection()
        {
            var isAuth = await HasWritePermission(SelectedProjection);
            if (!isAuth) return;
            var projection = SelectedProjection;
            var idUserName = await AuthenticatedUserService.GetIdentityUserName();
            if (string.IsNullOrEmpty(idUserName)) return;
            if (string.Compare(idUserName, projection.AssignedTo, StringComparison.InvariantCultureIgnoreCase) != 0) return;
            projection.Name = SelectedProjectionName;
            projection.ProjectionBeginDateTimeUtc = SelectedStartDate.Value.ToUniversalTime();
            projection.ProjectionEndDateTimeUtc = SelectedEndDate.Value.ToUniversalTime();
            var email = await AuthenticatedUserService.GetUserEmailFromIdentity();
            projection = projection.UpdateFromProjection(email);
            await ProjectionsWebApiClient.EditItemAsync(projection);
            Clear();
            await Load();
            NotificationService.Notify(Radzen.NotificationSeverity.Success, "Projection Edited");
        }

        protected async Task DeleteProjection()
        {
            await ProjectionsWebApiClient.DeleteItemAsync(SelectedProjection.Id);
            Clear();
            NotificationService.Notify(Radzen.NotificationSeverity.Warning, "Projection Deleted");
            await Load();
        }

        protected override void Clear()
        {
            SelectedProjection ??= Projections.FirstOrDefault();
            SelectedProjectionName = string.Empty;
            SelectedStartDate = DateTime.UtcNow;
            SelectedEndDate = DateTime.UtcNow;
            base.Clear();
        }

        protected async Task GetProjection(string id)
        {
            SelectedProjection = await ProjectionsWebApiClient.GetFromIdAsync(id);
            SelectedProjectionName = SelectedProjection.Name;
            SelectedStartDate = SelectedProjection.ProjectionBeginDateTimeUtc;
            SelectedEndDate = SelectedProjection.ProjectionEndDateTimeUtc;
            Mode = CommandEnum.Edit;
        }

        private async Task GetProjectionsFromWebApi() => Projections = await ProjectionsWebApiClient.GetAsync();
    }
}
