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

			_teamWrapper = Add.Panel( "Team" );
			_teamWrapper.SetClass("disabled", team is "" or "None"); // TODO: Clean this up when spectating players is a thing.
			_currentTeam = _teamWrapper.Add.Label( team );

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
			
			_teamWrapper.SetClass("disabled", player.Team is null or NoneTeam);
			_currentTeam.Text = player.Team?.Name;
		}
	}
}
