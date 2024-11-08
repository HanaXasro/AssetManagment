using Domain.Entities.UserEntity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AssetMangement.Controllers
{
    [Controller]
    public class BaseController : ControllerBase
    {
        public User User => (User)HttpContext.Items["Account"]!;
    }
}
