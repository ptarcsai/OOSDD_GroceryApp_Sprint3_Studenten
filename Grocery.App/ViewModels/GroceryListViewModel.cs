using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using System.Collections.ObjectModel;

namespace Grocery.App.ViewModels
{
    public partial class GroceryListViewModel : BaseViewModel
    {
        public ObservableCollection<GroceryList> GroceryLists { get; set; }
        private readonly IGroceryListService _groceryListService;

        public GroceryListViewModel(IGroceryListService groceryListService) 
        {
            Title = "Boodschappenlijst";
            _groceryListService = groceryListService;
            GroceryLists = new(_groceryListService.GetAll());
        }

        [RelayCommand]
        public async Task SelectGroceryList(GroceryList groceryList)
        {
            Dictionary<string, object> paramater = new() { { nameof(GroceryList), groceryList } };
            await Shell.Current.GoToAsync($"{nameof(Views.GroceryListItemsView)}?Titel={groceryList.Name}", true, paramater);
        }
        public override void OnAppearing() //Lijst legen en hervullen bij zoeken
        {
            base.OnAppearing();
            var all = _groceryListService.GetAll();
            GroceryLists.Clear();
            foreach (var l in all)
                GroceryLists.Add(l);
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();
            GroceryLists.Clear();
        }


        //UC9 Zoeken boodschappenlijsten
        [RelayCommand]
        private void PerformListSearch(string? query)
        {
            var term = (query ?? string.Empty).Trim();
            var all = _groceryListService.GetAll();

            var result = string.IsNullOrWhiteSpace(term)
                ? all
                : all.Where(l => l.Name?.Contains(term, StringComparison.OrdinalIgnoreCase) == true);

            GroceryLists.Clear();
            foreach (var l in result)
                GroceryLists.Add(l);
        }
    }
}
