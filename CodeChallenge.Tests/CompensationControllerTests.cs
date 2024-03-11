
using System;
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
    public class CompensationControllerTests
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
        public static void TearDownClass()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        /// <summary>
        /// Validates that creating a compensation for an existing employee returns HTTP Created,
        /// and the compensation details match the request.
        /// </summary>
        [TestMethod]
        public void CreateCompensation_ForExistingEmployee_ReturnsCreatedWithCorrectDetails()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f"; // Using John Lennon's ID
            var effectiveDate = new DateTime(2022, 3, 11);
            var salary = 75000.00m;

            var compensationRequest = new
            {
                EmployeeId = employeeId,
                EffectiveDate = effectiveDate,
                Salary = salary
            };

            var requestContent = new StringContent(new JsonSerialization().ToJson(compensationRequest), Encoding.UTF8, "application/json");

            // Execute
            var response = _httpClient.PostAsync("api/compensation", requestContent).Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var createdCompensation = response.DeserializeContent<Compensation>();
            Assert.AreEqual(employeeId, createdCompensation.Employee.EmployeeId);
            Assert.AreEqual(effectiveDate.Date, createdCompensation.EffectiveDate.Date); // Compare dates as the time component might differ
            Assert.AreEqual(salary, createdCompensation.Salary);
        }

        /// <summary>
        /// Validates that attempting to create a compensation for a non-existent employee returns HTTP NotFound.
        /// </summary>
        [TestMethod]
        public void CreateCompensation_ForNonExistentEmployee_ReturnsNotFound()
        {
            // Arrange
            var compensationRequest = new
            {
                EmployeeId = "Invalid_ID",
                EffectiveDate = new DateTime(2022, 3, 11),
                Salary = 50000.00m
            };

            var requestContent = new StringContent(new JsonSerialization().ToJson(compensationRequest), Encoding.UTF8, "application/json");

            // Execute
            var response = _httpClient.PostAsync("api/compensation", requestContent).Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        /// <summary>
        /// Validates retrieving a compensation by employee ID returns HTTP Ok with expected compensation details.
        /// </summary>
        [TestMethod]
        public void GetCompensationByEmployeeId_ReturnsOkWithCompensationDetails()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var effectiveDate = new DateTime(2022, 3, 11);
            var salary = 75000.00m;
            CreateCompensationForTest(employeeId, effectiveDate, salary);

            // Execute
            var response = _httpClient.GetAsync($"api/compensation/{employeeId}").Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var compensation = response.DeserializeContent<Compensation>();

            Assert.IsNotNull(compensation);
            Assert.AreEqual(employeeId, compensation.Employee.EmployeeId);
            Assert.AreEqual(effectiveDate, compensation.EffectiveDate);
            Assert.AreEqual(salary, compensation.Salary);
        }

        /// <summary>
        /// Validates that attempting to retrieve a compensation by an invalid employee ID returns HTTP NotFound.
        /// </summary>
        [TestMethod]
        public void GetCompensationByInvalidEmployeeId_ReturnsNotFound()
        {
            // Arrange
            var invalidEmployeeId = "Invalid_ID";

            // Execute
            var response = _httpClient.GetAsync($"api/compensation/{invalidEmployeeId}").Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        /// <summary>
        /// Helper method to create a compensation for a given employee. Used to ensure consistent test data.
        /// </summary>
        private void CreateCompensationForTest(string employeeId, DateTime effectiveDate, decimal salary)
        {
            var compensationRequest = new
            {
                EmployeeId = employeeId,
                EffectiveDate = effectiveDate,
                Salary = salary
            };

            var requestContent = new StringContent(new JsonSerialization().ToJson(compensationRequest), Encoding.UTF8, "application/json");
            _httpClient.PostAsync("api/compensation", requestContent).Wait();
        }
    }
}
