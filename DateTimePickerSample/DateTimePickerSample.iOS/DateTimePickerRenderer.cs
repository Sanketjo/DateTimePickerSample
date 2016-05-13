using System;
using System.Drawing;
using System.ComponentModel;
#if __UNIFIED__
using UIKit;
using Foundation;
#else
using MonoTouch.UIKit;
using MonoTouch.Foundation;
#endif
#if __UNIFIED__
using RectangleF = CoreGraphics.CGRect;
using SizeF = CoreGraphics.CGSize;
using PointF = CoreGraphics.CGPoint;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using DateTimePickerSample;
using DateTimePickerSample.iOS;

#else
using nfloat=System.Single;
using nint=System.Int32;
using nuint=System.UInt32;
#endif

[assembly: ExportRendererAttribute(typeof(DateTimePicker), typeof(DateTimePickerRenderer))]
namespace DateTimePickerSample.iOS
{
    internal class NoCaretField : UITextField
    {
        public NoCaretField() : base(new RectangleF())
        {
        }

        public override RectangleF GetCaretRectForPosition(UITextPosition position)
        {
            return new RectangleF();
        }
    }

    public class DateTimePickerRenderer : ViewRenderer<DateTimePicker, UITextField>
    {
        UIDatePicker _picker;

        protected override void OnElementChanged(ElementChangedEventArgs<DateTimePicker> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                var entry = new NoCaretField { BorderStyle = UITextBorderStyle.RoundedRect };

                entry.Started += OnStarted;
                entry.Ended += OnEnded;

                _picker = new UIDatePicker { Mode = UIDatePickerMode.DateAndTime, TimeZone = new NSTimeZone("UTC") };

                _picker.ValueChanged += HandleValueChanged;

                var width = UIScreen.MainScreen.Bounds.Width;
                var toolbar = new UIToolbar(new RectangleF(0, 0, width, 44)) { BarStyle = UIBarStyle.Default, Translucent = true };
                var spacer = new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace);
                var doneButton = new UIBarButtonItem(UIBarButtonSystemItem.Done, (o, a) => entry.ResignFirstResponder());

                toolbar.SetItems(new[] { spacer, doneButton }, false);

                entry.InputView = _picker;
                entry.InputAccessoryView = toolbar;
                entry.Font = UIFont.SystemFontOfSize(Element.FontSize);

                SetNativeControl(entry);
            }

            if (e.NewElement != null)
            {
                UpdateDateFromModel(false);
                UpdateMaximumDate();
                UpdateMinimumDate();
                UpdateTextColor();
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == DateTimePicker.DateTicksProperty.PropertyName ||
                e.PropertyName == DateTimePicker.FormatProperty.PropertyName)
                UpdateDateFromModel(true);
            else if (e.PropertyName == DateTimePicker.MinimumDateProperty.PropertyName)
                UpdateMinimumDate();
            else if (e.PropertyName == DateTimePicker.MaximumDateProperty.PropertyName)
                UpdateMaximumDate();
            else if (e.PropertyName == DateTimePicker.TextColorProperty.PropertyName)
                UpdateTextColor();
        }

        void HandleValueChanged(object sender, EventArgs e)
        {
            if (Element != null)
            {
                var date = _picker.Date.ToDateTime();
                ((IElementController)Element).SetValueFromRenderer(DateTimePicker.DateTicksProperty, date.Ticks);
            }
        }

        void OnEnded(object sender, EventArgs eventArgs)
        {
            ((IElementController)Element).SetValueFromRenderer(VisualElement.IsFocusedProperty, false);
        }

        void OnStarted(object sender, EventArgs eventArgs)
        {
            ((IElementController)Element).SetValueFromRenderer(VisualElement.IsFocusedProperty, true);
        }

        void UpdateDateFromModel(bool animate)
        {
            var date = new DateTime(Element.DateTicks);
            if (_picker.Date.ToDateTime() != date)
                _picker.SetDate(date.ToNSDate(), animate);

            Control.Text = date.ToString(Element.Format);
        }

        void UpdateMaximumDate()
        {
            _picker.MaximumDate = Element.MaximumDate.ToNSDate();
        }

        void UpdateMinimumDate()
        {
            _picker.MinimumDate = Element.MinimumDate.ToNSDate();
        }

        void UpdateTextColor()
        {
            Control.TextColor = Element.TextColor.ToUIColor();
        }
    }
}
