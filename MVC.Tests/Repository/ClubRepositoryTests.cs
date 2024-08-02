using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MVC.Data;
using MVC.Data.Enums;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Tests.Repository;

public class ClubRepositoryTests
{
    private async Task<AppDbContext> GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        
        var dbContext = new AppDbContext(options);
        await dbContext.Database.EnsureCreatedAsync();

        if (await dbContext.Clubs.AnyAsync())
        {
            return dbContext;
        }
        
        for (var i = 0; i <= 10; i++)
        {
            await dbContext.Clubs.AddAsync(new Club
            {
                Title = $"Club {i}",
                Description = $"Description {i}",
                Image = $"Image {i}",
                ClubCategory = ClubCategory.City,
                Address = new Address
                {
                    Street = "Street 1",
                    City = "City 1",
                    State = "State 1",
                }
            });
            await dbContext.SaveChangesAsync();
        }
        return dbContext;
    }

    [Fact]
    public async void ClubRepository_Add_ReturnsBool()
    {
        //Arrange
        var club = new Club
        {
            Title = "Club 1",
            Description = "Description 1",
            Image = "Image 1",
            ClubCategory = ClubCategory.City,
            Address = new Address
            {
                Street = "Street 1",
                City = "City 1",
                State = "State 1",
            }
        };
        
        var dbcontext = await GetDbContext();
        var clubRepository = new ClubRepository(dbcontext);
        
        //Act
        var result = await clubRepository.AddAsync(club);
        
        //Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async void ClubRepository_GetByIdAsync_ReturnsClub()
    {
        //Arrange
        var id = 1;
        var dbcontext = await GetDbContext();
        var clubRepository = new ClubRepository(dbcontext);
        
        //Act
        var result = clubRepository.GetByIdAsync(id);
        
        //Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Task<Club>>();
    }
    
    [Fact]
    public async void ClubRepository_GetAll_ReturnsIEnumerableClub()
    {
        //Arrange
        var dbcontext = await GetDbContext();
        var clubRepository = new ClubRepository(dbcontext);
        
        //Act
        var result = await clubRepository.GetAll();
        
        //Assert
        var clubs = result.ToList();
        clubs.Should().NotBeNull();
        clubs.Should().BeOfType<List<Club>>();
        clubs.Count.Should().Be(11);
    }
}