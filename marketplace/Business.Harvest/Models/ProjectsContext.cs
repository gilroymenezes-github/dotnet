
using Business.Abstractions;
using Business.Db;
using Business.Models.Projects;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Harvest.Projects
{
    public class ProjectsContext
    {
        private IRepository<object, ItemResponse<object>> repository;
        private HarvestClient connector;

        public ProjectsContext(HarvestClient connector, IRepository<object, ItemResponse<object>> repository)
        {
            this.connector = connector;
            this.repository = repository;
        }

        public async Task SynchronizeProjects(bool isOkToSync = default)
        {
            if (!isOkToSync) return;
            var projects = await connector.GetProjects();
            if (projects is null) return;
            var projectEntities = new List<object>();
            foreach(var project in projects)
            {
                var p = project as IDictionary<string, object>;
                var projectEntity = new 
                {
                    Id = Guid.NewGuid().ToString(),
                    DateTimeOnCreateUtc = DateTime.UtcNow,
                    DateTimeOnUpdateUtc = DateTime.UtcNow,
                    Code = p["code"]?.ToString(),
                    ProjectClient = GetClientFromObject(p["client"]),
                    IsActive = bool.Parse(p["is_active"]?.ToString() ?? string.Empty),
                    IsBillable = bool.Parse(p["is_billable"]?.ToString() ?? string.Empty),
                    HarvestProjectId = p["id"]?.ToString(),
                    //StartsOn = string.IsNullOrEmpty(p["starts_on"]?.ToString()) ? DateTime.MinValue : DateTime.Parse(p["starts_on"]?.ToString()),
                    //EndsOn = string.IsNullOrEmpty(p["starts_on"]?.ToString()) ? DateTime.MinValue : DateTime.Parse(p["ends_on"]?.ToString())
                };
                projectEntities.Add(projectEntity);
            }

            //projectEntities.ForEach(async p => await repository.CreateItemAsync(p.Id, p));
        }

        private ProjectClient GetClientFromObject(object o)
        {
            var values = o as IDictionary<string, object>;
            return new ProjectClient
            {
                ProjectClientId = values["id"]?.ToString(),
                Currency = values["currency"]?.ToString(),
                Name = values["name"]?.ToString()
            };
        }
    }
}
