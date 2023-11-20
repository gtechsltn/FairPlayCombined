﻿using FairPlayCombined.DataAccess.Data;
using FairPlayCombined.DataAccess.Models.FairPlayDatingSchema;
using FairPlayCombined.Models.FairPlayDating.KidStatus;
using FairPlayCombined.Models.Pagination;
using FairPlayCombined.Services.FairPlayDating;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;

namespace FairPlayCombined.AutomatedTests.ServicesTests.FairPlayDating
{
    [TestClass]
    public class KidStatusServiceTests
    {
        public static readonly MsSqlContainer _msSqlContainer
        = new MsSqlBuilder().Build();
        [ClassInitialize]
#pragma warning disable IDE0060 // Remove unused parameter
        public static async Task ClassInitializeAsync(TestContext testContext)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            await _msSqlContainer.StartAsync();
        }

        [ClassCleanup()]
        public static async Task ClassCleanupAsync()
        {
            if (_msSqlContainer.State == DotNet.Testcontainers.Containers.TestcontainersStates.Running)
            {
                await _msSqlContainer.StopAsync();
            }
        }

        [TestCleanup]
        public async Task TestCleanupAsync()
        {
            ServiceCollection services = new ServiceCollection();
            var cs = _msSqlContainer.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs);
                });
            services.AddTransient<KidStatusService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            foreach (var singleKidStatus in dbContext.KidStatus)
            {
                dbContext.KidStatus.Remove(singleKidStatus);
            }
            await dbContext.SaveChangesAsync();
        }

        [TestMethod]
        public async Task Test_CreateKidStatusAsync()
        {
            ServiceCollection services = new ServiceCollection();
            var cs = _msSqlContainer.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs);
                });
            services.AddTransient<KidStatusService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var KidStatusService = sp.GetRequiredService<KidStatusService>();
            CreateKidStatusModel createKidStatusModel = new CreateKidStatusModel()
            {
                Name = "TestModel"
            };
            await KidStatusService.CreateKidStatusAsync(createKidStatusModel, CancellationToken.None);
            var result = await dbContext.KidStatus.SingleOrDefaultAsync();
            Assert.IsNotNull(result);
            Assert.AreEqual(createKidStatusModel.Name, result.Name);
        }

        [TestMethod]
        public async Task Test_DeleteKidStatusAsync()
        {
            ServiceCollection services = new ServiceCollection();
            var cs = _msSqlContainer.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs);
                });
            services.AddTransient<KidStatusService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var KidStatusService = sp.GetRequiredService<KidStatusService>();
            KidStatus entity = new KidStatus()
            {
                Name = "TestModel"
            };
            await dbContext.KidStatus.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.KidStatusId);
            await KidStatusService.DeleteKidStatusByIdAsync(entity.KidStatusId, CancellationToken.None);
            var itemsCount = await dbContext.KidStatus.CountAsync(CancellationToken.None);
            Assert.AreEqual(0, itemsCount);
        }

        [TestMethod]
        public async Task Test_GetPaginatedKidStatusAsync()
        {
            ServiceCollection services = new ServiceCollection();
            var cs = _msSqlContainer.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs);
                });
            services.AddTransient<KidStatusService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var KidStatusService = sp.GetRequiredService<KidStatusService>();
            KidStatus entity = new KidStatus()
            {
                Name = "TestModel"
            };
            await dbContext.KidStatus.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.KidStatusId);
            var result = await KidStatusService.GetPaginatedKidStatusAsync(
                paginationRequest: new Models.Pagination.PaginationRequest()
                {
                    PageSize = 10,
                    StartIndex = 0,
                    SortingItems = new SortingItem[]
                    {
                        new SortingItem()
                        {
                            PropertyName = nameof(KidStatusModel.Name),
                            SortType = Common.GeneratorsAttributes.SortType.Descending
                        }
                    }
                }, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Items![0].KidStatusId, entity.KidStatusId);
        }

        [TestMethod]
        public async Task Test_GetKidStatusByIdAsync()
        {
            ServiceCollection services = new ServiceCollection();
            var cs = _msSqlContainer.GetConnectionString();
            services.AddDbContextFactory<FairPlayCombinedDbContext>(
                optionsAction =>
                {
                    optionsAction.UseSqlServer(cs);
                });
            services.AddTransient<KidStatusService>();
            var sp = services.BuildServiceProvider();
            var dbContext = sp.GetRequiredService<FairPlayCombinedDbContext>();
            await dbContext.Database.EnsureCreatedAsync();
            var KidStatusService = sp.GetRequiredService<KidStatusService>();
            KidStatus entity = new KidStatus()
            {
                Name = "TestModel"
            };
            await dbContext.KidStatus.AddAsync(entity, CancellationToken.None);
            await dbContext.SaveChangesAsync();
            Assert.AreNotEqual(0, entity.KidStatusId);
            var result = await KidStatusService.GetKidStatusByIdAsync(entity.KidStatusId, CancellationToken.None);
            Assert.IsNotNull(result);
            Assert.AreEqual(entity.Name, result.Name);
        }
    }
}