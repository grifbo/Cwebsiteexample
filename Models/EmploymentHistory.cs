using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IHLA_Template.Models
{
    public class EmploymentHistory
    {
		public int EmployerID { get; set; }
		public string Employer { get; set; }
		public string DOEStart { get; set; }
		public string DOEEnd { get; set; }
		public string JobTitle { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public int State { get; set; }
		public string Zip { get; set; }
		public string Supervisor { get; set; }
		public string ReasonLeaving { get; set; }
		public string Responsibilites { get; set; }

		public int InsertEmploymentHistory(int AppID, long loginID)
        {
			int HistoryID,CompletionCheck  = 0;

			Models.Database db = new Database();

			SqlConnection cn = null;
			if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
			SqlCommand cmd = new SqlCommand("INSERT_PastEmployer", cn);

			db.SetParameter(ref cmd, "@strEmployer", Employer, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strJobTitle", JobTitle, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@dtmStartDate", DOEStart, System.Data.SqlDbType.Date);
			db.SetParameter(ref cmd, "@dtmEndDate", DOEEnd, System.Data.SqlDbType.Date);
			db.SetParameter(ref cmd, "@strAddress", Address, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strCity", City, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@intStateID", State, System.Data.SqlDbType.Int);
			db.SetParameter(ref cmd, "@intLoginID", loginID, System.Data.SqlDbType.Int);
			db.SetParameter(ref cmd, "@strZip", Zip, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strSupervisor", Supervisor, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strReasonForLeaving", ReasonLeaving, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strResponsibilities", Responsibilites, System.Data.SqlDbType.NVarChar);

			db.SetParameter(ref cmd, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);
			db.SetParameter(ref cmd, "@uid", EmployerID, SqlDbType.BigInt, Direction: ParameterDirection.Output);

			
			cmd.ExecuteReader();

			EmployerID = Convert.ToInt32((long)cmd.Parameters["@uid"].Value);
			HistoryID = EmployerID;

			db.CloseDBConnection(ref cn);

			CompletionCheck = InsertEmployeeEmploymentHistory(AppID, HistoryID);

			return CompletionCheck;
		}

		public int InsertEmployeeEmploymentHistory(int AppID,int HistoryID)
		{
			Models.Database db = new Database();

			SqlConnection cn = null;
			if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");

			SqlCommand cmd = new SqlCommand("INSERT_EmployeePastEmployers", cn);

			db.SetParameter(ref cmd, "@intEmployeeID", AppID, System.Data.SqlDbType.Int);
			db.SetParameter(ref cmd, "@intPastEmployerID", HistoryID, System.Data.SqlDbType.Int);

			db.SetParameter(ref cmd, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

			cmd.ExecuteReader();

			db.CloseDBConnection(ref cn);

			return (int)cmd.Parameters["ReturnValue"].Value;
		}
	}
}