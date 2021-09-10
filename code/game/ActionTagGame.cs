using System.Collections.Generic;
using System.Threading.Tasks;
using ActionTag;
using ActionTag.Teams;
using Sandbox;

namespace ActionTag
{
	public partial class ActionTagGame : Sandbox.Game
	{
		public static ActionTagGame Instance { get; private set; }

		[Net]
		public BaseRound Round { get; private set; } = new WaitingRound();
		
		private List<BaseTeam> _teams;
		public NoneTeam NoneTeam { get; set; }
		public RunnerTeam RunnerTeam { get; set; }
		public TaggerTeam TaggerTeam { get; set; }

		public ActionTagGame()
		{
			Instance = this;
			
			if ( IsServer )
			{
				_ = new ActionTagHud();
			}

			_teams = new List<BaseTeam>();
			NoneTeam = new NoneTeam();
			RunnerTeam = new RunnerTeam();
			TaggerTeam = new TaggerTeam();
			AddTeam( NoneTeam );
			AddTeam( RunnerTeam );
			AddTeam( TaggerTeam );

			_ = StartGameTimer();
		}
		
		private void AddTeam( BaseTeam team )
		{
			_teams.Add( team );
			team.Index = _teams.Count;
		}

		public BaseTeam GetTeamByIndex( int index )
		{
			return _teams[index - 1];
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

			player.Respawn();
		}
	}
}
