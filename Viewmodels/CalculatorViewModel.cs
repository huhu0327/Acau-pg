using Acau_Playground.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MudBlazor;
using System.Windows.Input;

namespace Acau_Playground.Viewmodels
{
    public class CalculatorViewModel : ObservableObject
    {
        private readonly LocalStorageViewModel _storageViewModel;
        private readonly IClipboardService _clipboardService;
        private readonly ISnackbar _snackbar;

        private bool _isCopyClipboard;
        public bool IsCopyClipboard
        {
            get { return _isCopyClipboard; }
            set
            {
                SetProperty(ref _isCopyClipboard, value);
                _ = _storageViewModel.SetCopyClipboardAsync(IsCopyClipboard);
            }
        }

        private bool _isEdit;

        public bool IsEdit
        {
            get { return _isEdit; }
            set { _isEdit = value; }
        }


        private int _price;
        public int Price
        {
            get { return _price; }
            set { SetProperty(ref _price, value); }
        }


        public ICommand CalculateCommand { get; }

        public CalculatorViewModel(LocalStorageViewModel storage, IClipboardService clipboardService, ISnackbar snackbar)
        {
            _storageViewModel = storage;
            _clipboardService = clipboardService;
            _snackbar = snackbar;

            _snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomCenter;
            _snackbar.Configuration.VisibleStateDuration = 5000;
            _snackbar.Configuration.HideTransitionDuration = 500;
            _snackbar.Configuration.ShowCloseIcon = false;

            CalculateCommand = new RelayCommand(async () => await CalculateAsync());
        }

        public async Task OnInitializedAsync()
        {
            IsCopyClipboard = await _storageViewModel.GetCopyClipboardAsync();
        }

        private async Task CalculateAsync()
        {
            Price = new Random().Next(99999);

            if (IsCopyClipboard)
            {
                var task = new Task[] { Task.Run(() => _snackbar.Add("클립보드 복사 완료", Severity.Normal)), _clipboardService.CopyToClipboard($"/수표 {_price}") };
                await Task.WhenAll(task);
            }
        }
    }
}