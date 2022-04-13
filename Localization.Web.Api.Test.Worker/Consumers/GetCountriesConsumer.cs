using TestLocalization.BLL.Models;
using TestLocalization.BLL.Service;
using MassTransit;
using System.Threading.Tasks;

namespace TestLocalization.Worker.Consumers
{
    public class GetCountriesConsumer : IConsumer<IGetCountriesRequest>
    {
        private readonly ICountriesService _service;

        public GetCountriesConsumer(ICountriesService service)
        {
            _service = service;
        }

        public async Task Consume(ConsumeContext<IGetCountriesRequest> context)
        {
            var response =  _service.GetCounties();
            await context.RespondAsync<IOutputResult>(new
            {
                 OutputResult = response
            });
        }
    }
}
