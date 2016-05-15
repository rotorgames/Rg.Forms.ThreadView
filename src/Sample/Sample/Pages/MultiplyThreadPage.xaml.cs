using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Forms.ThreadView.Views.Controls;
using Xamarin.Forms;

namespace Sample.Pages
{
    public partial class MultiplyThreadPage : ContentPage
    {
        public MultiplyThreadPage()
        {
            InitializeComponent();

            for (int i = 0; i < 6; i++)
            {
                var threadView = new ThreadView();
                var threadStack = new StackLayout();

                for (int j = 0; j < 100; j++)
                {
                    threadStack.Children.Add(new Label
                    {
                        Text = "Label" + (j + i * 100)
                    });
                }

                threadView.Content = threadStack;
                ThreadViewStack.Children.Add(threadView);
            }
        }
    }
}
