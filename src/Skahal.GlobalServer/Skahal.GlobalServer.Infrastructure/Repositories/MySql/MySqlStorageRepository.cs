//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using Skahal.GlobalServer.Domain.Storages;
using Skahal.Data.MySql;

namespace Skahal.GlobalServer.Infrastructure.Repositories.MySql
{
	/// <summary>
	/// A MySql storage repository.
	/// </summary>
	public class MySqlStorageRepository : IStorageRepository
	{
		#region IStorageRepository implementation

		public void InsertStorage (Storage storage)
		{
			MySqlHelper.Update(@"
				INSERT 
					Storage(PlayerId, `Key`, `Value`) 
					VALUES(@PlayerId, @Key, @Value)",
                   "@Value", storage.Value,
                   "@PlayerId", storage.PlayerId,
                   "@Key", storage.Key);
		}

		public void UpdateStorage (Storage storage)
		{
			MySqlHelper.Update(@"
				UPDATE 
					Storage 
				SET 
					`Value` = @Value
				WHERE
					PlayerId = @PlayerId
				AND `Key`	 = @Key",
               "@Value", storage.Value,
               "@PlayerId", storage.PlayerId,
               "@Key", storage.Key);
		}

		public Storage FindStorage (string playerId, string key)
		{
			Storage storage = null;
			
			MySqlHelper.Select((row) => {
				storage = new Storage();
				storage.PlayerId  = row.GetString("PlayerId");
				storage.Key  = row.GetString("Key");
				storage.Value  = row.GetString("Value");
			}, 
			@"SELECT * FROM Storage
			WHERE
				PlayerId = @PlayerId
			AND `Key`	 = @Key", 
			"@PlayerId", playerId,
			"@Key", key);
			
			return storage;
		}

		#endregion
	}
}

