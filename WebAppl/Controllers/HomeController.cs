using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Network;
using Network.Source;

namespace WebAppl.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var network = new NeuralNetwork(new FileSource(),625, 13, 9);
            
            network.Train();

            return Json(1);
        }
        
    }
}