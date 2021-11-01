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

		private readonly Dictionary<Client, ScoreboardEntry> _entries = new();
		private readonly Dictionary<int, TeamSection> _teamSections = new();
		private Label _playerCount;

		public Scoreboard()
		{
			StyleSheet.Load( "/ui/Scoreboard.scss" );

			AddHeader();

			AddTeamHeader( new ChasersTeam() );
			AddTeamHeader( new RunnerTeam() );
			AddTeamHeader( new NoneTeam() );

			Instance = this;
		}

		public override void Tick()
		{
			base.Tick();

			var isDisabled = !Input.Down( InputButton.Score );
			SetClass( "disabled", isDisabled );

			if ( isDisabled )
				return;

			foreach ( var player in Client.All.Except( _entries.Keys ) )
			{
				AddPlayer( player );
				UpdatePlayer( player );
			}

			foreach ( var player in _entries.Keys.Except( Client.All ) )
			{
				RemovePlayer( player );
			}

			_playerCount.Text = Client.All.Count == 1 ? $"{Client.All.Count} Player" : $"{Client.All.Count} Players";
		}

		private void AddHeader()
		{
			var header = Add.Panel( "header" );
			header.Add.Panel( "icon" );

			var headerTitle = header.Add.Panel( "title" );

			headerTitle.Add.Label( Global.MapName, "map" );
			_playerCount = headerTitle.Add.Label( "", "players" );
		}

		private void AddTeamHeader( BaseTeam team )
		{
			var section = new TeamSection { };
			section.TeamContainer = Add.Panel( "team-container" );
			section.TeamHeader = section.TeamContainer.Add.Panel( "team-header" );
			section.Header = section.TeamContainer.Add.Panel( "table-header" );
			section.Canvas = section.TeamContainer.Add.Panel( "canvas" );
			section.TeamName = section.TeamHeader.Add.Label( team.ScoreboardName, "team-name" );

			_teamSections[team.Index] = section;
		}

		public void AddPlayer( Client entry )
		{
			if ( _entries.ContainsKey( entry ) )
			{
				return;
			}

			var teamIndex = entry.GetValue( "team", 0 );
			if ( !_teamSections.TryGetValue( teamIndex, out var section ) )
			{
				section = _teamSections[0];
			}

			var p = section.Canvas.AddChild<ScoreboardEntry>();
			p.UpdateFrom( entry );
			_entries[entry] = p;
		}

		public void UpdatePlayer( Client entry )
		{
			if ( entry == null )
				return;

			if ( !_entries.TryGetValue( entry, out var panel ) )
			{
				return;
			}

			var currentTeamIndex = 0;
			var newTeamIndex = entry.GetValue( "team", 0 );

			foreach ( var kv in _teamSections.Where( kv => kv.Value.Canvas == panel.Parent ) )
			{
				currentTeamIndex = kv.Key;
			}

			if ( currentTeamIndex != newTeamIndex )
			{
				panel.Parent = _teamSections[newTeamIndex].Canvas;
			}

			panel.UpdateFrom( entry );
		}


		public void RemovePlayer( Client client )
		{
			if ( !_entries.TryGetValue( client, out var panel ) )
			{
				return;
			}

			panel.Delete();
			_entries.Remove( client );
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
