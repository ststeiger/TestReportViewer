using System.Collections.Generic;

namespace ExportToWord
{
    public static class EmployeeRepository
    {
        public static List<Employee> GetAllEmployees()
        {
            Employee employee1 = new Employee {Name="Satyaki Mishra", EmployeeID="H0090", Designation="Software Engineer", Salary=10000 };
            Employee employee2 = new Employee { Name = "Raman Saha", EmployeeID = "A0090", Designation = "Test Lead", Salary = 15000 };
            Employee employee3 = new Employee { Name = "Purab Dash", EmployeeID = "ZD120", Designation = "Sr.Software Engineer", Salary = 13000 };
            return new List<Employee> { employee1, employee2, employee3 };
        }
    }
}