using System;
using System.Collections.Generic;
using System.Linq;
using Sandbox;

namespace ActionTag
{
	public static partial class Utils
	{
		// Disgusting hack since for some reason s&box is unable to detect the disconnected player.
		// If this list is not null, we assume they have disconnected.
		public static bool HasMinimumPlayers(List<Entity> disconnectedPlayers = null)
		{
			var disconnectedPlayerCount = disconnectedPlayers?.Count ?? 0;
			return Client.All.Count - disconnectedPlayerCount >= ActionTagGameSettings.MinimumPlayers;
		}

		public static List<ActionTagPlayer> GetAlivePlayers()
		{
			var alivePlayers = new List<ActionTagPlayer>();

			foreach ( var client in Client.All )		
			{
				if ( client.Pawn is ActionTagPlayer {IsSpectator: false} player )
				{
					alivePlayers.Add( player );
				}
			}

			return alivePlayers;
		}

		public static List<ActionTagPlayer> GetShuffledAlivePlayers()
		{
			var alivePlayers = GetAlivePlayers();
			return alivePlayers.OrderBy( ( _ ) => Rand.Int( 0, alivePlayers.Count ) ).ToList();
		}
	}
}
