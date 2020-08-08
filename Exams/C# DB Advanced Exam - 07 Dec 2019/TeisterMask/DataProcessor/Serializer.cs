using System.Globalization;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;
using TeisterMask.DataProcessor.ExportDto;
using TeisterMask.XmlHelper;
using Formatting = Newtonsoft.Json.Formatting;

namespace TeisterMask.DataProcessor
{
    using System;
    using Data;
    using Formatting = Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var projectsWithTasks = context.Projects
                .ToArray()
                .Where(p => p.Tasks.Count > 0)
                .Select(p => new ExportProjectDto()
                {
                    TasksCount = p.Tasks.Count,
                    ProjectName = p.Name,
                    HasEndDate = p.DueDate.HasValue
                    ? "Yes"
                    : "No",
                    TaskDtos = p.Tasks
                        .Select(t => new ExportTaskDto()
                        {
                            Name = t.Name,
                            LabelType = t.LabelType.ToString()
                        })
                        .OrderBy(t => t.Name)
                        .ToArray()
                })
                .OrderByDescending(p => p.TasksCount)
                .ThenBy(p => p.ProjectName)
                .ToArray();

            var xmlString = XmlConverter.Serialize(projectsWithTasks, "Projects");

            return xmlString;
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var mostBusiestEmployees = context.Employees
                .ToArray()
                .Where(e => e.EmployeesTasks.Any(et => et.Task.OpenDate >= date))
                .Select(e => new
                {
                    Username = e.Username,
                    Tasks = e.EmployeesTasks
                        .Where(et => et.Task.OpenDate >= date)
                        .OrderByDescending(et => et.Task.DueDate)
                        .ThenBy(et => et.Task.Name)
                        .Select(et => new
                        {
                            TaskName = et.Task.Name,
                            OpenDate = et.Task.OpenDate.ToString("d", CultureInfo.InvariantCulture),
                            DueDate = et.Task.DueDate.ToString("d", CultureInfo.InvariantCulture),
                            LabelType = et.Task.LabelType.ToString(),
                            ExecutionType = et.Task.ExecutionType.ToString()
                        })
                        .ToArray()
                })
                .ToArray()
                .OrderByDescending(e => e.Tasks.Length)
                .ThenBy(e => e.Username)
                .Take(10)
                .ToArray();

            var jsonString = JsonConvert.SerializeObject(mostBusiestEmployees, Formatting.Indented);

            return jsonString;
        }
    }
}

/*Export all projects that have at least one task. For each project, export its name, tasks count, and if it has end (due) date which is represented like "Yes" and "No". For each task, export its name and label type. Order the tasks by name (ascending). Order the projects by tasks count (descending), then by name (ascending). 
*/
