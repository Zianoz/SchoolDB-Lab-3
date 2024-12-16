using Microsoft.Data.SqlClient;

namespace SchoolDB
{
    public class Student
    {
        public int StudentID { get; set; }
        public string StudentFirstName { get; set; }
        public string StudentLastName { get; set; }
        public int StudentPersonNumber { get; set; }
        public int ClassID { get; set; }

        public static void FetchStudents()
        {
            using (var context = new SchoolContext())
            {
                Console.WriteLine("Choose sorting option: 1. First Name, 2. Last Name");
                string sortOption = Console.ReadLine();
                Console.WriteLine("Choose order: 1. Ascending, 2. Descending");
                string orderOption = Console.ReadLine();

                IQueryable<Student> query;

                if (sortOption == "1")
                {
                    query = orderOption == "1"
                        ? context.Students.OrderBy(s => s.StudentFirstName)
                        : context.Students.OrderByDescending(s => s.StudentFirstName);
                }
                else
                {
                    query = orderOption == "1"
                        ? context.Students.OrderBy(s => s.StudentLastName)
                        : context.Students.OrderByDescending(s => s.StudentLastName);
                }

                var students = query.Select(s => new
                {
                    StudentID = s.StudentID.ToString(),  //Convert to string or it breaks
                    StudentFirstName = s.StudentFirstName,
                    StudentLastName = s.StudentLastName,
                    StudentPersonNumber = s.StudentPersonNumber.ToString(),  //Convert to string or it breaks
                    ClassID = s.ClassID
                }).ToList();


                Console.WriteLine("Students:");
                foreach (var student in students)
                {
                    Console.WriteLine($"{student.StudentFirstName} {student.StudentLastName} ({student.ClassID})");
                }
            }
        }

        public static void FetchStudentsInClass()
        {
            using (var context = new SchoolContext())
            {
                //Get all distinct class IDs from the students
                var classes = context.Students.Select(s => s.ClassID).Distinct().ToList();

                // Step 2: Print all available classes
                Console.WriteLine("Available classes:");
                for (int i = 0; i < classes.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. Class {classes[i]}");
                }

                Console.WriteLine("Choose a class to view the students in them");
                string choice = Console.ReadLine();

                //Filter students based on the selected class
                IQueryable<Student> query = context.Students.Where(s => s.ClassID == Int32.Parse(choice));

                //Execute the query and fetch the students
                var studentsInClass = query.Select(s => new
                {
                    StudentID = s.StudentID.ToString(),  // Convert to string
                    StudentFirstName = s.StudentFirstName,
                    StudentLastName = s.StudentLastName,
                    StudentPersonNumber = s.StudentPersonNumber.ToString(),  // Convert to string
                    ClassID = s.ClassID
                }).ToList();

                //Print out the students in the selected class
                if (studentsInClass.Any())
                {
                    Console.WriteLine($"Here are all the students in class {choice}:");
                    foreach (var student in studentsInClass)
                    {
                        Console.WriteLine($"{student.StudentFirstName} {student.StudentLastName} (ID: {student.StudentID}, Person Number: {student.StudentPersonNumber})");
                    }
                }
                else
                {
                    Console.WriteLine($"No students found in class {choice}.");
                }
            }
        }
        public static void AddNewStudent(string connectionString)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    Console.WriteLine("Enter student's first name:");
                    string firstName = Console.ReadLine();

                    Console.WriteLine("Enter student's last name:");
                    string lastName = Console.ReadLine();

                    Console.WriteLine("Enter student's person number:");
                    string personNumber = Console.ReadLine();

                    Console.WriteLine("Enter the class ID the student belongs to:");
                    int classId;
                    while (!int.TryParse(Console.ReadLine(), out classId))
                    {
                        Console.WriteLine("Please enter a valid numeric class ID:");
                    }

                    string query = @"
                INSERT INTO Students (StudentFirstName, StudentLastName, StudentPersonNumber, ClassID) 
                VALUES (@FirstName, @LastName, @PersonNumber, @ClassID)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FirstName", firstName);
                        command.Parameters.AddWithValue("@LastName", lastName);
                        command.Parameters.AddWithValue("@PersonNumber", personNumber);
                        command.Parameters.AddWithValue("@ClassID", classId);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            Console.WriteLine("Student added successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Failed to add the student.");
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
