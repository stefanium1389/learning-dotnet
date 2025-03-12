using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Abstractions;
using Shared.Dtos;

namespace Presentation.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public CommentController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost]
        [Authorize(Roles = "Tourist")]
        public async Task<ActionResult<CommentDto>> PostCommentAsync([FromBody] CommentCreationDto dto)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            CommentDto commentDto = await _serviceManager.CommentService.CreateCommentAsync(dto, userId!);
            return Ok(commentDto);
        }
        [HttpPatch]
        [Route("{id}/approve")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<CommentDto>> ApproveComment([FromRoute] string id)
        {
            CommentDto commentDto = await _serviceManager.CommentService.ApproveCommentAsync(id);
            return Ok(commentDto);
        }
        [HttpPatch]
        [Route("{id}/decline")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult<CommentDto>> DeclineComment([FromRoute] string id)
        {
            CommentDto commentDto = await _serviceManager.CommentService.DeclineCommentAsync(id);
            return Ok(commentDto);
        }
    }
}
