using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using MsBanking.Common.Dto;
using MsBanking.Common.Entity;
using MsBanking.Core.Branch.Services;
using System.Text.Json;

namespace MsBanking.Core.Branch.Test
{
    public class BranchServiceTest
    {
        private readonly BranchService branchService;
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<IDistributedCache> mockDistributedCache;

        public BranchServiceTest()
        {
            var mockDb = TestHelper.GetInMemoryDbContext();
            mockMapper = new Mock<IMapper>();
            mockDistributedCache = new Mock<IDistributedCache>();
            branchService = new BranchService(mockDb, mockMapper.Object, mockDistributedCache.Object);

          
        }

        [Fact]
        public async Task GetBranchesAsync_ShouldReturnBranches()
        {
            // Arrange
            var branches = new List<BranchResponseDto>
            {
                new BranchResponseDto { Id = 1, Name = "Branch 1" },
                new BranchResponseDto { Id = 2, Name = "Branch 2" }
            };
            mockDistributedCache.Setup(x => x.GetString(It.IsAny<string>())).Returns((string)null);
            mockMapper.Setup(x => x.Map<List<BranchResponseDto>>(It.IsAny<List<MsBanking.Common.Entity.Branch>>())).Returns(branches);
            mockDistributedCache.Setup(x => x.SetString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DistributedCacheEntryOptions>()));

            // Act
            var result = await branchService.GetBranchesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetBranchesAsync_ShouldReturnBranchesFromCache()
        {
            // Arrange
            var branches = new List<BranchResponseDto>
            {
                new BranchResponseDto { Id = 1, Name = "Branch 1" },
                new BranchResponseDto { Id = 2, Name = "Branch 2" }
            };
            mockDistributedCache.Setup(x => x.GetString(It.IsAny<string>())).Returns(JsonSerializer.Serialize(branches));

            // Act
            var result = await branchService.GetBranchesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetBranchByCityIdAsync_ShouldReturnBranchByCityId()
        {
            // Arrange
            var branches = new List<BranchResponseDto>
            {
                new BranchResponseDto { Id = 1, Name = "Branch 1", CityId = 1 },
                new BranchResponseDto { Id = 2, Name = "Branch 2", CityId = 2 }
            };
            mockDistributedCache.Setup(x => x.GetString(It.IsAny<string>())).Returns((string)null);
            mockMapper.Setup(x => x.Map<List<BranchResponseDto>>(It.IsAny<List<MsBanking.Common.Entity.Branch>>())).Returns(branches);
            mockDistributedCache.Setup(x => x.SetString(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DistributedCacheEntryOptions>()));

            // Act
            var result = await branchService.GetBranchByCityIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.CityId);
        }

        [Fact]
        public async Task GetBranchByCityIdAsync_ShouldReturnBranchByCityIdFromCache()
        {
            // Arrange
            var branches = new List<BranchResponseDto>
            {
                new BranchResponseDto { Id = 1, Name = "Branch 1", CityId = 1 },
                new BranchResponseDto { Id = 2, Name = "Branch 2", CityId = 2 }
            };
            mockDistributedCache.Setup(x => x.GetString(It.IsAny<string>())).Returns(JsonSerializer.Serialize(branches));

            // Act
            var result = await branchService.GetBranchByCityIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.CityId);
        }

        [Fact]
        public async Task CreateBranchCityIdAsync_ShouldCreateObjectInDb()
        {
            //Arrange
            var branchDto = new BranchDto {  Name = "Branch 1", CityId = 1,Code =111,CountryId=1 };
            var branchEntity = new MsBanking.Common.Entity.Branch {  Name = "Branch 1", CityId = 1,Code =111,CountryId=1 };
            var branchResponseDto = new BranchResponseDto {  Name = "Branch 1", CityId = 1,Code =111,CountryId=1 };


            mockMapper.Setup(x => x.Map<MsBanking.Common.Entity.Branch>(It.IsAny<BranchDto>())).Returns(branchEntity);
            mockMapper.Setup(x => x.Map<BranchResponseDto>(It.IsAny<MsBanking.Common.Entity.Branch>())).Returns(branchResponseDto);
            mockDistributedCache.Setup(x => x.Remove(It.IsAny<string>())).Verifiable();


            //Act
            var result = await branchService.CreateBranchAsync(branchDto);
            //Assert
            Assert.NotNull(result);
            Assert.True(result.Name == "Branch 1");
            result.Should().BeEquivalentTo(branchResponseDto);
            result.Id.Should().BeGreaterThan(0);
        }
    }
}