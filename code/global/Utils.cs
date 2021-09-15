using System;
using System.Collections.Generic;
using System.Linq;
using ActionTag.Teams;
using Sandbox;

namespace ActionTag
{
	public static partial class Utils
	{
		public static bool HasMinimumPlayers()
		{
			return Client.All.Count >= ActionTagGameSettings.MinimumPlayers;
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

		public static List<ActionTagPlayer> GetShuffledAlivePlayers()
		{
			var alivePlayers = GetAlivePlayers();
			return alivePlayers.OrderBy( ( _ ) => Rand.Int( 0, alivePlayers.Count ) ).ToList();
		}
	}
}
