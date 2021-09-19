using ActionTag.Teams;
using Sandbox;
using Sandbox.UI;

namespace ActionTag
{
	public class PlayRound : BaseRound
    {
        public override string RoundName => "Action!";
        public override int RoundDuration
        {
            get => ActionTagGameSettings.TagRoundDuration;
        }

        protected override void OnStart()
        {
	        base.OnStart();

	        foreach (var chaser in Utils.GetAliveChasers())
	        {
		        chaser.Controller.IsFrozen = false;
	        }
        }

        public override void OnPlayerKilled(ActionTagPlayer player)
        {
	        base.OnPlayerKilled(player);
	        
	        CheckRoundStatus();
        }

        public override void OnPlayerTagged( ActionTagPlayer player )
        {
	        base.OnPlayerTagged( player );
	        
	        CheckRoundStatus();
        }

        protected override void OnTimeUp()
        {
            base.OnTimeUp();

            ActionTagGame.Instance?.ForceRoundChange(new PostRound());
        }

        public override void OnPlayerSpawn(ActionTagPlayer player)
        {
	        player.MakeSpectator();
        }
        
        public override void OnPlayerLeave(Entity entity)
        {
	        CheckRoundStatus();
        }

        public void CheckRoundStatus()
        {
	        var winningTeam = CheckForWinningTeam();
	        if ( winningTeam == ActionTagGame.Instance?.NoneTeam )
	        {
		        return;
	        }

	        RPC.SendMessage($"{winningTeam.ScoreboardName} Win!");
	        ActionTagGame.Instance?.ForceRoundChange(new PostRound());
        }

        /// <summary>
        /// Returns a team if they have won the round. If there isn't a winner yet, "NoneTeam" is returned.
        /// </summary>
        private BaseTeam CheckForWinningTeam()
        {
	        var alivePlayers = Utils.GetAlivePlayers();

	        var aliveChasers = 0;
	        var aliveNonFrozenRunners = 0;
	        foreach ( var player in alivePlayers )
	        {
		        switch (player.Team)
		        {
			        case ChasersTeam:
				        aliveChasers += 1;
				        break;
			        case RunnerTeam:
				        if (!player.Controller.IsFrozen)
							aliveNonFrozenRunners += 1;
				        break;
		        }
	        }

	        if ( aliveNonFrozenRunners == 0 )
	        {
		        return ActionTagGame.Instance?.ChasersTeam;
	        }

	        if ( aliveChasers == 0 )
	        {
		        return ActionTagGame.Instance?.RunnerTeam;
	        }

	        // The round continues.
	        return ActionTagGame.Instance?.NoneTeam;
        }
    }
}
