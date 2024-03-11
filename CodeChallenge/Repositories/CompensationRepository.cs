using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        private readonly EmployeeContext _employeeContext;

        public CompensationRepository(EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
        }

        public Compensation Add(Compensation compensation)
        {
            _employeeContext.Compensations.Add(compensation);
            return compensation;
        }

        public Compensation GetByEmployeeId(string employeeId)
        {
            return _employeeContext.Compensations.Where(c => c.Employee.EmployeeId == employeeId)
                .Include(c => c.Employee)
                .FirstOrDefault();
        }

        public Task SaveAsync()
        {
            return _employeeContext.SaveChangesAsync();
        }
    }
}
