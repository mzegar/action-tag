using System;
using ActionTag.Teams;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace ActionTag
{
	public class PlayerInfo : Panel
	{
		public static PlayerInfo Instance;
		private readonly Sandbox.UI.Panel _teamWrapper;
		private readonly Label _currentTeam;

		public PlayerInfo()
		{
			Instance = this;
			StyleSheet.Load( "/ui/PlayerInfo.scss" );

			var team = "";

			if ( Local.Pawn is ActionTagPlayer player )
			{
				team = player.Team.Name;
			}

			_teamWrapper = Add.Panel( "Team" );
			_teamWrapper.SetClass( "disabled", team is "" or "None" ); // TODO: Clean this up when spectating players is a thing.
			_currentTeam = _teamWrapper.Add.Label( team );
		}

		public override void Tick()
		{
			base.Tick();
		}

		public void UpdateCurrentTeam()
		{
			if ( Local.Pawn is not ActionTagPlayer player )
			{
				return;
			}

			_teamWrapper.SetClass( "disabled", player.Team is null or NoneTeam );
			_currentTeam.Text = player.Team?.Name;
		}
	}
}
