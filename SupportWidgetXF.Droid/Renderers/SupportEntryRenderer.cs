﻿using System;
using System.ComponentModel;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Views.InputMethods;
using Android.Widget;
using SupportWidgetXF.Droid.Renderers;
using SupportWidgetXF.Models.Widgets;
using SupportWidgetXF.Widgets;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SupportEntry), typeof(SupportEntryRenderer))]
namespace SupportWidgetXF.Droid.Renderers
{
	public class SupportEntryRenderer : EntryRenderer
    {
        private SupportEntry supportEntry;

        public SupportEntryRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.NewElement != null)
            {
                if (Element is SupportEntry)
                {
                    supportEntry = Element as SupportEntry;

                    InitilizeBorderEntry(supportEntry.EntryCornerColor.ToAndroid());

                    if (supportEntry.PaddingInside > 0)
                        InitlizePaddingInside();
                    else
                        InitilizeDrawableInsideView(supportEntry.DrawableInside, supportEntry.DrawableInsideAligment);

                    InitlizeReturnKey();

                    Control.Gravity = Android.Views.GravityFlags.CenterVertical;
                    Control.TextAlignment = Android.Views.TextAlignment.Center;
                    Control.EditorAction += (object sender, TextView.EditorActionEventArgs args) =>
                    {
                        SwitchReturnAction();
                        supportEntry.InvokeCompleted();
                    };
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName.Equals(nameof(SupportEntry.CurrentCornerColor)))
            {
                if (supportEntry != null)
                {
                    InitilizeBorderEntry(supportEntry.CurrentCornerColor.ToAndroid());
                }
            }
        }

        private void SwitchReturnAction()
        {
            var type = supportEntry.ReturnType;
            switch (type)
            {
                case SupportEntryReturnType.Go:
                    supportEntry.Unfocus();
                    break;
                case SupportEntryReturnType.Next:
                    supportEntry.OnNext();
                    break;
                case SupportEntryReturnType.Send:
                    supportEntry.Unfocus();
                    break;
                case SupportEntryReturnType.Search:
                    supportEntry.Unfocus();
                    break;
                case SupportEntryReturnType.Done:
                    supportEntry.Unfocus();
                    break;
                default:
                    supportEntry.Unfocus();
                    break;
            }
        }

        private void InitlizePaddingInside()
        {
            if (supportEntry.PaddingInside > 0)
            {
                Control.SetPadding((int)supportEntry.PaddingInside, 0, (int)supportEntry.PaddingInside, 0);
            }
        }

        private void InitlizeReturnKey()
        {
            var type = supportEntry.ReturnType;

            switch (type)
            {
                case SupportEntryReturnType.Go:
                    Control.ImeOptions = ImeAction.Go;
                    Control.SetImeActionLabel("Go", ImeAction.Go);
                    break;
                case SupportEntryReturnType.Next:
                    Control.ImeOptions = ImeAction.Next;
                    Control.SetImeActionLabel("Next", ImeAction.Next);
                    break;
                case SupportEntryReturnType.Send:
                    Control.ImeOptions = ImeAction.Send;
                    Control.SetImeActionLabel("Send", ImeAction.Send);
                    break;
                case SupportEntryReturnType.Search:
                    Control.ImeOptions = ImeAction.Search;
                    Control.SetImeActionLabel("Search", ImeAction.Search);
                    break;
                default:
                    Control.ImeOptions = ImeAction.Done;
                    Control.SetImeActionLabel("Done", ImeAction.Done);
                    break;
            }
        }

        private void InitilizeBorderEntry(Android.Graphics.Color color)
        {
            var gd = new GradientDrawable();
            gd.SetColor(supportEntry.BackgroundColorInside.ToAndroid());
            gd.SetCornerRadius((float)supportEntry.CornerRadius);
            gd.SetGradientRadius((float)supportEntry.CornerWidth);
            gd.SetStroke((int)supportEntry.CornerWidth, color);
            Control.SetBackground(gd);
        }

        private void InitilizeDrawableInsideView(string icon, SupportEntryDrawableInsideAligment aligment)
        {
            try
            {
                var imgId = SupportWidgetXFSetup.Activity.Resources.GetIdentifier(icon, "drawable", SupportWidgetXFSetup.Activity.PackageName);
                switch (aligment)
                {
                    case SupportEntryDrawableInsideAligment.Left:
                        Control.SetCompoundDrawablesWithIntrinsicBounds(imgId, 0, 0, 0);
                        break;
                    case SupportEntryDrawableInsideAligment.Right:
                        Control.SetCompoundDrawablesWithIntrinsicBounds(0, 0, imgId, 0);
                        break;
                    default:
                        break;
                }

                Control.Gravity = Android.Views.GravityFlags.CenterVertical;
                Control.SetPadding(10, 0, 10, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}