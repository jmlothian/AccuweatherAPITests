using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using AccuKS.Utility;

namespace AccuKS.Controllers
{
	public class WeatherController : ApiController
    {
		//values for LocationKey (id here) are not specified in API docs.  Results return something that looks like an integer.
		//To be safe, this treats it as a string.  If exact format was known, we would validate here and return BadRequest for invalid keys 
		// (or alternatively swap to int/long parameter)
		public IHttpActionResult GetCurrent(string id)
        {
			string retVal = RESTRequest.GetREST("http://apidev.accuweather.com", "/currentconditions/v1/" + id + ".json?apikey=782def8fa0774669b9d3fc5e0fd1b6c5&language=en");
			if(retVal == null)
			{
				return BadRequest();
			}
			return Ok(retVal);
        }
		public IHttpActionResult GetForecast(string id)
		{
			string retVal = RESTRequest.GetREST("http://apidev.accuweather.com", "/forecasts/v1/hourly/12hour/" + id + ".json?apikey=782def8fa0774669b9d3fc5e0fd1b6c5&language=en");
			if (retVal == null || retVal == "[]")
			{
				return BadRequest();
			}
			return Ok(retVal);
		}
	}
}