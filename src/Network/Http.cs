using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Alluseri.EvertalePonoserV2.Network;

public static class Http {
	public static HttpClient Client = new(new HttpClientHandler() {
		// AutomaticDecompression = DecompressionMethods.All,
		/*UseProxy = true,
		Proxy = new WebProxy() {

		},*/
		ClientCertificateOptions = ClientCertificateOption.Manual,
		ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, cetChain, policyErrors) => true
	});

	public static byte[] Post(string Endpoint, HttpContent Content, (string Name, string Value)[]? Headers) {
		HttpRequestMessage HRM = new(HttpMethod.Post, Endpoint);
		if (Headers != null)
			foreach ((string Name, string? Value) Header in Headers)
				if (Header.Name != "Content-Type")
					HRM.Headers.Add(Header.Name, Header.Value);
		if (Content is FormUrlEncodedContent)
			Content.Headers.ContentType!.CharSet = "UTF-8";
		HRM.Content = Content;

		return Client.Send(HRM).Content.ReadAsByteArrayAsync().Result;
	}
	public static void NoPost(string Endpoint, HttpContent Content, (string Name, string Value)[]? Headers) {
		HttpRequestMessage HRM = new(HttpMethod.Post, Endpoint);
		if (Headers != null)
			foreach ((string Name, string? Value) Header in Headers)
				if (Header.Name != "Content-Type")
					HRM.Headers.Add(Header.Name, Header.Value);
		if (Content is FormUrlEncodedContent)
			Content.Headers.ContentType!.CharSet = "UTF-8";
		HRM.Content = Content;
		Client.Send(HRM);
	}
}