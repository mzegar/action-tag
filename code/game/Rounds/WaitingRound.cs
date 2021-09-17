using System.Threading.Tasks;
using Sandbox;

namespace ActionTag
{
	public class WaitingRound : BaseRound
	{
		public override string RoundName => "Waiting for players";

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

		public override void OnPlayerSpawn( ActionTagPlayer player )
		{
			base.OnPlayerSpawn( player );
			
			player.SetTeam( ActionTagGame.Instance.NoneTeam );
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
					player.Controller.IsFrozen = false;
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
