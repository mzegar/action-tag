using System.Threading.Tasks;

using Sandbox;

namespace ActionTag
{
	public class PreRound : BaseRound
    {
        public override string RoundName => "Preparing Round";
        public override int RoundDuration
        {
            get => ActionTagGameSettings.PreRoundDuration;
        }

        private bool _playedCountDownSound = false;

        public override void OnPlayerKilled(ActionTagPlayer player)
        {
            _ = StartRespawnTimer(player);

            base.OnPlayerKilled(player);
        }

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
		        }
	        }
        }

        protected override void OnTimeUp()
        {
            base.OnTimeUp();

            ActionTagGame.Instance?.ChangeRound(new PlayRound());
        }

        private static async Task StartRespawnTimer(ActionTagPlayer player)
        {
            await Task.Delay(1000);

            if (player.IsValid() && ActionTagGame.Instance.Round is PreRound)
            {
                player.Respawn();
            }
        }

        public override void OnPlayerSpawn(ActionTagPlayer player)
        {
            AddPlayer(player);

            base.OnPlayerSpawn(player);
        }

        public override void OnSecond()
        {
	        if ( Host.IsServer )
	        {
		        base.OnSecond();

		        if ( TimeLeft <= 3 && !_playedCountDownSound )
		        {
			        Sound.FromScreen("countdown");
			        _playedCountDownSound = true;
		        }
	        }
        }
    }
}
