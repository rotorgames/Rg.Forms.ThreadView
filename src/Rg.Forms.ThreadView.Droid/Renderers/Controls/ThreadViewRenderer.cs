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
using Exception = System.Exception;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(ThreadView), typeof(ThreadViewRenderer))]
namespace Rg.Forms.ThreadView.Droid.Renderers.Controls
{
    public class ThreadViewRenderer : ViewRenderer<Views.Controls.ThreadView, View>
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

        private void OnContentChanged(object sender, System.EventArgs args)
        {
            var element = (Views.Controls.ThreadView) sender;
            CreateRenderer(element);
        }

        private void CreateRenderer(Views.Controls.ThreadView element)
        {
            StartTaskIfNeed(async () =>
            {
                var content = element.Content;

                try
                {
                    if (element.IsTimeOffset && element.InvokeOnMainThread) await Task.Delay(element.TimeOffset);
                    content.Parent = Element;
                }
                catch (Exception e)
                {
                    if (CatchInternalError(e))
                        throw;
                }

                if(Element?.Content == null)
                    return;

                var renderer = Platform.GetRenderer(content);
                if (renderer == null)
                {
                    renderer = Platform.CreateRenderer(element.Content);
                    Platform.SetRenderer(element.Content, renderer);
                }

                try
                {
                    content.Parent = null;

                    if (Element != null && content == element.Content)
                    {
                        ChangePackagerIfNeed();

                        ContentHelper.OnContentChanged(Element, Element.Content, content);
                        
                        BeginInvokeOnMainThreadIfNeed(async () =>
                        {
                            if (element.IsTimeOffset && !element.InvokeOnMainThread) await Task.Delay(element.TimeOffset);
                            
                            if (Element != null)
                            {
                                SetContent(renderer);
                            }
                        });
                    }
                }
                catch (Exception e)
                {
                    if (CatchInternalError(e))
                        throw;
                }
            });
        }

        private void SetContent(IVisualElementRenderer renderer)
        {
            if (Element == null) return;

            try
            {
                SetNativeControl(renderer.ViewGroup);
                Element.OnCreated();

                Element.Animate();
            }
            catch (Exception e)
            {
                if (CatchInternalError(e))
                    throw;
            }
            
        }

        private void ChangePackagerIfNeed()
        {
            var packager = PackagerHelper.GetPackager<Views.Controls.ThreadView, VisualElementPackager>(this);

            if(!(packager is ThreadViewPackager))
            {
                packager?.Dispose();

                var newPackager = new ThreadViewPackager(this);
                SetPackager(newPackager);
            }
        }

        private async void StartTaskIfNeed(Action action)
        {
            if (Element == null) return;

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
            if(Element == null) return;

            if (Element.InvokeOnMainThread)
            {
                action.Invoke();
            }
            else
            {
                Device.BeginInvokeOnMainThread(action);
            }
        }

        private bool CatchInternalError(Exception e)
        {
            System.Diagnostics.Debug.WriteLine(e.ToString());
            var isThrow = Element.OnThrowInternalException(e);

            return isThrow;
        }
    }
}