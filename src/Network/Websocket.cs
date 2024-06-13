using Newtonsoft.Json.Linq;
using System;
using System.Net.WebSockets;
using System.Threading;

namespace Alluseri.EvertalePonoserV2.Network;

public sealed class Websocket : IDisposable {
	private bool Aborted;
	private readonly ClientWebSocket Client;

	public Websocket(string Gateway, int Timeout = 10000, (string Name, string Value)[]? Headers = null) : this(new Uri(Gateway), Timeout, Headers) { }
	public Websocket(Uri Gateway, int Timeout = 10000, (string Name, string Value)[]? Headers = null) {
		CancellationTokenSource Src = new();
		Client = new ClientWebSocket();
		if (Headers != null)
			foreach ((string Name, string Value) Header in Headers)
				Client.Options.SetRequestHeader(Header.Name, Header.Value);
		if (Client.ConnectAsync(Gateway, Src.Token).Wait(Timeout) == false || Client.State != WebSocketState.Open) {
			Src.Cancel();
			throw new ArgumentException("Failed to establish connection!", nameof(Gateway));
		}
	}
	/// <returns>true IF ABORTED.</returns>
	public bool Send(string Data) => Send(System.Text.Encoding.UTF8.GetBytes(Data));

	/// <returns>true IF ABORTED.</returns>
	public bool Send(JObject Data) => Send(Data.ToString());

	/// <returns>true IF ABORTED.</returns>
	public bool Send(byte[] Data) {
		try {
			if (Aborted)
				return true;
			Client.SendAsync(Data, WebSocketMessageType.Text, true, CancellationToken.None).Wait();
			return Aborted = Client.State != WebSocketState.Open;
		} catch {
			return true;
		}
	}

	public void Dispose() {
		Client.Abort();
		Client.Dispose();
	}
}