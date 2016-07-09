using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Forms.ThreadView.Context;
using Rg.Forms.ThreadView.Helpers;
using Xamarin.Forms;

namespace Rg.Forms.ThreadView.Views.Controls
{
    [ContentProperty(nameof(Content))]
    public class ThreadView : TemplatedView
    {
        //TODO: Попробовать найти решение, в котором Entry и Image будут работать без InvokeOnMainThread
        //TODO: Возможно проблема с вылетами (Entry и Image) связанна с быстрой установкой BindingContext, возможно стоит подождать, пока создадутся все рендеры, а потом применять контекст

        internal event EventHandler ContentChanged;
        internal object InternalBindingContext;

        public static readonly BindableProperty IsThreadEnabledProperty = BindableProperty.Create(nameof(IsThreadEnabled), typeof(bool), typeof(ThreadView), true);

        public static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(ThreadView), null, propertyChanged: OnContentChange);

        public static readonly BindableProperty IsCreatedProperty = BindableProperty.Create(nameof(IsCreated), typeof(bool), typeof(ThreadView), false, BindingMode.OneWayToSource);

        public static readonly BindableProperty IsAnimatedProperty = BindableProperty.Create(nameof(IsAnimated), typeof(bool), typeof(ThreadView), true);

        public static readonly BindableProperty IsTimeOffsetProperty = BindableProperty.Create(nameof(IsTimeOffset), typeof(bool), typeof(ThreadView), true);

        public static readonly BindableProperty TimeOffsetProperty = BindableProperty.Create(nameof(TimeOffset), typeof(int), typeof(ThreadView), 120);

        public static readonly BindableProperty InvokeOnMainThreadProperty = BindableProperty.Create(nameof(InvokeOnMainThread), typeof(bool), typeof(ThreadView), false);

        public bool IsThreadEnabled
        {
            get { return (bool)GetValue(IsThreadEnabledProperty); }
            set { SetValue(IsThreadEnabledProperty, value); }
        }

        public View Content
        {
            get { return (View) GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public bool IsCreated
        {
            get { return (bool)GetValue(IsCreatedProperty); }
            internal set { SetValue(IsCreatedProperty, value); }
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

        public int TimeOffset
        {
            get { return (int) GetValue(TimeOffsetProperty); }
            set { SetValue(TimeOffsetProperty, value); }
        }

        public bool InvokeOnMainThread
        {
            get { return (bool)GetValue(InvokeOnMainThreadProperty); }
            set { SetValue(InvokeOnMainThreadProperty, value); }
        }

        public ThreadView()
        {
            if (Device.OS != TargetPlatform.Android)
            {
                IsCreated = true;
            }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            View content = Content;
            if (content != null && IsCreated)
            {
                SetInheritedBindingContext(content, BindingContext);
            }
            if (!(BindingContext is DefaultBindingContext) && !IsCreated)
            {
                InternalBindingContext = BindingContext;
                BindingContext = new DefaultBindingContext();
            }
        }

        private static void OnContentChange(BindableObject bindable, object oldvalue, object newvalue)
        {
            var element = (ThreadView) bindable;

            if (element.IsAnimated) element.PreparingAnimation();

            if (Device.OS != TargetPlatform.Android || !element.IsThreadEnabled)
            {
                ContentHelper.OnContentChanged(bindable, oldvalue, newvalue);
                if (element.IsAnimated) element.Animate();
            }
            else
            {
                element.IsCreated = false;
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