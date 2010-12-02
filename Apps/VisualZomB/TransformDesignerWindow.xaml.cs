﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
                FixedRotationDial.DoubleValue = (Object.RenderTransform as RotateTransform).Angle;
                MainTabs.IsEnabled = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FixedRotationDial.DataControlEnabled = true;
            FixedRotationDial.DataUpdated += delegate
            {
                (Object.RenderTransform as RotateTransform).Angle = FixedRotationDial.DoubleValue;
            };
            init = true;
        }

        private void endisable_transformers(object sender, RoutedEventArgs e)
        {
            if (Object.RenderTransform== MatrixTransform.Identity)
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
    }
}
