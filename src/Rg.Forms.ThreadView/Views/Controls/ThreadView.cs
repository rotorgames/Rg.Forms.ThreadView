﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Rg.Forms.ThreadView.Context;
using Rg.Forms.ThreadView.EventArgs;
using Rg.Forms.ThreadView.Helpers;
using Xamarin.Forms;

namespace Rg.Forms.ThreadView.Views.Controls
{
    [ContentProperty(nameof(Content))]
    public class ThreadView : TemplatedView
    {
        //TODO: Изменить OnBindingContextChanged, оставив в нем только base.OnBindingContextChanged(); так как если не вызывать base метод, то контекст никогда не измениться, все остальное не нужно
        //TODO: Попробовать найти решение, в котором Entry и Image будут работать без InvokeOnMainThread
        //TODO: Возможно проблема с вылетами (Entry и Image) связанна с быстрой установкой BindingContext, возможно стоит подождать, пока создадутся все рендеры, а потом применять контекст
        //TODO: Возможно не стоит менять биндинг контекст, вызывать анимации и делать SetControl, так как OnContentChanged и так это делает если не менять Packager

        private bool _privateIsCreated;

        internal event EventHandler ContentChanged;
        internal object InternalBindingContext;

        public event EventHandler<ThreadViewInternalExceptionArgs> ThrowInternalException;

        public static readonly BindableProperty IsThreadEnabledProperty = BindableProperty.Create(nameof(IsThreadEnabled), typeof(bool), typeof(ThreadView), true);

        public static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(ThreadView), null, propertyChanged: OnContentChange);

        public static readonly BindableProperty IsCreatedProperty = BindableProperty.Create(nameof(IsCreated), typeof(bool), typeof(ThreadView), false, BindingMode.OneWayToSource);

        public static readonly BindableProperty IsAnimatedProperty = BindableProperty.Create(nameof(IsAnimated), typeof(bool), typeof(ThreadView), true);

        public static readonly BindableProperty IsTimeOffsetProperty = BindableProperty.Create(nameof(IsTimeOffset), typeof(bool), typeof(ThreadView), true);

        public static readonly BindableProperty TimeOffsetProperty = BindableProperty.Create(nameof(TimeOffset), typeof(int), typeof(ThreadView), 120);

        public static readonly BindableProperty InvokeOnMainThreadProperty = BindableProperty.Create(nameof(InvokeOnMainThread), typeof(bool), typeof(ThreadView), false);

        public static readonly BindableProperty CreatedCommandProperty = BindableProperty.Create(nameof(CreatedCommand), typeof(ICommand), typeof(ThreadView));

        public static readonly BindableProperty IsThrowInternalExceptionProperty = BindableProperty.Create(nameof(IsThrowInternalException), typeof(bool), typeof(ThreadView), false);

        /// <summary>
        /// This is default value for <see cref="T:OnThrowInternalError"/>
        /// </summary>
        public bool IsThrowInternalException
        {
            get { return (bool)GetValue(IsThrowInternalExceptionProperty); }
            set { SetValue(IsThrowInternalExceptionProperty, value); }
        }

        /// <summary>
        /// This property enables or disables <see cref="T:ThreadView"/>
        /// </summary>
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

        /// <summary>
        /// This property is changed when <see cref="T:Content"/> is created or changed
        /// </summary>
        public bool IsCreated
        {
            get { return (bool)GetValue(IsCreatedProperty); }
            internal set { SetValue(IsCreatedProperty, value); }
        }

        /// <summary>
        /// This property is enables or disables animation which is started after <see cref="T:Content"/> was created
        /// </summary>
        public bool IsAnimated
        {
            get { return (bool) GetValue(IsAnimatedProperty); }
            set { SetValue(IsAnimatedProperty, value); }
        }

        /// <summary>
        /// This property enables or disables delay before adding created views in <see cref="T:Content"/> 
        /// </summary>
        public bool IsTimeOffset
        {
            get { return (bool) GetValue(IsTimeOffsetProperty); }
            set { SetValue(IsTimeOffsetProperty, value); }
        }

        /// <summary>
        /// Delay before adding created views in <see cref="T:Content"/>
        /// </summary>
        public int TimeOffset
        {
            get { return (int) GetValue(TimeOffsetProperty); }
            set { SetValue(TimeOffsetProperty, value); }
        }

        /// <summary>
        /// Create <see cref="T:Content"/> in main thread or in second thread
        /// </summary>
        public bool InvokeOnMainThread
        {
            get { return (bool)GetValue(InvokeOnMainThreadProperty); }
            set { SetValue(InvokeOnMainThreadProperty, value); }
        }

        /// <summary>
        /// This command is invoked after <see cref="T:Content"/> was created
        /// </summary>
        public ICommand CreatedCommand
        {
            get { return (ICommand)GetValue(CreatedCommandProperty); }
            set { SetValue(CreatedCommandProperty, value); }
        }

        public ThreadView()
        {
            if (Device.OS != TargetPlatform.Android)
            {
                _privateIsCreated = true;
                IsCreated = true;
            }
        }

        protected override void OnBindingContextChanged()
        {
            View content = Content;
            if (content == null)
            {
                base.OnBindingContextChanged();
            }
            else if (_privateIsCreated)
            {
                SetInheritedBindingContext(content, BindingContext);
            }
            else if (!(BindingContext is DefaultBindingContext) && !_privateIsCreated && BindingContext != null)
            {
                InternalBindingContext = BindingContext;
                BindingContext = new DefaultBindingContext();
            }
        }

        #region Override Blocked Methods

        protected override void OnChildMeasureInvalidated()
        {
            if (IsCreated)
                base.OnChildMeasureInvalidated();
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            if (IsCreated)
                base.OnSizeAllocated(width, height);
        }

        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            if (IsCreated)
                base.LayoutChildren(x, y, width, height);
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            if (IsCreated)
                return base.OnMeasure(widthConstraint, heightConstraint);

            return new SizeRequest();
        }

        #endregion

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
                element._privateIsCreated = false;
                element.IsCreated = false;
                element.ContentChanged?.Invoke(element, System.EventArgs.Empty);
            }
        }

        internal async void OnCreated()
        {
            _privateIsCreated = true;
            if (InternalBindingContext != null) BindingContext = InternalBindingContext;
            await Task.Delay(100);

            IsCreated = true;
            CreatedCommand?.Execute(null);
            InvalidateLayout();
        }

        internal void PreparingAnimation()
        {
            Opacity = 0;
        }

        internal async void Animate()
        {
            await Task.Delay(300);
            await this.FadeTo(1);
            Opacity = 1;
        }

        /// <summary>
        /// This method is invoked when error is thrown inside <see cref="T:TemplateUtilities.OnControlTemplateChanged"/>
        /// </summary>
        /// <param name="e">This is exception thrown</param>
        /// <returns>If return value will be true then exception is be thrown</returns>
        public virtual bool OnThrowInternalException(Exception e)
        {
            ThrowInternalException?.Invoke(this, new ThreadViewInternalExceptionArgs(e));
            return IsThrowInternalException;
        }
    }
}