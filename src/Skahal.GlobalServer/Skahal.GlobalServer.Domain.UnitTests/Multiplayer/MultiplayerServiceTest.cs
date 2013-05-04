//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using NUnit.Framework;
using Skahal.GlobalServer.Domain.Multiplayer;
using Skahal.GlobalServer.Domain.Players;
using System.Threading;
using Skahal.GlobalServer.Domain.Servers;
using Rhino.Mocks;

namespace Skahal.GlobalServer.Domain.UnitTests
{
	[TestFixture]
	public class MultiplayerServiceTest
	{
		#region Initialize
		[TestFixtureSetUp]
		public void InitializeClass()
		{
			ServerService.Initialize(MockRepository.GenerateMock<IServerRepository>());
		}
	
		[SetUp]
		public void InitializeTest()
		{
			MultiplayerService.Reset();
		}
		#endregion
		
		[Test]
		public void RegisterAndGetPlayersInLobbyTest ()
		{
			var availablePlayers = MultiplayerService.GetPlayersInLobby();
			Assert.IsNotNull(availablePlayers);
			Assert.AreEqual(0, availablePlayers.Count);
			
			var p1 = new Player("1", "127.0.0.1");
			p1.Name = "Um";
			p1.Device = PlayerDevice.Android;
			Assert.IsTrue(MultiplayerService.RegisterPlayer(p1));
			Assert.IsFalse(MultiplayerService.RegisterPlayer(p1));
			
			var p2 = new Player("2", "127.0.0.2");
			p2.Name = "Dois";
			p2.Device = PlayerDevice.iPhone;
			Assert.IsTrue(MultiplayerService.RegisterPlayer(p2));
			Assert.IsFalse(MultiplayerService.RegisterPlayer(p2));
			
			availablePlayers = MultiplayerService.GetPlayersInLobby();
			Assert.AreEqual(2, availablePlayers.Count);
			
			var p = availablePlayers[0];
			Assert.AreEqual("1", p.Id);
			Assert.AreEqual("Um", p.Name);
			Assert.AreEqual(p.Device, PlayerDevice.Android);
			Assert.AreEqual("127.0.0.1", p.IP);
			
			p = availablePlayers[1];
			Assert.AreEqual("2", p.Id);
			Assert.AreEqual("Dois", p.Name);
			Assert.AreEqual(p.Device, PlayerDevice.iPhone);
			Assert.AreEqual("127.0.0.2", p.IP);
		}

		[Test]
		public void RegisterPlayer_ServerStateWaitingUpdate_NonRegisterAnyPlayer ()
		{
			ServerService.ChangeServerState(ServerState.WaitingUpdate);

			var p1 = new Player("1", "127.0.0.1");
			p1.Name = "Um";
			p1.Device = PlayerDevice.Android;
			Assert.IsFalse(MultiplayerService.RegisterPlayer(p1));
			Assert.IsFalse(MultiplayerService.RegisterPlayer(p1));
			
			var p2 = new Player("2", "127.0.0.2");
			p2.Name = "Dois";
			p2.Device = PlayerDevice.iPhone;
			Assert.IsFalse(MultiplayerService.RegisterPlayer(p2));
			Assert.IsFalse(MultiplayerService.RegisterPlayer(p2));
			
			Assert.IsNull(MultiplayerService.CreateGame(p1.Id));
			Assert.IsNull(MultiplayerService.CreateGame(p2.Id));

			ServerService.ChangeServerState(ServerState.Online);

			Assert.IsTrue(MultiplayerService.RegisterPlayer(p1));
			Assert.IsTrue(MultiplayerService.RegisterPlayer(p2));
			Assert.IsNull(MultiplayerService.CreateGame(p1.Id));
			Assert.IsNotNull(MultiplayerService.CreateGame(p2.Id));
		}

