using Microsoft.AspNetCore.Mvc;
using ScoringAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScoringAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoanLimitController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok();
        }
        public static Dictionary<string, double> GradeWeights = new Dictionary<string, double>
        {
            { "AA", 1 },
            { "BB", 0.9 },
            { "CC", 0.8 },
            { "DD", 0.7 },
            { "EE", 0.6 },
            { "FF", 0.6 },
            { "GG", 0.5 },
            { "HH", 0.4 }
        };
        private double ComputeGradeScore(string grade, double probability)
        {
            switch (grade)
            {
                case "AA":
                    return 1.0 * probability;
                case "BB":
                    return 0.9 * probability;
                case "CC":
                    return 0.8 * probability;
                case "DD":
                    return 0.7 * probability;
                case "EE":
                case "FF":
                    return 0.6 * probability;
                case "GG":
                    return 0.5 * probability;
                case "HH":
                    return 0.4 * probability;
                default:
                    return 0.0;
            }
        }

        //Calculate the age
        private int GetAge(DateTime dateofbirth)
        {
            var today = DateTime.Today;
            var age = today.Year - dateofbirth.Year;
            if (dateofbirth > today.AddYears(-age))
                age--;

            return age;
        }

        private double ComputeKycScore(LoanRequest request, LoanRequestValidation validation, Root data)
        {
            // Validate customer ID number
            if (request.IdNumber != validation.IdNumber)
            {
                return 0;
            }

            // Validate customer age
            var age = DateTime.Today.Year - request.DateOfBirth.Year;
            if (age < 22)
            {
                return 0;
            }

            // Validate customer name 
            var nameScore = ComputeNameScore(data);
            if (nameScore < 0.7)
            {
                return 0;
            }

            // Validate customer phone number
            if (!validation.Phone.Contains(request.Phone))
            {
                return 0;
            }

            // All checks passed, return full score
            return 1;
        }

        private double ComputeNameScore( Root data)
        {
            // Compare customer names
            var dataset1 = data.DataSet1;
            var dataset2 = data.DataSet2;
            if (dataset1.Name != dataset2.Name)
            {
                return 0;
            }

            return 1;
        }

        //Calculate the customer credit score
        private double GetCreditSCore(string grade, double probability, [FromBody] Root data)
        {
            //Overall score = KYC weighted score + Grade weighted score
            //KYC weighted score = KYC score * 0.4
            //Grade weighted score = Grade score * 0.6
            //Overall score = (KYC score * 0.4) +(Grade score * 0.6)

            //Compute kyc score & grade score
            var kycScore = ComputeKycScore(data.DataSet1, data.DataSet2, data);

            var gradeScore = ComputeGradeScore(grade, probability);

            var totalScore = kycScore * 0.4 + gradeScore * 0.6;

            return totalScore;

        }
        //Get loan limit
        private double GetCustomerLoanLimit(Root data, string grade, double probability)
        {
            var kycScore = ComputeKycScore(data.DataSet1, data.DataSet2, data);
            var gradeScore = ComputeGradeScore(grade, probability);

           //var loanLimitScore = ComputeLoanLimitScore(kycScore, gradeScore);

            var loanLimit = (kycScore * 0.4 + gradeScore * 0.6) * probability * 100000;

            return loanLimit; // Multiply by 100000 to get loan limit in Ksh.

            //Suppose a base is 1 and 10000
            //var baselimit = 1;
            //var creditFactor = creditScore * 10000;
            //var loanLimit = baselimit * creditFactor;

            //return loanLimit;
        }

        //Calculate loan Limit; Validate Dataset1 and Dataset2
        [HttpPost("GetLoanLimit")]
        public IActionResult GetLoanLimit([FromBody] Root data)
        {

            var dataset1 = data.DataSet1;
            var dataset2 = data.DataSet2;
            var dataset3 = data.DataSet3;

            // Validate the data using DataSet2 and DataSet3
            if (dataset1.DateOfBirth != dataset2.DateOfBirth ||
                dataset1.IdNumber != dataset2.IdNumber ||
                dataset1.Name != dataset2.Name ||
                !dataset2.Phone.Contains(dataset1.Phone))

            {
                return BadRequest("Invalid data: Check if age is greater than 22 / other relevant data input");
            }

            var age = GetAge(dataset1.DateOfBirth);
            var kycScore = ComputeKycScore(data.DataSet1, data.DataSet2, data);           
            var creditScore = GetCreditSCore(dataset3.Grade, dataset3.Probability, data);
            //Assuming we use 100000 as the base limit
            var loanLimit =  kycScore * creditScore * 100000;

            return Ok(new { LoanLimit = loanLimit });
        }


        //[HttpPost("CalculateLoanLimit")]
        //public IActionResult IGetLoanLimit(LoanModel model)
        //{
        //    ResponseModel<List<LoanModel>> res = new ResponseModel<List<LoanModel>>();

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            List<LoanModel> result = _ar.GetLoanLimit(model).ToList();

        //            res.StatusCode = 200;
        //            res.Message = "SUCCESS";
        //            res.Data = result;

        //            return Ok(res);
        //        }
        //        catch (Exception ex)
        //        {
        //            res.StatusCode = 202;
        //            res.Message = "FAILED";
        //            res.Error = ex.Message;
        //            return Ok(res);
        //        }

        //    }
        //    else
        //    {

        //        var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //        res.StatusCode = 203;
        //        res.Message = "FAILED";
        //        res.Error = message;
        //        return Ok(res);

        //    }
        //}
    }
}
