//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using Skahal.GlobalServer.Domain.Players;

namespace Skahal.GlobalServer.Domain.Servers
{
	public class ServerStatistics
	{
		#region Properties
		public long TotalPlayersCount { get; set; }
		public long TotalGamesCount { get; set; }
		public long TotalRequestsCount { get; set; }
		public long TotaliPodDevicesCount { get; set; }
		public long TotaliPhoneDevicesCount { get; set; }
		public long TotaliPadDevicesCount { get; set; }
		public long TotalMacDevicesCount { get; set; }
		public long TotalWindowsDevicesCount { get; set; }
		public long TotalAndroidDevicesCount { get; set; }
		public long TotalEditorDevicesCount { get; set; }
		public int MaxSimultaneousGamesCount { get; set; }
		public long TotalEndedGamesCount { get; set; }
		public long TotalWebDevicesCount { get; set; }
		public long TotalQuitGamesCount { get; set; }
		#endregion

		#region Methods
		public long GetDeviceCount(PlayerDevice device)
		{
			switch(device)
			{
			case PlayerDevice.Android:
				return TotalAndroidDevicesCount;
		
			case PlayerDevice.Editor:
				return TotalEditorDevicesCount;

			case PlayerDevice.iPad:
				return TotaliPadDevicesCount;

			case PlayerDevice.iPhone:
				return TotaliPhoneDevicesCount;

			case PlayerDevice.iPod:
				return TotaliPodDevicesCount;

			case PlayerDevice.Mac:
				return TotalMacDevicesCount;

			case PlayerDevice.Web:
				return TotalWebDevicesCount;

			case PlayerDevice.Windows:
				return TotalWindowsDevicesCount;

			default:
				throw new InvalidOperationException("Invalid device.");
			}
		}
		#endregion
	}
}