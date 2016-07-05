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

        public static readonly BindableProperty IsCreatedProperty = BindableProperty.Create(nameof(IsCreated), typeof(bool), typeof(ThreadView), false, BindingMode.OneWayToSource);

        public static readonly BindableProperty IsAnimatedProperty = BindableProperty.Create(nameof(IsAnimated), typeof(bool), typeof(ThreadView), true);

        public static readonly BindableProperty IsTimeOffsetProperty = BindableProperty.Create(nameof(IsTimeOffset), typeof(bool), typeof(ThreadView), true);

        public static readonly BindableProperty TimeOffsetProperty = BindableProperty.Create(nameof(TimeOffset), typeof(uint), typeof(ThreadView), 120u);

        public static readonly BindableProperty InvokeOnMainThreadProperty = BindableProperty.Create(nameof(InvokeOnMainThread), typeof(bool), typeof(ThreadView), false);

        public View Content
        {
            get { return (View) GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public bool IsCreated
        {
            get { return (bool)GetValue(IsCreatedProperty); }
            private set { SetValue(IsCreatedProperty, value); }
        }

        public bool IsAnimated
        {
            get { return (bool) GetValue(IsAnimatedProperty); }
            set { SetValue(IsAnimatedProperty, value); }
        }

        public bool IsTimeOffset
        {
            get { return (bool) GetValue(IsTimeOffsetProperty); }
            set { SetValue(IsTimeOffsetProperty, value); }
        }

        public uint TimeOffset
        {
            get { return (uint) GetValue(TimeOffsetProperty); }
            set { SetValue(TimeOffsetProperty, value); }
        }

        public bool InvokeOnMainThread
        {
            get { return (bool)GetValue(InvokeOnMainThreadProperty); }
            set { SetValue(InvokeOnMainThreadProperty, value); }
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

            if (element.IsAnimated) element.PreparingAnimation();

            if (Device.OS != TargetPlatform.Android || !element.IsEnabled)
            {
                ContentHelper.OnContentChanged(bindable, oldvalue, newvalue);
                if (element.IsAnimated) element.Animate();
            }
            else
            {
                element.ContentChanged?.Invoke(element, EventArgs.Empty);
            }
        }

        internal void PreparingAnimation()
        {
            Opacity = 0;
        }

        internal async void Animate()
        {
            await Task.Delay((int) TimeOffset);
            await this.FadeTo(1);
        }
    }
}