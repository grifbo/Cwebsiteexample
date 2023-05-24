using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IHLA_Template.Models
{
	public class States
	{
		public static List<SelectListItem> GetStatesList()
		{
				var items = new List<SelectListItem> {
				new SelectListItem() { Text = "Alabama", Value = "1"},
				new SelectListItem() { Text = "Alaska", Value = "2"},
				new SelectListItem() { Text = "Arizona", Value = "3"},
				new SelectListItem() { Text = "Arkansas", Value = "4"},
				new SelectListItem() { Text = "California", Value = "5"},
				new SelectListItem() { Text = "Colorado", Value = "6"},
				new SelectListItem() { Text = "Connecticut", Value = "7"},
				new SelectListItem() { Text = "Delaware", Value = "8"},
				new SelectListItem() { Text = "Florida", Value = "9"},
				new SelectListItem() { Text = "Georgia", Value = "10"},
				new SelectListItem() { Text = "Hawaii", Value = "11"},
				new SelectListItem() { Text = "Idaho", Value = "12"},
				new SelectListItem() { Text = "Illinois", Value = "13"},
				new SelectListItem() { Text = "Indiana", Value = "14"},
				new SelectListItem() { Text = "Iowa", Value = "15"},
				new SelectListItem() { Text = "Kansas", Value = "16"},
				new SelectListItem() { Text = "Kentucky", Value = "17"},
				new SelectListItem() { Text = "Louisiana", Value = "18"},
				new SelectListItem() { Text = "Maine", Value = "19"},
				new SelectListItem() { Text = "Maryland", Value = "20"},
				new SelectListItem() { Text = "Massachusetts", Value = "21"},
				new SelectListItem() { Text = "Michigan", Value = "22"},
				new SelectListItem() { Text = "Minnesota", Value = "23"},
				new SelectListItem() { Text = "Mississippi", Value = "24"},
				new SelectListItem() { Text = "Missouri", Value = "25"},
				new SelectListItem() { Text = "Montana", Value = "26"},
				new SelectListItem() { Text = "Nebraska", Value = "27"},
				new SelectListItem() { Text = "Nevada", Value = "28"},
				new SelectListItem() { Text = "New Hampshire", Value = "29"},
				new SelectListItem() { Text = "New Jersey", Value = "30"},
				new SelectListItem() { Text = "New Mexico", Value = "31"},
				new SelectListItem() { Text = "New York", Value = "32"},
				new SelectListItem() { Text = "North Carolina", Value = "33"},
				new SelectListItem() { Text = "North Dakota", Value = "34"},
				new SelectListItem() { Text = "Ohio", Value = "35"},
				new SelectListItem() { Text = "Oklahoma", Value = "36"},
				new SelectListItem() { Text = "Oregon", Value = "37"},
				new SelectListItem() { Text = "Pennsylvania", Value = "38"},
				new SelectListItem() { Text = "Rhode Island", Value = "39"},
				new SelectListItem() { Text = "South Carolina", Value = "40"},
				new SelectListItem() { Text = "South Dakota", Value = "41"},
				new SelectListItem() { Text = "Tennessee", Value = "42"},
				new SelectListItem() { Text = "Texas", Value = "43"},
				new SelectListItem() { Text = "Utah", Value = "44"},
				new SelectListItem() { Text = "Vermont", Value = "45"},
				new SelectListItem() { Text = "Virginia", Value = "46"},
				new SelectListItem() { Text = "Washington", Value = "47"},
				new SelectListItem() { Text = "West Virginia", Value = "48"},
				new SelectListItem() { Text = "Wisconsin", Value = "49"},
				new SelectListItem() { Text = "Wyoming", Value = "50"}
			};

			return items;
		}
}
}