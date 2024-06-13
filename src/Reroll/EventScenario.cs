using Alluseri.EvertalePonoserV2.API;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;

#pragma warning disable IDE0022, CA1822

namespace Alluseri.EvertalePonoserV2.Reroll;

public class EventScenario {
	public string SessionID;

	public EventScenario(string SessionID) {
		this.SessionID = SessionID;
	}

	public void Run() {
		UpdateEventShops();
		Grind(DetermineGrind());
		ClaimEventShops();
	}

	public void UpdateEventShops() {

	}

	public GrindData? DetermineGrind() {
		return default;
	}

	public void Grind(GrindData? Data) {

	}

	public void ClaimEventShops() {

	}

	public struct GrindData {
		public string Target;
		public int Cost;
	}
}