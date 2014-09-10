using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccuMVC.Models
{

	//I'd be tempted to add MVC attributes to these model, but I don't use them (for example, 
	// I don't need to label any fields with a DisplayName on these pages).  And it looks
	// like most of the attributes I'd consider are used for data validation and DB 
	// binding, which aren't issues here.
	public class LocationModel
	{
		public int Key { get; set; }
		public string LocalizedName { get; set; }
		public string PrimaryPostalCode { get; set; }
		public string Country { get; set; }
		public string AdministrativeAreaLocalizedName { get; set; }
	}
}