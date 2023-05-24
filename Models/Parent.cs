using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IHLA_Template.Models
{
	public class Parent
	{
		public long UID = 0;
		public string FirstName = string.Empty;
		public string LastName = string.Empty;
		public string RelationshipToChild = string.Empty;
		public string Address = string.Empty;
		public string City = string.Empty;
		public int State = 0;
		public string ZipCode = string.Empty;
		public string HomePhoneNumber = string.Empty;
		public string MobilePhoneNumber = string.Empty;
		public string Email = string.Empty;
		public string WorkPhoneNumber = string.Empty;
		public string WorkName = string.Empty;
		public string WorkAddress = string.Empty;
		public string WorkCity = string.Empty;
		public string PlaceOfContact = string.Empty;
		public string SignUpDate = string.Empty;
		public string LoginInID = string.Empty;

		public List<SelectListItem> GetAllParentsItemList()
		{
			try {

				Database db = new Database();

				SqlConnection cn = new SqlConnection();
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("vParentsList", cn);
				List<SelectListItem> paymentList = new List<SelectListItem>();

				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				//db.SetParameter(ref da, "@user_id", ParentID, SqlDbType.NVarChar);

				try {
					DataSet ds = new DataSet();
					da.Fill(ds);

					int rowCount = ds.Tables[0].Rows.Count;

					if (rowCount > 0) {

						for (int i = 0; i < rowCount; i++) {
							Parent newParent = new Parent();
							DataRow dr = ds.Tables[0].Rows[i];
							newParent.UID = (int)dr["intParentID"];
							newParent.FirstName = (string)dr["strFirstName"];
							newParent.LastName = (string)dr["strLastName"];
							newParent.Address = (string)dr["strAddress"];

							newParent.City = (string)dr["strCity"];
							newParent.State = (int)dr["intStateID"];
							newParent.ZipCode = (string)dr["strZip"];
							newParent.Email = (string)dr["strEmail"];
							newParent.HomePhoneNumber = (string)dr["strPhoneNumber"];
							newParent.SignUpDate = ((DateTime)dr["dtmSignUpDate"]).ToString();
							newParent.LoginInID = ((int)dr["intLoginID"]).ToString();


							
							paymentList.Add( new SelectListItem() { Text = newParent.FirstName + " " + newParent.LastName, Value = newParent.UID.ToString() });
						}
					}
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally {
					db.CloseDBConnection(ref cn);
				}
				return paymentList;
			}
			catch (Exception ex) {
				throw new Exception(ex.Message);
			}
		}

		public Parent GetParentByUserID(long UserID)
		{
			try {

				Database db = new Database();
				Parent newParent = new Parent();

				SqlConnection cn = new SqlConnection();
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("vParentsSearchByUserID", cn);

				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				db.SetParameter(ref da, "@intLoginID", UserID, SqlDbType.NVarChar);

				try {
					DataSet ds = new DataSet();
					da.Fill(ds);

					int rowCount = ds.Tables[0].Rows.Count;

					if (rowCount > 0) {
							DataRow dr = ds.Tables[0].Rows[0];
							newParent.UID = (int)dr["intParentID"];
							newParent.FirstName = (string)dr["strFirstName"];
							newParent.LastName = (string)dr["strLastName"];
							newParent.Address = (string)dr["strAddress"];

							newParent.City = (string)dr["strCity"];
							newParent.State = (int)dr["intStateID"];
							newParent.ZipCode = (string)dr["strZip"];
							newParent.Email = (string)dr["strEmail"];
							newParent.HomePhoneNumber = (string)dr["strPhoneNumber"];
							newParent.SignUpDate = ((DateTime)dr["dtmSignUpDate"]).ToString();
							newParent.LoginInID = ((int)dr["intLoginID"]).ToString();
	
					}
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
	}
}