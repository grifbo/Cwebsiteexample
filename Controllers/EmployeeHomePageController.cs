using System;
using IHLA_Template.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IHLA_Template.Controllers
{
    public class EmployeeHomePageController : Controller
    {
        public ActionResult EmployeeHomePage()
        {
            Employee E = new Employee();

            E.EmployeeUser = E.EmployeeUser.GetUserSession();

            if (E.EmployeeUser.UID > 0)
            {
                return View(E);
            }
            else
            {
                return RedirectToAction("Login", "Profile");
            }
        }
    }
}
