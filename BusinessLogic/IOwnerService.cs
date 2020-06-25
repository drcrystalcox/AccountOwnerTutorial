using System;
using System.Collections.Generic;
using Entities.DataTransferObjects;

namespace BusinessLogic
{
	public interface IOwnerService
	{
		IEnumerable<OwnerDto> GetAllOwners();

		OwnerDto GetOwnerById(Guid ownerId);

		OwnerDto GetOwnerWithDetails(Guid ownerId);

		OwnerDto CreateOwner(OwnerForCreationDto owner);

		void UpdateOwner(Guid id, OwnerForUpdateDto owner);

		void DeleteOwner(OwnerDto owner);
	}
}