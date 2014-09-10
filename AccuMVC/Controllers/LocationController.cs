using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using AccuMVC.Models;
using AccuMVC.Utility;
using Newtonsoft.Json.Linq;

namespace AccuMVC.Controllers
{
    public class LocationController : Controller
    {
        // GET: Location
		public ActionResult Index(string SearchText)
        {
			LocationSearchModel LocSearch = new LocationSearchModel();
			LocSearch.SearchText = SearchText == null ? LocSearch.SearchText : SearchText;
			GetSearchResults(LocSearch);
            return View(LocSearch);
        }
		//This seems oddly placed because the result is determined by changing the LocationSearchModel.SearchText
		// But I didn't want to include logic directly in the Model to handle it.  In MVVM I'd use a VM property that
		// is bound to the model, but updates the search results within the setter.
		private void GetSearchResults(LocationSearchModel Model)
		{
			//reset the error flag
			Model.Error = false;
			if(Model.SearchText != "")
			{
				//clear any previous search
				Model.SearchResults.Clear();

				//hit API
				string retVal = RESTRequest.GetREST("http://" + WebConfigurationManager.AppSettings["AccuWeatherSite"], 
					"/locations/v1/search?q=" + Model.SearchText + "&apikey=" + WebConfigurationManager.AppSettings["AccuWeatherAPIKey"]);

				//check for any strangeness
				if (!string.IsNullOrEmpty(retVal))
				{
					try
					{
						//get an object we can use
						JArray jObject = JArray.Parse(retVal);
						for (int i = 0; i < jObject.Count; i++)
						{
							//create our local object and add it to the model
							LocationModel res = new LocationModel();
							res.Key = int.Parse(jObject[i]["Key"].ToString());
							res.LocalizedName = jObject[i]["LocalizedName"].ToString();
							res.PrimaryPostalCode = jObject[i]["PrimaryPostalCode"].ToString();
							res.Country = jObject[i]["Country"]["LocalizedName"].ToString();
							res.AdministrativeAreaLocalizedName = jObject[i]["AdministrativeArea"]["LocalizedName"].ToString();
							Model.SearchResults.Add(res);
						}
					}
					catch (Exception ex)
					{
						//log error here
						//set the model error flag so we can tell the user something went wrong on the backend
						Model.Error = true;
					}
				} else
				{
					//something went wrong with the API call
					Model.Error = true;
				}
			}
		}
    }
}