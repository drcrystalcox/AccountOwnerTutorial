using System;
using System.Collections.Generic;
using Entities.DataTransferObjects;
using Entities.Models;

namespace BusinessLogic
{
	public interface IOwnerService
	{
		IEnumerable<OwnerDto> GetAllOwners();

		OwnerDto GetOwnerDtoById(Guid ownerId);
		
		Owner GetOwnerById(Guid ownerId);

		OwnerDto GetOwnerWithDetails(Guid ownerId);

		OwnerDto CreateOwner(OwnerForCreationDto owner);

		void UpdateOwner(Guid id, OwnerForUpdateDto owner);

		void DeleteOwner(Owner owner);
	}
}