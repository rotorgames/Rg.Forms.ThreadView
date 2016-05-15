using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Forms.ThreadView.Helpers;
using Xamarin.Forms;

namespace Rg.Forms.ThreadView.Views.Controls
{
    [ContentProperty(nameof(Content))]
    public class ThreadView : TemplatedView
    {
        internal event EventHandler ContentChanged;

        public static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(ThreadView), null, propertyChanged: OnContentChange);

        public View Content
        {
            get { return (View)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public ThreadView()
        {
            
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            View content = Content;
            ControlTemplate controlTemplate = ControlTemplate;
            if (content != null && controlTemplate != null)
            {
                SetInheritedBindingContext(content, BindingContext);
            }
        }

        private static void OnContentChange(BindableObject bindable, object oldvalue, object newvalue)
        {
            var element = (ThreadView) bindable;

            if (Device.OS != TargetPlatform.Android || !element.IsEnabled)
            {
                ContentHelper.OnContentChanged(bindable, oldvalue, newvalue);
            }
            else
            {
                element.ContentChanged?.Invoke(element, EventArgs.Empty);
            }
        }
    }
}
