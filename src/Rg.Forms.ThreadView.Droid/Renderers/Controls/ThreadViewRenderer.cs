using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Java.Lang;
using Rg.Forms.ThreadView.Droid.Helpers;
using Rg.Forms.ThreadView.Droid.Packagers;
using Rg.Forms.ThreadView.Droid.Renderers.Controls;
using Rg.Forms.ThreadView.Helpers;
using Rg.Forms.ThreadView.Views.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ThreadView), typeof(ThreadViewRenderer))]
namespace Rg.Forms.ThreadView.Droid.Renderers.Controls
{
    public class ThreadViewRenderer : VisualElementRenderer<Views.Controls.ThreadView>
    {
        protected override void Dispose(bool disposing)
        {
            if (Element != null)
            {
                Element.ContentChanged -= OnContentChanged;
            }

            var packager = PackagerHelper.GetPackager<Views.Controls.ThreadView, ThreadViewPackager>(this);
            packager?.Destroy();

            base.Dispose(disposing);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Views.Controls.ThreadView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Element.ContentChanged += OnContentChanged;
                if (Element.Content != null)
                {
                    CreateRenderer(Element);
                }
            }
            if (e.OldElement != null)
            {
                e.OldElement.ContentChanged -= OnContentChanged;
            }
        }

        private void OnContentChanged(object sender, EventArgs args)
        {
            var element = (Views.Controls.ThreadView) sender;
            CreateRenderer(element);
        }

        private void CreateRenderer(Views.Controls.ThreadView element)
        {
            StartTaskIfNeed(() =>
            {
                var content = element.Content;

                var renderer = Platform.GetRenderer(element.Content);
                if (renderer == null)
                {
                    renderer = Platform.CreateRenderer(element.Content);
                    Platform.SetRenderer(element.Content, renderer);
                }

                if (Element != null && content == element.Content)
                {
                    ChangePackager();

                    BeginInvokeOnMainThreadIfNeed(async () =>
                    {
                        if (element.IsTimeOffset) await Task.Delay((int)element.TimeOffset);
                        SetContent(renderer);
                    });
                }
            });
        }

        private void SetContent(IVisualElementRenderer renderer)
        {
            if (Element == null) return;

            ContentHelper.OnContentChanged(Element, null, Element.Content);

            ViewGroup.AddView(renderer.ViewGroup);

            Element.Animate();
        }

        private void ChangePackager()
        {
            var packager = PackagerHelper.GetPackager<Views.Controls.ThreadView, VisualElementPackager>(this);

            packager?.Dispose();

            var newPackager = new ThreadViewPackager(this);
            SetPackager(newPackager);
        }

        private async void StartTaskIfNeed(Action action)
        {
            if (Element.InvokeOnMainThread)
            {
                Device.BeginInvokeOnMainThread(action);
            }
            else
            {
                await Task.Run(action);
            }
        }

        private void BeginInvokeOnMainThreadIfNeed(Action action)
        {
            if (Element.InvokeOnMainThread)
            {
                action.Invoke();
            }
            else
            {
                Device.BeginInvokeOnMainThread(action);
            }
        }
    }
}