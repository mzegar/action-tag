using Sandbox;

namespace ActionTag.Teams
{
	public partial class RunnerTeam : BaseTeam
	{
		public override string ScoreboardName => "🐇 Runners";
		public override string Name => "🐇 Runner";
		public override string Emoji => "🐇";
		public override int Index => 2;
		
		public override void OnJoin( ActionTagPlayer player )
		{
			base.OnJoin( player );

			player.Inventory.Add( new Hand(), true );
		}

		public override void OnLeave( ActionTagPlayer player )
		{
			base.OnLeave( player );
			
			player.Inventory.DeleteContents();
		}
	}
}
