using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace Sample.Pages
{
    public partial class StandartPage : ContentPage
    {
        public StandartPage()
        {
            InitializeComponent();

            var scroll = new ScrollView();

            var stack = new StackLayout();

            for (int i = 0; i < 600; i++)
            {
                stack.Children.Add(new Label
                {
                    Text = "Label "+i
                });
            }

            scroll.Content = stack;
            Content = scroll;
        }
    }
}
