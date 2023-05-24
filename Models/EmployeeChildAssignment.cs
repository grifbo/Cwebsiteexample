
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IHLA_Template.Models
{
    public class EmployeeChildAssignment
    {
        public int EmployeeChildAssignmentID = 0;
        public int EmployeeID = 0;
        public int ChildID = 0;
        public DateTime DateOfAssignment = DateTime.Today;
        public string Notes = String.Empty;
    }
}