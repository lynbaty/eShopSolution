using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModels.System.Languages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.System.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly eShopDbContext _context;

        public LanguageService(eShopDbContext context)
        {
            _context = context;
        }

        public async Task<List<LanguageDto>> GetAll()
        {
            var lang = await _context.Languages.Select(x => new LanguageDto()
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            return lang;
        }
    }
}