using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccuMVC.Models
{
	public class LocationSearchModel
	{
		public string SearchText { get; set; }
		public List<LocationModel> SearchResults { get; set; }
		public bool Error { get; set; }
		public LocationSearchModel()
		{
			//would of course be empty in production, just making testing easier at the moment
			SearchText = "State College";
			SearchResults = new List<LocationModel>();
			Error = false;
		}
	}
}