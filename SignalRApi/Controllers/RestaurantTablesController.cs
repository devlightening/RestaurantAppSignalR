﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;
using SignalR.DtoLayer.RestaurantTableDto;
using SignalR.EntityLayer.Entities;

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
        public IActionResult RestaurantTableList()
        {
            var values = _mapper.Map<List<ResultRestaurantTableDto>>(_restaurantTableService.TGetListAll());
            return Ok(values);
        }

        [HttpPost]
        public IActionResult CreateRestaurantTable(CreateRestaurantTableDto createRestaurantTableDto)
        {
            // Enum.IsDefined ile kontrol yapabilirsin ama bu genelde gerekmez, model binding başarısızsa zaten hata döner.
            if (!Enum.IsDefined(typeof(TableLocation), createRestaurantTableDto.Location))
            {
                return BadRequest("Geçersiz veya eksik masa konumu bilgisi.");
            }

            var newTable = new RestaurantTable()
            {
                TableNo = createRestaurantTableDto.TableNo,
                Status = createRestaurantTableDto.Status,
                Location = createRestaurantTableDto.Location
            };

            _restaurantTableService.TAdd(newTable);

            return Ok("Masa Başarıyla Eklendi");
        }



        [HttpDelete("{id}")]
        public IActionResult DeleteRestaurantTable(int id)
        {
            var table = _restaurantTableService.TGetById(id);
            if (table == null)
                return NotFound("Masa bulunamadı.");

            _restaurantTableService.TDelete(table);
            return Ok("Masa başarıyla silindi.");
        }

        [HttpPut]
        public IActionResult UpdateRestaurantTable(UpdateRestaurantTableDto updateRestaurantTableDto)
        {
            _restaurantTableService.TUpdate(new RestaurantTable()
            {
                RestaurantTableId = updateRestaurantTableDto.RestaurantTableId,
                TableNo = updateRestaurantTableDto.TableNo,
                Status = updateRestaurantTableDto.Status
            });
            return Ok("Masa Başarıyla Güncellendi");
        }


        [HttpGet("TotalTableCount")]
        public IActionResult TotalTableCount()
        {
            var count = _restaurantTableService.TTotalTableCount();
            return Ok(count);
        }

        [HttpGet("AvailableTableCount")]
        public IActionResult AvailableTableCount()
        {
            var count = _restaurantTableService.TAvailableTableCount();
            return Ok(count);
        }

        [HttpGet("NotAvailableTableCount")]
        public IActionResult NotAvailableTableCount()
        {
            var count = _restaurantTableService.TNotAvailableTableCount();
            return Ok(count);
        }

        [HttpGet("GetByTableNo")]
        public IActionResult GetByTableNo(int tableNo)
        {
            var table = _restaurantTableService.TGetByTableNo(tableNo);
            if (table == null) return NotFound();
            return Ok(table);
        }

        [HttpGet("AvailableTables")]
        public IActionResult AvailableTables()
        {
            var tables = _restaurantTableService.TGetAvailableTables();
            return Ok(tables);
        }

        [HttpGet("NotAvailableTables")]
        public IActionResult NotAvailableTables()
        {
            var tables = _restaurantTableService.TGetNotAvailableTables();
            return Ok(tables);
        }

        [HttpGet("TablesByStatus")]
        public IActionResult TablesByStatus(bool status)
        {
            var tables = _restaurantTableService.TGetTablesByStatus(status);
            return Ok(tables);
        }

        [HttpGet("{id}")]
        public IActionResult GetRestaurantTable(int id)
        {
            var table = _restaurantTableService.TGetById(id);
            if (table == null) return NotFound("Masa bulunamadı.");
            var result = _mapper.Map<ResultRestaurantTableDto>(table);
            return Ok(result);
        }

        [HttpGet("TablesByLocation")]
        public IActionResult TablesByLocation(TableLocation location)
        {
            var tables = _restaurantTableService.TGetTablesByLocation(location);
            if (tables == null || tables.Count == 0) return NotFound("Konuma göre masa bulunamadı.");
            return Ok(tables);
        }
    }
}
