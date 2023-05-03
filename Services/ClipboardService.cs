using Microsoft.JSInterop;

namespace Acau_Playground.Services
{
    public interface IClipboardService
    {
        public Task CopyToClipboard(string text);
    }
    public class ClipboardService : IClipboardService
    {
        private readonly IJSRuntime _jsRuntime;

        public ClipboardService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task CopyToClipboard(string text)
        {
            await _jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
        }
    }
}
