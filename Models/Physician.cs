using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IHLA_Template.Models
{
	public class Physician
	{
		public long UID = 0;
		public string PhysicianName = string.Empty;
		public string PhysicianStreetAddress = string.Empty;
		public string PhysicianCity = string.Empty;
		public int PhysicianState = 0;
		public string PhysicianPhoneNumber = string.Empty;
	}
}