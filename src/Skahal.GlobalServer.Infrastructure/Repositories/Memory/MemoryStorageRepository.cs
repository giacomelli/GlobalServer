using System;
using Skahal.GlobalServer.Domain.Storages;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Skahal.GlobalServer.Infrastructure.Repositories.Memory
{
	public class MemoryStorageRepository : IStorageRepository
	{
		#region Fields
		private Dictionary<string, Storage> m_storages = new Dictionary<string, Storage>();
		#endregion

		#region IStorageRepository implementation

		public void InsertStorage (Storage storage)
		{
			m_storages.Add(GetKey(storage), storage);
		}

		public void UpdateStorage (Storage storage)
		{
			m_storages[GetKey(storage)] = storage;
		}

		public Storage FindStorage (string playerId, string key)
		{
			return m_storages.Select(s => s.Value).FirstOrDefault(s => 
			                                 s.PlayerId.Equals(playerId, StringComparison.OrdinalIgnoreCase)
			                                 && s.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
		}

		private string GetKey(Storage storage)
		{
			return String.Format(CultureInfo.InvariantCulture, "{0}_{1}", storage.PlayerId, storage.Key);
		}
		#endregion
	}
}

