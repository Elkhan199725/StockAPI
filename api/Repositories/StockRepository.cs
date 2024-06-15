using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stocks.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks.Include(c=>c.Comments).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(c=> c.Comments).FirstOrDefaultAsync(c=> c.Id==id);
        }

        public async Task<Stock?> RemoveAsync(int id)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(s=> s.Id == id);
            if (stockModel == null) 
            {
                return null;
            }

            _context.Stocks.Remove(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<bool> StockExist(int id)
        {
            return await _context.Stocks.AnyAsync(s=>s.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequest stockRequest)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(s=> s.Id == id);

            if(existingStock == null){ return null; }

            existingStock.Symbol = stockRequest.Symbol;
            existingStock.Company = stockRequest.Company;
            existingStock.Purchase = stockRequest.Purchase;
            existingStock.Industry = stockRequest.Industry;
            existingStock.LastDiv = stockRequest.LastDiv;
            existingStock.MarketCap = stockRequest.MarketCap;
            
            await _context.SaveChangesAsync();
            return existingStock;
        }
    }
}