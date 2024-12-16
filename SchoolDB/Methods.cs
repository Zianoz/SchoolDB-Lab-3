using Microsoft.Data.SqlClient;

namespace SchoolDB
{
    internal class Methods
    {
        // Fetch employees using ADO.NET
        public static void FetchEmployees(string connectionString)
        {
            Console.WriteLine("Do you want to see all employees or filter by role? (all/role)");
            string filter = Console.ReadLine();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT EmployeeFirstName, EmployeeLastName, EmployeeRole FROM Employees";

                    if (filter?.ToLower() == "role")
                    {
                        Console.WriteLine("Enter role (e.g., Teacher, Principal, Administrator):");
                        string? role = Console.ReadLine();
                        query += " WHERE EmployeeRole = @Role";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Role", role);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                Console.WriteLine("Employees:");
                                while (reader.Read())
                                {
                                    Console.WriteLine($"{reader["EmployeeFirstName"]} {reader["EmployeeLastName"]} ({reader["EmployeeRole"]})");
                                }
                            }
                        }
                    }
                    else
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                Console.WriteLine("Employees:");
                                while (reader.Read())
                                {
                                    Console.WriteLine($"{reader["EmployeeFirstName"]} {reader["EmployeeLastName"]} ({reader["EmployeeRole"]})");
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public static void AddNewEmployee()
        {
            using (var context = new SchoolContext())
            {
                try
                {
                    Console.WriteLine("Enter employee's first name:");
                    string firstName = Console.ReadLine();

                    Console.WriteLine("Enter employee's last name:");
                    string lastName = Console.ReadLine();

                    Console.WriteLine("Enter employee's role (e.g., Teacher, Principal):");
                    string role = Console.ReadLine();

                    Employee newEmployee = new Employee
                    {
                        EmployeeFirstName = firstName,
                        EmployeeLastName = lastName,
                        EmployeeRole = role
                    };

                    context.Employees.Add(newEmployee);
                    context.SaveChanges();

                    Console.WriteLine("Employee added successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

    }
}
