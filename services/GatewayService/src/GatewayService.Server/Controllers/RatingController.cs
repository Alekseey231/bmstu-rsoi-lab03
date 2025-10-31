using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Sockets;
using GatewayService.Clients;
using GatewayService.Dto.Http;
using GatewayService.Dto.Http.Converters;
using GatewayService.Services.CircuitBreaker.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Refit;
using Swashbuckle.AspNetCore.Annotations;

namespace GatewayService.Server.Controllers;

[ApiController]
[Route("/api/v1/rating")]
public class RatingController : ControllerBase
{
    private readonly IRatingServiceClient _ratingServiceRequestClient;
    private readonly ILogger<RatingController> _logger;

    public RatingController(IRatingServiceClient ratingServiceRequestClient, ILogger<RatingController> logger)
    {
        _ratingServiceRequestClient = ratingServiceRequestClient;
        _logger = logger;
    }

    [HttpGet]
    [SwaggerOperation("Получить рейтинг пользователя", "Получить рейтинг пользователя")]
    [SwaggerResponse(statusCode: 200, type: typeof(UserRatingResponse), description: "Рейтинг пользователя")]
    [SwaggerResponse(statusCode: 500, type: typeof(ErrorResponse), description: "Ошибка на стороне сервера")]
    public async Task<IActionResult> GetUserRating([Required][FromHeader(Name = "X-User-Name")] string userName)
    {
        try
        {
            var rating = await _ratingServiceRequestClient.GetRatingAsync(userName);

            var dtoRating = RatingConverter.Convert(rating);

            return Ok(dtoRating);
        }
        catch (BrokenCircuitException e)
        {
            _logger.LogError(e, "Bonus Service unavailable");

            return StatusCode(503, new ErrorResponse("Bonus Service unavailable"));
        }
        catch (ApiException e) when (e.StatusCode == HttpStatusCode.ServiceUnavailable)
        {
            _logger.LogError(e, "Bonus Service unavailable");

            return StatusCode(503, new ErrorResponse("Bonus Service unavailable"));
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Bonus Service unavailable");
            
            return StatusCode(503, new ErrorResponse("Bonus Service unavailable"));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in method {Method}", nameof(GetUserRating));
            return StatusCode(500, new ErrorResponse("Неожиданная ошибка на стороне сервера."));
        }
    }
}