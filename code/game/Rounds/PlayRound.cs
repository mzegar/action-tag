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
	        AddSpectator( player );
	        base.OnPlayerKilled(player);
	        
	        CheckRoundStatus();
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
	        
	        
	        for ( var i = 0; i < Players.Count; ++i )
	        {
		        if ( i % 2 == 0 )
		        {
			        Players[i].Team = ActionTagGame.Instance.ChasersTeam;
		        }
		        else
		        {
			        Players[i].Team = ActionTagGame.Instance.RunnerTeam;
		        }
	        }
        }

        protected override void OnTimeUp()
        {
            base.OnTimeUp();

            ActionTagGame.Instance?.ForceRoundChange(new PostRound());
        }

        public override void OnPlayerSpawn(ActionTagPlayer player)
        {
            AddPlayer( player );
            base.OnPlayerSpawn( player );
        }
        
        public override void OnPlayerLeave(ActionTagPlayer player)
        {
	        base.OnPlayerLeave( player );
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
	        var aliveRunners = !Players.Any( ( p ) => p.Team is RunnerTeam && !p.IsSpectator );
	        if ( !aliveRunners )
	        {
		        return ActionTagGame.Instance?.ChasersTeam;
	        }
	        
	        var aliveChasers = !Players.Any( ( p ) => p.Team is ChasersTeam && !p.IsSpectator );
	        if ( !aliveChasers )
	        {
		        return ActionTagGame.Instance?.RunnerTeam;
	        }

	        // The round continues.
	        return new NoneTeam();
        }
    }
}
