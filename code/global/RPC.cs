using Sandbox;
using Sandbox.UI;

namespace ActionTag
{
	public static partial class RPC
	{
		[ClientRpc]
		public static void SendMessage( string message )
		{
			ChatBox.AddInformation( message );
		}

		[ClientRpc]
		public static void OnScoreboardUpdate( Client client )
		{
			Scoreboard.Instance?.UpdatePlayer( client );
		}
	}
}
