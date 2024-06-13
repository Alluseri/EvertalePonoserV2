using Alluseri.EvertalePonoserV2.API;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;

#pragma warning disable IDE0022, CA1822, IDE0220, IDE0008

namespace Alluseri.EvertalePonoserV2.Reroll;

public class DailyNuisanceScenario {
	public string SessionID;
	public bool AllianceAvailable;
	public bool ArenaAvailable;

	public DailyNuisanceScenario(string SessionID, bool AllianceAvailable, bool ArenaAvailable) {
		this.SessionID = SessionID;
		this.AllianceAvailable = AllianceAvailable;
		this.ArenaAvailable = ArenaAvailable;
	}

	public void Run() {
		FakeNotificationCheck();
		ClaimLoginRewards();
		ClaimSummonChests();
		ClaimAllianceShards();
		ClaimTowerChest();
		TowerFastForward();
		TowerFastForward();
		TowerFight();
		TowerExplore();
		if (ArenaAvailable)
			ArenaFight(ArenaFight(ArenaFight(ArenaFight(ArenaFight(null)))));
		// Please call after OnlineActScenario or EventScenario
		ClaimDailyQuests();
		ClaimDailyQuests();
		ClaimLifetimeQuests();
		ClaimOtherFreebies();
		ClaimHighActivity();
		ClaimMail();
		ClaimFountain();
	}

	public void FakeNotificationCheck() {
		EvertaleAPI.NotificationsCheck(SessionID);
	}

	public string? ArenaFight(string? PlayerId) {
		if (PlayerId == null) {
			string?[]? Enemies = EvertaleAPI.GetArenaEnemies(SessionID);
			if (Enemies == null || Enemies.Length == 0) {
				Console.WriteLine("Couldn't query arena enemies, skipping...");
				return null;
			}

			PlayerId = Enemies[0] ?? Enemies[1] ?? Enemies[2]; // pcodenz
			if (PlayerId == null) {
				Console.WriteLine("Couldn't find a nonnull enemy for arena, skipping...");
				return null;
			}
		}

		bool? V = EvertaleAPI.ArenaFight(SessionID, PlayerId);
		Console.WriteLine(V == null ? $"Couldn't determine victory against {PlayerId}, request failed?" : $"Arena fight vs {PlayerId}: {(V.Value ? "Victory!" : "Loss!")}");

		Thread.Sleep(6601); // Evertale moment

		return V.HasValue && V.Value ? PlayerId : null;
	}

	public void TowerExplore() {
		Console.WriteLine("Stub for tower exploration: unavailable due to level rq.");
	}

	public void TowerFight() {
		EvertaleAPI.ConquestFight(SessionID);
	}

	public void ClaimTowerChest() {
		EvertaleAPI.ConquestChestClaim(SessionID);
	}

	public void TowerFastForward() {
		Console.WriteLine("Stub for tower fast forward: unavailable due to level rq.");
	}

	public void ClaimHighActivity() {
		Console.WriteLine("Stub for high activity claim: unresearched.");
	}

	public void ClaimLoginRewards() {
		string[]? Rewards = EvertaleAPI.GetLoginRewards(SessionID);
		if (Rewards == null) {
			Console.WriteLine("Couldn't query login rewards, skipping...");
			return;
		}
		foreach (string Id in Rewards) {
			EvertaleAPI.ClaimLoginReward(SessionID, Id);
			Console.WriteLine("Claimed login reward: " + Id);
		}
	}

	public void ClaimSummonChests() {
		JArray? Available = EvertaleAPI.QueryGachaGeneric(SessionID)?["gacha"]?.Value<JArray>("chests");
		if (Available == null) {
			Console.WriteLine("Couldn't query summon chests to claim, skipping...");
			return;
		}
		foreach (string? Chest in Available) {
			if (Chest == null)
				continue;
			EvertaleAPI.ClaimGachaChest(SessionID, Chest);
			Console.WriteLine("Claimed gacha chest: " + Chest);
		}
	}

	public void ClaimAllianceShards() {
		if (AllianceAvailable) {
			Console.WriteLine("Stub for claim alliance shards: unresearched.");
			// Check if new portion of ss is available

			// Claim it!
		}
	}

	public void ClaimLifetimeQuests() {
		var Lifetimes = EvertaleAPI.GetMissionList(SessionID, EvertaleAPI.MissionType.Lifetime);
		if (Lifetimes == null) {
			Console.WriteLine("Couldn't query lifetime mission list, skipping...");
			return;
		}
		foreach ((string Task, EvertaleAPI.MissionCompletion State) Quest in Lifetimes) {
			if (Quest.State == EvertaleAPI.MissionCompletion.Completed) {
				EvertaleAPI.ClaimMission(SessionID, EvertaleAPI.MissionType.Lifetime, Quest.Task);
				Console.WriteLine("Claimed lifetime: " + Quest.Task);
			}
		}
	}

	public void ClaimDailyQuests() {
		var Dailies = EvertaleAPI.GetMissionList(SessionID, EvertaleAPI.MissionType.Daily);
		if (Dailies == null) {
			Console.WriteLine("Couldn't query daily mission list, skipping...");
			return;
		}
		foreach ((string Task, EvertaleAPI.MissionCompletion State) Quest in Dailies) {
			if (Quest.State == EvertaleAPI.MissionCompletion.Completed) {
				EvertaleAPI.ClaimMission(SessionID, EvertaleAPI.MissionType.Daily, Quest.Task);
				Console.WriteLine("Claimed daily: " + Quest.Task);
			}
		}
	}

	public void ClaimOtherFreebies() {
		/*
			/iapshops with an unknown additional parameter abTest will have [response]->iapshops->shops and ->subshops as follows:
			{
				HasClaim - might be absent from the json, you're looking for this to be set to true in ->shops
				chestClaimable - only present in ->subshops, unknown presence but you're looking for this to be set to true
				shop/subShop - corresponds to shop/sub parameters below
			}
			To claim shops, use /iapfreeclaim with additional parameters shop and product(? how to get it)
			To claim subshops, use /iaplist with additional parameters sub and chest=1
		*/
		Console.WriteLine("Stub for claim other freebies: unresearched.");
	}

	public void ClaimFountain() {
		// Check /iapshops (see full route above) for BeginnerInvestmentShop or (???), it will provide investmentSteps and currentStep
		/*
		int[] Limits = new int[] { 60, 600, 6000, 12000 };
		int Lim = 1;
		int? Stones;
		do {
			Stones = EvertaleAPI.Invest(SessionID, "BeginnerInvestmentShop");
			if (Stones == null) {
				Console.WriteLine("Couldn't invest into the beginner fountain, latest known value of ss: " + Stones + ".");
				return;
			} else
				Console.WriteLine("Invested into the beginner fountain to have " + Stones + " ss.");
		} while (Lim >= Limits.Length || Stones <= Limits[Lim++]);*/
		Console.WriteLine("Stub for claim fountain: unresearched.");
	}

	public void ClaimMail() {
		string[]? LootList = EvertaleAPI.GetLootList(SessionID);
		if (LootList == null) {
			Console.WriteLine("Couldn't query lootlist, skipping...");
			return;
		}
		EvertaleAPI.ClaimLoot(SessionID, LootList);
		Console.WriteLine("Claimed all available loot(Mail) through Claim All button.");
	}
}