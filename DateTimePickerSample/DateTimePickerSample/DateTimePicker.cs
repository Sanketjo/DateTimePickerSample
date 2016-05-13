using System;
using Xamarin.Forms;

namespace DateTimePickerSample
{
    public class DateTimePicker : View
    {
        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create("FontSize",
                                    typeof(int),
                                    typeof(DateTimePicker),
                                    10);

        public static readonly BindableProperty FormatProperty =
            BindableProperty.Create(nameof(Format),
                                    typeof(string),
                                    typeof(DateTimePicker),
                                    "dd/M/yyyy : hh:mm");

        public static readonly BindableProperty DateTicksProperty =
            BindableProperty.Create(nameof(DateTicks),
                                    typeof(long),
                                    typeof(DateTimePicker),
                                    default(long),
                                    propertyChanged: OnDateTicksPropertyChanged);

        public static readonly BindableProperty MinimumDateProperty =
            BindableProperty.Create(nameof(MinimumDate),
                                    typeof(DateTime),
                                    typeof(DateTimePicker),
                                    new DateTime(1900, 1, 1),
                                    validateValue: ValidateMinimumDate,
                                    coerceValue: CoerceMinimumDate);

        public static readonly BindableProperty MaximumDateProperty =
            BindableProperty.Create(nameof(MaximumDate),
                                    typeof(DateTime),
                                    typeof(DateTimePicker),
                                    new DateTime(2100, 12, 31),
                                    validateValue: ValidateMaximumDate,
                                    coerceValue: CoerceMaximumDate);

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor),
                                    typeof(Color),
                                    typeof(DateTimePicker),
                                    Color.Black);

        #region Properties

        public int FontSize
        {
            get { return (int)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        public long DateTicks
        {
            get { return (long)GetValue(DateTicksProperty); }
            set { SetValue(DateTicksProperty, value); }
        }

        public string Format
        {
            get { return (string)GetValue(FormatProperty); }
            set { SetValue(FormatProperty, value); }
        }

        public DateTime MaximumDate
        {
            get { return (DateTime)GetValue(MaximumDateProperty); }
            set { SetValue(MaximumDateProperty, value); }
        }

        public DateTime MinimumDate
        {
            get { return (DateTime)GetValue(MinimumDateProperty); }
            set { SetValue(MinimumDateProperty, value); }
        }

        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set { SetValue(TextColorProperty, value); }
        } 
        #endregion

        public event EventHandler<DateChangedEventArgs> DateSelected;

        static object CoerceDate(BindableObject bindable, object value)
        {
            var picker = (DateTimePicker)bindable;
            var dateValue = ((DateTime)value).Date;

            if (dateValue > picker.MaximumDate)
                dateValue = picker.MaximumDate;

            if (dateValue < picker.MinimumDate)
                dateValue = picker.MinimumDate;

            return dateValue;
        }

        static object CoerceMaximumDate(BindableObject bindable, object value)
        {
            var dateValue = ((DateTime)value).Date;
            var picker = (DateTimePicker)bindable;
            var date = new DateTime(picker.DateTicks);
            if (date > dateValue)
                date = dateValue;

            return dateValue;
        }

        static object CoerceMinimumDate(BindableObject bindable, object value)
        {
            var dateValue = ((DateTime)value).Date;
            var picker = (DateTimePicker)bindable;
            var date = new DateTime(picker.DateTicks);
            if (date < dateValue)
                date = dateValue;

            return dateValue;
        }

        static void OnDateTicksPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var datePicker = (DateTimePicker)bindable;
            var newDate = new DateTime((long)newValue);
            var oldDate = new DateTime((long)oldValue);
            var selected = datePicker.DateSelected;
            if (selected != null)
                selected(datePicker, new DateChangedEventArgs(oldDate, newDate));
        }

        static bool ValidateMaximumDate(BindableObject bindable, object value)
        {
            return (DateTime)value >= ((DateTimePicker)bindable).MinimumDate;
        }

        static bool ValidateMinimumDate(BindableObject bindable, object value)
        {
            return (DateTime)value <= ((DateTimePicker)bindable).MaximumDate;
        }
    }
}