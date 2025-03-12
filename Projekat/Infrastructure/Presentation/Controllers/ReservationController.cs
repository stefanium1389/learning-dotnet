using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared.Dtos;
using Shared.RequestFeatures;

namespace Presentation.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public ReservationController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpPost]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationCreationDto dto)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ReservationDto reservationDto = await _serviceManager.ReservationService.CreateReservationAsync(dto, userId!);
            return Ok(reservationDto);
        }
        [HttpPatch]
        [Authorize(Roles = "Tourist")]
        [Route("{id}/cancel")]
        public async Task<IActionResult> CancelReservation([FromRoute] string id)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            ReservationDto reservationDto = await _serviceManager.ReservationService.CancelReservationAsync(id, userId!);
            return Ok(reservationDto);
        }
        [HttpGet]
        [Authorize(Roles = "Tourist")]
        public async Task<IActionResult> GetOwnReservations([FromQuery] ReservationParameters reservationParameters)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (reservationDtos, metaData) = await _serviceManager.ReservationService.GetReservationsForUserId(userId!, reservationParameters);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));
            return Ok(reservationDtos);
        }
        [HttpGet]
        [Authorize(Roles = "Tourist,Manager")]
        [Route("{id}")]
        public async Task<IActionResult> GetOwnReservations([FromRoute] string id)
        {
            ReservationDto reservationDto = await _serviceManager.ReservationService.GetReservationById(id);
            return Ok(reservationDto);
        }
        [HttpGet]
        [Authorize(Roles = "Manager")]
        [Route("manager/arrangements")]
        public async Task<IActionResult> GetReservationsForOwnArrangements([FromQuery] ReservationParameters reservationParameters)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (reservationDtos, metaData) = await _serviceManager.ReservationService.GetReservationsForManagersArrangementsByUserId(userId!, reservationParameters);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));
            return Ok(reservationDtos);
        }
    }
}
