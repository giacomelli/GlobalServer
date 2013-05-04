//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using Skahal.Data.MySql;

#region Usings
using System;
using Skahal.GlobalServer.Domain.Servers;
#endregion

namespace Skahal.GlobalServer.Infrastructure.Repositories.MySql
{
	/// <summary>
	/// A MySql IServerRepository.
	/// </summary>
	public class MySqlServerRepository : IServerRepository
	{
		#region IServerRepository implementation
		public void SaveStatistics (ServerStatistics statistics)
		{
			MySqlHelper.Update(@"
				UPDATE 
						ServerStatistics 
				SET TotalPlayersCount 			= @TotalPlayersCount,
					TotaliPodDevicesCount		= @TotaliPodDevicesCount,
					TotaliPhoneDevicesCount 	= @TotaliPhoneDevicesCount, 
					TotaliPadDevicesCount 		= @TotaliPadDevicesCount,
					TotalMacDevicesCount		= @TotalMacDevicesCount,
					TotalWindowsDevicesCount 	= @TotalWindowsDevicesCount,
					TotalAndroidDevicesCount	= @TotalAndroidDevicesCount, 
					TotalEditorDevicesCount		= @TotalEditorDevicesCount, 
					TotalGamesCount				= @TotalGamesCount, 
					TotalRequestsCount			= @TotalRequestsCount, 
					MaxSimultaneousGamesCount	= @MaxSimultaneousGamesCount,
					TotalEndedGamesCount 		= @TotalEndedGamesCount,
					TotalWebDevicesCount 		= @TotalWebDevicesCount,
					TotalQuitGamesCount			= @TotalQuitGamesCount
			",
			"@TotalPlayersCount", statistics.TotalPlayersCount,
			"@TotaliPodDevicesCount", statistics.TotaliPodDevicesCount,
			"@TotaliPhoneDevicesCount", statistics.TotaliPhoneDevicesCount,
			"@TotaliPadDevicesCount", statistics.TotaliPadDevicesCount,
			"@TotalMacDevicesCount", statistics.TotalMacDevicesCount,
			"@TotalWindowsDevicesCount", statistics.TotalWindowsDevicesCount,
			"@TotalAndroidDevicesCount", statistics.TotalAndroidDevicesCount,
			"@TotalEditorDevicesCount", statistics.TotalEditorDevicesCount,
			"@TotalGamesCount", statistics.TotalGamesCount,
			"@TotalRequestsCount", statistics.TotalRequestsCount,
			"@MaxSimultaneousGamesCount", statistics.MaxSimultaneousGamesCount,
			"@TotalEndedGamesCount", statistics.TotalEndedGamesCount,
			"@TotalWebDevicesCount", statistics.TotalWebDevicesCount,
			"@TotalQuitGamesCount", statistics.TotalQuitGamesCount);
		}

		public ServerStatistics FindStatistics ()
		{
			var statistics = new ServerStatistics();
			
			MySqlHelper.Select((row) => {
				statistics.TotalAndroidDevicesCount = row.GetInt32("TotalAndroidDevicesCount");
				statistics.TotalEditorDevicesCount = row.GetInt32("TotalEditorDevicesCount");
				statistics.TotalGamesCount = row.GetInt32("TotalGamesCount");
				statistics.TotaliPadDevicesCount = row.GetInt32("TotaliPadDevicesCount");
				statistics.TotaliPhoneDevicesCount = row.GetInt32("TotaliPhoneDevicesCount");
				statistics.TotaliPodDevicesCount = row.GetInt32("TotaliPodDevicesCount");
				statistics.TotalMacDevicesCount = row.GetInt32("TotalMacDevicesCount");
				statistics.TotalPlayersCount = row.GetInt32("TotalPlayersCount");
				statistics.TotalRequestsCount = row.GetInt32("TotalRequestsCount");
				statistics.TotalWindowsDevicesCount = row.GetInt32("TotalWindowsDevicesCount");	
				statistics.MaxSimultaneousGamesCount = row.GetInt32("MaxSimultaneousGamesCount");
				statistics.TotalEndedGamesCount = row.GetInt32("TotalEndedGamesCount");
				statistics.TotalWebDevicesCount = row.GetInt32("TotalWebDevicesCount");
				statistics.TotalQuitGamesCount = row.GetInt32("TotalQuitGamesCount");
			}, 
			"SELECT * FROM ServerStatistics");
			
			return statistics;
		}
		#endregion
	}
}