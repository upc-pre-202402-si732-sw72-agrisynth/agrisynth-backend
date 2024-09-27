using agrisynth_backend.IAM.Application.Internal.OutboundServices;
using agrisynth_backend.IAM.Infrastructure.Pipeline.Middleware.Attributes;

namespace agrisynth_tests.IAM;

using System.Threading.Tasks;
using agrisynth_backend.IAM.Infrastructure.Pipeline.Middleware.Components;
using agrisynth_backend.IAM.Domain.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

public class RequestAuthorizationMiddlewareTests
{
    private readonly Mock<RequestDelegate> _nextMock;
    private readonly Mock<IUserQueryService> _userQueryServiceMock;
    private readonly Mock<ITokenService> _tokenServiceMock;
    private readonly RequestAuthorizationMiddleware _middleware;

    public RequestAuthorizationMiddlewareTests()
    {
        _nextMock = new Mock<RequestDelegate>();
        _userQueryServiceMock = new Mock<IUserQueryService>();
        _tokenServiceMock = new Mock<ITokenService>();
        _middleware = new RequestAuthorizationMiddleware(_nextMock.Object);
    }

    [Fact]
    public async Task InvokeAsync_ShouldSkipAuthorization_WhenAllowAnonymousAttributePresent()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers["Authorization"] = "Bearer token";
        var endpoint = new Endpoint(null, new EndpointMetadataCollection(new AllowAnonymousAttribute()), "test");
        context.Request.HttpContext.SetEndpoint(endpoint);

        await _middleware.InvokeAsync(context, _userQueryServiceMock.Object, _tokenServiceMock.Object);

        _nextMock.Verify(next => next(context), Times.Once);
    }
}