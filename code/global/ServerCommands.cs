﻿namespace ActionTag
{
	public static class ServerCommands
	{
		[ServerCmd( Name = "actiontag_playround_duration", Help = "Sets the duration of the play round." )]
		public static void PlayRoundDuration(int duration)
		{
			ActionTagGameSettings.PlayRoundDuration = duration;
		}
		
		[ServerCmd( Name = "actiontag_preround_duration", Help = "Sets the duration of the pre round." )]
		public static void PreRoundDuration(int duration)
		{
			ActionTagGameSettings.PreRoundDuration = duration;
		}
		
		[ServerCmd( Name = "actiontag_minimum_players", Help = "Sets the amount of players needed to start the game." )]
		public static void MinimumPlayers(int players)
		{
			ActionTagGameSettings.MinimumPlayers = players;
		}
	}
}