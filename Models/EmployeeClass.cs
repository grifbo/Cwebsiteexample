using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IHLA_Template.Models
{
    public class Employee
    {
        public User EmployeeUser = new User();
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeePhone { get; set; }
		public int EmployeeLoginID { get; set; }

    }
    public class ClassStudent
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string dateOfAssign { get; set; }
    }

    public class ListEmployeeStudent
    {
        public List<Employee> Classroom { get; set; }
        public List<ClassStudent> Children { get; set; }

        public static ListEmployeeStudent populate()
        {
            List<Employee> employees = new List<Employee>();
            List<ClassStudent> students = new List<ClassStudent>();
            ListEmployeeStudent list = new ListEmployeeStudent
            {
                Classroom = employees,
                Children = students
            };
            try
            {
                Database db = new Database();
                SqlConnection cn = null;
                if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
                SqlCommand cm = new SqlCommand("SELECT intEmployeeID, strFirstName, strLastName, strPhoneNumber from TEmployees WHERE strProcessing='HIRED'", cn);
                SqlDataReader result = cm.ExecuteReader();
                while (result.Read())
                {
                    Employee emp = new Employee()
                    {
                        EmployeeID = result.GetInt32(0),
                        EmployeeName = result.GetString(1) + " " + result.GetString(2),
                        EmployeePhone = result.GetString(3)
                    };
                    employees.Add(emp);
                }
                result.Close();
                cm.Cancel();
                cm = new SqlCommand("SELECT * from TChildren WHERE strStatus='ENROLLED'", cn);
                result = cm.ExecuteReader();
                while (result.Read())
                {
                    ClassStudent stu = new ClassStudent()
                    {
                        ID = result.GetInt32(0),
                        Name = result.GetString(1),
						Notes = result.GetString(10)
                    };
                    students.Add(stu);
                }
                result.Close();
                cm.Cancel();
                db.CloseDBConnection(ref cn);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            if(list.Children.Count < 1)
            {
                list.Children.Add(new ClassStudent() { ID = -1, Name = "No Student(in DB)" });
            }
            if (list.Classroom.Count < 1)
            {
                list.Classroom.Add(new Employee() { EmployeeID = -1, EmployeeName = "No Employee(in Db)" });
            }
            return list;
        }
    }
    public class EmployeeClass
    {
        
        public Employee self { get; set; }
        public  List<ClassStudent> Students { get; set; }

        public static List<EmployeeClass> populate()
        {
            List<EmployeeClass> employees = new List<EmployeeClass>();

            try
            {
                Database db = new Database();
                SqlConnection cn = null;
                if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
                SqlCommand cm = new SqlCommand("SELECT TEmployees.intEmployeeID, TEmployees.strFirstName, TEmployees.strLastName, TEmployees.strPhoneNumber, TEmployees.intLoginID FROM TEmployees", cn);

                SqlDataReader result = cm.ExecuteReader();
                while (result.Read())
                {
                    EmployeeClass newEmployee = new EmployeeClass()
                    {       
                        self = new Employee() {
                            EmployeeID = result.GetInt32(0),
                            EmployeeName = result.GetString(1) + " " + result.GetString(2),
                            EmployeePhone = result.GetString(result.GetOrdinal("strPhoneNumber")),
                            EmployeeLoginID = result.GetInt32(4)
                        },
                        Students = GetStudents(result.GetInt32(0))

                    };
                    employees.Add(newEmployee);
                }
                db.CloseDBConnection(ref cn);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            if(employees.Count < 1)
            {
                employees.Add(new EmployeeClass() { self = new Employee() { EmployeeID = -1, EmployeeName="No Employee in DB" }, Students = GetStudents(-1) });
            }

            return employees;
        }
        private static List<ClassStudent> GetStudents(int EmployeeID)
        {
            List<ClassStudent> students = new List<ClassStudent>();
            try
            {
                Database db = new Database();
                SqlConnection cn = null;
                if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
                SqlCommand cm = new SqlCommand("SELECT TChildren.intChildID, TChildren.strFirstName, TChildren.strLastName, TEmployeeChildAssignment.dtmDateOfAssignment, TEmployeeChildAssignment.strNotes FROM TChildren INNER JOIN TEmployeeChildAssignment on TChildren.intchildID = TEmployeeChildAssignment.intChildID WHERE TEmployeeChildAssignment.intEmployeeID=" + EmployeeID, cn);

                SqlDataReader result = cm.ExecuteReader();
                while (result.Read())
                {
                    ClassStudent stu = new ClassStudent()
                    {
                        ID = result.GetInt32(0),
                        Name = result.GetString(1) + " " + result.GetString(2), 
                        dateOfAssign = result.GetDateTime(3).ToString(),
                        Notes= result.GetString(4)
                    };
                    students.Add(stu);
                }
                db.CloseDBConnection(ref cn);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
            if (students.Count < 1)
            {
                ClassStudent guardian = new ClassStudent()
                {
                    ID = -1,
                    Name = "None"
                };
                students.Add(guardian);
            }
            return students;
        }
    }
}