using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Rg.Forms.ThreadView.Helpers
{
    public static class ContentHelper
    {
        public static void OnContentChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var assembly = Assembly.Load(new AssemblyName("Xamarin.Forms.Core"));
            var templateUtilitiesType = assembly.GetType("Xamarin.Forms.TemplateUtilities");

            var onContentChangedMethod = templateUtilitiesType.GetRuntimeMethod("OnContentChanged",
                new[] {typeof (BindableObject), typeof (object), typeof (object)});

            onContentChangedMethod?.Invoke(null, new[] {bindable, oldValue, newValue});
        }
    }
}
