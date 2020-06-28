using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using BusinessLogic;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Repository;
using Xunit;

namespace UnitTests
{
	public class OwnerServiceTests
	{
		[Fact]
		public void GetAllOwners_Sorts_Results_By_Name()
		{
			// Arrange
			var mockOwnerRepository = new Mock<IOwnerRepository>();

			var dylan = new Owner()
			{
				Id = Guid.NewGuid(),
				Name = "Dylan",
				Address = "my address"
			};

			var crystal = new Owner()
			{
				Id = Guid.NewGuid(),
				Name = "Crystal",
				Address = "my address"
			};

			var ownersToReturn = new List<Owner>()
			{
				dylan,
				crystal
			}.AsQueryable();

			mockOwnerRepository.Setup(m => m.FindAll()).Returns(ownersToReturn);

			var mockLogger = NullLogger<OwnerService>.Instance;

			var mockMapper = new Mock<IMapper>();

			var ownersAsDtos = ownersToReturn.Select(o => new OwnerDto()
			{
				Id = o.Id,
				Name = o.Name,
				Address = o.Address
			});

			mockMapper.Setup(m => m.Map<IEnumerable<OwnerDto>>(
				It.Is<IEnumerable<Owner>>(
					o => o.ToList()[0] == crystal && o.ToList()[1] == dylan))
			).Returns(ownersAsDtos);

			// Act
			var ownerService = new OwnerService(mockOwnerRepository.Object, mockLogger, mockMapper.Object);
			var result = ownerService.GetAllOwners();

			// Assert
			Assert.Equal(2, result.Count());
		}
	}
}
