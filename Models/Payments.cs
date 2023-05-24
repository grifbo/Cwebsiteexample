using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IHLA_Template.Models
{
	public class Payments
	{
		public long PaymentID = 0;
		public long ParentID = 0;
		public decimal Credit = 0.00m;
		public decimal Charge = 0.00m;
		public decimal Total = 0.00m;
		public DateTime Date;


		public List<Payments> GetAllPaymentsForParent(long ParentID)
		{
			try {

				Database db = new Database();

				SqlConnection cn = new SqlConnection();
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlDataAdapter da = new SqlDataAdapter("vParentsBilling", cn);
				List<Payments> paymentList = new List<Payments>();

				da.SelectCommand.CommandType = CommandType.StoredProcedure;

				db.SetParameter(ref da, "@intParentID", ParentID, SqlDbType.NVarChar);

				try {
					DataSet ds = new DataSet();
					da.Fill(ds);

					int rowCount = ds.Tables[0].Rows.Count;

					if (rowCount > 0) {

						for (int i = 0; i < rowCount; i++) {
							Payments newPayment = new Payments();
							DataRow dr = ds.Tables[0].Rows[i];
							newPayment.ParentID = ParentID;
							newPayment.Credit = (decimal)dr["decCredit"];
							newPayment.Charge = (decimal)dr["decCharge"];
							newPayment.Total = (decimal)dr["decTotal"];
							newPayment.Date = (DateTime)dr["dtmDate"];

							paymentList.Add(newPayment);
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

		public long InsertPayment(Payments p)
		{
			try {
				Database db = new Database();
				SqlConnection cn = null;
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("INSERT_Billing", cn);
				long intUID = -1;

				db.SetParameter(ref cm, "@uid", p.PaymentID, SqlDbType.BigInt, Direction: ParameterDirection.Output);
				db.SetParameter(ref cm, "@intParentID", p.ParentID, SqlDbType.NVarChar);
				db.SetParameter(ref cm, "@decCharge", p.Charge, SqlDbType.Decimal);
				db.SetParameter(ref cm, "@decCredit", p.Credit, SqlDbType.Decimal);
				db.SetParameter(ref cm, "@dtmDate", DateTime.Now, SqlDbType.DateTime);

				//db.SetParameter(ref cm, "ReturnValue", 0, SqlDbType.TinyInt, Direction: ParameterDirection.ReturnValue);

				cm.ExecuteReader();

				intUID = (long)cm.Parameters["@uid"].Value;
				db.CloseDBConnection(ref cn);

				if (intUID == -1) throw new Exception("Payment ID not created");

				return intUID;
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}
	}
	 
}