using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IHLA_Template.Models;

namespace IHLA_Template.Controllers
{
    public class StudentApplicationController : Controller
    {
		public ActionResult Index()
		{
			return View();
		}

        // GET: StudentApplication
        public ActionResult Application()
        {
			var states = States.GetStatesList();
			ViewData["States"] = states;
			ViewBag.Title = "Application";
			ViewBag.Message = "Application for employment:";

			return View();
		}

		[HttpPost]
		public ActionResult Application(FormCollection col)
		{
			try {
				int successFlag = -1;
				User u = new User();
				StudentApplication su = new StudentApplication();

				u = u.GetUserSession();

				// Get student info from form to model
				su.student.FirstName = col["student.FirstName"];
				su.student.LastName = col["student.LastName"];
				su.student.DateOfBirth = col["student.DateOfBirth"];
				su.student.ExpectedStartDate = col["student.ExpectedStartDate"].ToString();
				su.student.Address = col["student.Address"];
				su.student.City = col["student.City"];
				su.student.State = Int32.Parse(col["student.State"]); // col["StudentState"];
				su.student.ZipCode = col["student.ZipCode"];
				su.student.HomePhoneNumber = col["student.HomePhoneNumber"];

				su.student.HasAllergy = col["Allergy"];
				su.student.AllergyInterventionRequired = col["AllergyIntervention"];
				su.student.HasSpecialHealthCondition = col["HealthCondition"];
				su.student.HealthcareInterventionRequired = col["HealthConditionIntervention"];
				su.student.HasMedication = col["Medication"];
				su.student.MedicationInterventionRequired = col["MedicationIntervention"];
				su.student.HasDietaryRestriction = col["DietaryRestrictions"];
				su.student.DietaryInterventionRequired = col["DietaryRestrictionsIntervention"];
				if (col["student.AdditionalInformation"] != "") {
					su.student.AdditionalInformation = col["student.AdditionalInformation"] + "; ";
				}

				if (su.student.HasAllergy == "1") {
					su.student.AdditionalInformation += "ALLERGY: " + col["AllergyDescription"] + "; ";
				}
				if (su.student.HasSpecialHealthCondition == "1") {
					su.student.AdditionalInformation += "HEALTH CONDITION: " + col["HealthConditionDescription"] + "; ";
				}
				if (su.student.HasMedication == "1") {
					su.student.AdditionalInformation += "MEDICATION: " + col["MedicationDescription"] + "; ";
				}
				if (su.student.HasDietaryRestriction == "1") {
					su.student.AdditionalInformation += "DIETARY RESTRICTION: " + col["DietaryRestrictionsDescription"] + "; ";
				}

				if (su.student.AdditionalInformation == "") {
					su.student.AdditionalInformation = "none";
				}
				// Insert Student
				su.student.StudentID = su.InsertStudent(su);

				// Special Information

				if (su.student.HasAllergy == "1") {
					su.InsertChildAllergyJoin(1, su.student);
				}
				if (su.student.AllergyInterventionRequired == "1") {
					su.InsertChildAllergyJoin(2, su.student);
				}
				
				if (su.student.HasSpecialHealthCondition == "1") {
					su.InsertChildAllergyJoin(3, su.student);
				}
				if(su.student.HealthcareInterventionRequired == "1") {
					su.InsertChildAllergyJoin(4, su.student);
				}
				
				if (su.student.HasMedication == "1") {
					su.InsertChildAllergyJoin(5, su.student);
				}
				if (su.student.MedicationInterventionRequired == "1") {
					su.InsertChildAllergyJoin(6, su.student);
				}
				
				if (su.student.HasDietaryRestriction == "1") {
					su.InsertChildAllergyJoin(7, su.student);
				}
				if (su.student.DietaryInterventionRequired == "1") {
					su.InsertChildAllergyJoin(8, su.student);
				}


				// Get primary parent info 
				su.parent1.FirstName = col["parent1.FirstName"];
				su.parent1.LastName = col["parent1.LastName"];
				su.parent1.RelationshipToChild = col["parent1.RelationshipToChild"];
				su.parent1.Address = col["parent1.Address"];
				su.parent1.City = col["parent1.City"];
				su.parent1.State = Int32.Parse(col["parent1.State"]);
				su.parent1.ZipCode = col["parent1.ZipCode"];
				su.parent1.HomePhoneNumber = col["parent1.HomePhoneNumber"];
				su.parent1.MobilePhoneNumber = col["parent1.MobilePhoneNumber"];
				su.parent1.WorkPhoneNumber = col["parent1.WorkPhoneNumber"];
				su.parent1.WorkName = col["parent1.WorkName"];
				su.parent1.WorkAddress = col["parent1.WorkAddress"];
				su.parent1.WorkCity = col["parent1.WorkCity"];
				su.parent1.LoginInID = u.UID.ToString();
				su.parent1.Email = u.Email;
				// Insert Parent
				su.parent1.UID = su.InsertParent(su.parent1);
				//Update User ID parent flag
				u.IsParent = "Y";
				var at = u.Save();
				if (at != Models.User.ActionTypes.UpdateSuccessful) 
					{
						throw new Exception("Parent Flag not Updated");
					}

				// Insert StudentParent Record
				successFlag = su.InsertParentChildJoin(su.parent1, su.student);

				// Get Emergency contact info
				su.emergencyContact.FirstName = col["emergencyContact.FirstName"];
				su.emergencyContact.LastName = col["emergencyContact.LastName"];
				su.emergencyContact.RelationshipToChild = col[".emergencyContact.RelationshipToChild"];
				su.emergencyContact.City = col["emergencyContact.City"];
				su.emergencyContact.State = Int32.Parse(col["emergencyContact.State"]);
				su.emergencyContact.PhoneNumber = col["emergencyContact.PhoneNumber"];
				su.emergencyContact.OtherPhoneNumber = col["emergencyContact.OtherPhoneNumber"];
				// Insert Emergency Contact
				su.emergencyContact.UID = su.InsertEmergencyContact(su.emergencyContact);
				// Insert Student-Emergency Contact Join
				successFlag = su.InsertChildEmergencyContactJoin(su.emergencyContact, su.student);

				// Get physician info
				su.physician.PhysicianName = col["physician.PhysicianName"];
				su.physician.PhysicianStreetAddress = col["physician.PhysicianStreetAddress"];
				su.physician.PhysicianCity = col["physician.PhysicianCity"];
				su.physician.PhysicianState = Int32.Parse(col["physician.PhysicianState"]);
				su.physician.PhysicianPhoneNumber = col["physician.PhysicianPhoneNumber"];
				//Insert Physician
				su.physician.UID = su.InsertPhysician(su.physician);
				//Insert Student-Physician Join
				successFlag = su.InsertChildPhysicianJoin(su.physician, su.student);
				

				return RedirectToAction("Success");
			}
			catch (Exception ex) {
				return RedirectToAction("Error", "Home");
			}
		}

