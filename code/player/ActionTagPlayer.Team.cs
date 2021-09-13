using ActionTag.Teams;
using Sandbox;

namespace ActionTag
{
	public partial class ActionTagPlayer
	{
		[Net] private BaseTeam PreviousTeam { get; set; }

		[Net, OnChangedCallback] public BaseTeam Team { get; private set; }
		private void OnTeamChanged()
		{
			PlayerInfo.Instance?.UpdateCurrentTeam();
		}

		public void SetTeam(BaseTeam newTeam)
		{
			Host.AssertServer();

			using ( Prediction.Off() )
			{
				Team = newTeam;
				PreviousTeam?.OnLeave(this);
				Team?.OnJoin(this);
				PreviousTeam = newTeam;
			}
		}
	}
}
