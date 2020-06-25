using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repository;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace BusinessLogic
{
	public class OwnerService : IOwnerService
	{
		private readonly IOwnerRepository _ownerRepository;

		private readonly ILogger<OwnerService> _logger;

		private readonly IMapper _mapper;

		public OwnerService(IOwnerRepository ownerRepository, ILogger<OwnerService> logger, IMapper mapper)
		{
			_ownerRepository = ownerRepository;
			_logger = logger;
			_mapper = mapper;
		}

		public IEnumerable<OwnerDto> GetAllOwners()
		{
			var results = _ownerRepository.FindAll().OrderBy(ow => ow.Name).ToList();
			return _mapper.Map<IEnumerable<OwnerDto>>(results);
		}

		public OwnerDto GetOwnerById(Guid ownerId)
		{
			if (ownerId == Guid.Empty)
			{
				throw new ArgumentOutOfRangeException("The owner id may not be an empty guid.");
			}

			var result = _ownerRepository.FindByCondition(owner => owner.Id.Equals(ownerId)).FirstOrDefault();
			return _mapper.Map<OwnerDto>(result);
		}

		public OwnerDto GetOwnerWithDetails(Guid ownerId)
		{
			var result =  _ownerRepository.FindByCondition(owner => owner.Id.Equals(ownerId)).Include(ac => ac.Accounts).FirstOrDefault();
			return _mapper.Map<OwnerDto>(result);
		}

		public OwnerDto CreateOwner(OwnerForCreationDto owner)
		{
			// TODO Really, we could return a data structure IValidationResult<T>, where T is OwnerDto, that contains a list of valid errors and an OwnerDto "result" object
			// Just logging the validation errors at the moment.
			ValidateUserInput(owner);

			var ownerEntity = _mapper.Map<Owner>(owner);
			//var ownerEntity = new Owner()
			//{
			//	Address = owner.Address,
			//	DateOfBirth = owner.DateOfBirth,
			//	Id = Guid.NewGuid(),
			//	Accounts = new List<Account>()
			//};

			_ownerRepository.CreateOwner(ownerEntity);
			return _mapper.Map<OwnerDto>(owner);
		}

		private void ValidateUserInput(OwnerForCreationDto owner)
		{
			var context = new ValidationContext(owner, serviceProvider: null, items: null);
			var results = new List<ValidationResult>();

			var isValid = Validator.TryValidateObject(owner, context, results);

			if (!isValid)
			{
				foreach (var validationResult in results)
				{
					_logger.Log(LogLevel.Information, validationResult.ErrorMessage);
					// TODO: Want to actually build up an error message to return to the user, along with a bad request 400 to the user
				}
			}
		}

		public void UpdateOwner(Guid id, OwnerForUpdateDto owner)
		{
			// Want to try to find the owner first before updating. Throw error if the owner does not exist.
			// What if the user attempts to modify the id field for the owner? Throw error. 
			var ownerEntity = GetOwnerById(id);
			if (ownerEntity == null)
			{
				_logger.LogError($"Owner with id: {id}, hasn't been found in db.");
				return;
				// TODO: need to return notfound to the user
			}

			var ownerToUpdate = _mapper.Map<Owner>(owner);
			_ownerRepository.UpdateOwner(ownerToUpdate);
		}

		public void DeleteOwner(OwnerDto owner)
		{
			// TODO: Want to try to find the owner first before updating. Throw error if the owner does not exist.
			var ownerEntity = _mapper.Map<Owner>(owner);
			_ownerRepository.DeleteOwner(ownerEntity);
		}
    }
}
