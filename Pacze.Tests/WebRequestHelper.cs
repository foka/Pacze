using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Forcom.Perspectiv.TestCore.Rest
{
	public static class WebRequestHelper
	{
		public static string GetJson(string uri)
		{
			var request = CreateHttpGetRequest(uri, "json");
			return GetResponseContentString(request);
		}

		public static string GetXml(string uri)
		{
			var request = CreateHttpGetRequest(uri, "xml");
			return GetResponseContentString(request);
		}

		private static string GetResponseContentString(WebRequest request)
		{
			using (var response = request.GetResponse())
			using (var responseStream = response.GetResponseStream())
			using (var streamReader = new StreamReader(responseStream, Encoding.UTF8))
			{
				return streamReader.ReadToEnd();
			}
		}

		private static HttpWebRequest CreateHttpGetRequest(string uri, string applicationAcceptType)
		{
			var request = (HttpWebRequest)WebRequest.Create(uri);
			request.Method = "GET";
			request.Accept = "application/" + applicationAcceptType;
			return request;
		}

		public static WebInvokeResponse Get(string uri)
		{
			return WebInvoke(uri, "GET", "application/json", null);
		}

		public static WebInvokeResponse Get(string uri, Dictionary<string, string> headers)
		{
			return WebInvoke(uri, "GET", "application/json", null, headers);
		}

		public static WebInvokeResponse Post(string uri)
		{
			return WebInvoke(uri, "POST", "text/plain", null);
		}

		public static WebInvokeResponse Post(string uri, string content)
		{
			return WebInvoke(uri, "POST", "application/json", content);
		}

		public static WebInvokeResponse Post(string uri, string content, Dictionary<string, string> headers)
		{
			return WebInvoke(uri, "POST", "application/json", content, headers);
		}

		public static WebInvokeResponse Put(string uri, string content = null, IDictionary<string,string> headers = null)
		{
			return WebInvoke(uri, "PUT", "application/json", content, headers);
		}

		public static WebInvokeResponse Delete(string uri)
		{
			return WebInvoke(uri, "DELETE", "application/json", null);
		}

		public static WebInvokeResponse WebInvoke(string uri, string method, string accept, string content,
			IDictionary<string, string> headers = null, string contentType = null)
		{
			var request = (HttpWebRequest)WebRequest.Create(uri);
			request.Method = method;

			if (content != null)
			{
				var encoding = new UTF8Encoding();
				var bytes = encoding.GetBytes(content);
				request.ContentLength = bytes.Length;
				request.ContentType = contentType ?? "application/json; charset=utf-8";
			}
			else
			{
				request.ContentLength = 0;
			}

			request.Accept = accept;

			if (headers != null)
			{
				foreach (var header in headers)
					request.Headers.Add(header.Key, header.Value);
			}

			if (request.Method != "GET")
			{
				using (var requestStream = request.GetRequestStream())
				using (var streamWriter = new StreamWriter(requestStream))
				{
					streamWriter.Write(content);
				}
			}

			return GetWebInvokeResponse(request);
		}

		private static WebInvokeResponse GetWebInvokeResponse(HttpWebRequest request)
		{
			WebResponse response = null;
			try
			{
				response = request.GetResponse();
				return GetWebInvokeResponse(response);
			}
			catch (WebException wex)
			{
				return GetWebInvokeResponse(wex.Response);
			}
			finally
			{ 
				if (response != null)
					response.Close();
			}
		}

		private static WebInvokeResponse GetWebInvokeResponse(WebResponse response)
		{
			using (var responseStream = response.GetResponseStream())
			using (var streamReader = new StreamReader(responseStream))
			{
				return new WebInvokeResponse
				{
					StatusCode = ((HttpWebResponse)response).StatusCode,
					Body = streamReader.ReadToEnd(),
					Headers = response.Headers,
				};
			}
		}
	}
}
