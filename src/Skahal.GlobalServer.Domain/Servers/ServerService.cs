//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using Skahal.GlobalServer.Domain.Multiplayer;
using Skahal.GlobalServer.Domain.Players;
using System.Threading;


namespace Skahal.GlobalServer.Domain.Servers
{
	#region Enums
	/// <summary>
	/// The possibles states for the server.
	/// </summary>
	public enum ServerState 
	{
		/// <summary>
		/// Server is online and operation.
		/// </summary>
		Online,

		/// <summary>
		/// The server is online, but is refusing new connections because is waiting for update.
		/// </summary>
		WaitingUpdate
	}
	#endregion

	/// <summary>
	/// Domain server service.
	/// </summary>
	public static class ServerService
	{
		#region Fields
		private static object s_lock = new object();
		private static IServerRepository s_repository;
		private static ServerStatistics s_statistics;
		private static int s_requestsCount;
		private static ServerState s_serverState;
		#endregion

		#region Constructors
		static ServerService()
		{
			PlayerInactivityTimeoutSeconds = 120;
		}
		#endregion

		#region Properties
		public static float PlayerInactivityTimeoutSeconds { get; set; }
		#endregion


		#region Methods
		public static void Initialize(IServerRepository repository)
		{
			s_repository = repository;
			s_statistics = s_repository.FindStatistics();
			
			if(s_statistics == null)
			{
				s_statistics = new ServerStatistics();
			}
			
			var serverBackgroundThread = new Thread(() => {
				while(true)
				{
					try
					{
						Thread.Sleep(30000);
						MultiplayerService.UnregisterAllInactivePlayers();
						s_repository.SaveStatistics(s_statistics);
					}
					catch
					{
						// Try again later.
					}
				}
			});
			
			serverBackgroundThread.Start();

			MultiplayerService.GameQuit += delegate {
				s_statistics.TotalQuitGamesCount++;
			};

			s_serverState = ServerState.Online;
		}
		
		public static Server GetServer(Player player)
		{
			var server = new Server();
			server.AvailablePlayersCount = player == null ? MultiplayerService.CountAvailablePlayers() : MultiplayerService.CountAvailablePlayersForVersion(player.GameVersion);
			server.GamesCount = MultiplayerService.CountAllGames();
			server.RequestsCount = s_requestsCount;
			server.MinDequeueMessagesWaitingSeconds = 1f;
			server.MaxDequeueMessagesWaitingSeconds = 3f;
			server.DecreaseDequeueMessagesWaitingSeconds = 1f;
			server.State = s_serverState;
			
			return server;
		}
		
		public static ServerStatistics GetStatistics()
		{
			return s_statistics;
		}
		
		public static void RegisterRequest()
		{
			lock(s_lock)
			{
				s_statistics.TotalRequestsCount++;
				s_requestsCount++;
			}
		}
		
		public static bool RegisterPlayer(Player player)
		{
			bool canRegisterPlayer = false;

			if(s_serverState == ServerState.Online)
			{
				lock(s_lock)
				{
					s_statistics.TotalPlayersCount++;
					
					switch(player.Device)
					{
					case PlayerDevice.Android:
						s_statistics.TotalAndroidDevicesCount++;
						break;
					
					case PlayerDevice.Editor:
						s_statistics.TotalEditorDevicesCount++;
						break;
						
					case PlayerDevice.iPad:
						s_statistics.TotaliPadDevicesCount++;
						break;
						
					case PlayerDevice.iPhone:
						s_statistics.TotaliPhoneDevicesCount++;
						break;
						
					case PlayerDevice.iPod:
						s_statistics.TotaliPodDevicesCount++;
						break;
						
					case PlayerDevice.Mac:
						s_statistics.TotalMacDevicesCount++;
						break;
						
					case PlayerDevice.Windows:
						s_statistics.TotalWindowsDevicesCount++;
						break;

					case PlayerDevice.Web:
						s_statistics.TotalWebDevicesCount++;
						break;
					}
				}

				canRegisterPlayer = true;
			}

			return canRegisterPlayer;
		}
		
		public static void RegisterGame(MultiplayerGame game)
		{
			lock(s_lock)
			{
				s_statistics.TotalGamesCount++;

				var actualGamesCount = MultiplayerService.CountAllGames();

				if(s_statistics.MaxSimultaneousGamesCount < actualGamesCount)
				{
					s_statistics.MaxSimultaneousGamesCount = actualGamesCount;
				}

				game.Ended += delegate {
					s_statistics.TotalEndedGamesCount++;
				};
			}
		}
		
		public static void Reset()
		{
			s_requestsCount = 0;
		}

		public static bool ChangeServerState(ServerState state)
		{
			s_serverState = state;
			return true;
		}
		#endregion
	}
}