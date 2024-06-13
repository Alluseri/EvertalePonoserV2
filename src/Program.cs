#pragma warning disable CA2211

using Alluseri.EvertalePonoserV2.API;
using Alluseri.EvertalePonoserV2.Reroll;
using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace Alluseri.EvertalePonoserV2;

public static class Program {
	public static void Reroll(string Device, string OS, int Shard, string Language, string Timezone, EvertaleAPI.ProfileData? Data) {
		EvertaleUser? Reu = EvertaleUser.RegisterNew(Device, OS, Shard, Language, Timezone, null);
		if (Reu == null) {
			Console.WriteLine("Could not register an account to reroll.");
			return;
		}
		Console.WriteLine("[1/3] [1/3] Running offline act scenario...");
		OfflineActScenario Oas = new(Reu.SessionID, Data);
		Oas.Run();
		Console.WriteLine("[1/3] [2/3] Hacking sidequests...");
		Oas.HackSidequests();
		Console.WriteLine("[1/3] [3/3] Completing missing quests...");
		Oas.CompleteMissing();
		Console.WriteLine("[2/3] Running one time nuisance scenario...");
		OneTimeNuisanceScenario Otns = new(Reu.SessionID, true);
		Otns.Run();
		Console.WriteLine("[3/3] Running daily nuisance scenario...");
		DailyNuisanceScenario Dns = new(Reu.SessionID, false, false);
		Dns.Run();
		Console.WriteLine("Done! To get into your account, use the restore code: " + Reu.RCode);
		Console.WriteLine("lunahook.dev on top!");
	}

