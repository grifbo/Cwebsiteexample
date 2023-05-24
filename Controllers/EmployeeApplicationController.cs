using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IHLA_Template.Controllers
{
	public class EmployeeApplicationController : Controller
	{
		// GET: EmployeeApplication
		public ActionResult EmployeeApplications()
		{
			var items = new List<SelectListItem> {
			new SelectListItem() {Text = "Alabama", Value = "1"},
			new SelectListItem() {Text = "Alaska", Value = "2"},
			new SelectListItem() {Text = "Arizona", Value = "3"},
			new SelectListItem() {Text = "Arkansas", Value = "4"},
			new SelectListItem() {Text = "California", Value = "5"},
			new SelectListItem() {Text = "Colorado", Value = "6"},
			new SelectListItem() {Text = "Connecticut", Value = "7"},
			new SelectListItem() {Text = "Delaware", Value = "8"},
			new SelectListItem() {Text = "Florida", Value = "9"},
			new SelectListItem() {Text = "Georgia", Value = "10"},
			new SelectListItem() {Text = "Hawaii", Value = "11"},
			new SelectListItem() {Text = "Idaho", Value = "12"},
			new SelectListItem() {Text = "Illinois", Value = "13"},
			new SelectListItem() {Text = "Indiana", Value = "14"},
			new SelectListItem() {Text = "Iowa", Value = "15"},
			new SelectListItem() {Text = "Kansas", Value = "16"},
			new SelectListItem() {Text = "Kentucky", Value = "17"},
			new SelectListItem() {Text = "Louisiana", Value = "18"},
			new SelectListItem() {Text = "Maine", Value = "19"},
			new SelectListItem() {Text = "Maryland", Value = "20"},
			new SelectListItem() {Text = "Massachusetts", Value = "21"},
			new SelectListItem() {Text = "Michigan", Value = "22"},
			new SelectListItem() {Text = "Minnesota", Value = "23"},
			new SelectListItem() {Text = "Mississippi", Value = "24"},
			new SelectListItem() {Text = "Missouri", Value = "25"},
			new SelectListItem() {Text = "Montana", Value = "26"},
			new SelectListItem() {Text = "Nebraska", Value = "27"},
			new SelectListItem() {Text = "Nevada", Value = "28"},
			new SelectListItem() {Text = "New Hampshire", Value = "29"},
			new SelectListItem() {Text = "New Jersey", Value = "30"},
			new SelectListItem() {Text = "New Mexico", Value = "31"},
			new SelectListItem() {Text = "New York", Value = "32"},
			new SelectListItem() {Text = "North Carolina", Value = "33"},
			new SelectListItem() {Text = "North Dakota", Value = "34"},
			new SelectListItem() {Text = "Ohio", Value = "35"},
			new SelectListItem() {Text = "Oklahoma", Value = "36"},
			new SelectListItem() {Text = "Oregon", Value = "37"},
			new SelectListItem() {Text = "Pennsylvania", Value = "38"},
			new SelectListItem() {Text = "Rhode Island", Value = "39"},
			new SelectListItem() {Text = "South Carolina", Value = "40"},
			new SelectListItem() {Text = "South Dakota", Value = "41"},
			new SelectListItem() {Text = "Tennessee", Value = "42"},
			new SelectListItem() {Text = "Texas", Value = "43"},
			new SelectListItem() {Text = "Utah", Value = "44"},
			new SelectListItem() {Text = "Vermont", Value = "45"},
			new SelectListItem() {Text = "Virginia", Value = "46"},
			new SelectListItem() {Text = "Washington", Value = "47"},
			new SelectListItem() {Text = "West Virginia", Value = "48"},
			new SelectListItem() {Text = "Wisconsin", Value = "49"},
			new SelectListItem() {Text = "Wyoming", Value = "50"}};

			Models.EmployeeApplications Check = new Models.EmployeeApplications();

			ViewData["States"] = items;
			ViewBag.Title = "Application";
			ViewBag.Message = "Application for employment:";
			ViewBag.IDCheck = Check.CheckForApp();


			return View();
		}

		[HttpPost]
		public ActionResult EmployeeApplications(FormCollection col)
		{
			int CompletionCheck = 0;

			try
			{		
				Models.EmployeeApplications EA = new Models.EmployeeApplications();

				Models.User u = new Models.User();

				u = u.GetUserSession();

				EA.LogInID = u.UID;

				//General Infomation
				EA.FirstName = col["FirstName"];
				EA.LastName = col["LastName"];
				EA.Address = col["Address"];
				EA.City = col["City"];
				EA.State = Int32.Parse(col["State"]);
				EA.Zip = col["Zip"];
				EA.PhoneNumber = col["PhoneNumber"];
				EA.EmergencyContact = col["EmergencyContact"];
				EA.EmergecyNumber = col["EmergecyNumber"];
				EA.Email = col["Email"];

				//eligiblies
				EA.UsaEligible = char.Parse(col["UsaEligible"]);
				EA.Misdemeanor = char.Parse(col["Misdemeanor"]);
				EA.Felony = char.Parse(col["Felony"]);
				EA.CovictionExplained = col["CovictionExplained"];

				//shifts
				EA.FullTime = char.Parse(col["FullTime"]);
				EA.PartTime = char.Parse(col["PartTime"]);

				//etc
				EA.AgeCheck = col["AgeCheck"];
				EA.StartDate = col["StartDate"];
				EA.Pay = col["Pay"];

				//employer 1
				EA.em1.Employer = col["em1.Employer"];
				EA.em1.JobTitle = col["em1.JobTitle"];
				EA.em1.DOEStart = col["em1.DOEStart"];
				EA.em1.DOEEnd = col["em1.DOEEnd"];
				EA.em1.Address = col["em1.Address"];
				EA.em1.City = col["em1.City"];
				EA.em1.State = Int32.Parse(col["em1.State"]);
				EA.em1.Zip = col["em1.Zip"];
				EA.em1.Supervisor = col["em1.Supervisor"];
				EA.em1.ReasonLeaving = col["em1.ReasonLeaving"];
				EA.em1.Responsibilites = col["em1.Responsibilites"];

				//employer 2
				EA.em2.Employer = col["em2.Employer"];
				EA.em2.JobTitle = col["em2.JobTitle"];
				EA.em2.DOEStart = col["em2.DOEStart"];
				EA.em2.DOEEnd = col["em2.DOEEnd"];
				EA.em2.Address = col["em2.Address"];
				EA.em2.City = col["em2.City"];
				EA.em2.State = Int32.Parse(col["em2.State"]);
				EA.em2.Zip = col["em2.Zip"];
				EA.em2.Supervisor = col["em2.Supervisor"];
				EA.em2.ReasonLeaving = col["em2.ReasonLeaving"];
				EA.em2.Responsibilites = col["em2.Responsibilites"];

				//employer 3
				EA.em3.Employer = col["em3.Employer"];
				EA.em3.JobTitle = col["em3.JobTitle"];
				EA.em3.DOEStart = col["em3.DOEStart"];
				EA.em3.DOEEnd = col["em3.DOEEnd"];
				EA.em3.Address = col["em3.Address"];
				EA.em3.City = col["em3.City"];
				EA.em3.State = Int32.Parse(col["em3.State"]);
				EA.em3.Zip = col["em3.Zip"];
				EA.em3.Supervisor = col["em3.Supervisor"];
				EA.em3.ReasonLeaving = col["em3.ReasonLeaving"];
				EA.em3.Responsibilites = col["em3.Responsibilites"];

				//qualifilacations 
				EA.Skills = col["Skills"];
				EA.Certs = col["Certs"];

				//questionare
				EA.CareerGoals = col["CareerGoals"];
				EA.HireYou = col["HireYou"];
				EA.ProConduct = col["ProConduct"];
				EA.CustService = col["CustService"];
				EA.ToyFight = col["ToyFight"];

				CompletionCheck = EA.InsertApplication();				
				
				return RedirectToAction("Success");

			}
			catch (Exception ex)
			{
				return View(ex.Message);
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
			try
			{
				Models.User u = new Models.User();

				u.FirstName = col["FirstName"];
				u.LastName = col["LastName"];
				u.Email = col["Email"];
				u.UserID = col["UserID"];
				u.Password = col["Password"];

				if (u.FirstName.Length == 0 || u.LastName.Length == 0 || u.Email.Length == 0 || u.UserID.Length == 0 || u.Password.Length == 0)
				{
					u.ActionType = Models.User.ActionTypes.RequiredFieldsMissing;
					ViewBag.Message = "Required fields missing. Please try again";
					return View(u);
				}
				else
				{
					if (col["btnSubmit"] == "signup")
					{ //sign up button pressed
						Models.User.ActionTypes at = Models.User.ActionTypes.NoType;
						at = u.Save();
						switch (at)
						{
							case Models.User.ActionTypes.InsertSuccessful:
								u.SaveUserSession();
								return RedirectToAction("EmployeeApplications");
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
					else
					{
						return View(u);
					}
				}
			}
			catch (Exception)
			{
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
			try
			{
				Models.User u = new Models.User();

				if (col["btnSubmit"] == "signin")
				{
					u.UserID = col["UserID"];
					u.Password = col["Password"];

					u = u.Login();
					if (u != null && u.UID > 0)
					{
						if (u.IsAdmin == "Y")
						{
							u.SaveUserSession();
							return RedirectToAction("Index", "Admin");
						}
						else if (u.IsEmployee == "Y")
						{
							u.SaveUserSession();
							return RedirectToAction("EmployeeProfile", "EmployeeHomePage");
						}
						else
						{
							u.SaveUserSession();
							return RedirectToAction("EmployeeApplications");
						}
					}
					else
					{
						u = new Models.User();
						u.UserID = col["UserID"];
						u.ActionType = Models.User.ActionTypes.LoginFailed;
						ViewBag.Message = "Invalid Username or Password. Please try again";
					}
				}

				return View(u);
			}
			catch (Exception)
			{
				Models.User u = new Models.User();
				return View(u);
			}
		}
	}
}