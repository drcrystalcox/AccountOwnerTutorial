using System;
using System.Collections.Generic;
using BusinessLogic;
using Xunit;
using Moq;
using Repository;
using Microsoft.Extensions.Logging.Abstractions;
using AutoMapper;
using System.Linq;

namespace UnitTest {
    public class OwnerServiceTests{
        [Fact]
        public void GetAllOwners_Sorts_Results_By_Name(){
            //test one thing and one thing only usually
            
            //Arrange
            var mockOwnerRepository = new Mock<IOwnerRepository>();
            var mockLogger = NullLogger<OwnerService>.Instance;
            //var logger = new Mock<ILogger<OwnerService>>();
            // you can have strict or loose mockup 

            var mockMapper = new Mock<IMapper>();

            var ownerService = new OwnerService(mockOwnerRepository.Object,mockLogger,mockMapper.Object);

            //Act
            var result = ownerService.GetAllOwners();
            
            //Assert
            Assert.Equal(2,result.Count());
        }
    }
}