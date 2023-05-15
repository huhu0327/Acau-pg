using Acau_Playground.Extensions;
using Acau_Playground.Models;
using Acau_Playground.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using MudBlazor;
using Newtonsoft.Json;

namespace Acau_Playground.Viewmodels
{
    public class CalculatorViewModel : ObservableObject
    {
        private readonly LocalStorageViewModel _storageViewModel;
        private readonly FoodViewModel _foodViewModel;
        private readonly IClipboardService _clipboardService;
        private readonly ISnackbar _snackbar;

        private bool _isEditTable;
        public bool IsEditTable
        {
            get { return _isEditTable; }
            set
            {
                value.DefaultEquals(ref _isEditTable);

                if (!_isEditTable) UpdateTableStorage().GetAwaiter();
            }
        }

        private int _plusPrice;
        public int PlusPrice
        {
            get { return _plusPrice; }
            set
            {
                if (value.DefaultEquals(ref _plusPrice)) return;

                if (SelectedTableItems?.FirstOrDefault() is null) return;

                foreach (var item in SelectedTableItems)
                {
                    TableItems.First(i => i.Name.Contains(item.Name)).PurchasePrice = _plusPrice;
                }
            }
        }

        public HashSet<Food> TableItems { get; private set; } = default!;
        public HashSet<Food> SelectedTableItems { get; set; } = default!;

        public bool IsVisibleDialog { get; set; }
        public string SelectedJobName { get; set; } = string.Empty;
        public IEnumerable<string> SelectedFoodNames { get; set; } = Enumerable.Empty<string>();

        public int Price => TableItems.Sum(content => content.Sum);

        public CalculatorViewModel(IServiceProvider services)
        {
            _foodViewModel = services.GetRequiredService<FoodViewModel>();
            _storageViewModel = services.GetRequiredService<LocalStorageViewModel>();
            _clipboardService = services.GetRequiredService<IClipboardService>();
            _snackbar = services.GetRequiredService<ISnackbar>();

            ConfigurateSnackbar();
        }

        public async Task OnInitializedAsync()
        {
            var json = await _storageViewModel.GetTableItemAsync();

            TableItems = !string.IsNullOrWhiteSpace(json) ? JsonConvert.DeserializeObject<HashSet<Food>>(json)! : new HashSet<Food>();
        }

        private void ConfigurateSnackbar()
        {
            _snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomCenter;
            _snackbar.Configuration.VisibleStateDuration = 5000;
            _snackbar.Configuration.HideTransitionDuration = 500;
            _snackbar.Configuration.ShowCloseIcon = false;
        }

        public IEnumerable<string> GetItems()
        {
            if (string.IsNullOrWhiteSpace(SelectedJobName))
            {
                return Enumerable.Empty<string>();
            }

            var exceptionItems = TableItems.Select(item => item.Name);

            var result = _foodViewModel.GetFoodList(SelectedJobName)
                ?.Select(food => food.Name)
                .Where(food => !food.Contains("등급"))
                .Except(exceptionItems)
                ?? Enumerable.Empty<string>();

            return result;
        }

        public void RemoveItem(string foodName)
        {
            TableItems.RemoveWhere(item => item.Name.Contains(foodName));
        }

        public void RemoveItems()
        {
            foreach (var item in SelectedTableItems)
            {
                RemoveItem(item.Name);
            }
        }

        public void ResetItems()
        {
            foreach (var item in TableItems)
            {
                item.Init();
            }
        }

        public async Task CopyPriceAsync()
        {
            var task = new Task[] {
                    Task.Run(() => _snackbar.Add("클립보드 복사 완료", Severity.Normal)),
                    _clipboardService.CopyToClipboard($"/수표 {Price}")
                };

            await Task.WhenAll(task);
        }

        public IEnumerable<string> GetFoods()
        {
            return _foodViewModel.GetJobList();
        }

        private void AddFoods(IEnumerable<string> foods)
        {
            foreach (var foodName in foods)
            {
                var food = _foodViewModel.GetFood(foodName);
                TableItems.Add(food);
            }
        }

        public void ShowDialog()
        {
            IsVisibleDialog = true;
        }

        public void CloseDialog()
        {
            IsVisibleDialog = false;
            SelectedJobName = string.Empty;
            SelectedFoodNames = Enumerable.Empty<string>();
        }

        public void AddDialog()
        {
            if (SelectedFoodNames?.FirstOrDefault() is null)
            {
                return;
            }

            AddFoods(SelectedFoodNames);
            CloseDialog();
        }

        public async Task UpdateTableStorage()
        {
            if (TableItems?.FirstOrDefault() is null)
            {
                return;
            }

            var data = TableItems.Select(s => new Food()
            { Name = s.Name, PurchasePrice = s.PurchasePrice, ShopPrice = s.ShopPrice }).ToHashSet();
            var json = JsonConvert.SerializeObject(data);
            await _storageViewModel.SetTableItemAsync(json);
        }

    }
}