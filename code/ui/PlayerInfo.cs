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
		private readonly Label _currentHealth;
		
		public PlayerInfo()
		{
			Instance = this;
			StyleSheet.Load("/ui/PlayerInfo.scss");
			
			var team = "";
			var health = "";

			if ( Local.Pawn is ActionTagPlayer player )
			{
				team = player.Team.Name;
				health = $"✚ {player.Health.CeilToInt()}";
			}

			var teamWrapper = Add.Panel( "Team" );
			_currentTeam = teamWrapper.Add.Label( team );

			var healthWrapper = Add.Panel( "Health" );
			_currentHealth = healthWrapper.Add.Label( health );
		}

		public override void Tick()
		{
			base.Tick();
			
			if (Local.Pawn is not ActionTagPlayer player)
			{
				return;
			}
			
			_currentHealth.Text = $"✚ {player.Health.CeilToInt()}";
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
