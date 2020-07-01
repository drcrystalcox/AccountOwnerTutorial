using System;
using System.Collections.Generic;
using Entities.DataTransferObjects;
using Entities.Models;
using System.Threading.Tasks;

namespace BusinessLogic
{
	public interface IOwnerService
	{
		 Task<IEnumerable<OwnerDto>> GetAllOwnersAsync();

		 Task<OwnerDto> GetOwnerDtoByIdAsync(Guid ownerId);
		
		 Task<Owner> GetOwnerByIdAsync(Guid ownerId);

		 Task<OwnerDto> GetOwnerWithDetailsAsync(Guid ownerId);

		 Task<OwnerDto> CreateOwnerAsync(OwnerForCreationDto owner);

		 Task UpdateOwnerAsync(Guid id, OwnerForUpdateDto owner);

		 Task DeleteOwnerAsync(Owner owner);
	}
}