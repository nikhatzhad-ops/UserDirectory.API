using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserDirectory.API.Controllers;
using UserDirectory.API.Data;
using UserDirectory.API.DTOs;
using UserDirectory.API.Models;
using Xunit;

namespace UserDirectory.API.Tests
{
    public class UsersControllerTests
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetAll_ReturnsEmptyList_WhenNoUsers()
        {
            using var db = CreateContext();
            var controller = new UsersController(db);

            var result = await controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var users = Assert.IsAssignableFrom<IEnumerable<User>>(okResult.Value);
            Assert.Empty(users);
        }

        [Fact]
        public async Task Create_AddsUser()
        {
            using var db = CreateContext();
            var controller = new UsersController(db);

            var dto = new CreateUserDto { Name = "Alice", Email = "alice@example.com", Phone = "123" };
            var result = await controller.Create(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            var user = Assert.IsType<User>(created.Value);
            Assert.Equal("Alice", user.Name);
            Assert.Equal(1, db.Users.Count());
        }

        [Fact]
        public async Task Get_ReturnsUser_WhenExists()
        {
            using var db = CreateContext();
            var user = new User { Name = "Bob", Email = "bob@example.com" };
            db.Users.Add(user);
            await db.SaveChangesAsync();

            var controller = new UsersController(db);
            var result = await controller.Get(user.Id);

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<User>(okResult.Value);
            Assert.Equal(user.Id, returned.Id);
            Assert.Equal("Bob", returned.Name);
        }

        [Fact]
        public async Task Update_UpdatesUser_WhenExists()
        {
            using var db = CreateContext();
            var user = new User { Name = "Old", Email = "old@example.com" };
            db.Users.Add(user);
            await db.SaveChangesAsync();

            var controller = new UsersController(db);
            var dto = new UpdateUserDto { Name = "New", Email = "new@example.com", Phone = "555" };

            var result = await controller.Update(user.Id, dto);

            Assert.IsType<NoContentResult>(result);
            var updated = db.Users.Find(user.Id);
            Assert.Equal("New", updated.Name);
            Assert.Equal("new@example.com", updated.Email);
        }

        [Fact]
        public async Task Delete_RemovesUser_WhenExists()
        {
            using var db = CreateContext();
            var user = new User { Name = "ToDelete", Email = "del@example.com" };
            db.Users.Add(user);
            await db.SaveChangesAsync();

            var controller = new UsersController(db);
            var result = await controller.Delete(user.Id);

            Assert.IsType<NoContentResult>(result);
            Assert.Empty(db.Users.ToList());
        }
    }
}
