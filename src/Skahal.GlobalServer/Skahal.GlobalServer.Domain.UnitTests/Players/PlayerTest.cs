//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using NUnit.Framework;
using Skahal.GlobalServer.Domain.Players;

namespace Skahal.GlobalServer.Domain.UnitTests
{
	[TestFixture]
	public class PlayerTest
	{
		[Test]
		public void CreateTest ()
		{
			var playerId = Guid.NewGuid().ToString();	
			var target = new Player(playerId, "127.0.0.1");
			
			Assert.AreEqual(playerId, target.Id);
			
			Assert.AreEqual("127.0.0.1", target.IP);

			Assert.IsNull(target.Name);
			target.Name = "Hal";
			Assert.AreEqual("Hal", target.Name);
			
			target.Device = PlayerDevice.iPhone;
			Assert.AreEqual(PlayerDevice.iPhone, target.Device);	
			
			target.GameVersion = "1.4";
			Assert.AreEqual("1.4", target.GameVersion);
		}
	}
}

