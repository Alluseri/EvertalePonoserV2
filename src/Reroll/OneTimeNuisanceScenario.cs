using Alluseri.EvertalePonoserV2.API;
using System;

namespace Alluseri.EvertalePonoserV2.Reroll;

public class OneTimeNuisanceScenario {
	public string SessionID;
	public bool AutoQuitAlliance;

	public OneTimeNuisanceScenario(string SessionID, bool AutoQuitAlliance) {
		this.SessionID = SessionID;
		this.AutoQuitAlliance = AutoQuitAlliance;
	}

	public void Run() {
		ClaimFreeSR();
		ClaimAllianceJoin();
		ClaimTrainingDojo();
	}

	public void ClaimAllianceJoin() {
		if (EvertaleAPI.GuildAutoJoin(SessionID) != null) {
			EvertaleAPI.GuildLogin(SessionID);
			// TODO: Claim daily rewards before quitting, see DailyNuisanceScenario.ClaimAllianceShards
			if (AutoQuitAlliance) {
				EvertaleAPI.GuildQuit(SessionID);
				Console.WriteLine("Performed an alliance join/quit cycle. Claim through DailyNuisanceScenario.");
			} else
				Console.WriteLine("Autojoined an alliance. Claim rewards through DailyNuisanceScenario, quit manually.");
		} else
			Console.WriteLine("Failed to autojoin an alliance.");
	}

	public void ClaimFreeSR() {
		// This will break offline story first pass...
		/*EvertaleAPI.ClaimCode(SessionID, "tutgmob1");
		Console.WriteLine("Claimed free SR: Jedariel (tutgmob1).");*/
	}

	public void ClaimTrainingDojo() {

	}
}