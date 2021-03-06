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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace System451.Communication.Dashboard.ViZ
{
    /// <summary>
    /// Interaction logic for TransformDesigner.xaml
    /// </summary>
    public partial class TransformDesignerWindow : Window
    {
        private UIElement Object;
        bool init = false;
        public TransformDesignerWindow(UIElement ubject)
        {
            InitializeComponent();
            this.Object = SurfaceControl.GetSurfaceControlFromControl(ubject);
            if (Object.RenderTransform != MatrixTransform.Identity)
            {
                if (Object.RenderTransform is RotateTransform)
                {
                    FixedRotationDial.DoubleValue = (Object.RenderTransform as RotateTransform).Angle;
                    MainTabs.IsEnabled = true;
                }
                else if (Object.RenderTransform is TranslateTransform)
                {
                    MainTabs.IsEnabled = true;
                    TranslateTab.IsSelected = true;
                }
                else if (Object.RenderTransform is SkewTransform)
                {
                    MainTabs.IsEnabled = true;
                    SkewTab.IsSelected = true;
                    Xorigin.Text = (Object.RenderTransform as SkewTransform).CenterX.ToString();
                    Yorigin.Text = (Object.RenderTransform as SkewTransform).CenterY.ToString();
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FixedRotationDial.DataControlEnabled = true;
            bool cont = true;
            FixedRotationDial.DataUpdated += delegate
            {
                if (cont)
                {
                    (Object.RenderTransform as RotateTransform).Angle = Math.Round(FixedRotationDial.DoubleValue, 0);
                    cont = false;
                    FixedRotationDial.DoubleValue = Math.Round(FixedRotationDial.DoubleValue, 0);
                    cont = true;
                }
            };
            init = true;
        }

        private void endisable_transformers(object sender, RoutedEventArgs e)
        {
            if (Object.RenderTransform == MatrixTransform.Identity)
                Object.RenderTransform = new RotateTransform(0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //edit binding
            new BindingDesigner(Object.RenderTransform, Object.RenderTransform.GetType().GetProperty("Angle")
                , Designer.getDesigner().ZDash.Children).ShowDialog();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!init)
                return;
            if (e.AddedItems.Contains(FixedRotateTab) && Object.RenderTransform is RotateTransform)
                (Object.RenderTransform as RotateTransform).Angle = (Object.RenderTransform as RotateTransform).Angle;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Object.RenderTransform = MatrixTransform.Identity;
        }

        private void Transform_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!init)
                return;
            if (e.AddedItems.Contains(RotateTab) && !(Object.RenderTransform is RotateTransform))
                Object.RenderTransform = new RotateTransform(0);
            else if (e.AddedItems.Contains(TranslateTab) && !(Object.RenderTransform is TranslateTransform))
                Object.RenderTransform = new TranslateTransform(0, 0);
            else if (e.AddedItems.Contains(SkewTab) && !(Object.RenderTransform is SkewTransform))
                Object.RenderTransform = new SkewTransform(0, 0);
        }

        //set x
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new BindingDesigner(Object.RenderTransform, Object.RenderTransform.GetType().GetProperty("X")
                , Designer.getDesigner().ZDash.Children).ShowDialog();
        }

        private void SetYTransform_click(object sender, RoutedEventArgs e)
        {
            new BindingDesigner(Object.RenderTransform, Object.RenderTransform.GetType().GetProperty("Y")
                , Designer.getDesigner().ZDash.Children).ShowDialog();
        }

        private void SetSkewX_Click(object sender, RoutedEventArgs e)
        {
            new BindingDesigner(Object.RenderTransform, Object.RenderTransform.GetType().GetProperty("AngleX")
                , Designer.getDesigner().ZDash.Children).ShowDialog();
        }

        private void SetSkewY_Click(object sender, RoutedEventArgs e)
        {
            new BindingDesigner(Object.RenderTransform, Object.RenderTransform.GetType().GetProperty("AngleY")
                , Designer.getDesigner().ZDash.Children).ShowDialog();
        }

        private void Xorigin_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double i = double.Parse(Xorigin.Text);
                (Object.RenderTransform as SkewTransform).CenterX = i;
            }
            catch
            {
                Console.Beep();
            }
        }

        private void Yorigin_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                double i = double.Parse(Yorigin.Text);
                (Object.RenderTransform as SkewTransform).CenterY = i;
            }
            catch
            {
                Console.Beep();
            }
        }
    }
}
