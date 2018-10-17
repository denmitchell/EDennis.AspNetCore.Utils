using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EDennis.AspNetCore.Utils.ByteArray;
using EDennis.AspNetCore.Utils.TestApp2.Models;
using EDennis.AspNetCore.Utils.TestApp2.Utils;
using Microsoft.AspNetCore.Mvc;

namespace EDennis.AspNetCore.Utils.TestApp2.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase {

        // GET api/values
        [HttpGet]
        [Produces("text/plain")]
        public ActionResult<string> Get() {
            return "ABCDEFG";
        }


        // POST api/values
        [HttpPost]
        public Person Post([FromBody] Person person) {
            var person2 = new Person {
                LastName = person.FirstName,
                FirstName = person.LastName
            };
            return person2;
        }
    }
}
