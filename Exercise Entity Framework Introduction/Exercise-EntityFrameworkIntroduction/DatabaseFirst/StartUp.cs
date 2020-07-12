using System;

namespace SoftUni
{
    using System.Linq;
    using System.Text;
    using System.Globalization;
    using System.Collections.Generic;

    using Data;
    using Models;

    public class StartUp
    {

        public static void Main()
        {
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            var fullInfo = new StringBuilder();
        
            var employees = context.Employees
                .Select(e => new
                {
                    e.EmployeeId,
                    e.FirstName,
                    e.MiddleName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.EmployeeId);

            foreach (var employee in employees)
            {
                fullInfo.AppendLine($"{employee.FirstName} {employee.LastName + " " ?? ""}{employee.MiddleName} {employee.JobTitle} {employee.Salary:F2}");   
            }

            return fullInfo.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var fullInfo = new StringBuilder();

            var employeesWithSalariesOver50000 = context.Employees
                .Where(e => e.Salary > 50000)
                .Select(e => new
                {
                    e.FirstName,
                    e.Salary
                })
                .OrderBy(e => e.FirstName);

            foreach (var employee in employeesWithSalariesOver50000)
            {
                fullInfo.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
            }

            return fullInfo.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var fullInfo = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.Department.Name == "Research and Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary
                })
                .OrderBy(e => e.Salary)
                .ThenByDescending(e => e.FirstName);

            foreach (var employee in employees)
            {
                fullInfo.AppendLine($"{employee.FirstName} {employee.LastName} from {employee.DepartmentName} - ${employee.Salary:F2}");
            }

            return fullInfo.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            var sb =  new StringBuilder();

            var newStreetName = "Vitoshka 15";
            var townId = 4;

            var employee = context.Employees
                .First(e => e.LastName == "Nakov");

            employee.Address = new Address() { AddressText = newStreetName, TownId = townId };

            context.SaveChanges();

            var employees = context.Employees
                .OrderByDescending(e => e.AddressId)
                .Take(10)
                .Select( e => e.Address.AddressText)
                .ToList();

            foreach (var address in employees)
            {
                sb.AppendLine(address);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.EmployeesProjects
                            .Any(ep => ep.Project.StartDate.Year >= 2001
                                                       && ep.Project.StartDate.Year <= 2003))
                .Take(10)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    ManagerFirstName = e.Manager.FirstName,
                    ManagerLastName = e.Manager.LastName,
                    Projects = e.EmployeesProjects
                        .Select(p => new
                        {
                            PorjectName = p.Project.Name,
                            StartDate = p.Project.StartDate,
                            EndDate = p.Project.EndDate
                        })
                });

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

                foreach (var project in employee.Projects)
                {
                    var projectName = project.PorjectName;
                    var startDateString = project.StartDate.ToString("M/d/yyyy h:mm:ss tt",CultureInfo.InvariantCulture);
                    var endDate = project.EndDate.HasValue
                            ? project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt",CultureInfo.InvariantCulture)
                            : "not finished";

                    sb.AppendLine($"--{projectName} - {startDateString} - {endDate}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var addresses = context.Addresses
                .Select(a => new
                {
                    a.AddressText,
                    TownName = a.Town.Name,
                    EmployeesCount = a.Employees.Count
                })
                .OrderByDescending(a => a.EmployeesCount)
                .ThenBy(a => a.TownName)
                .ThenBy(a => a.AddressText)
                .Take(10);

            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.EmployeesCount} employees");
            }

            return sb.ToString().TrimEnd();
        }   

        public static string GetEmployee147(SoftUniContext context)
        {
            var employee = context.Employees
                .Where(e => e.EmployeeId == 147)
                .Select(e => new
                {
                    e.EmployeeId,
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    Projects = e.EmployeesProjects.Select(p => p.Project.Name)
                })
                .Single();

            var sb = new StringBuilder();

             sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");

            foreach (var project in employee.Projects.OrderBy(pName => pName))
            {
                sb.AppendLine(project);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var departmentsInfo = context.Departments
                .Where(x => x.Employees.Count > 5)
                .OrderBy(d => d.Employees.Count)
                .ThenBy(d => d.Name)
                .Select(d => new
                {
                    d.Name,
                    ManagerName = d.Manager.FirstName + " " + d.Manager.LastName,
                    EmployeesInfo = d.Employees
                        .Select(e => new
                        {
                            e.FirstName,
                            e.LastName,
                            e.JobTitle
                        })
                        .OrderBy(e => e.FirstName)
                        .ThenBy(e => e.LastName)
                });

            foreach (var department in departmentsInfo)
            {
                sb.AppendLine($"{department.Name} - {department.ManagerName}");

                foreach (var employee in department.EmployeesInfo)
                {
                    sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var projects = context.Projects
                .OrderByDescending(p => p.StartDate)
                .Select(p => new
                {
                    p.Name,
                    p.Description,
                    p.StartDate
                })
                .Take(10)
                .OrderBy(p => p.Name);

            foreach (var project in projects)
            {
                sb.AppendLine($"{project.Name}");
                sb.AppendLine($"{project.Description}");
                sb.AppendLine($"{project.StartDate:M/d/yyyy h:mm:ss tt}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var departmentsToIncreaseSalaries = new List<string>()
            {
                "Engineering",
                "Tool Design",
                "Marketing",
                "Information Services"
            };

            var employeesInDepartments = context.Employees
                .Where(e => departmentsToIncreaseSalaries.Contains(e.Department.Name))
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName);

            foreach (var employee in employeesInDepartments)
            {
                employee.Salary *= 1.12M;

                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:F2})");
            }

            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var employees = context.Employees
                .Where(e => e.FirstName.StartsWith("Sa"))
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.JobTitle,
                    e.Salary
                })
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName);

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:F2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var projectToDeleteId = 3;

            var removeEmployeesFromProject = context.EmployeesProjects
                .Where(ep => ep.ProjectId == projectToDeleteId);

            foreach (var employeeInProject in removeEmployeesFromProject)
            {
                context.EmployeesProjects.Remove(employeeInProject);
            }

            var project= context.Projects.Find(projectToDeleteId);
            context.Projects.Remove(project);

            context.SaveChanges();

            var projectsToPrint = context.Projects
                .Select(p => p.Name)
                .Take(10);

            foreach (var pName in projectsToPrint)
            {
                sb.AppendLine(pName);   
            }

            return sb.ToString().TrimEnd();
        }

        public static string RemoveTown(SoftUniContext context)
        {
            var sb = new StringBuilder();

            var townName = "Seattle";

            var employeesLivingInTown = context.Employees
                .Where(e => e.Address.Town.Name == townName);

            foreach (var employee in employeesLivingInTown)
            {
                employee.AddressId = null;
            }

            var addresses = context.Addresses
                .Where(a => a.Town.Name == townName);

            var addressCount = 0;
            foreach (var address in addresses)
            { 
                context.Addresses.Remove(address);
                addressCount++;
            }

            var town = context.Towns.Single(t => t.Name == townName);
            sb.AppendLine($"{addressCount} addresses in {town.Name} were deleted");

            context.Towns.Remove(town);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
    }
}
