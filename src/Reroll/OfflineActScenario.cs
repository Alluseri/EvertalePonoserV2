using Alluseri.EvertalePonoserV2.API;
using System;

namespace Alluseri.EvertalePonoserV2.Reroll;

public class OfflineActScenario {
	public string SessionID;
	public EvertaleAPI.ProfileData? Profile;

	public OfflineActScenario(string SessionID, EvertaleAPI.ProfileData? Profile) {
		this.SessionID = SessionID;
		this.Profile = Profile;
	}

	public void Run() {
		Chapter1();
		Chapter2();
		Chapter3();
		Chapter4();
		Chapter5();
		Chapter6();
	}

	/*
	Missing:
	act1rossetoriglandchest1, act1rossetoriglandchest6,
	act1toriglandtrialschest1

	Mispositioned:
	act1toriglandtrialschest2, ch1finnsroomchest1, act4dragonknightjoins, act5lutherbattle
	*/

	private void ClaimLog(string QuestName) {
		((int Gold, int Stones)? Currencies, int BattlePower)? Data = EvertaleAPI.ClaimQuest(SessionID, QuestName);
		if (Data == null) {
			Console.WriteLine("Failed to claim " + QuestName + ", continue?");
			Console.ReadLine();
		} else {
			Console.WriteLine("Claimed " + QuestName + " to have " + Data.Value.BattlePower + " BP" + (Data.Value.Currencies != null ? " and " + Data.Value.Currencies.Value.Stones + " soulstones." : "."));
		}
	}

	public void Chapter1() {
		ClaimLog("act1lightlingjoins");
		ClaimLog("act1desertlebattle");
		ClaimLog("act1startermonbattle");
		ClaimLog("act1secondossiabattle");
		if (Profile != null)
			EvertaleAPI.UpdateAccountSettings(SessionID, Profile.Value);
		ClaimLog("act1fireknightmale");
		ClaimLog("act1rossetoriglandchest4");
		ClaimLog("act1rossetoriglandchest2");
		ClaimLog("act1capedoutlander");
		ClaimLog("act1capedoutlanderjoins");
		ClaimLog("act1hammerpantsthief");
		ClaimLog("act1hammerpantsthiefjoins");
		ClaimLog("act1tasselmage");
		ClaimLog("act1firstfurcoatwolf");
		ClaimLog("ch1riglandchest1");
		ClaimLog("ch1riglandchest2");
		ClaimLog("ch1riglandchest3");
		ClaimLog("ch1riglandchest4");
		ClaimLog("ch1rknhqchest1");
		ClaimLog("ch1rknhqchest2");
		ClaimLog("ch1rknhqchest3");
		ClaimLog("ch1rknhqchest4");
		ClaimLog("act1fightergirl");
		ClaimLog("act1fightergirljoins");
		ClaimLog("act1pathtotrialchest1");
		ClaimLog("act1pathtotrialchest4");
		ClaimLog("act1firstbeastladyfighter");
		ClaimLog("act1trialschest1");
		ClaimLog("act1trialschest2");
		ClaimLog("act1trialencounter1");
		ClaimLog("act1trialschest3");
		ClaimLog("act1trialencounter2");
		ClaimLog("act1trialschest4");
		ClaimLog("act1trialschest5");
		ClaimLog("act1trialschest6");
		ClaimLog("act1trialschest7");
		ClaimLog("act1trialschest8");
		ClaimLog("act1trialboss");
		ClaimLog("act1chapter1complete");
	}

