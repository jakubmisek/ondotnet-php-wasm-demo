using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.JSInterop;
using Pchp.Core;
using PhpLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorApp2
{
    public class PhpComponent : ComponentBase
    {
        Context _ctx;

        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [JSInvokable]
        public void Call(string name)
        {
            _ctx.Call(name);
            this.StateHasChanged();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Context.AddScriptReference(typeof(Counter).Assembly);

            _ctx = Context.CreateEmpty();
        }

        protected override async Task OnInitializedAsync()
        {
            var dotnetobj = DotNetObjectReference.Create(this);
            await JSRuntime.InvokeVoidAsync("jsf.start", dotnetobj);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            var output = new StringWriter();
            _ctx.Output = output;
            var script = Context.TryGetDeclaredScript("index.php");
            script.Evaluate(_ctx, _ctx.Globals, null);

            builder.AddMarkupContent(0, output.ToString());
        }
    }
}
