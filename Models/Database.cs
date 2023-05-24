using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace IHLA_Template.Models
{
	public class Database
	{

		public bool GetDBConnection(ref SqlConnection SQLConn)
		{
			try {
				if (SQLConn == null) SQLConn = new SqlConnection();
				if (SQLConn.State != ConnectionState.Open) {
					SQLConn.ConnectionString = ConfigurationManager.AppSettings["AppDBConnect"];
					SQLConn.Open();
				}
				return true;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public bool CloseDBConnection(ref SqlConnection SQLConn)
		{
			try {
				if (SQLConn.State != ConnectionState.Closed) {
					SQLConn.Close();
					SQLConn.Dispose();
					SQLConn = null;
				}
				return true;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public User.ActionTypes InsertUser(User u)
		{
			try {
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_USER", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@uid", u.UID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
				SetParameter(ref cm, "@strUserName", u.UserID, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strPassword", u.Password, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strFirstName", u.FirstName, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strLastName", u.LastName, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strEmail", u.Email, SqlDbType.NVarChar);

				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				switch (intReturnValue) {
					case 1: // new user created
						u.UID = (long)cm.Parameters["@uid"].Value;
						return User.ActionTypes.InsertSuccessful;
					case -1:
						return User.ActionTypes.DuplicateEmail;
					case -2:
						return User.ActionTypes.DuplicateUserID;
					default:
						return User.ActionTypes.Unknown;
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public User Login(User u)
		{
			try {
				SqlConnection cn = new SqlConnection();
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("LOGIN", cn);
				DataSet ds;
				User newUser = null;

				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				SetParameter(ref da, "@strUserName", u.UserID, SqlDbType.NVarChar);
				SetParameter(ref da, "@strPassword", u.Password, SqlDbType.NVarChar);

				try {
					ds = new DataSet();
					da.Fill(ds);
					if (ds.Tables[0].Rows.Count > 0) {
						newUser = new User();
						DataRow dr = ds.Tables[0].Rows[0];
						newUser.UID = (int)dr["intLoginId"];
						newUser.UserID = u.UserID;
						newUser.Password = u.Password;
						newUser.FirstName = (string)dr["strFirstName"];
						newUser.LastName = (string)dr["strLastName"];
						newUser.Email = (string)dr["strEmail"];
						newUser.IsAdmin = (string)dr["strIsAdmin"];
						newUser.IsEmployee = (string)dr["strIsEmployee"];
						newUser.IsParent = (string)dr["strIsParent"];
					}
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally {
					CloseDBConnection(ref cn);
				}
				return newUser; //alls well in the world
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public User.ActionTypes UpdateUser(User u)
		{
			try {
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("UPDATE_USER", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@uid", u.UID, SqlDbType.BigInt);
				SetParameter(ref cm, "@strUserName", u.UserID, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strPassword", u.Password, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strFirstName", u.FirstName, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strLastName", u.LastName, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strEmail", u.Email, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strIsAdmin", u.IsAdmin, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strIsParent", u.IsParent, SqlDbType.NVarChar);
				SetParameter(ref cm, "@strIsEmployee", u.IsEmployee, SqlDbType.NVarChar);

				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				switch (intReturnValue) {
					case 1: //new updated
						return User.ActionTypes.UpdateSuccessful;
					default:
						return User.ActionTypes.Unknown;
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public int SetParameter(ref SqlCommand cm, string ParameterName, Object Value
			, SqlDbType ParameterType, int FieldSize = -1
			, ParameterDirection Direction = ParameterDirection.Input
			, Byte Precision = 0, Byte Scale = 0)
		{
			try {
				cm.CommandType = CommandType.StoredProcedure;
				if (FieldSize == -1)
					cm.Parameters.Add(ParameterName, ParameterType);
				else
					cm.Parameters.Add(ParameterName, ParameterType, FieldSize);

				if (Precision > 0) cm.Parameters[cm.Parameters.Count - 1].Precision = Precision;
				if (Scale > 0) cm.Parameters[cm.Parameters.Count - 1].Scale = Scale;

				cm.Parameters[cm.Parameters.Count - 1].Value = Value;
				cm.Parameters[cm.Parameters.Count - 1].Direction = Direction;

				return 0;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public int SetParameter(ref SqlDataAdapter cm, string ParameterName, Object Value
			, SqlDbType ParameterType, int FieldSize = -1
			, ParameterDirection Direction = ParameterDirection.Input
			, Byte Precision = 0, Byte Scale = 0)
		{
			try {
				cm.SelectCommand.CommandType = CommandType.StoredProcedure;
				if (FieldSize == -1)
					cm.SelectCommand.Parameters.Add(ParameterName, ParameterType);
				else
					cm.SelectCommand.Parameters.Add(ParameterName, ParameterType, FieldSize);

				if (Precision > 0) cm.SelectCommand.Parameters[cm.SelectCommand.Parameters.Count - 1].Precision = Precision;
				if (Scale > 0) cm.SelectCommand.Parameters[cm.SelectCommand.Parameters.Count - 1].Scale = Scale;

				cm.SelectCommand.Parameters[cm.SelectCommand.Parameters.Count - 1].Value = Value;
				cm.SelectCommand.Parameters[cm.SelectCommand.Parameters.Count - 1].Direction = Direction;

				return 0;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public User GetUserByUserIDandEmail(User u)
		{
			try {
				SqlConnection cn = new SqlConnection();
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("GET_USER_BY_ID_AND_EMAIL", cn);
				DataSet ds;
				User newUser = null;

				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				SetParameter(ref da, "@user_id", u.UserID, SqlDbType.NVarChar);
				SetParameter(ref da, "@email", u.Password, SqlDbType.NVarChar);

				try {
					ds = new DataSet();
					da.Fill(ds);
					if (ds.Tables[0].Rows.Count > 0) {
						newUser = new User();
						DataRow dr = ds.Tables[0].Rows[0];
						newUser.UID = (long)dr["UID"];
						newUser.UserID = u.UserID;
						newUser.Password = u.Password;
						newUser.FirstName = (string)dr["FirstName"];
						newUser.LastName = (string)dr["LastName"];
						newUser.Email = (string)dr["Email"];
					}
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally {
					CloseDBConnection(ref cn);
				}
				return newUser; //alls well in the world
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}
		public User.ActionTypes UpdatePassword(User u)
		{
			try {
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("UPDATE_PASSWORD", cn);
				int intReturnValue = -1;

				SetParameter(ref cm, "@uid", u.UID, SqlDbType.BigInt);
				SetParameter(ref cm, "@user_id", u.UserID, SqlDbType.NVarChar);
				SetParameter(ref cm, "@password", u.Password, SqlDbType.NVarChar);

				SetParameter(ref cm, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				switch (intReturnValue) {
					case 1: //new updated
						return User.ActionTypes.UpdateSuccessful;
					default:
						return User.ActionTypes.Unknown;
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}
	
		public User.ActionTypes EnrollChildern(EmployeeChildAssignment eca)
        {
			try
			{
				SqlConnection cn = null;
				if (!GetDBConnection(ref cn)) throw new Exception("Database did not connect");

				int isEnrolled = Convert.ToInt32(new SqlCommand("SELECT COUNT(intEmployeeID) FROM TEmployeeChildAssignment WHERE intChildID="+eca.ChildID,cn).ExecuteScalar());
				int intReturnValue = -1;
				SqlCommand cm;
				if (isEnrolled > 0)
                {//Update if exists
					cm = new SqlCommand("UPDATE_Enrolled", cn);
					return User.ActionTypes.UpdateSuccessful;
				}
				else
                {//Insert if Not
					cm= new SqlCommand("INSERT_TEmployeeChildAssignment", cn);
					SetParameter(ref cm, "@uid", eca.EmployeeChildAssignmentID, SqlDbType.BigInt);
					SetParameter(ref cm, "@dtmDateOfAssignment", eca.DateOfAssignment, SqlDbType.DateTime);
					SetParameter(ref cm, "@strNotes", eca.Notes, SqlDbType.NVarChar);
					SetParameter(ref cm, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

				}

				SetParameter(ref cm, "@intEmployeeID", eca.EmployeeID, SqlDbType.BigInt);
				SetParameter(ref cm, "@intChildID", eca.ChildID, SqlDbType.BigInt);
				

				cm.ExecuteReader();

				intReturnValue = (int)cm.Parameters["ReturnValue"].Value;
				CloseDBConnection(ref cn);

				switch (intReturnValue)
				{
					case 1: //new updated
						return User.ActionTypes.InsertSuccessful;
					default:
						return User.ActionTypes.Unknown;
				}
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
        }
	}
}