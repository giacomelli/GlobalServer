//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using System.Collections.Generic;

namespace Skahal.GlobalServer.Domain.Multiplayer.Patches
{
	public interface IMultiplayerPatch
	{
		#region Methods
		bool CanPatchForGameVersion(string gameVersion);
		bool CanDequeueMessages(IList<MultiplayerMessage> enqueuedMessages);
		#endregion
	}
}