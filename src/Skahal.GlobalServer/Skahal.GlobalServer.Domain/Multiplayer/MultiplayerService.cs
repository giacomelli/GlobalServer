//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using System.Collections.Generic;
using Skahal.GlobalServer.Domain.Players;
using Skahal.GlobalServer.Domain.Servers;
using System.Linq;

namespace Skahal.GlobalServer.Domain.Multiplayer
{
	public static class MultiplayerService
	{
		#region Events
		public static event EventHandler GameQuit;
		#endregion

		#region Fields
		private static Dictionary<string, Player> s_players = new Dictionary<string, Player>();
		private static List<MultiplayerGame> s_games = new List<MultiplayerGame>();
		private static object s_lock = new object();
		#endregion

		#region Public methods
		public static void Reset()
		{
			s_players.Clear();
			s_games.Clear();
		}
		
		public static bool RegisterPlayer(Player player)
		{
			lock(s_lock)
			{
				if(!s_players.ContainsKey(player.Id) && ServerService.RegisterPlayer(player))
				{
					player.State = PlayerState.Lobby;
					s_players.Add(player.Id, player);
					return true;
				}
				
				return false;
			}
		}
		
		public static IList<Player> GetAllPlayers()
		{
			return s_players.Values.ToList();
		}
		
		public static int CountAllPlayers()
		{
			return s_players.Count;
		}
		
		public static IList<Player> GetPlayersInLobby()
		{
			return s_players.Values.Where(p => p.State == PlayerState.Lobby).ToList();
		}
		
		public static int CountAvailablePlayers()
		{
			return s_players.Values.Count(p => p.State != PlayerState.Playing);
		}

		public static int CountAvailablePlayersForVersion(string gameVersion)
		{
			return s_players.Values.Count(p => p.State != PlayerState.Playing && p.GameVersion.Equals(gameVersion, StringComparison.InvariantCultureIgnoreCase));
		}
		
		public static MultiplayerGame CreateGame(string playerId)
		{
			lock(s_lock)
			{
				var player = GetPlayerById(playerId);
				
				if(player != null && player.State == PlayerState.Lobby)
				{
					player.State = PlayerState.WaitingOpponent;
					
					var query = from p in s_players.Values
								where 
									!p.Id.Equals(playerId) 
								&&	p.State == PlayerState.WaitingOpponent
								&&  p.GameVersion.Equals(player.GameVersion)
								orderby p.LastActivityDate descending
							select p;
					
					var opponent = query.FirstOrDefault();
					
					// Can create a game, so remove player for available players list.
					if(opponent != null)
					{
						var game = new MultiplayerGame(player, opponent);
						s_games.Add(game);
						ServerService.RegisterGame(game);
						
						return game;
					}
				}
				
				return null;
			}
		}
		
		public static bool UnregisterPlayer(string playerId, bool quit)
		{
			lock(s_lock)
			{
				bool unregistered = false;
				
				if(s_players.ContainsKey(playerId))
				{
					var player = s_players[playerId];
					player.State = quit ? PlayerState.Quit : PlayerState.ConnectionLost;
					s_players.Remove(playerId);
					unregistered = true;
				}
				
				var game = GetGameByPlayerId(playerId);
					
				if(game != null)
				{
					// Remove the game if all players are inactive.	
					if(!IsPlayerActive(game.GetOtherPlayer(playerId)))
					{
						s_games.Remove(game);
						unregistered = true;
					}
				}
				
				return unregistered;
			}
		}

		public static void UnregisterAllInactivePlayers()
		{
			var inactivePlayers = s_players.Where(p => !IsPlayerActive(p.Value)).ToList();

			foreach(var p in inactivePlayers)
			{
				UnregisterPlayer(p.Value.Id, false);
			}
		}

		public static void EnqueueMessage(MultiplayerMessage message)
		{
			lock(s_lock)
			{
				GetGameByPlayerId(message.PlayerId).EnqueueMessage(message);
			}
		}
		
		public static IList<MultiplayerMessage> DequeueMessages(string playerId)
		{
			return DequeueMessages (playerId, 0);
		}
		
		public static IList<MultiplayerMessage> DequeueMessages(string playerId, int lastDequeueMessageId)
		{
			lock(s_lock)
			{
				var game = GetGameByPlayerId(playerId);
			
				if(game == null || !IsPlayerActive(game.GetOtherPlayer(playerId)))
				{	
					var msgs = new List<MultiplayerMessage>();
					var player = GetPlayerById(playerId);
					
					// Does not exists anymore or is playing 
					// but game not exists (the other player has unregistered).
					if(player == null || player.State == PlayerState.Playing)
					{
						var msg = new MultiplayerMessage();
						msg.Name = "GameEnded";

						if(game != null && game.GetOtherPlayer(playerId).State == PlayerState.Quit)
						{
							msg.Value = "OnEnemyQuit";

							if(GameQuit != null)
							{
								GameQuit(typeof(MultiplayerService), EventArgs.Empty);
							}
						}
						else 
						{
							msg.Value = "OnConnectionLost";
						}

						
						UnregisterPlayer(playerId, false);
						msgs.Add(msg);
					}
					else
					{
						player.LastActivityDate = DateTime.Now;
					}
					
					return msgs;
				}
				else
				{
					game.MarkMessageAsReceived(playerId, lastDequeueMessageId);
					return game.DequeueMessages(playerId);
				}
			}
		}
		
		public static IList<MultiplayerGame> GetAllGames()
		{
			return s_games;
		}
		
		public static int CountAllGames()
		{
			return s_games.Count;
		}
		
		public static MultiplayerGame GetGameById(string gameId)
		{
			return s_games.FirstOrDefault(g => g.Id.Equals(gameId));
		}
		
		public static MultiplayerGame GetGameByPlayerId(string playerId)
		{
			return s_games.FirstOrDefault(g => g.HostPlayer.Id.Equals(playerId) || g.GuessPlayer.Id.Equals(playerId));
		}
		#endregion

		#region Private methods
		private static Player GetPlayerById(string playerId)
		{
			return s_players.Values.Where(p => p.Id.Equals(playerId)).FirstOrDefault();
		}
		
		
		private static MultiplayerGame GetGame(MultiplayerMessage message)
		{
			return GetGameById(message.GameId);
		}
		
		private static bool IsPlayerActive(string playerId)
		{
			var player = GetPlayerById(playerId);
			
			if(player == null)
			{
				return false;
			}
			else
			{
				return IsPlayerActive(player);
			}    	
		}

		private static bool IsPlayerActive(Player player)
		{
			return player.State != PlayerState.Quit 
				&& player.State != PlayerState.ConnectionLost
				&& (DateTime.Now - player.LastActivityDate).TotalSeconds <= ServerService.PlayerInactivityTimeoutSeconds;
		}
		#endregion
	}
}