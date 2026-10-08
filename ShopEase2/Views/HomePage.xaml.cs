using ShopEase2.ViewModels;

namespace ShopEase2.Views;

public partial class HomePage : ContentPage
{
 
    public HomePage(HomeViewModel vm)
    {
        InitializeComponent();

        BindingContext = vm;
    }
}