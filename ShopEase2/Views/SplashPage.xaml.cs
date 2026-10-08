using ShopEase2.ViewModels;
using Microsoft.Maui.Controls;
namespace ShopEase2.View;

public partial class SplashPage : ContentPage
{
    public SplashPage(SplashViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}