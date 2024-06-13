#pragma warning disable CS8618

using Alluseri.EvertalePonoserV2.Network;
using System;
using System.Net.WebSockets;
using System.Threading;

namespace Alluseri.EvertalePonoserV2.API;

public class EvertaleChat {
	private Websocket Backend;
	public bool Aborted { get; private set; }
	private EvertaleUser User;

	private int Sequence = 3;

	public EvertaleChat(EvertaleUser User) {
		this.User = User;
		try {
			Backend = new("wss://chatnewalt.prd.evertaleserver.com:443", 20000, new[] {
			("uid", User.UserID),
			("sesid", User.SessionID),
			("lang", User.Language)
		});
		} catch {
			Aborted = true;
			//Console.WriteLine("UID " + User.UserID + " failed to connect to the chat!");
			throw;
		}
		//Console.WriteLine("UID " + User.UserID + " connected to the chat!");
		Backend.Send("{\"action\":\"command\",\"type\":6,\"content\":null,\"sesid\":\"" + User.SessionID + "\",\"requestID\":1}");
		Aborted = Backend.Send("{\"action\":\"command\",\"type\":6,\"content\":\"Shard-\",\"sesid\":\"" + User.SessionID + "\",\"requestID\":2}");
		if (!Aborted)
			Thread.Sleep(500);
		else
			Backend.Dispose();
	}

	public bool Swap(bool ToGlobal) {
		if (Aborted)
			return false;
		else {
			if (Aborted = Backend.Send("{\"action\":\"command\",\"type\":6,\"content\":\"" + (ToGlobal ? "Cluster-" : "Shard-") + "\",\"sesid\":\"" + User.SessionID + "\",\"requestID\":" + Sequence++ + "}")) {
				Backend.Dispose();
				return false;
			} else
				return true;
		}
	}

	public bool Send(string Message) {
		if (Aborted)
			return false;
		else {
			if (Aborted = Backend.Send("{\"action\":\"command\",\"type\":1,\"content\":\"" + Message.Replace('\'', '’').Replace('"', '’').Trim() + "\",\"sesid\":\"" + User.SessionID + "\",\"requestID\":" + Sequence++ + "}")) {
				Backend.Dispose();
				return false;
			} else
				return true;
		}
	}
}