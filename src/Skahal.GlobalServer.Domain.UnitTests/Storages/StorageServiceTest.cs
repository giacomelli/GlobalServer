//  Author: Diego Giacomelli <giacomelli@gmail.com>
//  Copyright (c) 2012 Skahal Studios
using System;
using NUnit.Framework;
using Skahal.GlobalServer.Domain.Storages;
using Rhino.Mocks;

namespace Skahal.GlobalServer.Domain.UnitTests
{
	[TestFixture()]
	public class StorageServiceTest
	{
		[Test()]
		public void SaveStorage_StorageDoesNotExists_InsertsStorage ()
		{
			var storage = new Storage();
			storage.PlayerId = "playerId";
			storage.Key = "key";
			storage.Value = "value";

			var repository = MockRepository.GenerateMock<IStorageRepository>();
			repository.Expect(r => r.FindStorage("playerId", "key")).Return(null);
			repository.Expect(r => r.InsertStorage(storage));
			StorageService.Initialize(repository);

			StorageService.SaveStorage(storage);

			repository.VerifyAllExpectations();
		}

		[Test()]
		public void SaveStorage_StorageExists_UpdatesStorage ()
		{
			var oldStorage = new Storage();
			oldStorage.PlayerId = "playerId";
			oldStorage.Key = "key";
			oldStorage.Value = "oldValue";

			var newStorage = new Storage();
			newStorage.PlayerId = "playerId";
			newStorage.Key = "key";
			newStorage.Value = "newValue";
			
			var repository = MockRepository.GenerateMock<IStorageRepository>();
			repository.Expect(r => r.FindStorage("playerId", "key")).Return(oldStorage);
			repository.Expect(r => r.UpdateStorage(newStorage));
			StorageService.Initialize(repository);
			
			StorageService.SaveStorage(newStorage);
			
			repository.VerifyAllExpectations();
		}
	}
}

