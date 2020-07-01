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
using System.Threading.Tasks;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace BusinessLogic
{
	public class OwnerService : IOwnerService
	{
		private readonly IRepositoryWrapper _repositoryWrapper;

		private readonly ILogger<OwnerService> _logger;

		private readonly IMapper _mapper;

		public OwnerService(IRepositoryWrapper repoWrapper, ILogger<OwnerService> logger, IMapper mapper)
		{
			_repositoryWrapper=repoWrapper;
			_logger = logger;
			_mapper = mapper;
		}

		public async Task<IEnumerable<OwnerDto>>  GetAllOwnersAsync()
		{
			var results = await  _repositoryWrapper.Owner.GetAllOwnersAsync();
			return _mapper.Map<IEnumerable<OwnerDto>>(results);
		}

		public async Task<IEnumerable<OwnerDto>> GetAllOwnersNoAutoMapperAsync()
		{
			var results = await _repositoryWrapper.Owner.GetAllOwnersAsync();

			var ownerDtos = results.Select(o => new OwnerDto()
			{
				Id = o.Id,
				Name = o.Name,
				Address = o.Address,
				Accounts = o.Accounts.Select(a => new AccountDto()
				{
					AccountType = a.AccountType,
					DateCreated = a.DateCreated,
					Id = a.Id
				})
			});

			return ownerDtos;
		}

		public async Task<OwnerDto> GetOwnerDtoByIdAsync(Guid ownerId)
		{
			if (ownerId == Guid.Empty)
			{
				throw new ArgumentOutOfRangeException("The owner id may not be an empty guid.");
			}

			var result =  await _repositoryWrapper.Owner.GetOwnerByIdAsync(ownerId);
			//FindByCondition(owner => owner.Id.Equals(ownerId)).FirstOrDefault();
			return _mapper.Map<OwnerDto>(result);
		}

		public async Task<Owner> GetOwnerByIdAsync(Guid ownerId)
		{
			if (ownerId == Guid.Empty)
			{
				throw new ArgumentOutOfRangeException("The owner id may not be an empty guid.");
			}

			var result =  await _repositoryWrapper.Owner.GetOwnerByIdAsync(ownerId);
			//.FindByCondition(owner => owner.Id.Equals(ownerId)).FirstOrDefault();
			return result;
		}

		public async Task<OwnerDto> GetOwnerWithDetailsAsync(Guid ownerId)
		{
			var result = await  _repositoryWrapper.Owner.GetOwnerWithDetailsAsync(ownerId) ;
			return _mapper.Map<OwnerDto>(result);
		}

		public async Task<OwnerDto> CreateOwnerAsync(OwnerForCreationDto owner)
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

			await _repositoryWrapper.Owner.CreateOwnerAsync(ownerEntity);
			return _mapper.Map<OwnerDto>(ownerEntity);
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

		public async Task UpdateOwnerAsync(Guid id, OwnerForUpdateDto owner)
		{
			// Want to try to find the owner first before updating. Throw error if the owner does not exist.
			// What if the user attempts to modify the id field for the owner? Throw error. 
			var ownerEntity = await GetOwnerByIdAsync(id);
			if (ownerEntity == null)
			{
				_logger.LogError($"Owner with id: {id}, hasn't been found in db.");
				return;
				// TODO: need to return notfound to the user
			}

			_mapper.Map(owner, ownerEntity);
			 await _repositoryWrapper.Owner.UpdateOwnerAsync(ownerEntity);
		}

		public async Task DeleteOwnerAsync(Owner owner)
		{
			 await _repositoryWrapper.Owner.DeleteOwnerAsync(owner);
		}
    }
}
