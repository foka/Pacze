using System.Net;

namespace Forcom.Perspectiv.TestCore.Rest
{
	public class WebInvokeResponse
	{
		public HttpStatusCode StatusCode { get; set; }
		public string Body { get; internal set; }
		public WebHeaderCollection Headers { get; internal set; }
	}
}