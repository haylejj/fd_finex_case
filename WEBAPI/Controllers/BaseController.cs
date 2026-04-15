using Microsoft.AspNetCore.Mvc;
using WEBAPI.Application.Common;

namespace WEBAPI.Controllers;

public class BaseController : ControllerBase
{
    // buradaki amaç, tüm controller'larda ortak olan CreateResult metodunu tek bir yerde tanýmlayarak kod tekrarýný azaltmak ve standart bir API yanýt formatý saðlamaktýr.
    [NonAction]
    protected IActionResult CreateResult<T>(ServiceResult<T> result)
    {
        return new ObjectResult(result) { StatusCode = (int)result.StatusCode };
    }

    [NonAction]
    protected IActionResult CreateResult(ServiceResult result)
    {
        return new ObjectResult(result) { StatusCode = (int)result.StatusCode };
    }
}
