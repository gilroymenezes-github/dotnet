using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRcl.Components
{
    public class JsInteropService
    {
        public static ValueTask<string> ShowPrompt(IJSRuntime jsRuntime, string promptMessage)
        {
            return jsRuntime.InvokeAsync<string>(
                "hello.showPrompt",
                promptMessage);
        }

        public static ValueTask GreetMe(IJSRuntime jSRuntime)
        {
            return jSRuntime.InvokeVoidAsync(
                "hello.greetMe", null);
        }
    }
}
