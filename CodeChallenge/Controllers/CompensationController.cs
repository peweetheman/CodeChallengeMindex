using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;

namespace CodeChallenge.Controllers
{
    [Route("api/compensation")]
    public class CompensationController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICompensationService _compensationService;
        private readonly IEmployeeService _employeeService;

        public CompensationController(ILogger<CompensationController> logger, 
                                      ICompensationService compensationService,
                                      IEmployeeService employeeService)
        {
            _logger = logger;
            _compensationService = compensationService;
            _employeeService = employeeService;
        }

        [HttpPost]
        public IActionResult CreateCompensation([FromBody] Compensation compensation)
        {
            _logger.LogDebug($"Received compensation create request for employee '{compensation.EmployeeId}'");

            Employee employee = _employeeService.GetById(compensation.EmployeeId);
            if (employee == null)
                return NotFound($"Employee '{compensation.EmployeeId}' not found");

            Compensation newCompensation = _compensationService.Create(compensation);

            if (newCompensation == null)
                return NotFound();

            return CreatedAtRoute("getCompensationByEmployeeId", new { employeeId = newCompensation.EmployeeId }, newCompensation);
        }

        [HttpGet("{employeeId}", Name = "getCompensationByEmployeeId")]
        public IActionResult GetCompensationByEmployeeId(String employeeId)
        {
            _logger.LogDebug($"Received compensation get request for employee '{employeeId}'");

            var compensation = _compensationService.GetByEmployeeId(employeeId);

            if (compensation == null)
                return NotFound();

            return Ok(compensation);
        }
    }
}
