// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

namespace Api.Controllers
{
    [Route("identity")]
    //[Authorize]
    public class IdentityController : ControllerBase
    {
        [Authorize(AuthenticationSchemes = "MultiScheme", Policy = "AclSession")]
        public IActionResult Get()
        {
            Console.WriteLine(User.Claims);
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}