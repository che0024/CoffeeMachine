
using Microsoft.AspNetCore.Http.HttpResults;
using static CoffeeMachine.Program;
using CoffeeMachine;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq.Protected;

namespace TestProject1;

public class WeatherServiceTest
{
    private readonly WeatherService service;
    private readonly Mock<IHttpClientFactory> mockHttpClientFactory = new();
    private readonly Mock<DelegatingHandler> mockHandler = new();

    public WeatherServiceTest()
    {
        service = new WeatherService(mockHttpClientFactory.Object);
    }

    [Fact]
    public void IsAboveThirty()
    {
        //Arrange
        var testDateTime = new DateTimeOffset(2025, 3, 1, 10, 0, 0, TimeSpan.Zero);
        var expected = @"{
    ""coord"": {
        ""lon"": -80.6081,
        ""lat"": 28.0836
    },
    ""weather"": [
        {
            ""id"": 800,
            ""main"": ""Clear"",
            ""description"": ""clear sky"",
            ""icon"": ""01n""
        }
    ],
    ""base"": ""stations"",
    ""main"": {
        ""temp"": 30.01,
        ""feels_like"": 20.69,
        ""temp_min"": 20.02,
        ""temp_max"": 21.99,
        ""pressure"": 1017,
        ""humidity"": 55,
        ""sea_level"": 1017,
        ""grnd_level"": 1017
    },
    ""visibility"": 10000,
    ""wind"": {
        ""speed"": 3.6,
        ""deg"": 60
    },
    ""clouds"": {
        ""all"": 0
    },
    ""dt"": 1744848540,
    ""sys"": {
        ""type"": 1,
        ""id"": 4922,
        ""country"": ""US"",
        ""sunrise"": 1744800954,
        ""sunset"": 1744847278
    },
    ""timezone"": -14400,
    ""id"": 4163971,
    ""name"": ""Melbourne"",
    ""cod"": 200
}";

        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage(){ StatusCode = System.Net.HttpStatusCode.OK, Content = new StringContent(expected) }).Verifiable();

        var client = new HttpClient(mockHandler.Object);
        
        mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);

        //Act
        var result = service.IsAboveThirty();

        //Assert
        var okResult = Assert.IsType<bool>(result);
        Assert.True(okResult);
    }

    [Fact]
    public void IsBelowThirty()
    {
        //Arrange
        var expected = @"{
    ""main"": {
        ""temp"": 29.99
    }
}";//we are only interested in the temperature so I'm ommitting everything else from the response to make it less bloated. The actual response is tested in the first test case anyway.

        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK, Content = new StringContent(expected) }).Verifiable();

        var client = new HttpClient(mockHandler.Object);

        mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);

        //Act
        var result = service.IsAboveThirty();

        //Assert
        var okResult = Assert.IsType<bool>(result);
        Assert.False(okResult);
    }

    [Fact]
    public void IsJustThirty()
    {
        //Arrange
        var expected = @"{
    ""main"": {
        ""temp"": 30.00
    }
}";//we are only interested in the temperature so I'm ommitting everything else from the response to make it less bloated. The actual response is tested in the first test case anyway.

        mockHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage() { StatusCode = System.Net.HttpStatusCode.OK, Content = new StringContent(expected) }).Verifiable();

        var client = new HttpClient(mockHandler.Object);

        mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(client);

        //Act
        var result = service.IsAboveThirty();

        //Assert
        var okResult = Assert.IsType<bool>(result);
        Assert.False(okResult);
    }
}