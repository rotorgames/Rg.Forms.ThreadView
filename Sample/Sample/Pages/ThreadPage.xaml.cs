using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Forms.ThreadView.Views.Controls;
using Xamarin.Forms;

namespace Sample.Pages
{
    public partial class ThreadPage : ContentPage
    {
        public ThreadPage()
        {
            InitializeComponent();

            var scroll = new ScrollView();

            var threadView = new ThreadView();

            var stack = new StackLayout();

            for (int i = 0; i < 600; i++)
            {
                stack.Children.Add(new Label
                {
                    Text = "Label " + i
                });
            }

            threadView.Content = stack;
            scroll.Content = threadView;
            Content = scroll;
        }
    }
}
