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
		
		public Dictionary<int, ScoreboardEntry> Entries = new();
		public Dictionary<int, TeamSection> TeamSections = new();
		public Label playerCount;
		
		public Scoreboard()
		{
			StyleSheet.Load( "/ui/Scoreboard.scss" );

			PlayerScore.OnPlayerAdded += AddPlayer;
			PlayerScore.OnPlayerUpdated += UpdatePlayer;
			PlayerScore.OnPlayerRemoved += RemovePlayer;
			
			AddHeader();
			
			AddTeamHeader( ActionTagGame.Instance?.ChasersTeam );
			AddTeamHeader( ActionTagGame.Instance?.RunnerTeam );
			AddTeamHeader( ActionTagGame.Instance?.NoneTeam );

			foreach ( var player in PlayerScore.All )
			{
				AddPlayer( player );
			}
		}
		
		public override void Tick()
		{
			base.Tick();
			
			SetClass( "disabled", !Input.Down( InputButton.Score ) );
			playerCount.Text = Client.All.Count == 1 ? $"{Client.All.Count} Player" : $"{Client.All.Count} Players";
		}

		private void AddHeader()
		{
			var header = Add.Panel( "header" );
			header.Add.Panel( "icon" );

			var headerTitle = header.Add.Panel("title");

			headerTitle.Add.Label( Global.MapName, "map" );
			playerCount = headerTitle.Add.Label( "", "players" );
		}
		
		private void AddTeamHeader(BaseTeam team)
		{
			var section = new TeamSection{};
			section.TeamContainer = Add.Panel( "team-container" );
			section.TeamHeader = section.TeamContainer.Add.Panel( "team-header" );
			section.Header = section.TeamContainer.Add.Panel( "table-header" );
			section.Canvas = section.TeamContainer.Add.Panel( "canvas" );
			section.TeamName = section.TeamHeader.Add.Label( team.ScoreboardName, "team-name" );

			TeamSections[team.Index] = section;
		}

		private void AddPlayer( PlayerScore.Entry entry )
		{
			var teamIndex = entry.Get( "team", 0 );

			if ( !TeamSections.TryGetValue( teamIndex, out var section ) )
			{
				section = TeamSections[ 0 ];
			}

			var p = section.Canvas.AddChild<ScoreboardEntry>();
			p.UpdateFrom( entry );
			Entries[entry.Id] = p;
		}

		private void UpdatePlayer( PlayerScore.Entry entry )
		{
			if ( !Entries.TryGetValue( entry.Id, out var panel ) )
			{
				return;
			}

			var currentTeamIndex = 0;
			var newTeamIndex = entry.Get( "team", 0 );

			foreach (var kv in TeamSections.Where(kv => kv.Value.Canvas == panel.Parent))
			{
				currentTeamIndex = kv.Key;
			}

			if ( currentTeamIndex != newTeamIndex )
			{
				panel.Parent = TeamSections[newTeamIndex].Canvas;
			}

			panel.UpdateFrom( entry );
		}

		private void RemovePlayer( PlayerScore.Entry entry )
		{
			if ( !Entries.TryGetValue( entry.Id, out var panel ) )
			{
				return;
			}

			panel.Delete();
			Entries.Remove( entry.Id );
		}
	}
	
	public partial class ScoreboardEntry : Panel
	{
		public PlayerScore.Entry Entry;
		public Label PlayerName;

		public ScoreboardEntry()
		{
			AddClass( "entry" );

			PlayerName = Add.Label( "PlayerName", "name" );
		}

		public virtual void UpdateFrom( PlayerScore.Entry entry )
		{
			Entry = entry;

			PlayerName.Text = entry.GetString( "name" );

			SetClass( "me", Local.Client != null && entry.Get<ulong>( "steamid", 0 ) == Local.Client.SteamId );
		}
	}
}