		[Test]
		public void EnqueueAndDequeueMessagesTest()
		{
			var p1 = new Player("1", "127.0.0.1");
			p1.Name = "Um";
			p1.Device = PlayerDevice.Mac;
			Assert.IsTrue(MultiplayerService.RegisterPlayer(p1));
			
			var p2 = new Player("2", "127.0.0.2");
			p2.Name = "Dois";
			p2.Device = PlayerDevice.Windows;
			Assert.IsTrue(MultiplayerService.RegisterPlayer(p2));
			
			var game = MultiplayerService.CreateGame(p2.Id);
			Assert.IsNull(game);
			
			game = MultiplayerService.CreateGame(p1.Id);
			Assert.IsNotNull(game);
			Assert.IsNotNull(game.Id);
			Assert.AreEqual(p1.Id, game.HostPlayer.Id);
			Assert.AreEqual(p2.Id, game.GuessPlayer.Id);
			
			var msgs = game.GetPlayerMessages(p1.Id);
			Assert.AreEqual (1, msgs.Count);
			var msg = msgs[0];
			Assert.AreEqual(game.Id, msg.GameId);
			Assert.AreEqual(p1.Id, msg.PlayerId);
			Assert.AreEqual("GameCreated", msg.Name);
			Assert.AreEqual(p2, msg.Value);
			
			msgs = game.GetPlayerMessages(p2.Id);
			Assert.AreEqual (1, msgs.Count);
			msg = msgs[0];
			Assert.AreEqual(game.Id, msg.GameId);
			Assert.AreEqual(p2.Id, msg.PlayerId);
			Assert.AreEqual("GameCreated", msg.Name);
			Assert.AreEqual(p1, msg.Value);
			
			msg = new MultiplayerMessage();
			msg.GameId = game.Id;
			msg.PlayerId = p2.Id;
			msg.Name = "msg1";
			msg.Value = "v1";
			MultiplayerService.EnqueueMessage(msg);
			
			Assert.AreEqual(1, game.GetPlayerMessages(p1.Id).Count);
			
			msgs = game.GetPlayerMessages(p2.Id);
			Assert.AreEqual(2, msgs.Count);
			var actualMsg = msgs[1];
			
			Assert.AreEqual(msg.GameId, actualMsg.GameId);
			Assert.AreEqual(msg.PlayerId, actualMsg.PlayerId);
			Assert.AreEqual(msg.Name, actualMsg.Name);
			Assert.AreEqual(msg.Value, actualMsg.Value);
			
			Assert.AreEqual(3, game.GetAllMessages().Count);
			
			msgs = MultiplayerService.DequeueMessages(p1.Id);
			Assert.AreEqual("GameCreated", msgs[0].Name);
			msgs = MultiplayerService.DequeueMessages(p1.Id, msgs[0].Id);
			Assert.AreEqual(0, msgs.Count);
			
			msgs = MultiplayerService.DequeueMessages(p2.Id);
			Assert.AreEqual("GameCreated", msgs[0].Name);
			
			actualMsg = msgs[1];
			Assert.AreEqual(msg.GameId, actualMsg.GameId);
			Assert.AreEqual(msg.PlayerId, actualMsg.PlayerId);
			Assert.AreEqual(msg.Name, actualMsg.Name);
			Assert.AreEqual(msg.Value, actualMsg.Value);
			
			msgs = MultiplayerService.DequeueMessages(p1.Id);
			Assert.AreEqual(0, msgs.Count);
			
			msgs = MultiplayerService.DequeueMessages(p2.Id);
			Assert.AreEqual(2, msgs.Count);
		}
		
