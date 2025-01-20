using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using PositionsAPI.Controllers;


public class PositionsControllerTests{
    private readonly Mock<PositionService> _positionServiceMock;
    private readonly PositionsController _controller;

    public PositionsControllerTests() {
        _positionServiceMock = new Mock<PositionService>();
        _controller = new PositionsController(_positionServiceMock.Object, new Mock<ILogger<PositionsController>>().Object);
    }

    [Fact]
    public void GetDistances_ValidPosition_ReturnsCalculatedDistances(){
        
        var positionName = "Position1";
        _positionServiceMock.Setup(service => service.CalculateDistances(positionName))
                            .Returns(new List<object>
                            {
                                new { name = "Position2", distance_km = 3936.0 }
                            });
        
        var result = _controller.GetDistances(positionName) as OkObjectResult;
        var distances = result?.Value as List<object>;
        
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);

        dynamic firstDistance = distances?.First();

        Assert.Equal("Position2", firstDistance.name);
        Assert.Equal(3936.0, firstDistance.distance_km);
    }
    [Fact]
    public void GetDistances_PositionNotFound_ReturnsNotFound() {
        var positionName = "NonExistentPosition";

        _positionServiceMock.Setup(service => service.CalculateDistances(positionName))
                            .Throws(new KeyNotFoundException("Position not found"));

        var result = _controller.GetDistances(positionName) as NotFoundObjectResult;

        Assert.NotNull(result);
        Assert.Equal(404, result.StatusCode);
        Assert.Equal("Position not found", (result.Value as dynamic).message);
    }
   
}
