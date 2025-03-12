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
    [Route("api/accomodation")]
    [ApiController]
    public class AccomodationController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public AccomodationController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAccomodation([FromRoute] string id)
        {
            var accomodationDto = await _serviceManager.AccomodationService.GetAccomodationByIdAsync(id);
            return Ok(accomodationDto);
        }
        [HttpGet]
        [Route("{id}/units")]
        public async Task<IActionResult> GetAccomodationUnits([FromQuery] AccomodationUnitParameters accomodationUnitParameters, [FromRoute] string id)
        {
            var (accomodationUnitDtos, metaData) = await _serviceManager.AccomodationService.GetAccomodationUnitsForIdAsync(id, accomodationUnitParameters);
            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(metaData));
            return Ok(accomodationUnitDtos);
        }
        [HttpPost]
        [Route("{id}/units")]
        public async Task<IActionResult> CreateAccomodationUnit([FromRoute] string id, [FromBody] AccomodationUnitCreationDTO dto)
        {
            AccomodationUnitDto accomodationUnitDto = await _serviceManager.AccomodationService.CreateAccomodationUnitAsync(id, dto);
            return Ok(accomodationUnitDto);
        }
        [HttpDelete]
        [Route("units/{unitId}")]
        public async Task<IActionResult> DeleteAccomodationUnit([FromRoute] string unitId)
        {
            await _serviceManager.AccomodationService.DeleteAccomodationUnitAsync(unitId);
            return NoContent();
        }
        [HttpPut]
        [Route("units/{unitId}")]
        public async Task<IActionResult> UpdateAccomodationUnit([FromRoute] string unitId, [FromBody] AccomodationUnitCreationDTO dto)
        {
            AccomodationUnitDto accomodationUnitDto = await _serviceManager.AccomodationService.UpdateAccomodationUnitAsync(unitId, dto);
            return Ok(accomodationUnitDto);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAccomodation([FromBody] AccomodationCreationDto dto)
        {
            AccomodationDto newAccomodation = await _serviceManager.AccomodationService.CreateAccomodationAsync(dto);
            return Ok(newAccomodation);
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateAccomodation([FromBody] AccomodationCreationDto dto, [FromRoute] string id)
        {
            AccomodationDto newAccomodation = await _serviceManager.AccomodationService.UpdateAccomodationAsync(dto, id);
            return Ok(newAccomodation);
        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAccomodation([FromRoute] string id)
        {
            await _serviceManager.AccomodationService.DeleteAccomodationAsync(id);
            return NoContent();
        }
    }
}
