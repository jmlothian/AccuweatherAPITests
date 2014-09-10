using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccuMVC.Models
{
	public class ForecastWeatherModel
	{
		public string FormattedTime { get; set; }
		public string ForecastIcon { get; set; }
		public string IconPhrase { get; set; }
		public string Temperature { get; set; }
		public string TemperatureUnit { get; set; }
	}
}