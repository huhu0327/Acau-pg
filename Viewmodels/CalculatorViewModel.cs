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
                if (EqualityComparer<bool>.Default.Equals(_isEditTable, value)) return;
                _isEditTable = value;

                if (!_isEditTable) UpdateTable().GetAwaiter();
            }
        }

        private int _plusPrice;
        public int PlusPrice
        {
            get { return _plusPrice; }
            set
            {
                if (EqualityComparer<int>.Default.Equals(_plusPrice, value)) return;
                _plusPrice = value;

                foreach (var item in TableItems)
                {
                    item.PurchasePrice = _plusPrice;
                }
            }
        }

        public ISet<Food> TableItems { get; private set; } = new HashSet<Food>();
        public HashSet<Food> SelectedTableItems { get; set; } = new();

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

            if (!string.IsNullOrEmpty(json)) TableItems = JsonConvert.DeserializeObject<ISet<Food>>(json);
        }

        private void ConfigurateSnackbar()
        {
            _snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomCenter;
            _snackbar.Configuration.VisibleStateDuration = 5000;
            _snackbar.Configuration.HideTransitionDuration = 500;
            _snackbar.Configuration.ShowCloseIcon = false;
        }


        public void RemoveSelectedItem(Food food)
        {
            TableItems.Remove(food);
        }

        public void RemoveSelectedItems()
        {
            foreach (var item in SelectedTableItems)
            {
                TableItems.Remove(item);
            }
        }

        public void Reset()
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

        private void AddFoods(IEnumerable<string> foods)
        {
            foreach (var foodName in foods)
            {
                var food = _foodViewModel.GetFood(foodName);
                if (food is null) continue;
                TableItems.Add(food);
            }
        }

        public IEnumerable<string> GetFoods() => _foodViewModel.GetJobList();

        public IEnumerable<string> GetItems()
        {
            if (string.IsNullOrWhiteSpace(SelectedJobName)) return Enumerable.Empty<string>();

            var items = TableItems.Select(item => item.Name);

            var result = _foodViewModel.GetFoodList(SelectedJobName)
                ?.Select(food => food.Name)
                .Where(food => !food.Contains("등급"))
                .Except(items);

            return result;
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
            if (string.IsNullOrEmpty(SelectedJobName) || SelectedFoodNames.FirstOrDefault() is null) return;

            AddFoods(SelectedFoodNames);
            CloseDialog();
        }

        public async Task UpdateTable()
        {
            if (TableItems is null) return;

            ISet<Food> data = TableItems.Select(s => s with { Box = 0, Set = 0, Num = 0 }).ToHashSet();

            var json = JsonConvert.SerializeObject(data);
            await _storageViewModel.SetTableItemAsync(json);
        }

    }
}