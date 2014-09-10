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
    public class WeatherController : Controller
    {
        // GET: Weather
        public ActionResult Index(string id)
        {
			LocationWeatherModel Model = new LocationWeatherModel { Key = id };
			Model.Error = false;
			GetCurrent(Model);
			if(Model.Error == false)
				GetForecast(Model);
            return View(Model);
        }
		/// <summary>
		/// Given a model with a valid location key, attempts to retrieve the current weather
		/// </summary>
		/// <param name="Model"></param>
		private void GetCurrent(LocationWeatherModel Model)
		{
			Model.CurrentWeather = new CurrentWeatherModel();
			string retVal = RESTRequest.GetREST("http://" + WebConfigurationManager.AppSettings["AccuWeatherSite"]
				, "/currentconditions/v1/" + Model.Key + ".json?apikey="+ WebConfigurationManager.AppSettings["AccuWeatherAPIKey"] + "&language=en");
			if (!string.IsNullOrEmpty(retVal))
			{
				try
				{
					JArray jObject = JArray.Parse(retVal);
					for (int i = 0; i < jObject.Count; i++)
					{
						Model.CurrentWeather.ImperialTemp = jObject[i]["Temperature"]["Imperial"]["Value"].ToString();
						Model.CurrentWeather.MetricTemp = jObject[i]["Temperature"]["Metric"]["Value"].ToString();
						Model.CurrentWeather.WeatherImage = GetIcon(int.Parse(jObject[i]["WeatherIcon"].ToString()));
						Model.CurrentWeather.WeatherText = jObject[i]["WeatherText"].ToString();
						//although this comes as an array, it isn't clear why in the docs.  Just take the first value for now
						// but leave as a loop for later in case there are other values we can use
						break;
					}
				}
				catch (Exception ex)
				{
					//log error here
					Model.Error = true;
				}
			} else
			{
				Model.Error = true;
			}
		}
		/// <summary>
		/// Given a model with a valid location key, attempts to retrieve the current forecast
		/// </summary>
		/// <param name="Model"></param>
		private void GetForecast(LocationWeatherModel Model)
		{
			Model.ForecastWeather = new List<ForecastWeatherModel>();
			string retVal = RESTRequest.GetREST("http://" + WebConfigurationManager.AppSettings["AccuWeatherSite"], 
				"/forecasts/v1/hourly/12hour/" + Model.Key + ".json?apikey=" + WebConfigurationManager.AppSettings["AccuWeatherAPIKey"] + "&language=en");
			if (!string.IsNullOrEmpty(retVal))
			{
				try
				{
					JArray jObject = JArray.Parse(retVal);
					for (int i = 0; i < jObject.Count; i++)
					{
						ForecastWeatherModel forecast = new ForecastWeatherModel();
						forecast.ForecastIcon = GetIcon(int.Parse(jObject[i]["WeatherIcon"].ToString()));
						DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
						dt = dt.AddSeconds(double.Parse(jObject[i]["EpochDateTime"].ToString()));
						forecast.FormattedTime = dt.ToLocalTime().ToShortTimeString();
						forecast.IconPhrase = jObject[i]["IconPhrase"].ToString();
						forecast.Temperature = jObject[i]["Temperature"]["Value"].ToString();
						forecast.TemperatureUnit = jObject[i]["Temperature"]["Unit"].ToString();
						Model.ForecastWeather.Add(forecast);
					}
				}
				catch (Exception ex)
				{
					//log error here
					Model.Error = true;
				}
			} else
			{
				Model.Error = true;
			}
		}
		/// <summary>
		/// Creates a 0-padded string to be used with AccuWeather icons
		/// </summary>
		/// <param name="id">id of the icon as an integer</param>
		/// <returns>URL to an AccuWeather icon</returns>
		private string GetIcon(int id)
		{
			string icoNum = "";
			if(id < 10)
			{
				icoNum = "0" + id.ToString();
			} else
			{
				icoNum = id.ToString();
			}
			return "http://apidev.accuweather.com/developers/Media/Default/WeatherIcons/" + icoNum + "-s.png";
		}
    }
}