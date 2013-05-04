//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using NUnit.Framework;
using Skahal.GlobalServer.Domain.Multiplayer;
using Skahal.GlobalServer.Domain.Players;
using System.Threading;

namespace Skahal.GlobalServer.Domain.UnitTests
{
	[TestFixture()]
	public class MultiplayerGameTest
	{
		[Test()]
		public void Constructor_AllPlayersHasNoIP_CommunicationTypeIsAuthoritativeServer ()
		{
			var p1 = new Player("1", "");
			var p2 = new Player("2", null);
			var target = new MultiplayerGame(p1, p2);
			Assert.AreEqual(GameCommunicationType.AuthoritativeServer, target.CommunicationType);
		}
		
		[Test]
		public void Constructor_HostPlayerHasIPGuessPlayerHasNoIP_CommunicationTypeIsAuthoritativeServer()
		{
			var p1 = new Player("1", "1.1.1.1");
			var p2 = new Player("2", null);
			var target = new MultiplayerGame(p1, p2);
			Assert.AreEqual(GameCommunicationType.AuthoritativeServer, target.CommunicationType);
		}
		
		[Test]
		public void Constructor_AllPlayersHasIP_CommunicationTypeIsP2P()
		{
			var p1 = new Player("1", "1.1.1.1");
			var p2 = new Player("2", "2.2.2.2");
			var target = new MultiplayerGame(p1, p2);
			Assert.AreEqual(GameCommunicationType.P2P, target.CommunicationType);
		}
		
		[Test]
		public void Constructor_AllPlayersSpecified_PlayersStateIsPlaying()
		{	
			var p1 = new Player("1", "1.1.1.1");
			var p2 = new Player("2", "2.2.2.2");
			var target = new MultiplayerGame(p1, p2);
			Assert.AreEqual(PlayerState.Playing, target.HostPlayer.State);
			Assert.AreEqual(PlayerState.Playing, target.GuessPlayer.State);
		}
		
		[Test]
		public void MarkMessageAsReceived_HasNoMessage_ReturnsFalse()
		{
			var p1 = new Player("1", "1.1.1.1");
			var p2 = new Player("2", "2.2.2.2");
			var target = new MultiplayerGame(p1, p2);
			
			var msgs = target.DequeueMessages(p1.Id);
			var msgId = msgs[0].Id;
			target.MarkMessageAsReceived(p1.Id, msgId);
				
			Assert.IsFalse(target.MarkMessageAsReceived(p1.Id, msgId));
		}
		
		[Test]
		public void MarkMessageAsReceived_HasPreviousMesssagesNotReceived_MarkAllPreviousAsReceivedAndReturnsTrue()
		{
			var p1 = new Player("1", "1.1.1.1");
			var p2 = new Player("2", "2.2.2.2");
			var target = new MultiplayerGame(p1, p2);
			
			var msg = new MultiplayerMessage();
			msg.GameId = target.Id;
			msg.Name = "1";
			msg.Value = "1";
			msg.PlayerId = p1.Id;
			target.EnqueueMessage(msg);
			
			msg = new MultiplayerMessage();
			msg.GameId = target.Id;
			msg.Name = "2";
			msg.Value = "2";
			msg.PlayerId = p1.Id;
			target.EnqueueMessage(msg);
			
			var msgs = target.DequeueMessages(p1.Id);
			Assert.AreEqual(3, msgs.Count);
			var msgId = msgs[1].Id;
			Assert.IsTrue(target.MarkMessageAsReceived(p1.Id, msgId));
			
			msgs = target.DequeueMessages(p1.Id);
			Assert.AreEqual(1, msgs.Count);
			
			Assert.IsFalse(target.MarkMessageAsReceived(p1.Id, msgId));
			msgs = target.DequeueMessages(p1.Id);
			Assert.AreEqual(1, msgs.Count);
			msg = msgs[0];
			Assert.AreEqual("2", msg.Name);
			
			Assert.IsTrue(target.MarkMessageAsReceived(p1.Id, msg.Id));
			
			msgs = target.DequeueMessages(p1.Id);
			Assert.AreEqual(0, msgs.Count);
			
			msgs = target.DequeueMessages(p2.Id);
			Assert.AreEqual(1, msgs.Count);
		}
		
		//[Test]
		public void DequeueMessages_WithDiffTimes_LastActivityDateUpdated()
		{	
			var date = DateTime.Now;
			var p1 = new Player("1", "1.1.1.1");
			var p2 = new Player("2", "2.2.2.2");
			var target = new MultiplayerGame(p1, p2);
			
			Assert.IsTrue(p1.LastActivityDate > date);
			Assert.IsTrue(p2.LastActivityDate > date);
			
			date = DateTime.Now;
			Thread.Sleep(10);
			target.DequeueMessages(p1.Id);
			Assert.IsTrue(p1.LastActivityDate > date);
			Assert.IsTrue(p2.LastActivityDate < date);
			
			date = DateTime.Now;
			Thread.Sleep(10);
			target.DequeueMessages(p2.Id);
			Assert.IsTrue(p1.LastActivityDate < date);
			Assert.IsTrue(p2.LastActivityDate > date);
			
			date = DateTime.Now;
			Thread.Sleep(10);
			target.DequeueMessages(p1.Id);
			target.DequeueMessages(p2.Id);
			Assert.IsTrue(p1.LastActivityDate > date);
			Assert.IsTrue(p2.LastActivityDate > date);
		}

	}
}

