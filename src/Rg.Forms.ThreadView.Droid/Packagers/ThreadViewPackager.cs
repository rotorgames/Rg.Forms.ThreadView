using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace Rg.Forms.ThreadView.Droid.Packagers
{
    internal class ThreadViewPackager : VisualElementPackager
    {
        private IVisualElementRenderer _renderer;

        private List<IVisualElementRenderer> ChildViews
        {
            get
            {
                return (List<IVisualElementRenderer>)typeof(VisualElementPackager)
                    .GetField("_childViews", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.GetValue(this);
            }
            set
            {
                typeof(VisualElementPackager)
                    .GetField("_childViews", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.SetValue(this, value);
            }
        }

        public ThreadViewPackager(IVisualElementRenderer renderer) : base(renderer)
        {
            var handler = typeof(VisualElementPackager).GetField("_childAddedHandler", BindingFlags.NonPublic | BindingFlags.Instance);
            handler?.SetValue(this, (EventHandler<ElementEventArgs>) OnChildAdded);
        }

        public void Destroy()
        {
            ChildViews = null;
        }

        private void OnChildAdded(object sender, ElementEventArgs e)
        {
            //ParentOnChildAdded();

            var element = (VisualElement) e.Element;
            var renderer = Platform.GetRenderer(element);

            ParentAddChild(element, renderer);
        }

        private void ParentOnChildAdded()
        {
            var handler = typeof (VisualElementPackager).GetMethod("OnChildAdded",
                BindingFlags.NonPublic | BindingFlags.Instance);

            var eventArgs = new ElementEventArgs(new Page());

            handler?.Invoke(this, new [] { eventArgs });
        }

        private void ParentAddChild(VisualElement view, IVisualElementRenderer oldRenderer = null)
        {
            if (oldRenderer != null)
            {
                if (ChildViews == null)
                    ChildViews = new List<IVisualElementRenderer>();

                //_renderer.ViewGroup.AddView(oldRenderer.ViewGroup);
                ChildViews.Add(oldRenderer);
            }
            else
            {
                var handler = typeof(VisualElementPackager).GetMethod("AddChild",
                    BindingFlags.NonPublic | BindingFlags.Instance);

                handler?.Invoke(this, new object[] { view, null, null, false });
            }
        }
    }
}