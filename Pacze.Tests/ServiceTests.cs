using System;
using System.Net;
using Forcom.Perspectiv.TestCore.Rest;
using Nancy.Hosting.Self;
using NUnit.Framework;

namespace Pacze.Tests
{
	public class ServiceTests
	{
		[Test]
		public void Dupa()
		{
			const string operations = @"[{ ""op"": ""replace"", ""path"": ""/dupa"", ""value"": ""po"" }]";

			var response = WebRequestHelper.WebInvoke(HostUri + "/my-service/69", "PATCH", "application/json", operations);

			Console.Out.WriteLine(response.Body);
			Assert.AreEqual(response.StatusCode, HttpStatusCode.NoContent);
		}


		[SetUp]
		public void StartHost()
		{
			var hostConfiguration = new HostConfiguration
			{
				UrlReservations = { CreateAutomatically = true }
			};
			host = new NancyHost(hostConfiguration, new Uri(HostUri));
			host.Start();
		}

		[TearDown]
		public void CloseHost()
		{
			if (host != null)
				host.Dispose();
		}

		private NancyHost host;
		private const string HostUri = "http://localhost:12345";
	}
}