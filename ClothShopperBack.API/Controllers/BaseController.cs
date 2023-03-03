using ClothShopperBack.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace ClothShopperBack.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected async Task<ActionResult> MakeDefaultAction<T>(Func<Task<T>> func)
        {
            try
            {
                var data = await func();
                return Success(data);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        [NonAction]
        protected ObjectResult Success(object value)
        {
            var result = new
            {
                data = value
            };
            return Ok(result);
        }

        [NonAction]
        protected ObjectResult Error(string error)
        {
            var result = new
            {
                error = error
            };
            return BadRequest(result);
        }
    }
}