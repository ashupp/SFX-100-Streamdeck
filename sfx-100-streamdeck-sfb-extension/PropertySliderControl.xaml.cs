using System;
using System.Windows;

namespace sfx_100_streamdeck_sfb_extension
{

    public class KeyValueEventArgs: EventArgs
    {
        public int Key { get; set; }
        public int Value { get; set; }
    }

    /// <summary>
    /// Interaktionslogik für PropertySliderControl.xaml
    /// </summary>
    public partial class PropertySliderControl
    {
        #region DependencyProperties
        public static readonly DependencyProperty KeyProperty = DependencyProperty.Register("Key", typeof(int), typeof(PropertySliderControl));
        public static readonly DependencyProperty PNameProperty = DependencyProperty.Register("PName", typeof(string), typeof(PropertySliderControl));
        public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register("Description", typeof(string), typeof(PropertySliderControl));
        public static readonly DependencyProperty CurrentValueProperty = DependencyProperty.Register("CurrentValue", typeof(int), typeof(PropertySliderControl));
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(int), typeof(PropertySliderControl));
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(int), typeof(PropertySliderControl));
        public static readonly DependencyProperty DefaultValueProperty = DependencyProperty.Register("DefaultValue", typeof(int), typeof(PropertySliderControl));
        #endregion

        #region Members

        public int Key
        {
            get { return (int)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        public string PName
        {
            get { return (string)GetValue(PNameProperty); }
            set { SetValue(PNameProperty, value); }
        }

        public string Description
        {
            get { return (string)GetValue(DescriptionProperty); }
            set { SetValue(DescriptionProperty, value); }
        }

        public int CurrentValue
        {
            get { return (int)GetValue(CurrentValueProperty); }
            set
            {
                SetValue(CurrentValueProperty, value);
                propertySlider.Value = value;
            }
        }

        public int MinValue
        {
            get { return (int)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); }
        }

        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }
        public int DefaultValue
        {
            get { return (int)GetValue(DefaultValueProperty); }
            set { SetValue(DefaultValueProperty, value); }
        }

        #endregion

        #region Own Eventhandlers
        public event EventHandler<KeyValueEventArgs> SetValueClicked;
        public event EventHandler<KeyValueEventArgs> DefaultValueClicked;
        #endregion

        #region Main Entry
        
        public PropertySliderControl()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        #endregion

        #region Eventhandlers
        private void btnSetValue_Click(object sender, RoutedEventArgs e)
        {
            if (SetValueClicked != null)
            {
                var args = new KeyValueEventArgs()
                {
                    Key=Key,
                    Value=Convert.ToInt32(textBoxValue.Text)
                };
                SetValueClicked(this, args);
            }
        }

        private void btnResetDefault_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = MessageBox.Show("Reset to default (Value:" + DefaultValue + ")", "Are you sure?", MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                if (DefaultValueClicked == null) return;
                propertySlider.Value = DefaultValue;
                var args = new KeyValueEventArgs()
                {
                    Key=Key,
                    Value = Convert.ToInt32(DefaultValue)
                };
                DefaultValueClicked(this, args);
            }
        }

        #endregion
    }
}
