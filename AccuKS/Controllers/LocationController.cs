using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using AccuKS.Utility;
using Newtonsoft.Json.Linq;

namespace AccuKS.Controllers
{
	public class LocationController : ApiController
    {

        // GET: Location
		public IHttpActionResult GetSearch(string q)
        {
			if (!string.IsNullOrEmpty(q))
			{
				string retVal = RESTRequest.GetREST("http://apidev.accuweather.com", "/locations/v1/search?q=" + q + "&apikey=782def8fa0774669b9d3fc5e0fd1b6c5");
				if (retVal == "[]")
				{
					return NotFound();
				}
				else
				{
					return Ok(retVal);
				}
			}
			return NotFound();
        }
    }
}