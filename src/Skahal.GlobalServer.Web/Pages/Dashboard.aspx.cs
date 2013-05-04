//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using System.Web;
using System.Web.UI;
using Skahal.GlobalServer.Domain.Multiplayer;
using Skahal.GlobalServer.Domain.Servers;
using Skahal.GlobalServer.Domain.Players;
using System.Configuration;

namespace Skahal.GlobalServer.Web
{
	/// <summary>
	/// A basic dashboard for Global Server.
	/// </summary>
	public partial class Dashboard : System.Web.UI.Page
	{
		#region Properties
		public new Server Server { get; private set; }

		public ServerStatistics Statistics { get; private set; }

		public long TotalLostConnectionsGames
		{
			get{
				return Statistics.TotalGamesCount - Statistics.TotalEndedGamesCount - Statistics.TotalQuitGamesCount;
			}
		}
		
		public string LastErrorMessage
		{
			get{
				var error = HttpContext.Current.Server.GetLastError();
				
				if(error == null)
				{
					return string.Empty;
				}
				
				return error.Message;
				
			}
		}

		public string GameName 
		{
			get {
				return ConfigurationManager.AppSettings["GameName"];
			}
		}
		#endregion

		#region Methods
		protected override void OnLoad (EventArgs e)
		{
			Server = ServerService.GetServer(null);
			Statistics = ServerService.GetStatistics();
			
			base.OnLoad(e);
		
			gamesRepeater.DataSource = MultiplayerService.GetAllGames();
			gamesRepeater.DataBind();
			
			playersRepeater.DataSource =  MultiplayerService.GetAllPlayers();
			playersRepeater.DataBind();
		}
		#endregion
	}
}

