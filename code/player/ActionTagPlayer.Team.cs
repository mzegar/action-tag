using ActionTag.Teams;
using Sandbox;

namespace ActionTag
{
	public partial class ActionTagPlayer
	{
		[Net, OnChangedCallback] public int TeamIndex { get; set; }
		private void OnTeamIndexChanged()
		{
			PlayerInfo.Instance?.SetCurrentTeamIndex(TeamIndex);
		}
		
		private BaseTeam _team;

		public BaseTeam Team
		{
			get => _team;

			set
			{
				if ( value != null && value != _team )
				{
					_team?.Leave( this );
					_team = value;
					_team.Join( this );

					if ( IsServer )
					{
						TeamIndex = _team.Index;
						var client = GetClientOwner();
						client.SetScore( "team", TeamIndex );
					}
				}
			}
		}
	}
}
