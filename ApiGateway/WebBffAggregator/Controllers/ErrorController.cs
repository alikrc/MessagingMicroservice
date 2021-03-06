﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBffAggregator.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("/error")]
        public IActionResult Error() => Problem();
    }
}
