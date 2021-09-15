using System.Linq;
using ActionTag.Teams;
using Sandbox;

namespace ActionTag
{
	public class PlayRound : BaseRound
    {
        public override string RoundName => "Action!";
        public override int RoundDuration
        {
            get => ActionTagGameSettings.TagRoundDuration;
        }

        public override void OnPlayerKilled(ActionTagPlayer player)
        {
	        base.OnPlayerKilled(player);
	        
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
        
        public override void OnPlayerLeave(Entity ent)
        {
	        CheckRoundStatus();
        }

        public void CheckRoundStatus()
        {
	        if ( CheckForWinningTeam() != ActionTagGame.Instance?.NoneTeam )
	        {
		        ActionTagGame.Instance?.ForceRoundChange(new PostRound());
	        }
        }

        /// <summary>
        /// Returns a team if they have won the round. If there isn't a winner yet, "NoneTeam" is returned.
        /// </summary>
        private BaseTeam CheckForWinningTeam()
        {
	        var alivePlayers = Utils.GetAlivePlayers();
	        var aliveRunners = !alivePlayers.Any( ( p ) => p.Team is RunnerTeam && !p.IsSpectator );
	        if ( !aliveRunners )
	        {
		        return ActionTagGame.Instance?.ChasersTeam;
	        }
	        
	        var aliveChasers = !alivePlayers.Any( ( p ) => p.Team is ChasersTeam && !p.IsSpectator );
	        if ( !aliveChasers )
	        {
		        return ActionTagGame.Instance?.RunnerTeam;
	        }

	        // The round continues.
	        return new NoneTeam();
        }
    }
}
