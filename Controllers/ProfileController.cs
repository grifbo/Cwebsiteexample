using IHLA_Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IHLA_Template.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
			Models.User u = new Models.User();

			u = u.GetUserSession();

            return View(u);
        }

		[HttpPost]
		public ActionResult Index(FormCollection col)
		{
			try {
				Models.User u = new Models.User();
				u = u.GetUserSession();

				u.FirstName = col["FirstName"];
				u.LastName = col["LastName"];
				u.Email = col["Email"];
				u.UserID = col["UserID"];
				u.Password = col["Password"];

				if (col["btnSubmit"] == "submit") { 
					u.Save();
					u.SaveUserSession();
					ViewBag.Message = "Update Successful";
					return View(u);
				}
				else {
					return View(u);
				}
			}
			catch(Exception ex) {

				Models.User u = new Models.User();
				return View(u);

			}
			
			
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
								return RedirectToAction("Index");
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
						if (u.IsAdmin == "Y") {
							u.SaveUserSession();
							return RedirectToAction("Index", "Admin");
						}
						else if (u.IsEmployee == "Y")
						{
							u.SaveUserSession();
							return RedirectToAction("EmployeeProfile", "EmployeeHomePage");
						}
						else {
							u.SaveUserSession();
							return RedirectToAction("Index");
						}
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

		public ActionResult LogOut()
		{
			Models.User u = new Models.User();
			u.RemoveUserSession();
			return RedirectToAction("Index", "Home");
		}

		public ActionResult ForgotPassword()
		{
			Models.User u = new Models.User();

			return View();
		}

		[HttpPost]
		public ActionResult ForgotPassword(FormCollection col)
		{
			try {
				Database db = new Database();
				Models.User u = new Models.User();

				if (col["btnSubmit"] == "signin") {
					u.UserID = col["UserID"];
					u.Email = col["Email"];
				}

				u = u.GetUserByUserNameAndEmail(u);

				if (u != null) {
					//get user info
					u = u.GetUserByUserNameAndEmail(u);
					//create temp pass
					u.Password = u.UserID.Substring(0, 2) + DateTime.Now.ToString("dd") + u.Email.Substring(0, 1).ToUpper() + DateTime.Now.ToString("ss");
					//update info
					u.ActionType = u.Save();
					//send email with temp pass
					Email.SendPaswordResetEmail(u.Email, u.Password);
					// direct to reset page
					return RedirectToAction("ResetPassword");
				}

				else {
					ViewBag.Message = "User name not found. Please try again or create a new profile.";
					return View();
				}
			}	
			catch (Exception ex) {
				return RedirectToAction("Index", "Index", "Home");
			}
		}

		public ActionResult ResetPassword()
		{
			Models.User u = new Models.User();

			//u = User.GetUserSession();

			return View();
		}
		

		[HttpPost]
		public ActionResult ResetPassword(FormCollection col)
		{
			try {
				Database db = new Database();
				Models.User u = new Models.User();
				//if submit btn is clicked..
				if (col["btnSubmit"] == "signin") {
					//use UserID and Temp Password for login
					u.UserID = col["UserID"];
					u.Password = col["TempPass"];
					u = u.Login();
					//if that is successful, check that the new password matches the confirm password
					if (u != null && u.UID > 0) {
						if (col["NewPass"] == col["ConfirmPass"]) {
							//if they do, update the password
							u.Password = col["NewPass"];
							u.ActionType = u.Save();
							return RedirectToAction("Index");
						}
						else {
							ViewBag.Message = "New password does not match confirmation password. Please try again.";
							return View();
						}
					}
					//If login is not successfull, return error
					else {
						ViewBag.Message = "Temporary password is not valid for username. Please try again.";
						return View();
					}
				}



				return View();
			}
				
			catch (Exception ex) {
				return RedirectToAction("Index", "Index", "Home");
			}
		}


		public ActionResult Payments()
		{
			Payments p = new Payments();
			List<Payments> pl = new List<Payments>();

			User u = new User();
			u = u.GetUserSession();

			if (u.UID > 0) {
				Parent par = new Parent();
				par = par.GetParentByUserID(u.UID);

				pl = p.GetAllPaymentsForParent(par.UID);
				return View(pl);
			}
			else {
				return View();
			}
		}
	}
}