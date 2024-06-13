using Alluseri.EvertalePonoserV2.Network;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Alluseri.EvertalePonoserV2.API;

public static class EvertaleAPI {
	public const string EvertaleVersion = "2.0.83"; // HAHAHA THIS HASNT EVEN CHANGED SINCE I LAST OPENED THIS FILE
	public const string UserAgent = "UnityPlayer/2021.3.10f1 (UnityWebRequest/1.0, libcurl/7.80.0-DEV)";
	public const string UnityVersion = "2021.3.10f1";

	public static NewAccountData? RegisterAccount(string Device, string OS, int Shard, string Language, string Region) {
		try {
			return JObject.Parse(Http.Post("https://apialt.prd.evertaleserver.com/newuser", new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["platform"] = "android",
				["device"] = Device,
				["os"] = OS,
				["adid"] = "unknown",
				["shard"] = Shard.ToString(),
				["req"] = "newuser",
				["lang"] = Language,
				["region"] = Region,
				["requnique"] = "1"
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion)
			}).Gunzip())["newuser"]?.ToObject<NewAccountData>();
		} catch (Exception e) {
			Console.WriteLine(e);
			throw;
		}
	}

	public static LoginData? CreateSession(string UserID, string CLID, string Language, string Region, int Shard, string Device, string OS) {
		try {
			return JObject.Parse(Http.Post("https://apialt.prd.evertaleserver.com/login", new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["platform"] = "android",
				["device"] = Device,
				["os"] = OS,
				["adid"] = "unknown",
				["shardpick"] = Shard.ToString(),
				["lang"] = Language,
				["region"] = Region,
				["requnique"] = "1",
				["uid"] = UserID,
				["clid"] = CLID,
				["bundle"] = "com.zigzagame.evertale",
				["ver"] = EvertaleVersion,
				["req"] = "login",
				["unique"] = Guid.NewGuid().ToString()
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion)
			}).Gunzip())["login"]?.ToObject<LoginData>();
		} catch (Exception e) {
			Console.WriteLine(e);
			return null;
		}
	}

	public static RestoreData? RestoreAccount(string Code, string Language, string Region) { // Ported indirectly from old sources
		try {
			return JObject.Parse(Http.Post("https://apialt.prd.evertaleserver.com/recover", new FormUrlEncodedContent(new Dictionary<string, string?>() {
				{"mcode", Code},
				{"lang", Language},
				{"region", Region},
				{"requnique", "8"},
				{"req", "recover"}
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion)
			}).Gunzip())["recover"]?.ToObject<RestoreData>();
		} catch (Exception e) {
			Console.WriteLine(e);
			return null;
		}
	}

	public static void UpdateAccountSettings(string SessionID, ProfileData Data) {
		try {
			Http.NoPost("https://apialt.prd.evertaleserver.com/usersettings", new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["args"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Data, Formatting.None))),
				["sesid"] = SessionID,
				["requnique"] = "1",
				["reqid"] = "1",
				["req"] = "usersettings"
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion),
				("evertale", SessionID)
			});
		} catch (Exception e) {
			Console.WriteLine(e);
			return;
		}
	}

	public static ((int Gold, int Stones)? Currencies, int BattlePower)? ClaimQuest(string SessionID, string QuestName) {
		try {
			JObject Quest = JObject.Parse(Http.Post("https://apialt.prd.evertaleserver.com/questclaim", new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["qid"] = QuestName,
				["sesid"] = SessionID,
				["requnique"] = "1",
				["reqid"] = "1",
				["req"] = "questclaim"
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion),
				("evertale", SessionID)
			}).Gunzip());
			int? BattlePower = Quest["totalpower"]?.Value<int>("tp");
			if (BattlePower == null)
				return null;
			if (Quest["currencies"] != null) {
				return ((Quest["currencies"]!.Value<int>("gold"), Quest["currencies"]!.Value<int>("stones")), BattlePower.Value);
			} else {
				return (null, BattlePower.Value);
			}
		} catch (Exception e) {
			Console.WriteLine(e);
			return null;
		}
	}

	public static int? Invest(string SessionID, string ShopName) {
		try {
			JObject Investment = JObject.Parse(Http.Post("https://apialt.prd.evertaleserver.com/invest", new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["shop"] = ShopName,
				["sesid"] = SessionID,
				["requnique"] = "1",
				["reqid"] = "1",
				["req"] = "invest"
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion),
				("evertale", SessionID)
			}).Gunzip());
			return Investment["currencies"]?.Value<int>("stones");
		} catch (Exception e) {
			Console.WriteLine(e);
			return null;
		}
	}

	public static void ClaimCode(string SessionID, string Code) {
		try {
			Http.NoPost("https://apialt.prd.evertaleserver.com/codeclaim", new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["code"] = Code,
				["sesid"] = SessionID,
				["requnique"] = "1",
				["reqid"] = "1",
				["req"] = "codeclaim"
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion),
				("evertale", SessionID)
			});
		} catch (Exception e) {
			Console.WriteLine(e);
			return;
		}
	}

	public static void NotificationsCheck(string SessionID) {
		try {
			Http.NoPost("https://apialt.prd.evertaleserver.com/notificationscheck", new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["sesid"] = SessionID,
				["requnique"] = "1",
				["reqid"] = "1",
				["req"] = "notificationscheck"
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion),
				("evertale", SessionID)
			});
		} catch (Exception e) {
			Console.WriteLine(e);
			return;
		}
	}

	public static void ConquestFight(string SessionID) {
		try {
			Http.NoPost("https://apialt.prd.evertaleserver.com/conquest", new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["sesid"] = SessionID,
				["requnique"] = "1",
				["reqid"] = "1",
				["req"] = "conquest"
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion),
				("evertale", SessionID)
			});
		} catch (Exception e) {
			Console.WriteLine(e);
			return;
		}
	}

	public static void ConquestChestClaim(string SessionID) {
		try {
			Http.NoPost("https://apialt.prd.evertaleserver.com/conquest", new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["chestclaim"] = "1",
				["sesid"] = SessionID,
				["requnique"] = "1",
				["reqid"] = "1",
				["req"] = "conquest"
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion),
				("evertale", SessionID)
			});
		} catch (Exception e) {
			Console.WriteLine(e);
			return;
		}
	}

	public static string[]? GetLoginRewards(string SessionID) {
		try {
			JObject Rewards = JObject.Parse(Http.Post("https://apialt.prd.evertaleserver.com/loginrewards", new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["sesid"] = SessionID,
				["requnique"] = "1",
				["reqid"] = "1",
				["req"] = "loginrewards"
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion),
				("evertale", SessionID)
			}).Gunzip());
			return Rewards["loginrewards"]?.Value<JArray>("pending")?.ToObject<string[]>();
		} catch (Exception e) {
			Console.WriteLine(e);
			return null;
		}
	}

	public static void ClaimLoginReward(string SessionID, string RewardID) {
		try {
			Http.NoPost("https://apialt.prd.evertaleserver.com/claimlogin", new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["track"] = RewardID,
				["sesid"] = SessionID,
				["requnique"] = "1",
				["reqid"] = "1",
				["req"] = "claimlogin"
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion),
				("evertale", SessionID)
			});
		} catch (Exception e) {
			Console.WriteLine(e);
			return;
		}
	}

	public static string[]? GetLootList(string SessionID) {
		try {
			JObject Result = JObject.Parse(Http.Post("https://apialt.prd.evertaleserver.com/lootlist", new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["sesid"] = SessionID,
				["requnique"] = "1",
				["reqid"] = "1",
				["req"] = "lootlist"
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion),
				("evertale", SessionID)
			}).Gunzip());
			return Result.Value<JObject>("lootlist")?.Properties().Select(Prop => Prop.Name).ToArray();
		} catch (Exception e) {
			Console.WriteLine(e);
			return null;
		}
	}

	public static string?[]? GetArenaEnemies(string SessionID) { // The request also allows to know enemy BP, attempts and stuff, but wdc about those
		try {
			JObject Rewards = JObject.Parse(Http.Post("https://apialt.prd.evertaleserver.com/screenvalues", new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["sesid"] = SessionID,
				["requnique"] = "1",
				["reqid"] = "1",
				["req"] = "screenvalues",
				["replays"] = "1",
				["screen"] = "Arena",
				["activeLineup"] = "0",
				["activeEventLineup"] = "0"
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion),
				("evertale", SessionID)
			}).Gunzip());
			return Rewards["screenvalues"]?["objects"]?["Opponents"]?.Value<JArray>("kvlist")?.Select(ArenaEntry => ArenaEntry["request"]?["args"]?.Value<string>("challenge")).ToArray();
		} catch (Exception e) {
			Console.WriteLine(e);
			return null;
		}
	}

	public static bool? ArenaFight(string SessionID, string PlayerId) {
		try {
			JObject Result = JObject.Parse(Http.Post("https://apialt.prd.evertaleserver.com/arena", new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["sesid"] = SessionID,
				["requnique"] = "1",
				["reqid"] = "1",
				["req"] = "arena",
				["challenge"] = PlayerId
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion),
				("evertale", SessionID)
			}).Gunzip());
			string? ReplayStr = Result["arena"]?.Value<string>("replay");
			bool? Victory = null;
			if (ReplayStr != null) {
				Victory = JObject.Parse(ReplayStr).Value<bool>("win");
			}
			Http.NoPost("https://apialt.prd.evertaleserver.com/userdata", new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["uid"] = PlayerId,
				["sesid"] = SessionID,
				["requnique"] = "1",
				["reqid"] = "1",
				["req"] = "userdata"
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion),
				("evertale", SessionID)
			});
