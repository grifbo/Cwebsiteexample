using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IHLA_Template.Models
{
	public class StudentApplication
	{
		public Student student = new Student();
		public Parent parent1 = new Parent();
		public Parent parent2 = new Parent();
		public EmergencyContact emergencyContact = new EmergencyContact();
		public Physician physician = new Physician();

		public long InsertStudent(StudentApplication sa)
		{
			try {
				Database db = new Database();
				SqlConnection cn = null;
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_Child", cn);
				long intUID = -1;

				db.SetParameter(ref cm, "@uid", sa.student.StudentID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
				db.SetParameter(ref cm, "@strFirstName", sa.student.FirstName, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@strLastName", sa.student.LastName, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@dtmDOB", sa.student.DateOfBirth, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@strExpectedStartDate", sa.student.ExpectedStartDate, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@strAddress", sa.student.Address, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@strCity", sa.student.City, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@intStateID", sa.student.State, SqlDbType.Int);
				db.SetParameter(ref cm, "@strZip", sa.student.ZipCode, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@strPhoneNumber", sa.student.HomePhoneNumber, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@strDescription", sa.student.AdditionalInformation, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@dtmApplication", DateTime.Now, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@strStatus", "PENDING", SqlDbType.NVarChar);

				//db.SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intUID = (long)cm.Parameters["@uid"].Value;
				db.CloseDBConnection(ref cn);

				if (intUID == -1) throw new Exception("Student ID not created");

				return intUID;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}


		public long InsertParent(Parent p)
		{
			try {
				Database db = new Database();
				SqlConnection cn = null;
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_PARENT", cn);
				long intUID = -1;

				db.SetParameter(ref cm, "@uid", p.UID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
				db.SetParameter(ref cm, "@strFirstName", p.FirstName, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@strLastName", p.LastName, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@strAddress", p.Address, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@strCity", p.City, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@intStateID", p.State, SqlDbType.Int);
				db.SetParameter(ref cm, "@strZip", p.ZipCode, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@strEmail", p.Email, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@strPhoneNumber", p.HomePhoneNumber, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@dtmSignUpDate", DateTime.Now, SqlDbType.DateTime);
				db.SetParameter(ref cm, "@intLoginID", p.LoginInID, SqlDbType.Int);

				//db.SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intUID = (long)cm.Parameters["@uid"].Value;
				db.CloseDBConnection(ref cn);

				if (intUID == -1) throw new Exception("Parent ID not created");

				return intUID;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public int InsertParentChildJoin(Parent p, Student s )
		{
			try {
				Database db = new Database();
				SqlConnection cn = null;
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_ParentChild", cn);
				int ReturnValue = -1;

				db.SetParameter(ref cm, "@intParentID", p.UID, SqlDbType.BigInt);
				db.SetParameter(ref cm, "@intChildID", s.StudentID, SqlDbType.BigInt);


				db.SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				ReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				db.CloseDBConnection(ref cn);

				if (ReturnValue == -1) throw new Exception("Unable to Join Parent/Student");

				return ReturnValue;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}


		public long InsertEmergencyContact(EmergencyContact e)
		{
			try {
				Database db = new Database();
				SqlConnection cn = null;
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_EmergencyContact", cn);
				long intUID = -1;

				db.SetParameter(ref cm, "@uid", e.UID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
				db.SetParameter(ref cm, "@strFirstName", e.FirstName, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@strLastName", e.LastName, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@strCity", e.City, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@intStateID", e.State, SqlDbType.Int);
				db.SetParameter(ref cm, "@strPhoneNumber", e.PhoneNumber, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@strAlternateNumber", e.OtherPhoneNumber, SqlDbType.NVarChar);

				//db.SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intUID = (long)cm.Parameters["@uid"].Value;
				db.CloseDBConnection(ref cn);

				if (intUID == -1) throw new Exception("Parent ID not created");

				return intUID;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public int InsertChildEmergencyContactJoin(EmergencyContact e, Student s)
		{
			try {
				Database db = new Database();
				SqlConnection cn = null;
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_ChildEmergencyContact", cn);
				int ReturnValue = -1;

				db.SetParameter(ref cm, "@intEmergencyContactID", e.UID, SqlDbType.BigInt);
				db.SetParameter(ref cm, "@intChildID", s.StudentID, SqlDbType.BigInt);


				db.SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				ReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				db.CloseDBConnection(ref cn);

				if (ReturnValue == -1) throw new Exception("Unable to join Student/Emergency Contact");

				return ReturnValue;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public long InsertPhysician(Physician p)
		{
			try {
				Database db = new Database();
				SqlConnection cn = null;
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_Physician", cn);
				long intUID = -1;

				db.SetParameter(ref cm, "@uid", p.UID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
				db.SetParameter(ref cm, "@strPhysicianName", p.PhysicianName, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@strAddress", p.PhysicianStreetAddress, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@strCity", p.PhysicianCity, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@intStateID", p.PhysicianState, SqlDbType.Int);
				db.SetParameter(ref cm, "@strPhoneNumber", p.PhysicianPhoneNumber, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@strAlternatePhone", p.PhysicianPhoneNumber, SqlDbType.NVarChar);

				//db.SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intUID = (long)cm.Parameters["@uid"].Value;
				db.CloseDBConnection(ref cn);

				if (intUID == -1) throw new Exception("Physician ID not created");

				return intUID;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public int InsertChildPhysicianJoin(Physician p, Student s)
		{
			try {
				Database db = new Database();
				SqlConnection cn = null;
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_ChildPhysician", cn);
				int ReturnValue = -1;

				db.SetParameter(ref cm, "@intPhysicianID", p.UID, SqlDbType.BigInt);
				db.SetParameter(ref cm, "@intChildID", s.StudentID, SqlDbType.BigInt);


				db.SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				ReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				db.CloseDBConnection(ref cn);

				if (ReturnValue == -1) throw new Exception("Unable to join Student/Physician");

				return ReturnValue;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public int InsertChildAllergyJoin(int i, Student s)
		{
			try {
				Database db = new Database();
				SqlConnection cn = null;
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_ChildAllergies", cn);
				int ReturnValue = -1;

				db.SetParameter(ref cm, "@intSevereAllergyID", i, SqlDbType.BigInt);
				db.SetParameter(ref cm, "@intChildID", s.StudentID, SqlDbType.BigInt);


				db.SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				ReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				db.CloseDBConnection(ref cn);

				if (ReturnValue == -1) throw new Exception("Unable to Join Allergy/Student");

				return ReturnValue;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}
	}
}