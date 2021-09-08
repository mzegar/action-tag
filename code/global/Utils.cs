using Sandbox;

namespace ActionTag
{
	public static partial class Utils
	{
		public static bool HasMinimumPlayers()
		{
			return Client.All.Count >= ActionTagGameSettings.MinimumPlayers;
		}
	}
}
