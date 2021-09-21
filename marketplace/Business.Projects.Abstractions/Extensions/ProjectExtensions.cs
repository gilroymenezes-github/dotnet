using Business.Projects.Abstractions.Models;
using System;

namespace Business.Projects.Abstractions.Extensions
{
    public static class ProjectExtensions
    {
        public static Project UpdateFromProject(this Project project)
        {
            project.UpdatedAtDateTimeUtc = DateTime.UtcNow;
            return project;
        }

    }
}
