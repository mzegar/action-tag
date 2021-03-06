using System;
using System.Collections.Generic;
using System.Linq;
using ActionTag.Teams;
using Sandbox;

namespace ActionTag
{
	public static partial class Utils
	{
		// Hacky, but we can factor in any disconnected players using the param.
		public static bool HasMinimumPlayers(List<Entity> disconnectPlayers = null)
		{
			var disconnectPlayerCount = disconnectPlayers?.Count ?? 0;
			return Client.All.Count - disconnectPlayerCount >= ActionTagGameSettings.MinimumPlayers;
		}

		public static List<ActionTagPlayer> GetAlivePlayers()
		{
			var alivePlayers = new List<ActionTagPlayer>();

			foreach ( var client in Client.All )		
			{
				if ( client.Pawn is ActionTagPlayer {Health: > 0} player )
				{
					alivePlayers.Add( player );
				}
			}

			return alivePlayers;
		}

		public static List<ActionTagPlayer> GetAliveChasers()
		{
			var aliveChasers = new List<ActionTagPlayer>();
			
			foreach ( var client in Client.All )		
			{
				if ( client.Pawn is ActionTagPlayer {Health: > 0, Team: ChasersTeam} player )
				{
					aliveChasers.Add( player );
				}
			}

			return aliveChasers;
		}

		public static List<Client> GetShuffledClients()
		{
			var players = Client.All;
			return players.OrderBy( ( _ ) => Rand.Int( 0, players.Count ) ).ToList();
		}
	}
}
