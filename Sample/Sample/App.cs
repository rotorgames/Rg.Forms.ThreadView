using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Sample
{
    public class App : Application
    {
        public App()
        {
            // The root page of your application

            var mainPage = new ContentPage();

            var stack = new StackLayout();

            for (var i = 0; i < 100; i++)
            {
                stack.Children.Add(new Label()
                {
                    Text = "Test "+1
                });
            }

            mainPage.Content = stack;

            MainPage = mainPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
