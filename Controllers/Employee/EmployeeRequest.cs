namespace AppRequest.Controllers.Employee;

public record EmployeeRequest(string Email, string Password, string Name, string EmployeeCode);