	public void Chapter2() {
		ClaimLog("ch1finnsroomchest1");
		ClaimLog("act2firstbrochewitch");
		ClaimLog("ch2riglandtokawazu2");
		ClaimLog("act2hoodedfrog");
		ClaimLog("act2hoodedfrogmanjoins");
		ClaimLog("ch2sidequest1");
		ClaimLog("act2firstjewelmermaid");
		ClaimLog("act2firstjewelmermaid");
		ClaimLog("pathtokisenkyo2");
		ClaimLog("pathtokisenkyo1");
		ClaimLog("act2hoodedthief");
		ClaimLog("act2hoodedthiefjoins");
		ClaimLog("pathtokisenkyo3");
		ClaimLog("act2secondbeastladyfighter");
		ClaimLog("act2pathtodungeon2");
		ClaimLog("act2pathtodungeon1");
		ClaimLog("act2facemaskfrog");
		ClaimLog("act2facemaskfrog");
		ClaimLog("act2tasselmage");
		ClaimLog("act2pathtodungeon3");
		ClaimLog("act2pathtodungeon4");
		ClaimLog("act2kisenkyobattle");
		ClaimLog("ch2kisenkyochest1");
		ClaimLog("ch2kisenkyochest2");
		ClaimLog("ch2kisenkyochest3");
		ClaimLog("act2dungeon00101");
		ClaimLog("act2dungeon00102");
		ClaimLog("act2dungeon00103");
		ClaimLog("act2dungeon00104");
		ClaimLog("act2dungeon00105");
		ClaimLog("act2cavesofreiaboss");
		ClaimLog("act2cavesofreiaboss");
		ClaimLog("act2aeonknightsbattle");
		ClaimLog("act2fourthossiabattle");
		ClaimLog("act2chapter2complete");
	}

	public void Chapter3() {
		ClaimLog("act3firstcrescentbladeelf");
		// Missing 
		ClaimLog("act3pathtomerizantechest1");
		ClaimLog("act3pathtomerizantechest2");
		ClaimLog("act3secondfireknightmale");
		ClaimLog("act3pathtomerizantechest3");
		ClaimLog("act3pathtomerizantechest5");
		ClaimLog("act3thirdbeastladyfighter");
		ClaimLog("act3tattooedshaman");
		ClaimLog("act3tattooedshamanjoins");
		ClaimLog("act3merizantetolugananchest3");
		ClaimLog("act3maleknight");
		ClaimLog("act3maleknightjoins");
		ClaimLog("ch3sidequest1");
		ClaimLog("act3merizantetolugananchest1");
		ClaimLog("act3merizantetolugananchest4");
		ClaimLog("ch3myshaengage");
		ClaimLog("ch3merizantechest2");
		ClaimLog("ch3merizantechest1");
		ClaimLog("ch3merizantechest3");
		ClaimLog("act3pathtolugananchest1");
		ClaimLog("act3pathtolugananchest2");
		ClaimLog("act3secondfurcoatwolf");
		ClaimLog("act3secondfurcoatwolfjoins");
		ClaimLog("ch3urgroachest1");
		ClaimLog("ch3urgroachest2");
		ClaimLog("act3druidlizard");
		ClaimLog("act3druidlizardjoins");
		ClaimLog("ch3xaheoyai");
		ClaimLog("ch3trainer");
		ClaimLog("ch3astridmyshaweird2");
		ClaimLog("ch3drukefound");
		ClaimLog("act3chapter3complete");
	}

	public void Chapter4() {
		ClaimLog("act1toriglandtrialschest2");
		ClaimLog("act4pathtonushellechest1");
		ClaimLog("act4secondjewelmermaid");
		ClaimLog("act4secondjewelmermaidjoins");
		ClaimLog("act4pathtonushellechest2");
		ClaimLog("act4pathtonushellechest4");
		ClaimLog("act4fireknightmale");
		ClaimLog("act4tohymeliatonushellchest1");
		ClaimLog("act4facemaskfrogjoins");
		ClaimLog("act4facemaskfrog");
		ClaimLog("act4tohymeliatonushellchest2");
		ClaimLog("act4tohymeliatonushellchest3");
		ClaimLog("act4pathtodarkforestchest1");
		ClaimLog("act4pathtodarkforestchest5");
		ClaimLog("act4pathtodarkforestchest3");
		ClaimLog("act4pathtodarkforestchest2");
		ClaimLog("act4dragonknight");
		ClaimLog("ch4sidequest2");
		ClaimLog("act4redelfspy");
		ClaimLog("act4darkforestchest2");
		ClaimLog("act4forestmonsterbattle");
		ClaimLog("act4darkforestchest4");
		ClaimLog("act4darkforestchest6");
		ClaimLog("act4darkforestchest7");
		ClaimLog("act4elminabattle");
		ClaimLog("elminafoodsupplyqid");
		ClaimLog("act4knightmonsterbattle");
		ClaimLog("act4firstgyurellebattle");
		ClaimLog("act4secondgyurellebattle");
		ClaimLog("act4chapter4complete");
	}

