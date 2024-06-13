namespace Alluseri.EvertalePonoserV2.API;

public class EvertaleUser {
	public readonly string UserID;
	public readonly string SessionID;
	public readonly string Language;
	public readonly string Region;
	public readonly int? Shard;
	public readonly string? CLID;
	public readonly string? Device;
	public readonly string? OS;
	public readonly string? RCode;

	public EvertaleUser(string UserID, string? CLID, string SessionID, string? Device, string? OS, int? Shard, string Language, string Region, string? RCode = null) {
		this.UserID = UserID;
		this.SessionID = SessionID;
		this.Language = Language;
		this.Region = Region;
		this.Shard = Shard;
		this.CLID = CLID;
		this.Device = Device;
		this.OS = OS;
		this.RCode = RCode;
	}

	public EvertaleAPI.LoginData? Relog()
	=> EvertaleAPI.CreateSession(UserID, CLID!, Language, Region, Shard!.Value, Device!, OS!);

	public static EvertaleUser? RegisterNew(string Device, string OS, int Shard, string Language, string Region, EvertaleAPI.ProfileData? Data) {
		EvertaleAPI.NewAccountData? NAD = EvertaleAPI.RegisterAccount(Device, OS, Shard, Language, Region);
		if (NAD == null)
			return null;
		EvertaleAPI.LoginData? LD = EvertaleAPI.CreateSession(NAD.UID, NAD.CLID, Device, OS, Shard, Language, Region);
		if (LD == null)
			return null;
		if (Data.HasValue)
			EvertaleAPI.UpdateAccountSettings(LD.SessionID, Data.Value);
		return new EvertaleUser(NAD.UID, NAD.CLID, LD.SessionID, Device, OS, Shard, Language, Region, NAD.RCode);
	}
}