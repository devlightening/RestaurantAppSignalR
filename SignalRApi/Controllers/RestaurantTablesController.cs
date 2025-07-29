using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.RestaurantTableDto;
using SignalR.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantTablesController : ControllerBase
    {
        private readonly IRestaurantTableService _restaurantTableService;
        private readonly IMapper _mapper;

        public RestaurantTablesController(IRestaurantTableService restaurantTableService, IMapper mapper)
        {
            _restaurantTableService = restaurantTableService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> RestaurantTableList()
        {
            var values = _mapper.Map<List<ResultRestaurantTableDto>>(await _restaurantTableService.TGetListAllAsync());
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRestaurantTable(CreateRestaurantTableDto createRestaurantTableDto)
        {
            if (!Enum.IsDefined(typeof(TableLocation), createRestaurantTableDto.Location))
            {
                return BadRequest("Geçersiz veya eksik masa konumu bilgisi.");
            }
            if (createRestaurantTableDto == null) return BadRequest("Masa verisi boş olamaz.");

            var newTable = _mapper.Map<RestaurantTable>(createRestaurantTableDto);
            await _restaurantTableService.TAddAsync(newTable);

            return CreatedAtAction(nameof(GetRestaurantTable), new { id = newTable.RestaurantTableId }, newTable);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurantTable(int id)
        {
            var table = await _restaurantTableService.TGetByIdAsync(id);
            if (table == null)
                return NotFound("Masa bulunamadı.");

            await _restaurantTableService.TDeleteAsync(table);
            return Ok("Masa başarıyla silindi.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRestaurantTable(UpdateRestaurantTableDto updateRestaurantTableDto)
        {
            if (updateRestaurantTableDto == null) return BadRequest("Güncellenecek masa verisi boş olamaz.");

            var existingTable = await _restaurantTableService.TGetByIdAsync(updateRestaurantTableDto.RestaurantTableId);
            if (existingTable == null)
            {
                return NotFound($"ID {updateRestaurantTableDto.RestaurantTableId} ile masa bulunamadı.");
            }

            _mapper.Map(updateRestaurantTableDto, existingTable);
            await _restaurantTableService.TUpdateAsync(existingTable);
            return Ok("Masa Başarıyla Güncellendi");
        }

        [HttpGet("TotalTableCount")]
        public async Task<IActionResult> TotalTableCount()
        {
            var count = await _restaurantTableService.TTotalTableCount();
            return Ok(count);
        }

        [HttpGet("AvailableTableCount")]
        public async Task<IActionResult> AvailableTableCount()
        {
            var count = await _restaurantTableService.TAvailableTableCount();
            return Ok(count);
        }

        [HttpGet("NotAvailableTableCount")]
        public async Task<IActionResult> NotAvailableTableCount()
        {
            var count = await _restaurantTableService.TNotAvailableTableCount();
            return Ok(count);
        }

        [HttpGet("GetByTableNo")]
        public async Task<IActionResult> GetByTableNo(int tableNo)
        {
            var table = await _restaurantTableService.TGetByTableNo(tableNo);
            if (table == null) return NotFound();
            return Ok(table);
        }

        [HttpGet("AvailableTables")]
        public async Task<IActionResult> AvailableTables()
        {
            var tables = await _restaurantTableService.TGetAvailableTables();
            return Ok(tables);
        }

        [HttpGet("NotAvailableTables")]
        public async Task<IActionResult> NotAvailableTables()
        {
            var tables = await _restaurantTableService.TGetNotAvailableTables();
            return Ok(tables);
        }

        [HttpGet("TablesByStatus")]
        public async Task<IActionResult> TablesByStatus(bool status)
        {
            var tables = await _restaurantTableService.TGetTablesByStatus(status);
            return Ok(tables);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRestaurantTable(int id)
        {
            var table = await _restaurantTableService.TGetByIdAsync(id);
            if (table == null) return NotFound("Masa bulunamadı.");
            var result = _mapper.Map<ResultRestaurantTableDto>(table);
            return Ok(result);
        }

        [HttpGet("TablesByLocation")]
        public async Task<IActionResult> TablesByLocation(TableLocation location)
        {
            var tables = await _restaurantTableService.TGetTablesByLocation(location);
            if (tables == null || tables.Count == 0) return NotFound("Konuma göre masa bulunamadı.");
            return Ok(tables);
        }
    }
}
