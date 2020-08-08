using System.Globalization;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json;
using TeisterMask.Data.Models;
using TeisterMask.Data.Models.Enums;
using TeisterMask.DataProcessor.ImportDto;
using TeisterMask.XmlHelper;

namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

    using Data;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var sb = new StringBuilder();

            var projectsDtos = XmlConverter.Deserializer<ImportProjectDto>(xmlString, "Projects");

            var projects = new List<Project>();
            foreach (var projectDto in projectsDtos)
            {
                DateTime openDate;
                var openDateIsValid =
                    DateTime.TryParseExact(projectDto.OpenDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out openDate);

                DateTime? dueDate = null;
                if (projectDto.DueDate != null)
                {
                    DateTime tempDateTime;
                    var dueDateIsValid = DateTime.TryParseExact(projectDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tempDateTime);

                    if (dueDateIsValid)
                    {
                        dueDate = tempDateTime;
                    }
                }

                if (!IsValid(projectDto) || !openDateIsValid)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var project = new Project()
                {
                    Name = projectDto.Name,
                    OpenDate = openDate,
                    DueDate = dueDate
                };

                var tasks = new List<Task>();
                foreach (var taskDto in projectDto.TasksDtos)
                {
                    DateTime taskOpenDate;
                    var taskOpenDateIsValid = DateTime.TryParseExact(taskDto.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out taskOpenDate);

                    DateTime taskDueDate;
                    var taskDueDateIsValid = DateTime.TryParseExact(taskDto.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out taskDueDate);

                    var taskOpenDateIsAfterProjectOpenDate = taskOpenDate > project.OpenDate;
                    var taskDueDateIsBeforeProjectDueDate = taskDueDate < project.DueDate || project.DueDate == null;

                    if (!IsValid(taskDto)
                    || !taskOpenDateIsValid
                    || !taskDueDateIsValid
                    || !taskOpenDateIsAfterProjectOpenDate
                    || !taskDueDateIsBeforeProjectDueDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var task = new Task()
                    {
                        Name = taskDto.Name,
                        OpenDate = taskOpenDate,
                        DueDate = taskDueDate,
                        ExecutionType = (ExecutionType)taskDto.ExecutionType,
                        LabelType = (LabelType)taskDto.LabelType
                    };

                    project.Tasks.Add(task);
                }

                projects.Add(project);

                sb.AppendFormat(SuccessfullyImportedProject, project.Name, project.Tasks.Count);
                sb.AppendLine();
            }

            context.Projects.AddRange(projects);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var employeeDtos = JsonConvert.DeserializeObject<ImportEmployeeDto[]>(jsonString);

            var employees = new List<Employee>();
            foreach (var employeeDto in employeeDtos)
            {
                if (!IsValid(employeeDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                employeeDto.TaskIds = employeeDto.TaskIds.Distinct().ToArray();

                var employee = new Employee()
                {
                    Username = employeeDto.UserName,
                    Email = employeeDto.Email,
                    Phone = employeeDto.Phone
                };

                foreach (var taskId in employeeDto.TaskIds)
                {
                    if (!context.Tasks.Any(t => t.Id == taskId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var employeeTask = new EmployeeTask()
                    {
                        EmployeeId = employee.Id,
                        TaskId = taskId
                    };

                    employee.EmployeesTasks.Add(employeeTask);
                }

                employees.Add(employee);
                sb.AppendFormat(SuccessfullyImportedEmployee, employee.Username, employee.EmployeesTasks.Count);
                sb.AppendLine();
            }

            context.Employees.AddRange(employees);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}