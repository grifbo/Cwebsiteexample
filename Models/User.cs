using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IHLA_Template.Models
{
	public class User
	{
		public long UID = 0;
		public string FirstName = string.Empty;
		public string LastName = string.Empty;
		public string UserID = string.Empty;
		public string Password = string.Empty;
		public string Email = string.Empty;
		public string IsParent = "N";
		public string IsAdmin = "N";
		public string IsEmployee = "N";
		public List<Payments> PaymentList = null;

		public ActionTypes ActionType = ActionTypes.NoType;

		public bool IsAuthenticated
		{
			get {
				if (UID > 0) return true;
				return false;
			}
		}

		public enum ActionTypes
		{
			NoType = 0,
			InsertSuccessful = 1,
			UpdateSuccessful = 2,
			DuplicateEmail = 3,
			DuplicateUserID = 4,
			Unknown = 5,
			RequiredFieldsMissing = 6,
			LoginFailed = 7
		}

		public User Login()
		{
			try {
				Database db = new Database();
				return db.Login(this);
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public User.ActionTypes Save()
		{
			try {
				Database db = new Database();
				if (UID == 0) { //insert new user
					this.ActionType = db.InsertUser(this);
				}
				else {
					this.ActionType = db.UpdateUser(this);
				}
				return this.ActionType;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public bool RemoveUserSession()
		{
			try {
				HttpContext.Current.Session["CurrentUser"] = null;
				return true;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public User GetUserSession()
		{
			try {
				User u = new User();
				if (HttpContext.Current.Session["CurrentUser"] == null) {
					return u;
				}
				u = (User)HttpContext.Current.Session["CurrentUser"];
				return u;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public bool SaveUserSession()
		{
			try {
				HttpContext.Current.Session["CurrentUser"] = this;
				return true;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}

		public User GetUserByUserNameAndEmail(User u)
		{
			Database db = new Database();
			try {
				SqlConnection cn = new SqlConnection();
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("GET_BY_USERNAME_EMAIL", cn);
				DataSet ds;
				User newUser = null;

				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				db.SetParameter(ref da, "@strUserName", u.UserID, SqlDbType.NVarChar);
				db.SetParameter(ref da, "@strEmail", u.Email, SqlDbType.NVarChar);

				try {
					ds = new DataSet();
					da.Fill(ds);
					if (ds.Tables[0].Rows.Count > 0) {
						newUser = new User();
						DataRow dr = ds.Tables[0].Rows[0];
						newUser.UID = (int)dr["intLoginID"];
						newUser.UserID = u.UserID;
						newUser.Password = (string)dr["strPassword"];
						newUser.FirstName = (string)dr["strFirstName"];
						newUser.LastName = (string)dr["strLastName"];
						newUser.Email = u.Email;
					}
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally {
					db.CloseDBConnection(ref cn);
				}
				return newUser;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}
	}
}