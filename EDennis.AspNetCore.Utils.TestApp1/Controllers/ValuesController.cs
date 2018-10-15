using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EDennis.AspNetCore.Utils.ByteArray;
using EDennis.AspNetCore.Utils.TestApp1.Utils;
using Microsoft.AspNetCore.Mvc;

namespace EDennis.AspNetCore.Utils.TestApp1.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase {

        // GET api/values
        [HttpGet]
        public ActionResult<string> Get() {
            return "ABCDEFG";
        }


        // POST api/values
        [HttpPost]
        public byte[] Post([ModelBinder(BinderType = typeof(ByteArrayModelBinder))] byte[] bytes) {
            //return the byte-wise complement of the array (to make the 
            //return bytes different from the submitted bytes)
            var flipped = ByteManipulator.FlipBytes(bytes);
            return flipped;
        }

    }
}
