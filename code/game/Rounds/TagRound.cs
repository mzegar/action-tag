using Sandbox;

namespace ActionTag
{
	public class TagRound : BaseRound
    {
        public override string RoundName => "Tag!";
        public override int RoundDuration
        {
            get => ActionTagGameSettings.TagRoundDuration;
        }

        public override void OnPlayerKilled(ActionTagPlayer player)
        {
	        AddSpectator( player );
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

	        var unassignedPlayers = Players;
	        
        }

        protected override void OnTimeUp()
        {
            base.OnTimeUp();

            ActionTagGame.Instance?.ChangeRound(new PreRound());
        }

        public override void OnPlayerSpawn(ActionTagPlayer player)
        {
            AddPlayer( player );
            base.OnPlayerSpawn( player );
        }
    }
}
