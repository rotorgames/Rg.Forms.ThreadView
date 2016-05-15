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

            for (int i = 0; i < 600; i++)
            {
                ThreadViewStack.Children.Add(new Label
                {
                    Text = "Label " + i
                });
            }
        }
    }
}
