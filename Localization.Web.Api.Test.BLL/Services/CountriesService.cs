using TestLocalization.BLL.Models;

namespace TestLocalization.BLL.Service
{
    public class CountriesService : ICountriesService
    {
        public OutputResult GetCounties()
        {
            return new OutputResult
            {
                IsErrorOccurred = false,
                Message = "SuccessMessage"
            };
        }
    }
}
