using Blazored.LocalStorage;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Acau_Playground.Viewmodels
{
    public class LocalStorageViewModel : ObservableObject
    {
        private readonly ILocalStorageService _localStorage;
        //public IDictionary<string, string> Storage { get; private set; }

        public LocalStorageViewModel(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task OnInitializedAsync()
        {
            if (!await _localStorage.ContainKeyAsync("CopyClipboard"))
            {
                await SetCopyClipboardAsync(true);
            }
        }

        public async Task SetThemeAsync(bool isDark) => await _localStorage.SetItemAsync("DarkMode", isDark);
        public async Task<bool> GetThemeAsync() => await _localStorage.GetItemAsync<bool>("DarkMode");

        public async Task SetCopyClipboardAsync(bool isCopy) => await _localStorage.SetItemAsync("CopyClipboard", isCopy);
        public async Task<bool> GetCopyClipboardAsync() => await _localStorage.GetItemAsync<bool>("CopyClipboard");

    }
}
