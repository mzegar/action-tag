using ActionTag.Teams;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace ActionTag
{
	public class PlayerInfo : Panel
	{
		public static PlayerInfo Instance;
		private readonly Label _currentTeam;
		
		public PlayerInfo()
		{
			Instance = this;
			StyleSheet.Load("/ui/PlayerInfo.scss");
			_currentTeam = Add.Label( Local.Pawn is ActionTagPlayer player ? player.Team.Name : "", "team" );
		}

		public void UpdateCurrentTeam()
		{
			if ( Local.Pawn is not ActionTagPlayer player )
			{
				return;
			}
			
			_currentTeam.Text = player.Team.Name;
		}
	}
}
