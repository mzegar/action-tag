using System.Threading.Tasks;

using Sandbox;

namespace ActionTag
{
	public class PreRound : BaseRound
    {
        public override string RoundName => "Preparing";
        public override int RoundDuration
        {
            get => ActionTagGameSettings.PreRoundDuration;
        }

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

            ActionTagGame.Instance?.ChangeRound(new TagRound());
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
    }
}
