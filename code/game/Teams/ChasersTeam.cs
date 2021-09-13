namespace ActionTag.Teams
{
	public partial class ChasersTeam : BaseTeam
	{
		public override string Name => "🏃‍♂️ Chaser";

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