	public void Chapter5() {
		ClaimLog("act5firstredelfspy");
		ClaimLog("act5newforestchest1");
		ClaimLog("act5newforestchest3");
		ClaimLog("act5blackknight");
		ClaimLog("act4dragonknightjoins");
		ClaimLog("act5toelvenlenttosilverdrakechest1");
		ClaimLog("act5toelvenlenttosilverdrakechest4");
		ClaimLog("act5toelvenlenttosilverdrakechest2");
		ClaimLog("ch5elvenlentchest1");
		ClaimLog("ch5elvenlentchest2");
		ClaimLog("ch5elvenlentchest3");
		ClaimLog("ch5callenslabchest4");
		ClaimLog("ch5callenslabchest3");
		ClaimLog("ch5callenslabchest2");
		ClaimLog("ch5callenslabchest1");
		ClaimLog("act5toelvenlenttosilverdrakechest6");
		ClaimLog("act5redelfspyjoins");
		ClaimLog("act5secondredelfspy");
		ClaimLog("act5secondcrescentbladeelf");
		ClaimLog("act5firstgamlanminesbattle");
		ClaimLog("act5secondgamlanminesbattle");
		ClaimLog("act5thirdgamlanminesbattle");
		ClaimLog("act5fourthgamlanminesbattle");
		ClaimLog("act5tomineschest1");
		ClaimLog("ch5assassinsbattle");
		ClaimLog("act5chapter5complete");
	}

	public void Chapter6() {
		ClaimLog("act5lutherbattle");
		ClaimLog("act6fourthbeastladyfighter");
		ClaimLog("act6fourthbeastladyfighterjoins");
		ClaimLog("act6pathtosilverlodgechest2");
		ClaimLog("act6pathtosilverlodgechest3");
		ClaimLog("act6frogknight");
		ClaimLog("act6frogknightjoins");
		ClaimLog("act6pathtosilverlodgechest6");
		ClaimLog("act6pathtosilverlodgechest5");
		ClaimLog("act6blackknight");
		ClaimLog("act6venshadayne");
		ClaimLog("act6thirdcrescentbladeelfjoins");
		ClaimLog("ch6silverlodgechest2");
		ClaimLog("ch6silverlodgechest1");
		ClaimLog("act6firstsilverlodgesiegebattle");
		ClaimLog("act6secondsilverlodgesiegebattle");
		ClaimLog("ch6silverlodgetavernchest1");
		ClaimLog("ch6silverlodgetavernchest2");
		ClaimLog("act6seconddragonknight");
		ClaimLog("act6seconddragonknightjoins");
		ClaimLog("act6fireknightmale");
		ClaimLog("act6aeonknightbattle1");
		ClaimLog("act6toaeontowerchest3");
		ClaimLog("act6toaeontowerchest1");
		ClaimLog("act6firstnorzabattle");
		ClaimLog("act6aeonknightbattle2");
		ClaimLog("act6aeonknightbattle3");
		ClaimLog("act6aeonknightbattle4");
		ClaimLog("act6aeonknighttowerbattle1");
		ClaimLog("act6aeonknighttowerbattle2");
		ClaimLog("act6aeonknighttowerbattle3");
		ClaimLog("act6secondnorzabattle");
		ClaimLog("act6thirdnorzabattle");
		ClaimLog("act6aeonknighttowerbattle4");
		ClaimLog("act6eternalbattle");
		ClaimLog("act6secondarcanebattle");
		ClaimLog("chapter6complete");
	}

	public void HackSidequests() {
		ClaimLog("ch1sidequest1");
		ClaimLog("ch1sidequest2");
		ClaimLog("ch1sidequest3");
		ClaimLog("ch2sidequest1");
		ClaimLog("ch2sidequest2");
		ClaimLog("ch3sidequest1");
		ClaimLog("ch3sidequest2");
		ClaimLog("ch4sidequest1");
		ClaimLog("ch4sidequest2");
		ClaimLog("ch5sidequest1");
		ClaimLog("ch6sidequest1");
	}

	public void CompleteMissing() {
		ClaimLog("act1rossetoriglandchest1");
		ClaimLog("act1rossetoriglandchest6");
		ClaimLog("act1toriglandtrialschest1");
	}
}