using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IHLA_Template.Models
{
	public class EmployeeApplications
	{
		public long UID = 0;

		public DateTime DateOfApplication { get; set; }
		public long LogInID { get; set; }

		//Employee info
		public int EmployeeID { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public int State { get; set; }
		public string Zip { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string EmergencyContact { get; set; }
		public string EmergecyNumber { get; set; }

		//eligiblies
		public char UsaEligible { get; set; }
		public char HealthcareExclusion { get; set; }
		public char Misdemeanor { get; set; }
		public char Felony { get; set; }
		public string CovictionExplained { get; set; }

		//shifts
		public char FullTime { get; set; }
		public char PartTime { get; set; }

		//etc info

		public string AgeCheck { get; set; }
		public string StartDate { get; set; }
		public string Pay { get; set; }

		//Employment History
		public EmploymentHistory em1 = new EmploymentHistory();
		public EmploymentHistory em2 = new EmploymentHistory();
		public EmploymentHistory em3 = new EmploymentHistory();

		//skills and certs
		public string Skills { get; set; }
		public string Certs { get; set; }

		//Questionare
		public string CareerGoals { get; set; }
		public string HireYou { get; set; }
		public string ProConduct { get; set; }
		public string CustService { get; set; }
		public string ToyFight { get; set; }

		public int InsertApplication()
		{

			int ApplicationID = 0;
			int CompletedCheck = 0;

			Models.Database db = new Database();

			SqlConnection cn = null;
			if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
			SqlCommand cmd = new SqlCommand("INSERT_Employee", cn);

			db.SetParameter(ref cmd, "@strFirstName", FirstName, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strLastName", LastName, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strAddress", Address, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strCity", City, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@intStateID", State, System.Data.SqlDbType.Int);
			db.SetParameter(ref cmd, "@strZip", Zip, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strEmail", Email, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strIsCitizen", UsaEligible, System.Data.SqlDbType.VarChar);
			db.SetParameter(ref cmd, "@intLoginID", LogInID, System.Data.SqlDbType.VarChar);
			db.SetParameter(ref cmd, "@strHasMisdemeanor", Misdemeanor, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strHasFelony", Felony, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strExplanation", CovictionExplained, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strFullTime", FullTime, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strPartTime", PartTime, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strIsOld", AgeCheck, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@dtmDesiredStartDate", StartDate, System.Data.SqlDbType.Date);
			db.SetParameter(ref cmd, "@decDesiredPay", Pay, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strPhoneNumber", PhoneNumber, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@dtmDateOfApplication", DateTime.Today, System.Data.SqlDbType.Date);
			db.SetParameter(ref cmd, "@strSkills", Skills, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strCertifications", Certs, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strEmergencyContactName", EmergencyContact, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strEmergencyPhoneNumber", EmergecyNumber, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@intPTO", 80, System.Data.SqlDbType.Int);
			db.SetParameter(ref cmd, "@strCareerGoals", CareerGoals, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strWhyHire", HireYou, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strProConduct", ProConduct, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strCustService", CustService, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strToyFight", ToyFight, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strProcessing", "PENDING", System.Data.SqlDbType.NVarChar);

			db.SetParameter(ref cmd, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);
			db.SetParameter(ref cmd, "@uid", EmployeeID, SqlDbType.BigInt, Direction: ParameterDirection.Output);

			cmd.ExecuteReader();

			EmployeeID = Convert.ToInt32((long)cmd.Parameters["@uid"].Value);
			ApplicationID = EmployeeID;

			if (ApplicationID > 0)
			{
				CompletedCheck = 1;
			}

			db.CloseDBConnection(ref cn);

			CompletedCheck += PastEmployees(ApplicationID, LogInID);

			return CompletedCheck;
		}

		public int PastEmployees(int ApplicationID, long loginID)
		{
			int CompletionCheck = 0;


			if (em1.Employer != null && em1.Employer != string.Empty)
			{
				CompletionCheck += em1.InsertEmploymentHistory(ApplicationID, loginID);
			}

			if (em2.Employer != null && em2.Employer != string.Empty)
			{
				CompletionCheck += em2.InsertEmploymentHistory(ApplicationID, loginID);
			}

			if (em3.Employer != null && em3.Employer != string.Empty)
			{
				CompletionCheck += em3.InsertEmploymentHistory(ApplicationID, loginID);
			}

			return CompletionCheck;

		}

		public bool CheckForApp()
        {
			bool Check = false;

			IHLA_Template.Models.User u = new IHLA_Template.Models.User();
			u = u.GetUserSession();

			try
			{

				Database db = new Database();

				SqlConnection cn = new SqlConnection();
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("Select * FROM TEmployees Where intLoginID = " + u.UID, cn);
				da.SelectCommand.CommandType = CommandType.Text;

				try
				{
					DataSet ds = new DataSet();
					if (da.Fill(ds) > 0)
					{
						Check = true;
					}
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally
				{
					db.CloseDBConnection(ref cn);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}


			return Check;
        }


	}
}