using System.Collections.Generic;
using System.Threading.Tasks;

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
	        var alivePlayers = Utils.GetAlivePlayers();
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
			        if ( Utils.HasMinimumPlayers() )
			        {
				        Sound.FromScreen("countdown");
				        _playedCountDownSound = true;
			        }
		        }
	        }
        }
    }
}
