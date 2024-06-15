using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace api.Controller
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StockController : ControllerBase
    {

        private readonly IStockRepository _stockRepository;
        public StockController(IStockRepository stockRepository)
        {
           _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var stocks = await _stockRepository.GetAllAsync();
            
            var stockDtos = stocks.Select(s=> s.TOStockDto()).ToList();

            return Ok(stockDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var stock = await _stockRepository.GetByIdAsync(id);
            if(stock == null)
            {
                return NotFound();
            }
            return Ok(stock.TOStockDto());
        }
        
            [HttpPost]
            public async Task<IActionResult> Create([FromBody] StockRequestDto stockRequest)
            {
                if (stockRequest == null)
                {
                    return BadRequest("Stock request cannot be null.");
                }

                var stockModel = stockRequest.ToStockCreateDto();
                await _stockRepository.CreateAsync(stockModel);
                return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.TOStockDto());
            }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequest updateStock)
        {
            if (updateStock == null)
            {
                return BadRequest("Update stock request cannot be null.");
            }

            var stockModel = await _stockRepository.UpdateAsync(id, updateStock);

            if (stockModel == null)
            {
                return NotFound();
            }

            return Ok(stockModel.TOStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Remove([FromRoute] int id)
        {
            var stockModel = await _stockRepository.RemoveAsync(id);
            if(stockModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}   