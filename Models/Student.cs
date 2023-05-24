using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IHLA_Template.Models
{
	public class Student
	{
		public long StudentID = 0;
		public string FirstName = string.Empty;
		public string LastName = string.Empty;
		public string DateOfBirth = string.Empty;
		public string ExpectedStartDate = string.Empty;
		public string Address = string.Empty;
		public string City = string.Empty;
		public int State = 0;
		public string ZipCode = string.Empty;
		public string HomePhoneNumber = string.Empty;
		public string ApplicationDate = string.Empty;

		public string HasAllergy = string.Empty;
		public string AllergyInterventionRequired = string.Empty;
		public string HasSpecialHealthCondition = string.Empty;
		public string HealthcareInterventionRequired = string.Empty;
		public string HasMedication = string.Empty;
		public string MedicationInterventionRequired = string.Empty;
		public string HasDietaryRestriction = string.Empty;
		public string DietaryInterventionRequired = string.Empty;

		public string IsToiletTrained = string.Empty;

		public string AdditionalInformation = string.Empty;

		public static void EnrollStudent(Student s)
		{
			Database db = new Database();

			try {
				SqlConnection cn = null;
				if (!db.GetDBConnection(ref cn)) throw new Exception("Database did not connect");
				SqlCommand cm = new SqlCommand("UPDATE_Enrolled", cn);

				db.SetParameter(ref cm, "@intChildID", s.StudentID, SqlDbType.BigInt);

				cm.ExecuteReader();
				
				db.CloseDBConnection(ref cn);

			
			}
			catch (Exception ex) { throw new Exception(ex.Message); }
		}
	}
}