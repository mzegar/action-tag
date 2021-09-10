using System.Threading.Tasks;
using Sandbox;

namespace ActionTag
{
	public class WaitingRound : BaseRound
	{
		public override string RoundName => "Waiting for more players";

		public override void OnSecond()
		{
			if ( Host.IsServer && Utils.HasMinimumPlayers() )
			{ 
				ActionTagGame.Instance.ForceRoundChange( new PreRound() );
			}
		}

		public override void OnPlayerKilled( ActionTagPlayer player )
		{
			_ = StartRespawnTimer( player );

			base.OnPlayerKilled( player );
		}

		protected override void OnStart()
		{
			if ( !Host.IsServer )
			{
				return;
			}

			foreach ( var client in Client.All )
			{
				if ( client.Pawn is ActionTagPlayer player )
				{
					player.Respawn();
				}
			}
		}

		private static async Task StartRespawnTimer( Player player )
		{
			await Task.Delay( 1000 );

			if ( player.IsValid() && ActionTagGame.Instance.Round is WaitingRound )
			{
				player.Respawn();
			}
		}
	}
}