		[Test]
		public void UnregisterPlayerTest()
		{
			ServerService.ChangeServerState(ServerState.Online);

			var p1 = new Player("1", "127.0.0.1");
			p1.Name = "Um";
			p1.Device = PlayerDevice.Android;
		
			var p2 = new Player("2", "127.0.0.2");
			p2.Name = "Dois";
			p2.Device = PlayerDevice.iPhone;
			
			// Register players, create game and unregister p1.
			MultiplayerService.RegisterPlayer(p1);
			MultiplayerService.RegisterPlayer(p2);
			Assert.AreEqual(0, MultiplayerService.GetAllGames().Count);
			
			MultiplayerService.CreateGame(p1.Id);
			Assert.AreEqual(0, MultiplayerService.GetAllGames().Count);
			
			MultiplayerService.CreateGame(p2.Id);
			Assert.AreEqual(1, MultiplayerService.GetAllGames().Count);
			
			MultiplayerService.UnregisterPlayer(p1.Id, true);
			Assert.AreEqual(1, MultiplayerService.GetAllGames().Count);
			
			MultiplayerService.UnregisterPlayer(p2.Id, true);
			Assert.AreEqual(0, MultiplayerService.GetAllGames().Count);

			// Register players, create game and unregister p2.
			Assert.IsTrue(MultiplayerService.RegisterPlayer(p1));
			Assert.IsTrue(MultiplayerService.RegisterPlayer(p2));
			Assert.AreEqual(0, MultiplayerService.GetAllGames().Count);
			
			MultiplayerService.CreateGame(p1.Id);
			Assert.AreEqual(0, MultiplayerService.GetAllGames().Count);
			
			MultiplayerService.CreateGame(p2.Id);
			Assert.AreEqual(1, MultiplayerService.GetAllGames().Count);
			
			MultiplayerService.UnregisterPlayer(p2.Id, true);
			Assert.AreEqual(1, MultiplayerService.GetAllGames().Count);

			MultiplayerService.UnregisterPlayer(p1.Id, true);
			Assert.AreEqual(0, MultiplayerService.GetAllGames().Count);
		}
		
		[Test]
		public void DequeueMessages_HasTwoMessagesForPlayer_TwoMessagesAreDequeued()
		{
			var p1 = new Player("1", "127.0.0.1");
			p1.Name = "Um";
			p1.Device = PlayerDevice.Mac;
			MultiplayerService.RegisterPlayer(p1);
			
			var p2 = new Player("2", "127.0.0.2");
			p2.Name = "Dois";
			p2.Device = PlayerDevice.Windows;
			MultiplayerService.RegisterPlayer(p2);
			
			var game = MultiplayerService.CreateGame(p2.Id);	
			game = MultiplayerService.CreateGame(p1.Id);
				
			var msg = new MultiplayerMessage();
			msg.GameId = game.Id;
			msg.PlayerId = p2.Id;
			msg.Name = "msg1";
			msg.Value = "v1";
			MultiplayerService.EnqueueMessage(msg);
			
			
			var msgs = MultiplayerService.DequeueMessages(p1.Id);
			Assert.AreEqual(1, msgs.Count);
			msg = msgs[0];
			Assert.AreEqual(p1.Id, msg.PlayerId);
			Assert.AreEqual("GameCreated", msg.Name);
			Assert.AreEqual(typeof(Player), msg.Value.GetType());
			Assert.AreEqual(game.Id, msg.GameId);
			
			msgs = MultiplayerService.DequeueMessages(p1.Id);
			Assert.AreEqual(1, msgs.Count);
			
			msgs = MultiplayerService.DequeueMessages(p1.Id, msgs[0].Id);
			Assert.AreEqual(0, msgs.Count);
			
			msgs = MultiplayerService.DequeueMessages(p2.Id);
			Assert.AreEqual(2, msgs.Count);
			
			msg = msgs[0];
			Assert.AreEqual(p2.Id, msg.PlayerId);
			Assert.AreEqual("GameCreated", msg.Name);
			Assert.AreEqual(typeof(Player), msg.Value.GetType());
			Assert.AreEqual(game.Id, msg.GameId);
			
			msg = msgs[1];
			Assert.AreEqual(p2.Id, msg.PlayerId);
			Assert.AreEqual("msg1", msg.Name);
			Assert.AreEqual("v1", msg.Value);
			Assert.AreEqual(game.Id, msg.GameId);
			
			msgs = MultiplayerService.DequeueMessages(p1.Id);
			Assert.AreEqual(0, msgs.Count);
		}

