using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoringAPI.Models
{
    public class LoanModel
    {

    }
    public class LoanRequest
    {
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string IdNumber { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }

    public class LoanRequestValidation
    {
        public DateTime DateOfBirth { get; set; }
        public string IdNumber { get; set; }
        public string Name { get; set; }
        public List<string> Phone { get; set; }
    }

    public class LoanProbability
    {
        public string Grade { get; set; }
        public double Probability { get; set; }
    }
    public class Root
    {
        public LoanRequest DataSet1 { get; set; }
        public LoanRequestValidation DataSet2 { get; set; }
        public LoanProbability DataSet3 { get; set; }
    }
    public class LoanResult
    {
        public double LoanLimit { get; set; }
    }
}
