using System;
using Sandbox;

namespace ActionTag
{
	public static class ServerCommands
	{
		[ServerCmd( Name = "actiontag_taground_duration", Help = "Sets the duration of the tag round." )]
		public static void TagRoundDuration(int duration = 0)
		{
			if ( duration == 0 )
			{
				Log.Info($"TagRoundDuration: {duration}");
				return;
			}
			
			ActionTagGameSettings.TagRoundDuration = duration;
		}
		
		[ServerCmd( Name = "actiontag_preround_duration", Help = "Sets the duration of the pre round." )]
		public static void PreRoundDuration(int duration = 0)
		{
			if ( duration == 0 )
			{
				Log.Info($"PreRoundDuration: {duration}");
				return;
			}
			
			ActionTagGameSettings.PreRoundDuration = duration;
		}
		
		[ServerCmd( Name = "actiontag_minimum_players", Help = "Sets the amount of players needed to start the game." )]
		public static void MinimumPlayers(int players = 0)
		{
			if ( players == 0 )
			{
				Log.Info($"MinimumPlayers: {players}");
				return;
			}
			
			ActionTagGameSettings.MinimumPlayers = players;
		}
		
		[ServerCmd( Name = "actiontag_setteam", Help = "Sets the role of the specified player." )]
		public static void SetTeam(string teamName = "")
		{
			if ( teamName == string.Empty )
			{
				return;
			}

			if ( ConsoleSystem.Caller.Pawn is not ActionTagPlayer player || player.IsSpectator )
			{
				return;
			}

			if ( ActionTagGame.Instance?.Round is not PlayRound round )
			{
				return;
			}

			player.Team = teamName.ToLower() switch
			{
				"runner" => ActionTagGame.Instance?.RunnerTeam,
				"chaser" => ActionTagGame.Instance?.ChasersTeam,
				_ => player.Team
			};
			
			round.CheckRoundStatus();
		}
	}
}
