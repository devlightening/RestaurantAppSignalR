using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.FeatureDto;
using SignalR.EntityLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeaturesController : ControllerBase
    {
        private readonly IFeatureService _featureService;
        private readonly IMapper _mapper;
        public FeaturesController(IFeatureService featureService, IMapper mapper)
        {
            _featureService = featureService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> FeatureList()
        {
            var values = _mapper.Map<List<ResultFeatureDto>>(await _featureService.TGetListAllAsync());
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeature(CreateFeatureDto createFeatureDto)
        {
            if (createFeatureDto == null) return BadRequest("Özellik verisi boş olamaz.");

            var feature = _mapper.Map<Feature>(createFeatureDto);
            await _featureService.TAddAsync(feature);

            return CreatedAtAction(nameof(GetFeature), new { id = feature.FeatureId }, feature);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFeature(int id)
        {
            var value = await _featureService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile özellik bulunamadı.");
            }
            await _featureService.TDeleteAsync(value);
            return Ok("Özellik Başarıyla Silindi");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFeature(UpdateFeatureDto updateFeatureDto)
        {
            if (updateFeatureDto == null) return BadRequest("Güncellenecek özellik verisi boş olamaz.");

            var existingFeature = await _featureService.TGetByIdAsync(updateFeatureDto.FeatureId);
            if (existingFeature == null)
            {
                return NotFound($"ID {updateFeatureDto.FeatureId} ile özellik bulunamadı.");
            }

            _mapper.Map(updateFeatureDto, existingFeature);
            await _featureService.TUpdateAsync(existingFeature);

            return Ok("Özellik Başarıyla Güncellendi");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFeature(int id)
        {
            var value = await _featureService.TGetByIdAsync(id);
            if (value == null)
            {
                return NotFound($"ID {id} ile özellik bulunamadı.");
            }
            var result = _mapper.Map<ResultFeatureDto>(value);
            return Ok(result);
        }
    }
}
