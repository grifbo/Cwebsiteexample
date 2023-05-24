using IHLA_Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IHLA_Template.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {

			AdminModel am = new AdminModel();
			am.AdminUser = am.AdminUser.GetUserSession();

			DateTime dtmToday = DateTime.Now;
			string strToday = dtmToday.ToString("yyyy") + "-" + dtmToday.ToString("MM") + "-" + dtmToday.ToString("dd");

			am.UpcomingTours = am.GetUpcomingTours(strToday);

			am.PendingStudentList = am.GetPendingStudentList();
			am.PendingEmployeeList = am.GetPendingEmployeeList();

			if (am.AdminUser.UID > 0) {
				return View(am);
			}
			else {
				return RedirectToAction("Login", "Profile");
			}
        }

		public ActionResult Payments()
		{
			Parent p = new Parent();
			var parents = p.GetAllParentsItemList();
			ViewData["Parents"] = parents;

			return View();
		}

		[HttpPost]
		public ActionResult Payments(FormCollection col)
		{
			try {
				Payments p = new Payments();

				p.ParentID = Int32.Parse(col["ParentID"]);
				p.Date = DateTime.Now;
				if (col["Charge"] != "") {
					p.Charge = Convert.ToDecimal(col["Charge"]);
				}
				if (col["Credit"] != "") {
					p.Credit = Convert.ToDecimal(col["Credit"]);
				}

				p.PaymentID = p.InsertPayment(p);
				if (p.PaymentID > 0) {
					ViewBag.Message = "Record Added: #" + p.PaymentID;
				}

				Parent par = new Parent();
				var parents = par.GetAllParentsItemList();
				ViewData["Parents"] = parents;

				return View();
			}
			catch(Exception ex) {
				ViewBag.Message = "Failed to add record: " + ex.Message;

				Parent par = new Parent();
				var parents = par.GetAllParentsItemList();
				ViewData["Parents"] = parents;

				return View();
			}
		}

		public ActionResult PendingStudents()
		{

			AdminModel am = new AdminModel();
			am.AdminUser = am.AdminUser.GetUserSession();

			am.PendingStudentList = am.GetPendingStudentList();

			if (am.AdminUser.UID > 0) {
				return View(am);
			}
			else {
				return RedirectToAction("Login", "Profile");
			}
		}


		public ActionResult StudentApplication(FormCollection col)
		{
			var states = States.GetStatesList();
			ViewData["States"] = states;

			var	studentID = col["btnSubmit"];
			


			AdminModel am = new AdminModel();
			am.AdminUser = am.AdminUser.GetUserSession();

			//Get Student by Student ID
			am.StudentApplicant = am.GetStudentByStudentID(studentID);
			var statename = states.First(c => c.Value == am.StudentApplicant.State.ToString());
			ViewData["StudentState"] = statename.Text;
			//Get ParentID by StudentID
			//Get Parent by ParentID
			am.ParentApplicant = am.GetParentByStudentID(studentID);
			statename = states.First(c => c.Value == am.ParentApplicant.State.ToString());
			ViewData["ParentState"] = statename.Text;

			if (am.AdminUser.UID > 0) {
				return View(am);
			}
			else {
				return RedirectToAction("Login", "Profile");
			}
		}

		public ActionResult EmployeeApplication(FormCollection col)
		{
			var states = States.GetStatesList();
			ViewData["States"] = states;

			var EmployeeID = col["btnSubmit"];

			AdminModel am = new AdminModel();
			am.AdminUser = am.AdminUser.GetUserSession();

			//Get Student by Employee ID
			am.EmployeeApplicant = am.GetEmployeeByEmployeeID(EmployeeID);
			var statename = states.First(c => c.Value == am.EmployeeApplicant.State.ToString());
			ViewBag.state = statename.Text;

			if (am.AdminUser.UID > 0)
			{
				ViewBag.ID = EmployeeID;
				return View(am.EmployeeApplicant);
			}
			else
			{
				return RedirectToAction("Login", "Profile");
			}
		}

		public ActionResult PendingEmployee()
		{

			AdminModel am = new AdminModel();
			am.AdminUser = am.AdminUser.GetUserSession();

			am.PendingEmployeeList = am.GetPendingEmployeeList();

			if (am.AdminUser.UID > 0)
			{
				return View(am);
			}
			else
			{
				return RedirectToAction("Login", "Profile");
			}
		}


		public ActionResult AdminEmployeeApproval(FormCollection col)
		{
			int UID = Int32.Parse(col["ID"]);
			int approval = Int32.Parse(col["Approval"]);
			int Flag = 0;

			AdminEmployeeApproval AEA = new AdminEmployeeApproval();

			Flag = AEA.AdminEmployeeApprove(UID, approval);

			switch (Flag)
            {
				case 1:
					ViewBag.CompletionFlag = "Employee has been approved!";
					break;
				case 2:
					ViewBag.CompletionFlag = "Employee has been denied!";
					break;
				default:
					ViewBag.CompletionFlag = "Error. Please contact the System admin.";
					break;
			}

			return View();
		}


		public ActionResult AdminFamilyPage(int UID)
		{
			AdminFamilyPage AFP = new AdminFamilyPage();

			List<AdminFamilyPageParentInfo> P = new List<AdminFamilyPageParentInfo>();

			P = AFP.GetParentByStudentUserID(UID);

			ViewBag.ChildFirstName = AFP.StudentsFirstName;
			ViewBag.ChildLastName = AFP.StudentsLastName;
			ViewBag.ChildDOB = AFP.Age;

			return View(P);
		}

		public ActionResult AdminFamilyDeletion(int UID)
		{
			AdminFamilyDelete AFD = new AdminFamilyDelete();

			ViewBag.CompletionFlag = AFD.AdminFamilyDeletion(UID);

			return View();
		}

		public ActionResult AddEvent()
		{
			return View();
		}

		[HttpPost]
		public ActionResult AddEvent(FormCollection col)
		{
			try {
				AdminModel am = new AdminModel();

				am.NewEvent.Event = col["NewEvent.Event"];
				am.NewEvent.EventDate = Convert.ToDateTime(col["NewEvent.EventDate"]);
				am.NewEvent.Time = col["Time"];
				am.NewEvent.Description = col["NewEvent.Description"];

				var successFlag = am.InsertNewEvent(am.NewEvent);

				if (successFlag > 0) {
					ViewBag.Message = "Event successfully added";
				}
				else {
					ViewBag.Message = "Error adding event";
				}
			}
			catch(Exception ex) {
				ViewBag.Message = "Event successfully added";
			}

			return View();
		}

		[HttpPost]
		public ActionResult ApproveStudent(FormCollection col)
		{
			try {
				AdminModel am = new AdminModel();

				var studentID = col["btnSubmit"];

				var intStudentID = Convert.ToInt32(studentID);

				am.EnrollStudent(intStudentID);


				return View();
			}
			catch {
				return RedirectToAction("Error", "Shared");
			}
		}
	}
}
