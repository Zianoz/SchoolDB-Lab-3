using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
namespace SchoolDB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Connection string to connect to the SchoolDB database
            string connectionString = "Server=(localdb)\\mssqllocaldb;Database=SchoolDB;Trusted_Connection=True";

            while (true) // Loop for navigation
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Fetch Employees");
                Console.WriteLine("2. Fetch Students");
                Console.WriteLine("3. Fetch Students in a Class");
                Console.WriteLine("4. Fetch Grades from last month");
                Console.WriteLine("5. Check Grades");
                Console.WriteLine("6. Add new student");
                Console.WriteLine("7. Add new employee");
                Console.WriteLine("8. Exit");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Methods.FetchEmployees(connectionString);
                        break;
                    case "2":
                        Student.FetchStudents();
                        break;
                    case "3":
                        Student.FetchStudentsInClass();
                        break;
                    case "4":
                        Grades.FetchGrades(connectionString);
                        break;
                    case "5":
                        Grades.AverageGrade(connectionString);
                        break;
                    case "6":
                        Student.AddNewStudent(connectionString);
                        break;
                    case "7":
                        Methods.AddNewEmployee();
                        break;
                    case "8":
                        return;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        break;
                }
            }
        }

    }
}
