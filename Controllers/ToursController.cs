using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IHLA_Template.Controllers
{
    public class ToursController : Controller
    {
        // GET: Tours
        public ActionResult NewTour()
        {
            return View();
        }

		[HttpPost]
		public ActionResult NewTour(FormCollection col)
		{
			try {
				Models.Tours t = new Models.Tours();

				//string date = col["Date"].ToString("MM/dd/yyyy");

				t.FirstName = col["FirstName"];
				t.LastName = col["LastName"];
				t.Email = col["Email"];
				t.Date = col["Date"];
				t.Time = col["Time"];


				/*
				
				Need a stored proc for returning one if the date matches up

				int intCheckDate = t.CheckDate();

				if(intCheckDate == 1)
				{
					Throw("Date already in use");
				}
				else
				{
					int succesFlag = t.InsertTourDate();
	
					TempData["Date"] = t;

					return RedirectToAction("DateScheduled","Date");
					
					//I think we can remove the if statement if we can use the successflag as a variable to change the text
					return RedirectToAction("DateScheduled","Tours", new {SuccessFlag = successFlag,Date = t.Date,Time = t.Time});		
				}

				return RedirectToAction("DateScheduled", succesFlag);
				}
				// IF NULL good to proceed
				// If !NULL there is already something scheduled

				*/

				int successFlag = t.InsertTourDate();

				return RedirectToAction("DateScheduled","Tours", new {SuccessFlag = successFlag,Date = t.Date,Time = t.Time});
			}
			catch(Exception ex) {

				return RedirectToAction("DateScheduled", "Tours", new { SuccessFlag = 0});

				//ViewBag.Message = "Error:" + ex;
				//return View();

			}
		}

		public ActionResult DateScheduled(int SuccessFlag, String Time,String Date)
        {

			ViewBag.SuccessFlag = SuccessFlag;

            if(SuccessFlag == 1){
				ViewBag.Date = Date;
				ViewBag.Time = Time;
			}

			return View();
        }
	}

}