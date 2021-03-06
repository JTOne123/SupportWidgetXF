﻿using System.ComponentModel;
using System.Threading.Tasks;
using Android.Content;
using Android.Widget;
using SupportWidgetXF.Droid.Renderers;
using SupportWidgetXF.Droid.Renderers.AutoComplete;
using SupportWidgetXF.Droid.Renderers.DropCombo;
using SupportWidgetXF.Widgets;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(SupportSearchResultList), typeof(SupportSearchResultListRenderer))]
namespace SupportWidgetXF.Droid.Renderers
{
    public class SupportSearchResultListRenderer : SupportBaseAutoCompleteRenderer<SupportSearchResultList, AutoCompleteTextView>
    {
        private bool FlagSetText = false;

        public SupportSearchResultListRenderer(Context context) : base(context)
        {
        }

        protected virtual void OriginalView_TextChanged(object sender, Android.Text.TextChangedEventArgs e)
        {
            //dm cam sua cho nay
            if (FlagSetText)
                FlagSetText = false;
            else
                SupportView.SendOnTextChanged(e.Text.ToString());
        }

        protected override void OnInitializeOriginalView()
        {
            OriginalView = new AutoCompleteTextView(Context);
            OriginalView.TextChanged += OriginalView_TextChanged;
            base.OnInitializeOriginalView();
        }

        protected override void OnInitializeAdapter()
        {
            base.OnInitializeAdapter();
            dropItemAdapter = new DropItemAdapterAsync(Context, SupportItemList, SupportView, this);
            OriginalView.Adapter = dropItemAdapter;
        }

        public override void IF_ItemSelectd(int position)
        {
            FlagSetText = true;
            SupportView.IsFocus = true;

            var text = ((DropItemAdapterAsync)dropItemAdapter).items[position].IF_GetTitle();
            SupportView.Text = text;

            Task.Delay(10).ContinueWith(delegate
            {
                SupportWidgetXFSetup.Activity.RunOnUiThread(delegate
                {
                    OriginalView.SetSelection(text.Length);
                    OriginalView.DismissDropDown();
                });
            });
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName.Equals(nameof(SupportViewDrop.ItemsSource)))
            {
                Task.Delay(10).ContinueWith(delegate
                {
                    SupportWidgetXFSetup.Activity.RunOnUiThread(delegate {
                        OriginalView.ShowDropDown();
                    });
                });
            }
        }
    }
}
