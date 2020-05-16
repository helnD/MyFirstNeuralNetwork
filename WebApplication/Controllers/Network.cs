using Microsoft.AspNetCore.Mvc;


namespace WebApplication.Controllers
{
    public class Network : Controller
    {

        [HttpGet]
        public IActionResult Index()
        {
            return Json(1);
        }
        
    }
}