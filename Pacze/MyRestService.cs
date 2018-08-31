using System;
using System.Collections.Generic;
using Nancy;
using Nancy.JsonPatch;
using Newtonsoft.Json;

namespace Pacze
{
	public class MyRestService : NancyModule
	{
		public MyRestService() : base("/my-service")
		{
			Patch["/{id}"] = p => Paczuj((string)p.id);
		}

		private object Paczuj(string id)
		{
//			var d = new { dupa = "przed" }; // CouldNotParsePath Property '/dupa' on target object cannot be set
			var d = new Test
			{
				dupa = "przed",
				items = new List<Item>
				{
					new Item { name = "one" },
					new Item { name = "two" },
				}
			};
			var result = this.JsonPatch(d);
			
			Console.Out.WriteLine("{0} {1} {2}", result.Succeeded, result.FailureReason, result.Message);
			Console.Out.WriteLine(JsonConvert.SerializeObject(d));

			return HttpStatusCode.NoContent;
		}
	}

	public class Test
	{
		public string dupa { get; set; }
		public IEnumerable<Item> items { get; set; } 
	}

	public class Item
	{
		public string name { get; set; }
	}
}
