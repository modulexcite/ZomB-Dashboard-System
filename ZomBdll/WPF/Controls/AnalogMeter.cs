﻿/*
 * ZomB Dashboard System <http://firstforge.wpi.edu/sf/projects/zombdashboard>
 * Copyright (C) 2009-2010, Patrick Plenefisch and FIRST Robotics Team 451 "The Cat Attack"
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace System451.Communication.Dashboard.WPF.Controls
{
    /// <summary>
    /// Interaction logic for AnalogMeter.xaml
    /// </summary>
    [Design.ZomBControl("Analog Meter",
        Description = "This shows 0-1024, useful for analog inputs",
        IconName = "AnalogMeterIcon",
        TypeHints = ZomBDataTypeHint.Number)]
    [Design.ZomBDesignableProperty("Foreground")]
    [Design.ZomBDesignableProperty("Background")]
    [Design.ZomBDesignableProperty("BorderBrush")]
    [Design.ZomBDesignableProperty("BorderThickness")]
    [Design.ZomBDesignableProperty("DoubleValue", DisplayName = "Value")]
    public class AnalogMeter : ZomBGLControl, IMultiValueConverter, IZomBDataControl
    {
        static AnalogMeter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnalogMeter),
            new FrameworkPropertyMetadata(typeof(AnalogMeter)));
        }

        public AnalogMeter()
        {
            this.Background = Brushes.PaleGreen;
            this.Foreground = Brushes.DarkSlateGray;
            this.SnapsToDevicePixels = true;
            this.BorderBrush = Brushes.Black;
            this.BorderThickness = new Thickness(1.0);
            this.Width = 100;
            this.Height = 50;
        }

        static void MaxUpdated(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            AnalogMeter am = (o as AnalogMeter);
            am.DoubleValue = Math.Max(am.Min, Math.Min(am.DoubleValue, (double)e.OldValue));
            am.IntValue = (int)am.DoubleValue;
            am.StringValue = am.DoubleValue.ToString();
        }
        static void MinUpdated(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            AnalogMeter am = (o as AnalogMeter);
            am.DoubleValue = Math.Max((double)e.OldValue, Math.Min(am.DoubleValue, am.Max));
            am.IntValue = (int)am.DoubleValue;
            am.StringValue = am.DoubleValue.ToString();
        }

        [Design.ZomBDesignable(), Description("The maximum value we are going to get."), Category("Behavior")]
        public double Max
        {
            get { return (double)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Max.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register("Max", typeof(double), typeof(AnalogMeter), new UIPropertyMetadata(1024.0, new PropertyChangedCallback(AnalogMeter.MaxUpdated)));


        [Design.ZomBDesignable(), Description("The minimum value we are going to get."), Category("Behavior")]
        public double Min
        {
            get { return (double)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Min.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register("Min", typeof(double), typeof(AnalogMeter), new UIPropertyMetadata(0.0, new PropertyChangedCallback(AnalogMeter.MinUpdated)));

        public object Convert(object[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var ths = value[0] as AnalogMeter;
            if (ths == null)
                return null;
            return Math.Max(-90, Math.Min(((Math.Max(ths.Min, Math.Min((double)value[1], ths.Max)) - ths.Min) / (ths.Max - ths.Min) * 180) - 90, 90));
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        [Design.ZomBDesignerVerb("Set range to analog")]
        public void SetRangeToAnalog()
        {
            Min = 0;
            Max = 1024;
            DoubleValue = 0;
        }

        [Design.ZomBDesignerVerb("Set range to volts (0-5)")]
        public void SetRangeToAnalogV()
        {
            Min = 0;
            Max = 5;
            DoubleValue = 0;
        }

        [Design.ZomBDesignerVerb("Set range to degrees")]
        public void SetRangeToDegrees()
        {
            Min = -90;
            Max = 90;
            DoubleValue = 0;
        }

        [Design.ZomBDesignerVerb("Set range to normalized")]
        public void SetRangeToNormalized()
        {
            Min = -1;
            Max = 1;
            DoubleValue = 0;
        }

        #region IZomBDataControl Members

        public event ZomBDataControlUpdatedEventHandler DataUpdated;

        bool dce = false;
        public bool DataControlEnabled
        {
            get
            {
                return dce;
            }
            set
            {
                if (dce != value)
                {
                    dce = value;
                    if (dce)
                    {
                        this.MouseLeftButtonUp += AlertControl_MouseLeftButtonUp;
                        this.MouseMove += ValueMeter_MouseMove;
                        this.MouseLeftButtonDown += ValueMeter_MouseLeftButtonDown;
                    }
                    else
                    {
                        this.MouseLeftButtonUp -= AlertControl_MouseLeftButtonUp;
                        this.MouseMove -= ValueMeter_MouseMove;
                        this.MouseLeftButtonDown -= ValueMeter_MouseLeftButtonDown;
                    }
                }
            }
        }

        bool draggin = false;

        void ValueMeter_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            draggin = true;
            this.CaptureMouse();
        }

        void ValueMeter_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (draggin)
            {
                DoubleValue = Math.Max(this.Min, Math.Min(this.Max, PointToAngle(e.GetPosition(this)) / 180 * (this.Max - this.Min) + this.Min));
                if (DataUpdated != null)
                    DataUpdated(this, new ZomBDataControlUpdatedEventArgs(ControlName, DoubleValue.ToString()));
            }
        }
        double PointToAngle(Point p)
        {
            double r = Math.Atan((p.X - (this.ActualWidth / 2)) / ((this.ActualHeight * .87) - p.Y)) / Math.PI * 180;
            if (double.IsNaN(r))
                return 0;
            if (p.Y > (this.ActualHeight * .87))
            {
                if (p.X > (this.ActualWidth / 2))
                    return 180;
                return 0;
            }
            return r+90;
        }

        void AlertControl_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            draggin = false;
            ReleaseMouseCapture();
        }

        #endregion
    }
}