	public static void Main(string[] Args) {
		Console.WriteLine("Evertale Ponoser V2 by Alluseri");
		Console.WriteLine("~Поносим Evertale по L7~");
		switch (Args.Length == 0 ? "help" : Args[0].ToLower()) {
			default:
			Console.WriteLine("Available subcommands:");
			Console.WriteLine("reroll - Creates and rerolls a new account in interactive mode.");
			Console.WriteLine("arena [sessid] [enemy id] - Fights an enemy in the arena. Ignores rank checks.");
			Console.WriteLine("test-api - Tests if you can access the API and the API calls are up-to-date.");
			Console.WriteLine("advertise [restorecode] - Advertises this project in local and cross-world chat.");
			Console.WriteLine("advertise-leak [restorecode] - Advertises this project in local and cross-world chat and leaks restore code.");
			break;
			case "reroll": {
				Console.WriteLine("Interactive mode. Fields not marked with * may be left empty.");
				Console.Write("Please enter the device name: ");
				string Devn = Console.ReadLine()!;
				if (string.IsNullOrEmpty(Devn))
					Devn = "Genuine Lunahook Branded Phone";
				Console.Write("Please enter the OS: ");
				string Osn = Console.ReadLine()!;
				if (string.IsNullOrEmpty(Osn))
					Osn = "Android OS 11 / API-30 (RP1A.200720.012/A225FXXU2AUH1)";
				Console.Write("Please enter the shard*: ");
				int Shard = int.Parse(Console.ReadLine()!);
				Console.Write("Please enter the Alpha-2 language: ");
				string Alp2 = Console.ReadLine()!;
				if (string.IsNullOrEmpty(Alp2))
					Alp2 = "jp";
				Console.Write("Please enter the region timezone(e.g. JST): ");
				string Rtz = Console.ReadLine()!;
				if (string.IsNullOrEmpty(Rtz))
					Rtz = "JST";
				Console.Write("Please enter your nickname(or leave empty to choose later in game, THAT MIGHT BE BANNABLE THOUGH): ");
				string Nick = Console.ReadLine()!;
				Console.WriteLine("Bravo 6, going dark.");
				Reroll(Devn, Osn, Shard, Alp2, Rtz, string.IsNullOrEmpty(Nick) ? null : new EvertaleAPI.ProfileData(Nick));
			}
			break;
			case "arena":
			Console.WriteLine("Trying to fight on SESSID " + Args[1] + " against " + Args[2]);
			bool? V = EvertaleAPI.ArenaFight(Args[1], Args[2]);
			Console.WriteLine("Result: " + (V == null ? "Blocked by server" : V.Value ? "Victory!" : "Loss!"));
			break;
			case "test-api": {
				EvertaleUser? Eu = EvertaleUser.RegisterNew("Lunahook", "Lunahook", 1, "ru", "GMT", new EvertaleAPI.ProfileData("lunahook.dev"));
				if (Eu == null) {
					Console.WriteLine("[API Test] Account failed to register.");
					return;
				}
				EvertaleChat Ec;
				try {
					Ec = new(Eu);
				} catch {
					Console.WriteLine("[API Test] Failed to connect to chat.");
					throw;
				}
				if (Ec.Swap(true)) {
					Thread.Sleep(500);
				} else {
					Console.WriteLine("[API Test] Failed to swap to global.");
					return;
				}
				Console.WriteLine("[API Test] Is successful: " + Ec.Send("Get good, get lunahook.dev! Free rerolls for everyone."));
			}
			break;
			case "advertise": {
				EvertaleAPI.RestoreData? Rdt = EvertaleAPI.RestoreAccount(Args[1], "jp", "JST");
				if (Rdt == null) {
					Console.WriteLine("Failed to restore account to advertise.");
					return;
				}
				EvertaleAPI.LoginData? Ld = EvertaleAPI.CreateSession(Rdt.UID, Rdt.CLID, "jp", "JST", 1, "Catgirls", "NekoOS");
				if (Ld == null) {
					Console.WriteLine($"Failed to create session to advertise: {Rdt.UID}, {Rdt.CLID}");
					return;
				}
				EvertaleUser Evt = new(Rdt.UID, Rdt.CLID, Ld.SessionID, "Catgirls", "NekoOS", 1, "jp", "JST");
				Console.WriteLine("Advertise session created.");
				EvertaleChat Ec;
				try {
					Ec = new(Evt);
				} catch {
					Console.WriteLine("[API Test] Failed to connect to chat.");
					throw;
				}
				if (Ec.Send("lunahook.dev on top! Free rerolls for everyone! " + Random.Shared.Next(0xFFFF))) {
					Thread.Sleep(500);
				} else {
					Console.WriteLine("[API Test] Failed to send local message.");
					return;
				}
				if (Ec.Swap(true)) {
					Thread.Sleep(500);
				} else {
					Console.WriteLine("[API Test] Failed to swap to global.");
					return;
				}
				if (Ec.Send("lunahook.dev on top! Free rerolls for everyone! " + Random.Shared.Next(0xFFFF))) {
					Thread.Sleep(500);
				} else {
					Console.WriteLine("[API Test] Failed to send global message.");
					return;
				}
				Console.WriteLine("Successfully advertised!");
			}
			break;
			case "advertise-leak": {
				EvertaleAPI.RestoreData? Rdt = EvertaleAPI.RestoreAccount(Args[1], "jp", "JST");
				if (Rdt == null) {
					Console.WriteLine("Failed to restore account to advertise.");
					return;
				}
				EvertaleAPI.LoginData? Ld = EvertaleAPI.CreateSession(Rdt.UID, Rdt.CLID, "jp", "JST", 1, "Catgirls", "NekoOS");
				if (Ld == null) {
					Console.WriteLine($"Failed to create session to advertise: {Rdt.UID}, {Rdt.CLID}");
					return;
				}
				EvertaleUser Evt = new(Rdt.UID, Rdt.CLID, Ld.SessionID, "Catgirls", "NekoOS", 1, "jp", "JST");
				Console.WriteLine("Advertise session created.");
				EvertaleChat Ec;
				try {
					Ec = new(Evt);
				} catch {
					Console.WriteLine("[API Test] Failed to connect to chat.");
					throw;
				}
				if (Ec.Send("X" + Random.Shared.Next(0xFFFF) + "X lunahook.dev on top! Free rerolls for everyone! This account restore code is " + Args[1])) {
					Thread.Sleep(500);
				} else {
					Console.WriteLine("[API Test] Failed to send local message.");
					return;
				}
				if (Ec.Swap(true)) {
					Thread.Sleep(500);
				} else {
					Console.WriteLine("[API Test] Failed to swap to global.");
					return;
				}
				if (Ec.Send("X" + Random.Shared.Next(0xFFFF) + "X lunahook.dev on top! Free rerolls for everyone! This account restore code is " + Args[1])) {
					Thread.Sleep(500);
				} else {
					Console.WriteLine("[API Test] Failed to send global message.");
					return;
				}
				Console.WriteLine("Successfully advertised!");
			}
			break;
		}
	}
}