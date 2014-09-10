using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace AccuKS.Utility
{
	public static class RESTRequest
	{
		public static string GetREST(string BaseAddress, string Path)
		{
			string Response = null;
			using(var client = new HttpClient())
			{
				client.BaseAddress = new Uri(BaseAddress);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage response = client.GetAsync(Path).Result;
				if (response.IsSuccessStatusCode)
				{
					Response = response.Content.ReadAsStringAsync().Result;
				}
			}
			return Response;
		}
	}
}