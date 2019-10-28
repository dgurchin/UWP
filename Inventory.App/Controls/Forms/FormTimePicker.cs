using System;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace Inventory.Controls
{
    public class FormTimePicker : TimePicker, IFormControl
    {
        public event EventHandler<FormVisualState> VisualStateChanged;

        private Grid _contentGrid = null;
        /*private TextBlock _hourInput = null;
        private TextBlock _minuteInput = null;
        private TextBlock _periodInput = null;
        private Border _firstBorder = null;
        private Border _secondBorder = null;
        private Border _thirdBorder = null;*/

        private bool _isInitialized = false;

        public FormTimePicker()
        {
            DefaultStyleKey = typeof(FormTimePicker);
        }

        public FormVisualState VisualState { get; private set; }

        #region Mode*
        public FormEditMode Mode
        {
            get { return (FormEditMode)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        private static void ModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as FormTimePicker;
            control.UpdateMode();
            control.UpdateVisualState();
        }

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register(nameof(Mode), typeof(FormEditMode), typeof(FormTimePicker), new PropertyMetadata(FormEditMode.Auto, ModeChanged));
        #endregion

        protected override void OnApplyTemplate()
        {
            _contentGrid = base.GetTemplateChild("FlyoutButtonContentGrid") as Grid;
            /*_firstBorder = base.GetTemplateChild("FirstPickerHost") as Border;
            _secondBorder = base.GetTemplateChild("SecondPickerHost") as Border;
            _thirdBorder = base.GetTemplateChild("ThirdPickerHost") as Border;

            _hourInput = base.GetTemplateChild("HourTextBlock") as TextBlock;
            _minuteInput = base.GetTemplateChild("MinuteTextBlock") as TextBlock;
            _periodInput = base.GetTemplateChild("PeriodTextBlock") as TextBlock;*/

            _isInitialized = true;

            UpdateMode();
            UpdateVisualState();

            base.OnApplyTemplate();
        }

        protected override void OnTapped(TappedRoutedEventArgs e)
        {
            if (Mode == FormEditMode.Auto)
            {
                SetVisualState(FormVisualState.Focused);
            }

            base.OnTapped(e);
        }

        private void UpdateMode()
        {
            switch (Mode)
            {
                case FormEditMode.Auto:
                    VisualState = FormVisualState.Idle;
                    break;
                case FormEditMode.ReadWrite:
                    VisualState = FormVisualState.Ready;
                    break;
                case FormEditMode.ReadOnly:
                    VisualState = FormVisualState.Disabled;
                    break;
            }
        }

        public void SetVisualState(FormVisualState visualState)
        {
            if (Mode == FormEditMode.ReadOnly)
            {
                visualState = FormVisualState.Disabled;
            }

            if (visualState != VisualState)
            {
                VisualState = visualState;
                UpdateVisualState();
                VisualStateChanged?.Invoke(this, visualState);
            }
        }

        private void UpdateVisualState()
        {
            if (_isInitialized)
            {
                switch (VisualState)
                {
                    case FormVisualState.Idle:
                        /*_contentGrid.Opacity = 0.40;
                        _hourInput.Opacity = 1;
                        _minuteInput.Opacity = 1;
                        _periodInput.Opacity = 1;*/
                        _contentGrid.Background = TransparentBrush;
                        break;
                    case FormVisualState.Ready:
                        /*_contentGrid.Opacity = 1.0;
                        _hourInput.Opacity = 1;
                        _minuteInput.Opacity = 1;
                        _periodInput.Opacity = 1;*/
                        _contentGrid.Background = OpaqueBrush;
                        break;
                    case FormVisualState.Focused:
                        /*_contentGrid.Opacity = 1.0;
                        _hourInput.Opacity = 1;
                        _minuteInput.Opacity = 1;
                        _periodInput.Opacity = 1;*/
                        _contentGrid.Background = OpaqueBrush;
                        break;
                    case FormVisualState.Disabled:
                        /*_contentGrid.Opacity = 0.40;
                        _hourInput.Opacity = 1;
                        _minuteInput.Opacity = 1;
                        _periodInput.Opacity = 1;*/
                        _contentGrid.Background = TransparentBrush;
                        IsEnabled = false;
                        Opacity = 0.75;
                        break;
                }
            }
        }

        private readonly Brush TransparentBrush = new SolidColorBrush(Colors.Transparent);
        private readonly Brush OpaqueBrush = new SolidColorBrush(Colors.White);
    }
}
