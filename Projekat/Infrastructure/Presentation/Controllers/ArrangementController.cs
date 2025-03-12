using Service.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Shared.RequestFeatures;
using Shared.Dtos;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [Route("api/arrangement")]
    [ApiController]
    public class ArrangementController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public ArrangementController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArrangementPreviewDto>>> GetUpcomingArrangements([FromQuery] ArrangementParameters arrangementParameters)
        {
            var (arrangementDtos, metaData) = await _serviceManager.ArrangementService.GetUpcomingArrangementsAsync(arrangementParameters);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));
            return Ok(arrangementDtos);
        }

        [HttpGet]
        [Route("past")]
        public async Task<ActionResult<IEnumerable<ArrangementPreviewDto>>> GetPastAndCurrentArrangements([FromQuery] ArrangementParameters arrangementParameters)
        {
            var (arrangementDtos, metaData) = await _serviceManager.ArrangementService.GetPastAndCurrentArrangementsAsync(arrangementParameters);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));
            return Ok(arrangementDtos);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ArrangementDto>> GetArrangementById([FromRoute] string id)
        {
            var arrangementDto = await _serviceManager.ArrangementService.GetArrangementByIdAsync(id);
            return Ok(arrangementDto);
        }
        [HttpGet]
        [Route("{id}/comments")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetArrangementComments([FromQuery] CommentParameters commentParameters, [FromRoute] string id)
        {
            var (commentDtos, metaData) = await _serviceManager.ArrangementService.GetApprovedArrangementCommentsAsync(commentParameters, id);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));
            return Ok(commentDtos);
        }
        [HttpGet]
        [Route("{id}/allComments")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetArrangementAllComments([FromQuery] CommentParameters commentParameters, [FromRoute] string id)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var (commentDtos, metaData) = await _serviceManager.ArrangementService.GetAllArrangementCommentsAsync(commentParameters, id, userId!);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));
            return Ok(commentDtos);
        }
        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<ArrangementDto>> CreateArrangement([FromBody] ArrangementCreationDto dto)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var arrangementDto = await _serviceManager.ArrangementService.CreateArrangementAsync(dto, userId!);
            return Ok(arrangementDto);
        }
        [HttpPut]
        [Route("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<ArrangementDto>> UpdateArrangementById([FromBody] ArrangementCreationDto dto, [FromRoute] string id)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var arrangementDto = await _serviceManager.ArrangementService.UpdateArrangementAsync(dto, id, userId!);
            return Ok(arrangementDto);
        }
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteArrangementById([FromRoute] string id)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            await _serviceManager.ArrangementService.DeleteArrangementAsync(id, userId!);
            return NoContent();
        }
    }
}
