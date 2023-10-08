using ClothShopperBack.BLL.Services;
using Quartz;

namespace ClothShopperBack.API.Jobs
{
    public class UpdateClothesJob : IJob
    {
        IClothService _clothService;

        public UpdateClothesJob(IClothService clothService)
        {
            _clothService = clothService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _clothService.UpdateCachedClothes();
        }
    }
}