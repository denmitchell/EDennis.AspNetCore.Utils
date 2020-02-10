using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.IO;
using System.Threading.Tasks;

namespace EDennis.AspNetCore.Utils.ByteArray {

    /// <summary>
    /// This class is used to bind the request body to a byte array.
    /// A controller action method having a single parameter 
    /// identified as [FromBody] byte[] requestBytes will bind to
    /// the requestBytes parameter.
    /// </summary>
    public class ByteArrayModelBinder : IModelBinder {

        /// <summary>
        /// Performs the model binding for a byte array
        /// </summary>
        /// <param name="bindingContext">context holding model binding info</param>
        /// <returns></returns>
        public Task BindModelAsync(ModelBindingContext bindingContext) {

            //initialize the bound value
            byte[] outBytes = null;

            //populate the bound value from the request body stream
            using(var outStream = new MemoryStream()) {
                bindingContext.HttpContext.Request.Body.CopyToAsync(outStream);
                outBytes = outStream.ToArray();
            }

            //update the status so validation methods are aware
            bindingContext.Result = ModelBindingResult.Success(outBytes);

            //return a completed task object
            return Task.CompletedTask;
        }

    }
}
