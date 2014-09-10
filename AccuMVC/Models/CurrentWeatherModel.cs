using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccuMVC.Models
{
	public class CurrentWeatherModel
	{
		public string WeatherImage { get; set; }
		public string WeatherText { get; set; }
		public string MetricTemp { get; set; }
		public string ImperialTemp { get; set; }
	}
}