		[Test]
		public void DequeueMessages_WithPlayerQuit_GameEndedByQuit()
		{
			var p1 = new Player("1", "127.0.0.1");
			p1.Name = "Um";
			p1.Device = PlayerDevice.Mac;
			MultiplayerService.RegisterPlayer(p1);
			
			var p2 = new Player("2", "127.0.0.2");
			p2.Name = "Dois";
			p2.Device = PlayerDevice.Windows;
			MultiplayerService.RegisterPlayer(p2);
			
			MultiplayerService.CreateGame(p2.Id);	
			MultiplayerService.CreateGame(p1.Id);
			
			var msgs = MultiplayerService.DequeueMessages(p1.Id);
			msgs = MultiplayerService.DequeueMessages(p1.Id, msgs[msgs.Count - 1].Id);
			
			msgs = MultiplayerService.DequeueMessages(p2.Id);
			msgs = MultiplayerService.DequeueMessages(p2.Id, msgs[msgs.Count - 1].Id);
		
			MultiplayerService.UnregisterPlayer(p1.Id, true);

			msgs = MultiplayerService.DequeueMessages(p1.Id);
			Assert.AreEqual(0, msgs.Count);

			msgs = MultiplayerService.DequeueMessages(p2.Id);
			Assert.AreEqual(1, msgs.Count);
			Assert.AreEqual("GameEnded", msgs[0].Name);
			Assert.AreEqual("OnEnemyQuit", msgs[0].Value);
		}

		[Test]
		public void DequeueMessages_WithPlayerInactive_GameEndedByTimeout()
		{
			var p1 = new Player("1", "127.0.0.1");
			p1.Name = "Um";
			p1.Device = PlayerDevice.Mac;
			MultiplayerService.RegisterPlayer(p1);
			
			var p2 = new Player("2", "127.0.0.2");
			p2.Name = "Dois";
			p2.Device = PlayerDevice.Windows;
			MultiplayerService.RegisterPlayer(p2);
			
			MultiplayerService.CreateGame(p2.Id);	
			MultiplayerService.CreateGame(p1.Id);
				
			var msgs = MultiplayerService.DequeueMessages(p1.Id);
			msgs = MultiplayerService.DequeueMessages(p1.Id, msgs[msgs.Count - 1].Id);
			
			msgs = MultiplayerService.DequeueMessages(p2.Id);
			msgs = MultiplayerService.DequeueMessages(p2.Id, msgs[msgs.Count - 1].Id);
			
			Thread.Sleep(Convert.ToInt32(ServerService.PlayerInactivityTimeoutSeconds) * 1000 + 1000);
			msgs = MultiplayerService.DequeueMessages(p1.Id);
			Assert.AreEqual(1, msgs.Count);
			Assert.AreEqual("GameEnded", msgs[0].Name);
			Assert.AreEqual("OnConnectionLost", msgs[0].Value);
			
			msgs = MultiplayerService.DequeueMessages(p2.Id);
			Assert.AreEqual(1, msgs.Count);
			Assert.AreEqual("GameEnded", msgs[0].Name);
			Assert.AreEqual("OnConnectionLost", msgs[0].Value);
		}
		
