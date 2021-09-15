using System.Collections.Generic;
using System.Threading.Tasks;
using ActionTag.Teams;
using Sandbox;

namespace ActionTag
{
	public class PreRound : BaseRound
    {
        public override string RoundName => "Run and Hide!";
        public override int RoundDuration
        {
            get => ActionTagGameSettings.PreRoundDuration;
        }

        private bool _playedCountDownSound = false;
        private List<ActionTagPlayer> _runners = new List<ActionTagPlayer>();
        private List<ActionTagPlayer> _chasers = new List<ActionTagPlayer>();

        protected override void OnStart()
        {
	        if ( !Host.IsServer )
	        {
		        return;
	        }
	        
	        _playedCountDownSound = false;
	        
	        foreach ( var client in Client.All )
	        {
		        if ( client.Pawn is ActionTagPlayer player )
		        {
			        player.Respawn();
			        player.SetTeam( ActionTagGame.Instance.NoneTeam );
		        }
	        }
	        
	        AssignTeams();
        }

        private void AssignTeams()
        {
	        var alivePlayers = Utils.GetShuffledAlivePlayers();
	        for ( var i = 0; i < alivePlayers.Count; ++i )
	        {
		        var player = alivePlayers[i].GetClientOwner();
		        if ( i % 2 == 0 )
		        {
			        Log.Info($"{player.Name} is a runner");
			        alivePlayers[i].SetTeam(ActionTagGame.Instance.RunnerTeam);
		        }
		        else
		        {
			        Log.Info($"{player.Name} is a chaser");
			        alivePlayers[i].SetTeam(ActionTagGame.Instance.ChasersTeam);
		        }
	        }
        }

        // Let's let people join during this round. Assign them to the team with less players.
        public override void OnPlayerSpawn( ActionTagPlayer player )
        {
	        base.OnPlayerSpawn( player );

	        if ( !Host.IsServer )
	        {
		        return;
	        }

	        if ( player.Team is NoneTeam or null )
	        {
		        if ( _chasers.Count >= _runners.Count )
		        {
			        player.SetTeam( ActionTagGame.Instance?.RunnerTeam );
			        _runners.Add( player );
		        }
		        else
		        {
			        player.SetTeam( ActionTagGame.Instance?.ChasersTeam );
			        _chasers.Add( player );
		        }
	        }
        }

        protected override void OnTimeUp()
        {
            base.OnTimeUp();

            ActionTagGame.Instance?.ChangeRound(new PlayRound());
        }

        public override void OnPlayerKilled(ActionTagPlayer player)
        {
	        _ = StartRespawnTimer( player );

	        base.OnPlayerKilled( player );
        }
        
        private static async Task StartRespawnTimer( Player player )
        {
	        await Task.Delay( 1000 );

	        if ( player.IsValid() && ActionTagGame.Instance.Round is PreRound )
	        {
		        player.Respawn();
	        }
        }
        
        public override void OnPlayerLeave(Entity ent)
        {
	        if ( Host.IsServer && !Utils.HasMinimumPlayers(new List<Entity>(){ent}) )
	        {
		        ActionTagGame.Instance?.ForceRoundChange(new WaitingRound());
	        }
        }

        public override void OnSecond()
        {
	        if ( Host.IsServer )
	        {
		        base.OnSecond();

		        if ( TimeLeft <= 2 && !_playedCountDownSound )
		        {
			        // Just in case we don't have enough players, let's not play the sound queue.
			        // OnTimeUp will handle the transition back to WaitingRound.
			        if ( Utils.HasMinimumPlayers() )
			        {
				        Sound.FromScreen("countdown");
				        _playedCountDownSound = true;
			        }
		        }
	        }
        }

        protected override void OnFinish()
        {
	        base.OnFinish();
	        
	        _runners.Clear();
	        _chasers.Clear();
        }
    }
}
