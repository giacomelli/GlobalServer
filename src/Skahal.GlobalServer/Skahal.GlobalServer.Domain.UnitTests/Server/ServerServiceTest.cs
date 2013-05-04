//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using NUnit.Framework;
using Skahal.GlobalServer.Domain.Servers;
using System.Threading.Tasks;
using Rhino.Mocks;
using Skahal.GlobalServer.Domain.Players;

namespace Skahal.GlobalServer.Domain.UnitTests
{
	[TestFixture()]
	public class ServerServiceTest
	{
		#region Initialize
		[TestFixtureSetUp]
		public void InitializeClass()
		{
			ServerService.Initialize(MockRepository.GenerateMock<IServerRepository>());
		}
		#endregion
		
		#region Tests
		[Test()]
		public void RegisterRequest_ParallelRequests_RequestsCountOk ()
		{	
			ServerService.Reset();
			Parallel.For(0, 1000, (i) => {
				ServerService.RegisterRequest();
			});           
			
			Assert.AreEqual(1000, ServerService.GetServer(null).RequestsCount);
		}
		
		[Test]
		public void Reset_WithRequestsCountGreaterThanZero_RequestsCountEqualZero()
		{
			ServerService.RegisterRequest();
			ServerService.Reset();
			Assert.AreEqual(0, ServerService.GetServer(null).RequestsCount);
		}

		[Test]
		public void RegisterPlayer_WithDevice_IncrementCounters()
		{
			AssertRegisterPlayer(PlayerDevice.Android);
			AssertRegisterPlayer(PlayerDevice.Editor);
			AssertRegisterPlayer(PlayerDevice.iPad);
			AssertRegisterPlayer(PlayerDevice.iPhone);
			AssertRegisterPlayer(PlayerDevice.iPod);
			AssertRegisterPlayer(PlayerDevice.Mac);
			AssertRegisterPlayer(PlayerDevice.Windows);
			AssertRegisterPlayer(PlayerDevice.Web);

		}

		[Test]
		public void ChangeState_WaitingUpdate_CantRegisterPlayers()
		{
			ServerService.Initialize(MockRepository.GenerateMock<IServerRepository>());
			var player = new Player("ID", "IP");
			player.Device = PlayerDevice.Android;

			// Online.
			Assert.IsTrue(ServerService.RegisterPlayer(player));
			var statistics = ServerService.GetStatistics();
			Assert.AreEqual(1, statistics.TotalPlayersCount);

			// Waiting for update.
			ServerService.ChangeServerState(ServerState.WaitingUpdate);
			Assert.IsFalse(ServerService.RegisterPlayer(player));
			statistics = ServerService.GetStatistics();
			Assert.AreEqual(1, statistics.TotalPlayersCount);

			// Back online.
			ServerService.ChangeServerState(ServerState.Online);
			Assert.IsTrue(ServerService.RegisterPlayer(player));
			statistics = ServerService.GetStatistics();
			Assert.AreEqual(2, statistics.TotalPlayersCount);

		}
		#endregion

		#region Helpers
		private static void AssertRegisterPlayer(PlayerDevice device)
		{
			ServerService.Initialize(MockRepository.GenerateMock<IServerRepository>());
			var player = new Player("ID", "IP");
			player.Device = device;
			ServerService.RegisterPlayer(player);

			var statistics = ServerService.GetStatistics();
			Assert.AreEqual(1, statistics.GetDeviceCount(device), device + " should be incremented");
			Assert.AreEqual(1, statistics.TotalPlayersCount, device + "shoulb be increment on TotalPlayersCount");

			ServerService.RegisterPlayer(player);

			statistics = ServerService.GetStatistics();
			Assert.AreEqual(2, statistics.GetDeviceCount(device), device + " should be incremented");
			Assert.AreEqual(2, statistics.TotalPlayersCount, device + "shoulb be increment on TotalPlayersCount");
		}

		#endregion
	}
}