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
			_currentTeam = Add.Label( "", "team" );
		}

		public void SetCurrentTeamIndex( int teamIndex )
		{
			_currentTeam.Text = ActionTagGame.Instance.GetTeamByIndex( teamIndex ).Name;
		}
	}
}
