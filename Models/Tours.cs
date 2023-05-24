using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IHLA_Template.Models
{
	public class Tours
	{
		public long UID = 0;
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Date { get; set; }
		public string Time { get; set; }
		public string Email { get; set; }

	

		public int CheckDate()
		{
			Models.Database db = new Database();

			SqlConnection cn = null;
			if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
			
			SqlCommand cmd = new SqlCommand("vParentTours", cn);

			db.SetParameter(ref cmd, "@dtmDateOfTour", Date, System.Data.SqlDbType.Date);
			db.SetParameter(ref cmd, "@strTimeOfToure", Time, System.Data.SqlDbType.NVarChar);

			db.SetParameter(ref cmd, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

			cmd.ExecuteReader();

			db.CloseDBConnection(ref cn);

			return (int)cmd.Parameters["ReturnValue"].Value;
		}

		public int InsertTourDate()
		{
			Models.Database db = new Database();

			SqlConnection cn = null;
			if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
			SqlCommand cmd = new SqlCommand("INSERT_TOUR", cn);

			db.SetParameter(ref cmd, "@strVisitorFirstName", FirstName, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@strVisitorLastName", LastName, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@Email", Email, System.Data.SqlDbType.NVarChar);
			db.SetParameter(ref cmd, "@dtmdate", Date, System.Data.SqlDbType.Date);
			db.SetParameter(ref cmd, "@strTime", Time, System.Data.SqlDbType.NVarChar);

			db.SetParameter(ref cmd, "ReturnValue", 0, SqlDbType.Int, Direction: ParameterDirection.ReturnValue);

			cmd.ExecuteReader();

			db.CloseDBConnection(ref cn);

			return (int)cmd.Parameters["ReturnValue"].Value;
		}

		public List<Tours> GetUpcomingTours(DateTime today)
		{
			try {

				Database db = new Database();

				SqlConnection cn = new SqlConnection();
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter(("select * From TVisits WHERE dtmDate >=" + today) , cn);
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
	}
}