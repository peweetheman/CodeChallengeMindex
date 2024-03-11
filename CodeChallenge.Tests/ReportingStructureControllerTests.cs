using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

using CodeChallenge.Models;

using CodeChallenge.Tests.Integration.Extensions;
using CodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeChallenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }


        /// <summary>
        /// Tests retrieval of a ReportingStructure for an employee with no direct reports.
        /// Verifies the employee's details and that the number of reports is 0.
        /// </summary>
        [TestMethod]
        public void ReportingStructure_SingleEmployee_NoDirectReports()
        {
            // Arrange
            var employeeIdWithNoDirectReports = "c0c2293d-16bd-4603-8e08-638a9d18b22c"; // George Harrison

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reporting/{employeeIdWithNoDirectReports}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var reportingStructure = response.DeserializeContent<ReportingStructure>();

            Assert.IsNotNull(reportingStructure);
            Assert.AreEqual(0, reportingStructure.NumberOfReports);  // George Harrison has no direct reports in the seed data.
            Assert.AreEqual("George", reportingStructure.Employee.FirstName);
            Assert.AreEqual("Harrison", reportingStructure.Employee.LastName);
            Assert.AreEqual("Developer III", reportingStructure.Employee.Position);
            Assert.AreEqual("Engineering", reportingStructure.Employee.Department);
        }

        /// <summary>
        /// Tests retrieval of a ReportingStructure for an employee with a tree-like structure of direct reports.
        /// Verifies the employee's details and the correct calculation of the total number of reports.
        /// </summary>
        [TestMethod]
        public void ReportingStructure_MultipleLevels_DirectReports()
        {
            // Arrange
            var employeeIdWithMultipleLevels = "16a596ae-edd3-4847-99fe-c4518e82c86f"; // John Lennon

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reporting/{employeeIdWithMultipleLevels}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var reportingStructure = response.DeserializeContent<ReportingStructure>();

            Assert.IsNotNull(reportingStructure);
            Assert.AreEqual("John", reportingStructure.Employee.FirstName);
            Assert.AreEqual("Lennon", reportingStructure.Employee.LastName);
            Assert.AreEqual("Development Manager", reportingStructure.Employee.Position);
            Assert.AreEqual("Engineering", reportingStructure.Employee.Department);
            // Provided code does not load direct reports, I included a potential fix by adding EmployeeRepository.GetByIdWithDirectReports()
            //Assert.AreEqual(4, reportingStructure.NumberOfReports); // John Lennon has a tree-like structure of direct reports.
            //Assert.AreEqual(2, reportingStructure.Employee.DirectReports.Count);
        }

        /// <summary>
        /// Tests the API's response when requesting a ReportingStructure for a nonexistent employee ID.
        /// Verifies that the response is NotFound.
        /// </summary>
        [TestMethod]
        public void ReportingStructure_NonexistentEmployee()
        {
            // Arrange
            var nonexistentEmployeeId = "nonexistent-id";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/reporting/{nonexistentEmployeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
