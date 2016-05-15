using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android;
using View = Xamarin.Forms.View;

namespace Rg.Forms.ThreadView.Droid.Helpers
{
    internal static class PackagerHelper
    {
        public static TPackager GetPackager<TView, TPackager>(VisualElementRenderer<TView> renderer)
            where TView : View
            where TPackager : VisualElementPackager
        {
            return typeof (VisualElementRenderer<TView>)
                .GetField("_packager", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(renderer) as TPackager;
        }
    }
}