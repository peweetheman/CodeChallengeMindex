# Patrick's Mindex Coding Challenge

## Updates

- Created new classes for the tasks described in the prompt at the bottom of this README. 
- Created and updated API endpoint documentation below.
- Created EmployeeRepository.GetByIdWithDirectReports() and related methods since provided implementation lacked this functionality.
- Created integration unit tests for the ReportingStructure and Compensation API endpoints.

## API Endpoints Documentation

### Employee Management
#### Create Employee

    Endpoint: POST /api/employee
    Description: Creates a new employee record.
    Payload: JSON representing an Employee object.
    Response: Returns the created Employee object with a 201 status code.

#### Get Employee by ID

    Endpoint: GET /api/employee/{id}
    Description: Retrieves an employee by their unique identifier.
    Parameters: id - The ID of the employee to retrieve.
    Response: Returns an Employee object if found, otherwise returns a 404 status code.

#### Update (Replace) Employee

    Endpoint: PUT /api/employee/{id}
    Description: Updates an existing employee record with new details.
    Parameters: id - The ID of the employee to update.
    Payload: JSON representing the new Employee details.
    Response: Returns the updated Employee object.

### Compensation Management
#### Create Compensation

    Endpoint: POST /api/compensation
    Description: Creates a compensation record for an employee.
    Payload: JSON representing a Compensation object including EmployeeId.
    Response: Returns the created Compensation object with a 201 status code if the employee exists, otherwise a 404 status code.

#### Get Compensation by Employee ID

    Endpoint: GET /api/compensation/{employeeId}
    Description: Retrieves compensation details for a specific employee.
    Parameters: employeeId - The ID of the employee whose compensation details are to be retrieved.
    Response: Returns a Compensation object if found, otherwise returns a 404 status code.

### Reporting Structure Management
#### Get Reporting Structure by Employee ID

    Endpoint: GET /api/reporting/{id}
    Description: Retrieves the reporting structure for a given employee.
    Parameters: id - The ID of the employee whose reporting structure is requested.
    Response: Returns a ReportingStructure object, including the number of reports and details, if the employee exists, otherwise a 404 status code.
--------
## What's Provided
A simple [.Net 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) web application has been created and bootstrapped 
with data. The application contains information about all employees at a company. On application start-up, an in-memory 
database is bootstrapped with a serialized snapshot of the database. While the application runs, the data may be
accessed and mutated in the database without impacting the snapshot.

### How to Run
You can run this by executing `dotnet run` on the command line or in [Visual Studio Community Edition](https://www.visualstudio.com/downloads/).


### How to Use
The following endpoints are available to use:
```
* CREATE
    * HTTP Method: POST 
    * URL: localhost:8080/api/employee
    * PAYLOAD: Employee
    * RESPONSE: Employee
* READ
    * HTTP Method: GET 
    * URL: localhost:8080/api/employee/{id}
    * RESPONSE: Employee
* UPDATE
    * HTTP Method: PUT 
    * URL: localhost:8080/api/employee/{id}
    * PAYLOAD: Employee
    * RESPONSE: Employee
```
The Employee has a JSON schema of:
```json
{
  "type":"Employee",
  "properties": {
    "employeeId": {
      "type": "string"
    },
    "firstName": {
      "type": "string"
    },
    "lastName": {
          "type": "string"
    },
    "position": {
          "type": "string"
    },
    "department": {
          "type": "string"
    },
    "directReports": {
      "type": "array",
      "items" : "string"
    }
  }
}
```
For all endpoints that require an "id" in the URL, this is the "employeeId" field.

## What to Implement
Clone or download the repository, do not fork it.

### Task 1
Create a new type, ReportingStructure, that has two properties: employee and numberOfReports.

For the field "numberOfReports", this should equal the total number of reports under a given employee. The number of 
reports is determined to be the number of directReports for an employee and all of their direct reports. For example, 
given the following employee structure:
```
                    John Lennon
                /               \
         Paul McCartney         Ringo Starr
                               /        \
                          Pete Best     George Harrison
```
The numberOfReports for employee John Lennon (employeeId: 16a596ae-edd3-4847-99fe-c4518e82c86f) would be equal to 4. 

This new type should have a new REST endpoint created for it. This new endpoint should accept an employeeId and return 
the fully filled out ReportingStructure for the specified employeeId. The values should be computed on the fly and will 
not be persisted.

### Task 2
Create a new type, Compensation. A Compensation has the following fields: employee, salary, and effectiveDate. Create 
two new Compensation REST endpoints. One to create and one to read by employeeId. These should persist and query the 
Compensation from the persistence layer.

## Delivery
Please upload your results to a publicly accessible Git repo. Free ones are provided by Github and Bitbucket.
