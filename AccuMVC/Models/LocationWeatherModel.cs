using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccuMVC.Models
{
	public class LocationWeatherModel
	{
		public string Key { get; set; }
		public CurrentWeatherModel CurrentWeather { get; set; }
		public List<ForecastWeatherModel> ForecastWeather { get; set; }
		public bool Error { get; set; }
	}
}