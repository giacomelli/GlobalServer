//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using System.Web;
using System.Web.UI;
using System.Net.Sockets;
using Skahal.GlobalServer.Domain.Storages;

namespace Skahal.GlobalServer.Web
{
	public class SetValue : ServiceMethodBase<object>
	{
		#region implemented abstract members of Skahal.GlobalServer.ServiceMethodBase
		protected override object Execute (ServiceMethodContext context)
		{
			var storage = new Storage();
			storage.PlayerId = context.PlayerId;
			storage.Key = context.StorageKey;
			storage.Value = context.StorageValue;

			StorageService.SaveStorage(storage);

			return null;
		}
		#endregion
	}	
}