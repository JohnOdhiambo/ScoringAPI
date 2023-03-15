using ScoringAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoringAPI.Service
{
   public interface ILoanLimit
    {
        IEnumerable<LoanModel> GetLoanLimit(LoanModel model);
    }
}
