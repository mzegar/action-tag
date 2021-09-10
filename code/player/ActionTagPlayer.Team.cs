using ActionTag.Teams;
using Sandbox;

namespace ActionTag
{
	public partial class ActionTagPlayer
	{
		[Net, OnChangedCallback]
		public BaseTeam Team { get; set; }
		private void OnTeamChanged()
		{
			PlayerInfo.Instance?.SetCurrentTeamIndex(Team);
		}
	}
}
