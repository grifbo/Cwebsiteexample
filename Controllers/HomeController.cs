using IHLA_Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IHLA_Template.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
			// ****************************************
			// This is only to test the email functions
			// ****************************************

            ViewBag.Message = "Your application description page.";

			String date = "6/24/2021";
			String time = "6:00pm";

			//Models.Email.SendTourConfirmationEmail(date, time, "");

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult StudentApplication()
        {
            return View();
        }

        public ActionResult Admin()
        {
			//need to open database using the uid
			//Models.Database db = new Models.Database();

			//these are temp numbers
			//Create a view to grab the real numbers
			string strFirstName = "Tim";
			int intChildrenApps = 10;
			int intEmployeeApps = 5;
			int intEmployeesEmployed = 4;
			int intChildrenEnrolled = 20;


			ViewBag.FirstName = strFirstName;
			ViewBag.childrenApps = intChildrenApps;
			ViewBag.EmployeeApps = intEmployeeApps;
			ViewBag.childrenEnrolled = intChildrenEnrolled;
			ViewBag.EmployeesEnployed = intEmployeesEmployed;
			return View();
        }

		public ActionResult Tour()
		{
			return View();
		}

		public ActionResult NewFamilies()
		{
			return View();
		}

		public ActionResult Register()
		{
			return View();
		}

		public ActionResult EmployeeSchedule()
		{
			return View();
		}

		public ActionResult PayHistory()
		{
			return View();
		}

		public ActionResult FamilyListInfo()
		{
			/*  //Direct Method
			List<FamilyList> model = new List<FamilyList>();
			List<FamilyList.Guardian> parents = new List<FamilyList.Guardian>();
			parents.Add(new FamilyList.Guardian() { Name = "Linda", Relation = "Mother" });
			parents.Add(new FamilyList.Guardian() { Name = "Robin", Relation = "Father" });
			model.Add(new FamilyList(){
				ChildName="Sheldon",ChildAge=23,ChildClassroom="2-A",Parents = parents,
				PickUpPerson = new FamilyList.Guardian() { Name="Wron", Relation="Grandson"}
			});
			model.Add(new FamilyList(){
				ChildName = "Molly",ChildAge = 13,ChildClassroom = "1-B",Parents = parents,
				PickUpPerson = new FamilyList.Guardian() { Name = "Wron", Relation = "Grandson" }
			});*/
			List<FamilyList> model = FamilyList.GetFamilyLists();
			return View(model);
		}

		public ViewResult AdminManageClasses()
        {
			ListEmployeeStudent model = ListEmployeeStudent.populate();	
			return View(model);
        }
		[HttpPost]
		public ViewResult AdminManageClasses(string childern, string classroom, DateTime DOA)
		{
			
				ListEmployeeStudent model = ListEmployeeStudent.populate();
				Database db = new Database();
			try {
				var selectedChild = from child in model.Children
									where child.ID.ToString() == childern
									select child;

				User.ActionTypes result = db.EnrollChildern(new EmployeeChildAssignment() {
					ChildID = Convert.ToInt32(childern),
					EmployeeID = Convert.ToInt32(classroom),
					DateOfAssignment = Convert.ToDateTime(DOA),
					Notes = selectedChild.First().Notes
				});
				if (result == Models.User.ActionTypes.InsertSuccessful) {
					ViewBag.message = "Student enrolled successfully";
					ViewBag.m_type = "success";
				}
				else if (result == Models.User.ActionTypes.UpdateSuccessful) {
					ViewBag.message = "Student class changed";
					ViewBag.m_type = "success";
				}
				else {
					ViewBag.message = "Student enrolled successfully";
					ViewBag.m_type = "danger";
				}
				return View(model);
			}
			catch {
				ViewBag.message = "Error Processing";
				ViewBag.m_type = "danger";

				return View(model); 
			}
		}

		public ActionResult ClassRoster()
		{
			return View();
		}

		public ActionResult AdminOpenRequest()
		{
			return View();
		}

		public ActionResult AdminManageEmployees()
		{
			return View();
		}

		public ActionResult AdminFamily()
        {
			return View();
        }

		public ActionResult EmployeeClassesRoaster()
        {

			List<EmployeeClass> model = EmployeeClass.populate();
			//model = model.Where(x => x.self.EmployeeID == 1).ToList();
			Models.User u = new Models.User();
			u = u.GetUserSession();

			var singleEmployee = from emp in model
								 where emp.self.EmployeeLoginID == u.UID
								 select emp;
			model = singleEmployee.ToList();

			return View(model);
        }

		public ActionResult UpcomingEvents_Lunch()
		{
			Models.Events GrabEvents = new Events();

			List<Events> E = new List<Events>();

			DateTime dtmToday = DateTime.Now;
			string strToday = dtmToday.ToString("yyyy") + "-" + dtmToday.ToString("MM") + "-" + dtmToday.ToString("dd");

			E = GrabEvents.GetUpcomingEvents(strToday);

			return View(E);
		}
	}
}
