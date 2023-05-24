using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IHLA_Template.Models
{
    public class Events
    {
		public long EventID { get; set; }
        public DateTime EventDate { set; get; }
        public String Event { set; get; }
		public String Description { set; get; }
		public String Time { set; get; }

		public List<Events> GetEvents()
        {
            try
            {

                Database db = new Database();

                SqlConnection cn = new SqlConnection();
                if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
                SqlDataAdapter da = new SqlDataAdapter("select * From TEvents", cn);
                List<Events> EventsList = new List<Events>();

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
                            Events newEvent = new Events();
                            DataRow dr = ds.Tables[0].Rows[i];
                            newEvent.EventDate = (DateTime)dr["dtmDateOfEvent"];
                            newEvent.Event = (string)dr["strEvent"];


                            EventsList.Add(newEvent);
                        }
                    }
                }
                catch (Exception ex) { throw new Exception(ex.Message); }
                finally
                {
                    db.CloseDBConnection(ref cn);
                }
                return EventsList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

		public List<Events> GetUpcomingEvents(string strToday)
		{
			try {

				Database db = new Database();

				SqlConnection cn = new SqlConnection();
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("select * From TEvents where dtmDateOFEvent >= '" + strToday + "'", cn);
				List<Events> EventsList = new List<Events>();

				da.SelectCommand.CommandType = CommandType.Text;

				try {
					DataSet ds = new DataSet();
					da.Fill(ds);

					int rowCount = ds.Tables[0].Rows.Count;

					if (rowCount > 0) {

						for (int i = 0; i < rowCount; i++) {
							Events newEvent = new Events();
							DataRow dr = ds.Tables[0].Rows[i];
							newEvent.EventDate = (DateTime)dr["dtmDateOfEvent"];
							newEvent.Event = (string)dr["strEvent"];
							newEvent.Description = (string)dr["strDescription"];
							newEvent.Time = (string)dr["strTime"];


							EventsList.Add(newEvent);
						}
					}
				}
				catch (Exception ex) { throw new Exception(ex.Message); }
				finally {
					db.CloseDBConnection(ref cn);
				}
				return EventsList;
			}
			catch (Exception ex) {
				throw new Exception(ex.Message);
			}
		}
	}
}