		public ActionResult Success()
		{
			return View();
		}

		public ActionResult SignUp()
		{
			Models.User u = new Models.User();
			return View(u);
		}

		[HttpPost]
		public ActionResult SignUp(FormCollection col)
		{
			try {
				Models.User u = new Models.User();

				u.FirstName = col["FirstName"];
				u.LastName = col["LastName"];
				u.Email = col["Email"];
				u.UserID = col["UserID"];
				u.Password = col["Password"];

				if (u.FirstName.Length == 0 || u.LastName.Length == 0 || u.Email.Length == 0 || u.UserID.Length == 0 || u.Password.Length == 0) {
					u.ActionType = Models.User.ActionTypes.RequiredFieldsMissing;
					ViewBag.Message = "Required fields missing. Please try again";
					return View(u);
				}
				else {
					if (col["btnSubmit"] == "signup") { //sign up button pressed
						Models.User.ActionTypes at = Models.User.ActionTypes.NoType;
						at = u.Save();
						switch (at) {
							case Models.User.ActionTypes.InsertSuccessful:
								u.SaveUserSession();
								return RedirectToAction("Application");
							case Models.User.ActionTypes.DuplicateEmail:
								ViewBag.Message = "Email already used. Please try again";
								return View();
							case Models.User.ActionTypes.DuplicateUserID:
								ViewBag.Message = "User ID already used. Please try again";
								return View();
							//break;
							default:
								return View(u);
								//break;
						}
					}
					else {
						return View(u);
					}
				}
			}
			catch (Exception) {
				Models.User u = new Models.User();
				return View(u);
			}
		}

		public ActionResult LogIn()
		{
			Models.User u = new Models.User();

			return View();
		}

		[HttpPost]
		public ActionResult LogIn(FormCollection col)
		{
			try {
				Models.User u = new Models.User();

				if (col["btnSubmit"] == "signin") {
					u.UserID = col["UserID"];
					u.Password = col["Password"];

					u = u.Login();
					if (u != null && u.UID > 0) {
						
							u.SaveUserSession();
							return RedirectToAction("Application");
						
					}
					else {
						u = new Models.User();
						u.UserID = col["UserID"];
						u.ActionType = Models.User.ActionTypes.LoginFailed;
						ViewBag.Message = "Invalid Username or Password. Please try again";
					}
				}

				return View(u);
			}
			catch (Exception) {
				Models.User u = new Models.User();
				return View(u);
			}
		}
	}
}
