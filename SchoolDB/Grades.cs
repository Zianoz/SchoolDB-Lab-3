using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDB
{
    internal class Grades
    {
        //Grade fetching using ADO.NET
        public static void FetchGrades(string connectionString)
        {
            Console.WriteLine("Here are the grades that have been set the last month");
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Database connection established.");

                    // Updated SQL query to handle DATE type
                    string query = @"
                    SELECT GradeID, StudentID, EmployeeID, Grade, AssignedDate 
                    FROM Grades 
                    WHERE AssignedDate >= CAST(DATEADD(MONTH, -1, GETDATE()) AS DATE);";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("Grades from the last month:");
                            while (reader.Read())
                            {
                                var gradeId = reader["GradeID"].ToString();
                                var studentId = reader["StudentID"].ToString();
                                var employeeId = reader["EmployeeID"].ToString();
                                var grade = reader["Grade"].ToString();
                                var assignedDate = reader["AssignedDate"].ToString();

                                // Format and display the result
                                Console.WriteLine($"Grade ID: {gradeId}, Student ID: {studentId}, " +
                                                  $"Grade: {grade}, Assigned by Employee ID: {employeeId}, " +
                                                  $"Assigned Date: {assignedDate}");
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

        public static void AverageGrade(string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string query = @"
                SELECT 
                    Classes.ClassName,
                    AVG(CASE 
                        WHEN Grades.Grade = 'A' THEN 5
                        WHEN Grades.Grade = 'B' THEN 4
                        WHEN Grades.Grade = 'C' THEN 3
                        WHEN Grades.Grade = 'D' THEN 2
                        WHEN Grades.Grade = 'F' THEN 1
                        ELSE NULL END) AS AverageGrade,
                    MAX(CASE 
                        WHEN Grades.Grade = 'A' THEN 5
                        WHEN Grades.Grade = 'B' THEN 4
                        WHEN Grades.Grade = 'C' THEN 3
                        WHEN Grades.Grade = 'D' THEN 2
                        WHEN Grades.Grade = 'F' THEN 1
                        ELSE NULL END) AS HighestGrade,
                    MIN(CASE 
                        WHEN Grades.Grade = 'A' THEN 5
                        WHEN Grades.Grade = 'B' THEN 4
                        WHEN Grades.Grade = 'C' THEN 3
                        WHEN Grades.Grade = 'D' THEN 2
                        WHEN Grades.Grade = 'F' THEN 1
                        ELSE NULL END) AS LowestGrade
                FROM Grades
                INNER JOIN Students ON Grades.StudentID = Students.StudentID
                INNER JOIN Classes ON Students.ClassID = Classes.ClassID
                GROUP BY Classes.ClassName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            Console.WriteLine("Class Name | Average Grade | Highest Grade | Lowest Grade");
                            Console.WriteLine(new string('-', 50));

                            while (reader.Read())
                            {
                                string className = reader["ClassName"].ToString();
                                string averageGrade = reader["AverageGrade"]?.ToString() ?? "N/A";
                                string highestGrade = reader["HighestGrade"]?.ToString() ?? "N/A";
                                string lowestGrade = reader["LowestGrade"]?.ToString() ?? "N/A";

                                Console.WriteLine($"{className} | {averageGrade} | {highestGrade} | {lowestGrade}");
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

    }
}