		[Test]
		public void CreateGame_TryWithPlayerAlreadyPlayer_DontCreateAnotherGame()
		{
			var p1 = new Player("1", "127.0.0.1");
			p1.Name = "Um";
			p1.Device = PlayerDevice.Mac;
			MultiplayerService.RegisterPlayer(p1);
			
			var p2 = new Player("2", "127.0.0.2");
			p2.Name = "Dois";
			p2.Device = PlayerDevice.Windows;
			MultiplayerService.RegisterPlayer(p2);
			
			var game = MultiplayerService.CreateGame(p1.Id);
			Assert.IsNull(game);
			Assert.AreEqual(PlayerState.WaitingOpponent, p1.State);
			
			MultiplayerService.CreateGame(p1.Id);
			Assert.IsNull(game);
			Assert.AreEqual(PlayerState.WaitingOpponent, p1.State);
			
			game = MultiplayerService.CreateGame(p2.Id);
			Assert.IsNotNull(game);
			Assert.AreEqual(PlayerState.Playing, p1.State);
			Assert.AreEqual(PlayerState.Playing, p2.State);
			
			game = MultiplayerService.CreateGame(p2.Id);
			Assert.IsNull(game);
			Assert.AreEqual(PlayerState.Playing, p1.State);
			Assert.AreEqual(PlayerState.Playing, p2.State);
		}
		
		[Test]
		public void CreateGame_WithDifferentsGameVersions_CreateGamesWithSameGameVersion()
		{
			var p1 = new Player("1", "127.0.0.1");
			p1.Name = "Um";
			p1.Device = PlayerDevice.Mac;
			p1.GameVersion = "1.0";
			MultiplayerService.RegisterPlayer(p1);
			
			var p2 = new Player("2", "127.0.0.2");
			p2.Name = "Dois";
			p2.Device = PlayerDevice.Windows;
			p2.GameVersion = "1.1";
			MultiplayerService.RegisterPlayer(p2);
			
			var p3 = new Player("3", "127.0.0.3");
			p3.Name = "Tres";
			p3.Device = PlayerDevice.iPad;
			p3.GameVersion = "1.0";
			MultiplayerService.RegisterPlayer(p3);
			
			var p4 = new Player("4", "127.0.0.4");
			p4.Name = "Quatro";
			p4.Device = PlayerDevice.iPhone;
			p4.GameVersion = "1.1";
			MultiplayerService.RegisterPlayer(p4);
			
			var game = MultiplayerService.CreateGame(p1.Id);
			Assert.IsNull(game);
			Assert.AreEqual(PlayerState.WaitingOpponent, p1.State);
			Assert.AreEqual(PlayerState.Lobby, p2.State);
			Assert.AreEqual(PlayerState.Lobby, p3.State);
			Assert.AreEqual(PlayerState.Lobby, p4.State);
			
			game = MultiplayerService.CreateGame(p2.Id);
			Assert.IsNull(game);
			Assert.AreEqual(PlayerState.WaitingOpponent, p1.State);
			Assert.AreEqual(PlayerState.WaitingOpponent, p2.State);
			Assert.AreEqual(PlayerState.Lobby, p3.State);
			Assert.AreEqual(PlayerState.Lobby, p4.State);
			
			game = MultiplayerService.CreateGame(p3.Id);
			Assert.IsNotNull(game);
			Assert.AreEqual(PlayerState.Playing, p1.State);
			Assert.AreEqual(PlayerState.WaitingOpponent, p2.State);
			Assert.AreEqual(PlayerState.Playing, p3.State);
			Assert.AreEqual(PlayerState.Lobby, p4.State);

			var statistics = ServerService.GetStatistics();
			Assert.AreEqual(1, statistics.MaxSimultaneousGamesCount);
			
			game = MultiplayerService.CreateGame(p4.Id);
			Assert.IsNotNull(game);
			Assert.AreEqual(PlayerState.Playing, p1.State);
			Assert.AreEqual(PlayerState.Playing, p2.State);
			Assert.AreEqual(PlayerState.Playing, p3.State);
			Assert.AreEqual(PlayerState.Playing, p4.State);

			statistics = ServerService.GetStatistics();
			Assert.AreEqual(2, statistics.MaxSimultaneousGamesCount);
		}
	}
}