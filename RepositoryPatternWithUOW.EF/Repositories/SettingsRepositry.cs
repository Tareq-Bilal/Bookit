using Microsoft.VisualBasic;
using RepositoryPatternWithUOW.Core.Models;
using RepositoryPatternWithUOW.Core.Repositories;
using RepositoryPatternWithUOW.Core.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RepositoryPatternWithUOW.EF.Repositories
{
    public class SettingsRepositry : BaseRepository<Settings>, ISettingsRepository
    {
        private readonly ApplicationDbContext _context;

        public SettingsRepositry(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public decimal GetDamagedReturnCopyFee()
        {
            throw new NotImplementedException();
        }

        public decimal GetLateReturnFee()
        {
            return _context.Settings
                .Where(s => s.Key == BookReturnCondition.enSetting.LateReturnFeePerDay.ToString())
                .Select(s => s.Value)
                .SingleOrDefault();
        }
    }
}
