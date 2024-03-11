using System.Linq;
using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.Extensions.Logging;

namespace CodeChallenge.Services
{
    public class ReportingStructureService : IReportingStructureService
    {
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<ReportingStructureService> _logger;

        public ReportingStructureService(ILogger<ReportingStructureService> logger, IEmployeeService employeeService)
        {
            _employeeService = employeeService;
            _logger = logger;
        }

        public ReportingStructure Create(string id)
        {
            var employee = _employeeService.GetById(id);
            if (employee == null) return null;

            // Recursively Calculates Direct Reports Count
            var reportsCount = GetReportingCount(employee);

            return new ReportingStructure()
            {
                NumberOfReports = reportsCount,
                Employee = employee
            };
        }

        private int GetReportingCount(Employee e)
        {
            if (e.DirectReports == null) return 0;
            int directReportsCount = e.DirectReports.Count;

            foreach (Employee employee in e.DirectReports)
            {
                // Recursive Call to add DirectReports of child
                directReportsCount += GetReportingCount(employee);
            }

            return directReportsCount;
        }

    }
}