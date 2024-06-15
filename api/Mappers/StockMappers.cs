using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Stock;
using api.Models;
using System.Linq.Expressions;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockDTO TOStockDto (this Stock stockModel)
        {
            return new StockDTO
            {
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                Company = stockModel.Company,
                Purchase = stockModel.Purchase,
                LastDiv = stockModel.LastDiv,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Comments.Select(c=> c.ToCommentDto()).ToList()
            };
        }

        public static Expression<Func<Stock, StockDTO>> ToStockDtoExpression()
        {
            return stock => new StockDTO
            {
                Id = stock.Id,
                Symbol = stock.Symbol,
                Company = stock.Company,
                Purchase = stock.Purchase,
                LastDiv = stock.LastDiv,
                Industry = stock.Industry,
                MarketCap = stock.MarketCap,
                Comments = stock.Comments.Select(c=> c.ToCommentDto()).ToList()
            };
        }
        

        public static Stock ToStockCreateDto(this StockRequestDto stockRequest)
        {
            return new Stock
            {
                
                Symbol = stockRequest.Symbol,
                Company = stockRequest.Company,
                Purchase = stockRequest.Purchase,
                LastDiv = stockRequest.LastDiv,
                Industry = stockRequest.Industry,
                MarketCap = stockRequest.MarketCap
            };
        }

        public static Stock ToStockUpdateDto(this UpdateStockRequest updateStock)
        {
            return new Stock
            {
                
                Symbol = updateStock.Symbol,
                Company = updateStock.Company,
                Purchase = updateStock.Purchase,
                LastDiv = updateStock.LastDiv,
                Industry = updateStock.Industry,
                MarketCap = updateStock.MarketCap
            };
        }
        
    }
}