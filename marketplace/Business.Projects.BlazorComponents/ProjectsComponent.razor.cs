using Business.Abstractions;
using Business.Abstractions.Auth;
using Business.Customers.Abstractions.Clients;
using Business.Customers.Abstractions.Extensions;
using Business.Customers.Abstractions.Models;
using Business.ExchangeRates.Abstractions.Clients;
using Business.ExchangeRates.Abstractions.Extensions;
using Business.ExchangeRates.Abstractions.Models;
using Business.Projects.Abstractions;
using Business.Projects.Abstractions.Extensions;
using Business.Projects.Abstractions.Models;
using Business.WebApp.Abstractions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Projects.BlazorComponents
{
    public partial class ProjectsComponent : BaseComponent
    {
        [Inject] ProjectsWebApiClient ProjectsWebApiClient { get; set; }
        [Inject] CustomersWebApiClient CustomersWebApiClient { get; set; }
        [Inject] CurrenciesWebApiClient CurrenciesWebApiClient { get; set; }
        [Inject] ProjectTypesWebApiClient ValuesWebApiClient { get; set; }

        public string Filter { get; set; }

        private IEnumerable<Project> projects;
        private IEnumerable<Customer> customers;
        private IEnumerable<Currency> currencies;
        private IEnumerable<ProjectType> projectTypes;

        protected List<ProjectViewModel> ProjectViewModelItems;
        protected Project SelectedProject;
        protected ProjectType SelectedProjectType;
        protected string HoursValueAsString;
        protected string HourlyRateValueAsString;
        protected string CostValueAsString;
        protected string SelectedProjectTypeNameAsString;

        dynamic _customer; // for radzen binding
        dynamic currentCustomer { get { return _customer; } set { if (_customer != value) { _customer = value; StateHasChanged(); } } }
        dynamic previousCustomer { get; set; }
        
        protected override async Task OnInitializedAsync()
        {
            await Load();
            SelectedProjectType ??= projectTypes.FirstOrDefault();
            SelectedProjectTypeNameAsString = SelectedProjectType.Name;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender) await StartHubConnection("BroadcastMessageOnProjects");
            else await LoadProjects();
        }

        protected async Task Load()
        {
            var tasks = new List<Task>();
            tasks.Add(GetCustomersFromWebApi());
            tasks.Add(GetCurrenciesFromWebApi());
            tasks.Add(GetValuesForProjectTypes());
            await Task.WhenAll(tasks);
            currentCustomer = customers.FirstOrDefault();
        }

        private async Task LoadProjects()
        {
            await GetProjectsFromWebApi(currentCustomer.Id);
            if (projects is null) return;
            ProjectViewModelItems = projects.Select(p => new ProjectViewModel { Project = p }).ToList();
            ProjectViewModelItems.ForEach(p =>
            {
                p.Customer = customers.FirstOrDefault(c => c.Id == p.Project?.CustomerId) ?? new Customer().EmptyCustomer();
                p.Currency = currencies.FirstOrDefault(c => c.Id == p?.Customer?.CurrencyId) ?? new Currency().EmptyCurrency();
            });
            if (currentCustomer.Id != previousCustomer?.Id)
            {
                StateHasChanged();
            }
            previousCustomer = currentCustomer;
        }

        protected bool IsVisible(ProjectViewModel project)
        {
            if (string.IsNullOrEmpty(Filter)) return true;
            if (project.Project.Name.Contains(Filter, StringComparison.OrdinalIgnoreCase)) return true;
            if (project.Customer.Name.Contains(Filter, StringComparison.OrdinalIgnoreCase)) return true;
            if (project.Project.ProjectId.StartsWith(Filter)) return true;
            return false;
        }

        protected async Task GetProject(string id, string customerId)
        {
            SelectedProject = await ProjectsWebApiClient.GetProjectFromIdAndCustomerIdAsync(id, customerId);  //ProjectsWebApiClient.GetFromIdAsync(id);
            HoursValueAsString = SelectedProject?.Hours.ToString();
            HourlyRateValueAsString = SelectedProject?.HourlyRate.ToString();
            CostValueAsString = SelectedProject?.Cost.ToString();
            Mode = CommandEnum.Edit;
        }

        //protected string GetProjectType(string name) => 
        
        protected override void Clear()
        {
            HoursValueAsString = string.Empty;
            HourlyRateValueAsString = string.Empty;
            CostValueAsString = string.Empty;
            SelectedProjectType = projectTypes.FirstOrDefault();
            SelectedProjectTypeNameAsString = SelectedProjectType.Name;
            SelectedProject = default(Project);
            currentCustomer = customers.FirstOrDefault();
            previousCustomer = null;
            base.Clear();
        }

        protected async Task EditProject()
        {
            var project = SelectedProject;
            project.Hours = string.IsNullOrEmpty(HoursValueAsString) ? project.Hours : double.Parse(HoursValueAsString);
            project.HourlyRate = string.IsNullOrEmpty(HourlyRateValueAsString) ? project.HourlyRate : decimal.Parse(HourlyRateValueAsString);
            project.Cost = string.IsNullOrEmpty(CostValueAsString) ? project.Cost : decimal.Parse(CostValueAsString);
            project.ProjectTypeName = SelectedProjectType.Name;
            project = project.UpdateFromProject();
            await ProjectsWebApiClient.EditItemAsync(project);
            Clear();
            await Load();
            NotificationService.Notify(Radzen.NotificationSeverity.Success, "Project Edited");
        }

        protected void OnChange(object value)
        {
            SelectedProjectType = projectTypes.FirstOrDefault(pt => pt.Name == value.ToString());
            StateHasChanged();
        }


        private async Task GetProjectsFromWebApi(string customerId) => projects = await ProjectsWebApiClient.GetProjectsFromCustomerIdAsync(customerId);

        private async Task GetCustomersFromWebApi() => customers = await CustomersWebApiClient.GetAsync();

        private async Task GetCurrenciesFromWebApi() => currencies = await CurrenciesWebApiClient.GetAsync();

        private async Task GetValuesForProjectTypes() => projectTypes = await ValuesWebApiClient.GetProjectTypesAsync();
    }
}
