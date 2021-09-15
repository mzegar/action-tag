using System.Collections.Generic;
using System.Threading.Tasks;
using ActionTag.Teams;
using Sandbox;

namespace ActionTag
{
	public partial class ActionTagGame : Sandbox.Game
	{
		public static ActionTagGame Instance { get; private set; }

		[Net]
		public BaseRound Round { get; private set; } = new WaitingRound();
		
		public NoneTeam NoneTeam { get; }
		public RunnerTeam RunnerTeam { get; }
		public ChasersTeam ChasersTeam { get; }

		public ActionTagGame()
		{
			Instance = this;
			
			if ( IsServer )
			{
				_ = new ActionTagHud();
			}
			
			NoneTeam = new NoneTeam();
			RunnerTeam = new RunnerTeam();
			ChasersTeam = new ChasersTeam();

			_ = StartGameTimer();
		}

		/// <summary>
		/// Changes the round if minimum players is met. Otherwise, force changes to "WaitingRound"
		/// </summary>
		/// <param name="round"> The round to change to if minimum players is met.</param>
		public void ChangeRound(BaseRound round)
		{
			Assert.NotNull(round);

			ForceRoundChange(Utils.HasMinimumPlayers() ? round : new WaitingRound());
		}

		/// <summary>
		/// Force changes a round regardless of player count.
		/// </summary>
		/// <param name="round"> The round to change to.</param>
		public void ForceRoundChange(BaseRound round)
		{
			Round.Finish();
			Round = round;
			Round.Start();
		}
		
		private async Task StartGameTimer()
		{
			ForceRoundChange(new WaitingRound());

			while (true)
			{
				Round?.OnSecond();
				await Task.NextPhysicsFrame();
			}
		}

		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			var player = new ActionTagPlayer();
			client.Pawn = player;

			if ( Round is WaitingRound )
			{
				player.Respawn();
			}
			else
			{
				player.MakeSpectator();
			}
		}

		public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
		{
			base.ClientDisconnect( cl, reason );

			Round?.OnPlayerLeave( cl.Pawn );
		}

		public override void OnKilled( Entity entity)
		{
			if ( entity is ActionTagPlayer player )
			{
				Round?.OnPlayerKilled( player );
			}

			base.OnKilled( entity);
		}
	}
}
