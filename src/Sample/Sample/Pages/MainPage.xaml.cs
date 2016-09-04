using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Sample.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OpenThreadView(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ThreadPage());
        }

        private async void OpenStandartView(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new StandartPage());
        }

        private async void OpenMultiplyThreadView(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MultiplyThreadPage());
        }

        private async void OpenMainThreadView(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MainThreadPage());
        }
    }
}
