using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IHLA_Template.Models
{
	public class AdminModel
	{
		public User AdminUser = new User();
		public List<Student> CurrentStudentList = new List<Student>();
		public List<Student> PendingStudentList = new List<Student>();
		public Student StudentApplicant = new Student();
		public Parent ParentApplicant = new Parent();
		public List<Parent> ParentList = new List<Parent>();
		public List<EmployeeApplications> EmployeeList = new List<EmployeeApplications>();
		public List<Payments> PaymentList = new List<Payments>();
		public List<User> UserList = new List<User>();
		public List<EmergencyContact> EmergencyContactList = new List<EmergencyContact>();
		public Events NewEvent = new Events();
		public List<EmployeeApplications> PendingEmployeeList = new List<EmployeeApplications>();
		public EmployeeApplications EmployeeApplicant = new EmployeeApplications();
		public List<Tours> UpcomingTours = new List<Tours>();

		public List<Tours> GetUpcomingTours(string today)
		{
			try {

				Database db = new Database();

				SqlConnection cn = new SqlConnection();
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter(("select * From TVisits WHERE dtmDate >= '" + today + "'"), cn);
				List<Tours> UpcomingTours = new List<Tours>();

				da.SelectCommand.CommandType = CommandType.Text;

				try {
					DataSet ds = new DataSet();
					da.Fill(ds);

					int rowCount = ds.Tables[0].Rows.Count;

					if (rowCount > 0) {

						for (int i = 0; i < rowCount; i++) {
							Tours newTour = new Tours();
							DataRow dr = ds.Tables[0].Rows[i];
							newTour.FirstName = (string)dr["strVisitorFirstName"];
							newTour.LastName = (string)dr["strVisitorLastName"];
							newTour.Date = (string)dr["dtmdate"];
							newTour.Time = (string)dr["strTime"];


							UpcomingTours.Add(newTour);
						}
					}
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally {
					db.CloseDBConnection(ref cn);
				}
				return UpcomingTours;
			}
			catch (Exception ex) {
				throw new Exception(ex.Message);
			}
		}

		public void EnrollStudent(int StudentID)
		{
			Database db = new Database();

			try {
				SqlConnection cn = null;
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("UPDATE_Enrolled", cn);

				db.SetParameter(ref cm, "@intChildID", StudentID, SqlDbType.BigInt);

				cm.ExecuteReader();

				db.CloseDBConnection(ref cn);


			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public List<Student> GetPendingStudentList()
		{
			try {

				Database db = new Database();

				SqlConnection cn = new SqlConnection();
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("vPendingChild", cn);
				List<Student> PendingStudents = new List<Student>();
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				db.SetParameter(ref da, "@strStatus", "PENDING", SqlDbType.NVarChar);

				try {
					DataSet ds = new DataSet();
					da.Fill(ds);

					int rowCount = ds.Tables[0].Rows.Count;

					if (rowCount > 0) {

						for (int i = 0; i < rowCount; i++) {
							Student newStudent = new Student();
							DataRow dr = ds.Tables[0].Rows[i];
							newStudent.StudentID = (int)dr["intChildID"];
							newStudent.FirstName = (string)dr["strFirstName"];
							newStudent.LastName = (string)dr["strLastName"];
							newStudent.DateOfBirth = ((DateTime)dr["dtmDOB"]).ToString("MM/dd/yyyy");
							newStudent.ExpectedStartDate = (string)dr["strExpectedStartDate"];
							newStudent.Address = (string)dr["strAddress"];
							newStudent.City = (string)dr["strCity"];
							newStudent.State = (int)dr["intStateID"];
							newStudent.ZipCode = (string)dr["strZip"];
							newStudent.HomePhoneNumber = (string)dr["strPhoneNumber"];
							newStudent.AdditionalInformation = (string)dr["strDescription"];
							newStudent.ApplicationDate = ((DateTime)dr["dtmApplication"]).ToString("MM/dd/yyyy");

							PendingStudents.Add(newStudent);
						}
					}
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally {
					db.CloseDBConnection(ref cn);
				}
				return PendingStudents;
			}
			catch (Exception ex) {
				throw new Exception(ex.Message);
			}

		}

		public string GetParentIDByStudentID(string ChildID)
		{
			try {

				Database db = new Database();

				SqlConnection cn = new SqlConnection();
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("vParentChild", cn);
				string strParentID = string.Empty;
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				db.SetParameter(ref da, "@intChildID", Convert.ToInt32(ChildID), SqlDbType.Int);
				db.SetParameter(ref da, "@intParentID", Convert.ToInt32(ChildID), SqlDbType.Int);
				//db.SetParameter(ref da, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				try {
					DataSet ds = new DataSet();
					da.Fill(ds);

					int rowCount = ds.Tables[0].Rows.Count;

					if (rowCount > 0) {

						for (int i = 0; i < rowCount; i++) {
							Student newStudent = new Student();
							DataRow dr = ds.Tables[0].Rows[i];
							strParentID = ((int)dr["intParentID"]).ToString();
						}
					}
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally {
					db.CloseDBConnection(ref cn);
				}
				return strParentID;
			}
			catch (Exception ex) {
				throw new Exception(ex.Message);
			}

		}

		public Student GetStudentByStudentID(string ChildID)
		{
			try {

				Database db = new Database();

				SqlConnection cn = new SqlConnection();
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("vParentChild", cn);
				Student newStudent = new Student();
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				db.SetParameter(ref da, "@intChildID", Convert.ToInt32(ChildID), SqlDbType.Int);
				db.SetParameter(ref da, "@intParentID", Convert.ToInt32(ChildID), SqlDbType.Int);

				try {
					DataSet ds = new DataSet();
					da.Fill(ds);
					
							
							DataRow dr = ds.Tables[0].Rows[0];
							newStudent.StudentID = (int)dr[2];
							newStudent.FirstName = (string)dr[4];
							newStudent.LastName = (string)dr[5];
							newStudent.DateOfBirth = ((DateTime)dr[6]).ToString("MM/dd/yyyy");
							newStudent.ExpectedStartDate = (string)dr[7];
							newStudent.Address = (string)dr[8];
							newStudent.City = (string)dr[9];
							newStudent.State = (int)dr[10];
							newStudent.ZipCode = (string)dr[11];
							newStudent.HomePhoneNumber = (string)dr[12];
							newStudent.AdditionalInformation = (string)dr[13];
							newStudent.ApplicationDate = ((DateTime)dr[14]).ToString("MM/dd/yyyy");
					
						
					
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally {
					db.CloseDBConnection(ref cn);
				}
				return newStudent;
			}
			catch (Exception ex) {
				throw new Exception(ex.Message);
			}

		}

		public Parent GetParentByStudentID(string ChildID)
		{
			try {

				Database db = new Database();

				SqlConnection cn = new SqlConnection();
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("vParentChild", cn);
				Parent newParent = new Parent();
				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				db.SetParameter(ref da, "@intChildID", Convert.ToInt32(ChildID), SqlDbType.Int);
				db.SetParameter(ref da, "@intParentID", Convert.ToInt32(ChildID), SqlDbType.Int);

				try {
					DataSet ds = new DataSet();
					da.Fill(ds);


					DataRow dr = ds.Tables[0].Rows[0];
					newParent.UID = (int)dr[16];
					newParent.FirstName = (string)dr[17];
					newParent.LastName = (string)dr[18];
					newParent.Address = (string)dr[19];
					newParent.City = (string)dr[20];
					newParent.State = (int)dr[21];
					newParent.ZipCode = (string)dr[22];
					newParent.HomePhoneNumber = (string)dr[24];
					


				}
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally {
					db.CloseDBConnection(ref cn);
				}
				return newParent;
			}
			catch (Exception ex) {
				throw new Exception(ex.Message);
			}

		}

		public List<EmployeeApplications> GetPendingEmployeeList()
		{
			try
			{

				Database db = new Database();

				SqlConnection cn = new SqlConnection();
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("Select * FROM TEmployees Where strProcessing = 'PENDING'", cn);
				List<EmployeeApplications> PendingEmployee = new List<EmployeeApplications>();
				da.SelectCommand.CommandType = CommandType.Text;

				try
				{
					DataSet ds = new DataSet();
					da.Fill(ds);

					int rowCount = ds.Tables[0].Rows.Count;

					if (rowCount > 0)
					{

						for (int i = 0; i < rowCount; i++)
						{
							EmployeeApplications newEmployee = new EmployeeApplications();
							DataRow dr = ds.Tables[0].Rows[i];
							newEmployee.UID = (int)dr["intEmployeeID"];
							newEmployee.FirstName = (string)dr["strFirstName"];
							newEmployee.LastName = (string)dr["strLastName"];
							newEmployee.DateOfApplication = (DateTime)dr["dtmDateOfApplication"];

							PendingEmployee.Add(newEmployee);
						}
					}
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally
				{
					db.CloseDBConnection(ref cn);
				}
				return PendingEmployee;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

		}

		public EmployeeApplications GetEmployeeByEmployeeID(string EmployeeID)
		{
			try
			{

				Database db = new Database();

				SqlConnection cn = new SqlConnection();
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("Select * FROM TEmployees Where intEmployeeID = " + EmployeeID, cn);
				EmployeeApplications newEmployee = new EmployeeApplications();
				da.SelectCommand.CommandType = CommandType.Text;

				//db.SetParameter(ref da, "@UID", Convert.ToInt32(EmployeeID), SqlDbType.Int);

				try
				{
					DataSet ds = new DataSet();
					da.Fill(ds);

					DataRow dr = ds.Tables[0].Rows[0];
					newEmployee.FirstName = (string)dr["strFirstName"];
					newEmployee.LastName = (string)dr["strLastName"];
					newEmployee.Address = (string)dr["strAddress"];
					newEmployee.City = (string)dr["strCity"];
					newEmployee.State = (int)dr["intStateID"];
					newEmployee.Zip = (string)dr["strZip"];
					newEmployee.PhoneNumber = (string)dr["strPhoneNumber"];
					newEmployee.Email = (string)dr["strEmail"];
					newEmployee.EmergencyContact = (string)dr["strEmergencyContactName"];
					newEmployee.EmergecyNumber = (string)dr["strEmergencyPhoneNumber"];
					newEmployee.UsaEligible = char.Parse((string)dr["strIsCitizen"]);
					newEmployee.Misdemeanor = char.Parse((string)dr["strHasMisdemeanor"]);
					newEmployee.Felony = char.Parse((string)dr["strHasFelony"]);
					newEmployee.CovictionExplained = (string)dr["strExplanation"];
					newEmployee.FullTime = char.Parse((string)dr["strFullTime"]);
					newEmployee.PartTime = char.Parse((string)dr["strPartTime"]);
					newEmployee.AgeCheck = (string)dr["strIsOld"];
					newEmployee.StartDate = ((DateTime)dr["dtmDesiredStartDate"]).ToString("MM/dd/yyyy");
					newEmployee.Pay = ((decimal)dr["decDesiredPay"]).ToString();
					newEmployee.Skills = (string)dr["strSkills"];
					newEmployee.Certs = (string)dr["strCertifications"];
					newEmployee.CareerGoals = (string)dr["strCareerGoals"];
					newEmployee.HireYou = (string)dr["strWhyHire"];
					newEmployee.ProConduct = (string)dr["strProConduct"];
					newEmployee.CustService = (string)dr["strCustService"];
					newEmployee.ToyFight = (string)dr["strToyFight"];


				}
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally
				{
					db.CloseDBConnection(ref cn);
				}
				return newEmployee;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}

		}
		public int InsertNewEvent(Events ne)
		{
			try {
				Database db = new Database();
				SqlConnection cn = null;
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_TEvents", cn);
				int ReturnValue = -1;

				db.SetParameter(ref cm, "@uid", ne.EventID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
				db.SetParameter(ref cm, "@strEvent", ne.Event, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@dtmDateOfEvent", ne.EventDate, SqlDbType.DateTime);
				db.SetParameter(ref cm, "@strDescription", ne.Description, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@strTime", ne.Time, SqlDbType.NVarChar);

				db.SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);
				cm.ExecuteReader();

				ReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				db.CloseDBConnection(ref cn);

				if (ReturnValue == -1) throw new Exception("Event not added");

				return ReturnValue;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		

	}

	public class AdminFamilyPage
	{
		public string StudentsFirstName { set; get; }
		public string StudentsLastName { set; get; }
		public DateTime Age { set; get; }

		public List<AdminFamilyPageParentInfo> GetParentByStudentUserID(long StudentUserID)
		{
			try
			{

				Database db = new Database();

				SqlConnection cn = new SqlConnection();
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");

				SqlCommand cmd = new SqlCommand("", cn);
				db.SetParameter(ref cmd, "@intLoginID", StudentUserID, SqlDbType.NVarChar);
				db.SetParameter(ref cmd, "@strFirstName", StudentsFirstName, System.Data.SqlDbType.NVarChar, Direction: ParameterDirection.ReturnValue);
				db.SetParameter(ref cmd, "@strLastName", StudentsLastName, System.Data.SqlDbType.NVarChar, Direction: ParameterDirection.ReturnValue);
				db.SetParameter(ref cmd, "@dtmDOB", Age, System.Data.SqlDbType.Date, Direction: ParameterDirection.ReturnValue);

				cmd.ExecuteReader();

				SqlDataAdapter da = new SqlDataAdapter("", cn);
				List<AdminFamilyPageParentInfo> ParentList = new List<AdminFamilyPageParentInfo>();

				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				db.SetParameter(ref da, "@intLoginID", StudentUserID, SqlDbType.NVarChar);

				try
				{
					DataSet ds = new DataSet();
					da.Fill(ds);

					int rowCount = ds.Tables[0].Rows.Count;

					if (rowCount > 0)
					{
						AdminFamilyPageParentInfo Parent = new AdminFamilyPageParentInfo();
						DataRow dr = ds.Tables[0].Rows[0];
						Parent.ParentUID = (int)dr["intUID"];
						Parent.FirstName = (string)dr["strFirstName"];
						Parent.LastName = (string)dr["strLastName"];
						Parent.PhoneNumber = (string)dr["strPhoneNumber"];

						ParentList.Add(Parent);
					}
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally
				{
					db.CloseDBConnection(ref cn);
				}
				return ParentList;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

	}

	public class AdminFamilyPageParentInfo
	{
		public int ParentUID { set; get; }
		public string FirstName { set; get; }
		public string LastName { set; get; }
		public string PhoneNumber { set; get; }
	}

	public class AdminFamilyDelete
	{
		public int AdminFamilyDeletion(int UID)
        {
			int CompletionCheck = 0;

			Database db = new Database();

			SqlConnection cn = new SqlConnection();
			if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");

			SqlCommand cmd = new SqlCommand("", cn);
			db.SetParameter(ref cmd, "@intParentID", UID, SqlDbType.NVarChar);

			db.SetParameter(ref cmd, "@intCompletionCheck", CompletionCheck, System.Data.SqlDbType.Date, Direction: ParameterDirection.ReturnValue);

			cmd.ExecuteReader();

			return CompletionCheck;
        }
	}

	public class AdminEmployeeApproval
	{

		public int AdminEmployeeApprove(int UID, int approval)
		{
			string result = "";
			
            switch (approval)
			{
				case 1:
					result = "HIRED";
					break;
				case 2:
					result = "DENIED";
					break;
				default:
					return 0;
			}

			try {

                Models.Database db = new Database();

                SqlConnection cn = null;
                if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
                SqlCommand cmd = new SqlCommand("UPDATE_Processing", cn);

				db.SetParameter(ref cmd, "@intEmployeeID", UID, SqlDbType.Int);
				db.SetParameter(ref cmd, "@strProcessing", result, SqlDbType.NVarChar);

				cmd.ExecuteReader();

                db.CloseDBConnection(ref cn);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }


            return approval;
		}
	}


}