#if DEBUG
			if (Victory == null) {
				Console.WriteLine("[DEBUG] ArenaFight = null: " + Result);
			}
#endif
			return Victory;
		} catch (Exception e) {
			Console.WriteLine(e);
			return null;
		}
	}

	public static (int Gold, int Stones)? ClaimLoot(string SessionID, string[] Loot) {
		try {
			Dictionary<string, string?> Body = new() {
				["sesid"] = SessionID,
				["requnique"] = "1",
				["reqid"] = "1",
				["req"] = "lootclaim"
			};
			foreach (string Id in Loot) {
				Body[Id] = "1";
			}
			JToken? Claimed = JObject.Parse(Http.Post("https://apialt.prd.evertaleserver.com/lootclaim", new FormUrlEncodedContent(Body), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion),
				("evertale", SessionID)
			}).Gunzip())["currencies"];
			return Claimed == null ? null : (Claimed.Value<int>("gold"), Claimed.Value<int>("stones"));
		} catch (Exception e) {
			Console.WriteLine(e);
			return null;
		}
	}

	public static string? GuildAutoJoin(string SessionID) {
		try {
			JObject Result = JObject.Parse(Http.Post("https://apialt.prd.evertaleserver.com/gautojoin", new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["sesid"] = SessionID,
				["requnique"] = "1",
				["reqid"] = "1",
				["req"] = "gautojoin"
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion),
				("evertale", SessionID)
			}).Gunzip());
			return Result["gautojoin"]?.Value<string?>("gid");
		} catch (Exception e) {
			Console.WriteLine(e);
			return null;
		}
	}

	public static void GuildLogin(string SessionID) {
		try {
			Http.NoPost("https://apialt.prd.evertaleserver.com/glogin", new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["sesid"] = SessionID,
				["requnique"] = "1",
				["reqid"] = "1",
				["req"] = "glogin"
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion),
				("evertale", SessionID)
			});
		} catch (Exception e) {
			Console.WriteLine(e);
			return;
		}
	}

	public static void GuildQuit(string SessionID) {
		try {
			Http.NoPost("https://apialt.prd.evertaleserver.com/redisobject", new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["sesid"] = SessionID,
				["reqid"] = "1",
				["type"] = "gquits"
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion),
				("evertale", SessionID)
			});
			Http.NoPost("https://apialt.prd.evertaleserver.com/gquit", new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["sesid"] = SessionID,
				["requnique"] = "1",
				["reqid"] = "1",
				["req"] = "gquit"
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion),
				("evertale", SessionID)
			});
		} catch (Exception e) {
			Console.WriteLine(e);
			return;
		}
	}

	public enum MissionType {
		Daily, Lifetime
	}

	private static string Query(this MissionType T, string Query) => (T == MissionType.Lifetime ? 'l' : 'd') + Query;

	public static (string Task, MissionCompletion State)[]? GetMissionList(string SessionID, MissionType Type) {
		try {
			string Q = Type.Query("msnlist");
			JObject Result = JObject.Parse(Http.Post("https://apialt.prd.evertaleserver.com/" + Q, new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["sesid"] = SessionID,
				["requnique"] = "1",
				["reqid"] = "1",
				["req"] = Q
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion),
				("evertale", SessionID)
			}).Gunzip());
			if (Result[Q] == null)
				return null;
			JObject Dmsn = Result.Value<JObject>(Q)!;
			(string Task, MissionCompletion State)[] Tasks = new (string Task, MissionCompletion State)[Dmsn.Count];
			int i = 0; // lol
			foreach (JObject Task in Dmsn.Values()) {
				Tasks[i++] = (Task.Value<string>("task")!, Task.Value<string>("status") switch {
					"completed" => MissionCompletion.Completed,
					"not_started" => MissionCompletion.NotStarted,
					"in_progress" => MissionCompletion.InProgress,
					_ => MissionCompletion.Claimed, // safety "claimed"
				});
			}
			return Tasks;
		} catch (Exception e) {
			Console.WriteLine(e);
			return null;
		}
	}

	public static void ClaimMission(string SessionID, MissionType Type, string Task) {
		try {
			string Q = Type.Query("msnclaim");
			Http.NoPost("https://apialt.prd.evertaleserver.com/" + Q, new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["sesid"] = SessionID,
				["requnique"] = "1",
				["reqid"] = "1",
				["req"] = Q,
				["name"] = Task
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion),
				("evertale", SessionID)
			});
		} catch (Exception e) {
			Console.WriteLine(e);
			return;
		}
	}

	public static JObject? QueryGachaGeneric(string SessionID) {
		try {
			return JObject.Parse(Http.Post("https://apialt.prd.evertaleserver.com/gacha", new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["sesid"] = SessionID,
				["requnique"] = "1",
				["reqid"] = "1",
				["req"] = "gacha"
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion),
				("evertale", SessionID)
			}).Gunzip());
		} catch (Exception e) {
			Console.WriteLine(e);
			return null;
		}
	}

	public static void ClaimGachaChest(string SessionID, string Chest) {
		try {
			Http.NoPost("https://apialt.prd.evertaleserver.com/gacha", new FormUrlEncodedContent(new Dictionary<string, string?>() {
				["sesid"] = SessionID,
				["requnique"] = "1",
				["reqid"] = "1",
				["req"] = "gacha",
				["chest"] = Chest
			}), new[] {
				("Content-Type", "application/x-www-form-urlencoded;charset=UTF-8"),
				("User-Agent", UserAgent),
				("X-Unity-Version", UnityVersion),
				("evertale", SessionID)
			});
		} catch (Exception e) {
			Console.WriteLine(e);
			return;
		}
	}

	public enum MissionCompletion {
		NotStarted, InProgress, Completed, Claimed
	}

	[JsonObject]
	public class RestoreData {
		[JsonProperty("uid")]
		public string UID;
		[JsonProperty("clid")]
		public string CLID;

		[JsonConstructor]
		public RestoreData(string UID, string CLID) {
			this.UID = UID;
			this.CLID = CLID;
		}
	}

	[JsonObject]
	public class NewAccountData {
		[JsonProperty("uid")]
		public string UID;
		[JsonProperty("clid")]
		public string CLID;
		[JsonProperty("fcode")]
		public string FCode;
		[JsonProperty("rcode")]
		public string RCode;
		[JsonProperty("created")]
		public long Created;

		[JsonConstructor]
		public NewAccountData(string UID, string CLID, string FCode, string RCode, long Created) {
			this.UID = UID;
			this.CLID = CLID;
			this.FCode = FCode;
			this.RCode = RCode;
			this.Created = Created;
		}
	}

	[JsonObject]
	public class LoginData {
		[JsonProperty("sesid")]
		public string SessionID;
		[JsonProperty("flags")]
		public string[] Flags;
		[JsonProperty("replays")]
		public int Replays;
		[JsonProperty("shardOffset")]
		public int ShardOffset;
		[JsonProperty("created")]
		public long Created;

		[JsonConstructor]
		public LoginData(string SessionID, string[] Flags, int Replays, int ShardOffset, long Created) {
			this.SessionID = SessionID;
			this.Flags = Flags;
			this.Replays = Replays;
			this.ShardOffset = ShardOffset;
			this.Created = Created;
		}
	}

	[JsonObject]
	public struct ProfileData {
		[JsonProperty("name")]
		public string Name;
		[JsonProperty("message", NullValueHandling = NullValueHandling.Include)]
		public string? Description = null;
		[JsonProperty("favorite", NullValueHandling = NullValueHandling.Include)]
		public string? Favorite = null;
		[JsonProperty("gender")]
		public Gender Gender = Gender.None;
		[JsonProperty("blockQuakes")]
		public bool BlockQuakes = false;
		[JsonProperty("blockCrumbs")]
		public bool BlockCrumbs = false;

		public ProfileData(string Name) {
			this.Name = Name;
		}
	}

	public enum Gender : int {
		None, Male, Female, Other
	}
}