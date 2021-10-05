using System.Collections.Generic;
using System.Linq;
using ActionTag.Teams;
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

namespace ActionTag
{
	public class Scoreboard : Panel
	{
		public struct TeamSection
		{
			public Label TeamName;
			public Sandbox.UI.Panel TeamIcon;

			public Sandbox.UI.Panel TeamContainer;

			public Sandbox.UI.Panel Header;
			public Sandbox.UI.Panel TeamHeader;
			public Sandbox.UI.Panel Canvas;
		}

		public static Scoreboard Instance;

		private readonly Dictionary<int, ScoreboardEntry> _entries = new();
		private readonly Dictionary<int, TeamSection> _teamSections = new();
		private Label _playerCount;
		
		public Scoreboard()
		{
			StyleSheet.Load( "/ui/Scoreboard.scss" );

			AddHeader();
			
			AddTeamHeader( new ChasersTeam() );
			AddTeamHeader( new RunnerTeam() );
			AddTeamHeader( new NoneTeam() );

			foreach ( var player in Client.All )
			{
				AddPlayer( player );
			}
			
			Instance = this;
		}
		
		public override void Tick()
		{
			base.Tick();
			
			SetClass( "disabled", !Input.Down( InputButton.Score ) );
			_playerCount.Text = Client.All.Count == 1 ? $"{Client.All.Count} Player" : $"{Client.All.Count} Players";
		}

		private void AddHeader()
		{
			var header = Add.Panel( "header" );
			header.Add.Panel( "icon" );

			var headerTitle = header.Add.Panel("title");

			headerTitle.Add.Label( Global.MapName, "map" );
			_playerCount = headerTitle.Add.Label( "", "players" );
		}
		
		private void AddTeamHeader(BaseTeam team)
		{
			var section = new TeamSection{};
			section.TeamContainer = Add.Panel( "team-container" );
			section.TeamHeader = section.TeamContainer.Add.Panel( "team-header" );
			section.Header = section.TeamContainer.Add.Panel( "table-header" );
			section.Canvas = section.TeamContainer.Add.Panel( "canvas" );
			section.TeamName = section.TeamHeader.Add.Label( team.ScoreboardName, "team-name" );

			_teamSections[team.Index] = section;
		}

		public void AddPlayer( Client entry )
		{
			if ( _entries.ContainsKey( entry.UserId ) )
			{
				return;
			}
			
			// TODO: Set value here to 0, waiting for sbox fix.
			if ( !_teamSections.TryGetValue( 0, out var section ) )
			{
				section = _teamSections[ 0 ];
			}

			var p = section.Canvas.AddChild<ScoreboardEntry>();
			p.UpdateFrom( entry );
			_entries[entry.UserId] = p;
		}

		public void UpdatePlayer( Client entry )
		{
			if ( !_entries.TryGetValue( entry.UserId, out var panel ) )
			{
				return;
			}
			
			var currentTeamIndex = 0;
			var newTeamIndex = entry.GetValue( "team", 0 );
			
			foreach (var kv in _teamSections.Where(kv => kv.Value.Canvas == panel.Parent))
			{
				currentTeamIndex = kv.Key;
			}
			
			if ( currentTeamIndex != newTeamIndex )
			{
				panel.Parent = _teamSections[newTeamIndex].Canvas;
			}
			
			panel.UpdateFrom( entry );
		}

		
		public void RemovePlayer( int userId )
		{
			if ( !_entries.TryGetValue( userId, out var panel ) )
			{
				return;
			}

			panel.Delete();
			_entries.Remove( userId );
		}
	}
	
	public class ScoreboardEntry : Panel
	{
		private readonly Label _playerName;

		public ScoreboardEntry()
		{
			AddClass( "entry" );

			_playerName = Add.Label( "PlayerName", "name" );
		}

		public virtual void UpdateFrom( Client entry )
		{
			_playerName.Text = entry.Name;
			SetClass( "me", Local.Client != null && entry.GetValue<ulong>( "steamid", 0 ) == Local.Client.SteamId );
		}
	}
}
