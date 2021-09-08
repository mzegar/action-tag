using Sandbox;

namespace ActionTag
{
	public partial class Game : Sandbox.Game
	{
		public Game()
		{
			if ( IsServer )
			{
				new Hud();
			}
		}
		
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			var player = new Player();
			client.Pawn = player;

			player.Respawn();
		}
	}
}
