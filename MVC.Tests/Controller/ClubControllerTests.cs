using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Controllers;
using MVC.Interfaces;
using MVC.Models;

namespace MVC.Tests.Controller;

public class ClubControllerTests
{
    private readonly IClubRepository _clubRepository;
    private readonly IPhotoService _photoService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ClubController _clubController;
    
    public ClubControllerTests()
    {
        //Dependency Injection
        _clubRepository = A.Fake<IClubRepository>();
        _photoService = A.Fake<IPhotoService>();
        _httpContextAccessor = A.Fake<IHttpContextAccessor>();
        
        //SUT
        _clubController = new ClubController(_clubRepository, _photoService, _httpContextAccessor);
    }

    [Fact]
    public void ClubController_Index_ReturnSuccess()
    {
        //Arrange
        IEnumerable<Club> clubs = A.Fake<IEnumerable<Club>>();
        A.CallTo(() => _clubRepository.GetAll()).Returns(clubs);

        //Act
        var result = _clubController.Index();
        
        //Assert
        result.Should().BeOfType<Task<IActionResult>>();
    }

    [Fact]
    public void ClubController_Details_ReturnSuccess()
    {
        //Arrange
        var id = 1;
        Club club = A.Fake<Club>();
        A.CallTo(() => _clubRepository.GetByIdAsync(id)).Returns(club);
        
        //Act
        var result = _clubController.Details(id);
        
        //Assert
        result.Should().BeOfType<Task<IActionResult>>();
    }
}