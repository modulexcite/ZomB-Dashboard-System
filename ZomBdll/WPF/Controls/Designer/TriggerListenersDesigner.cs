﻿/*
 * ZomB Dashboard System <http://firstforge.wpi.edu/sf/projects/zombdashboard>
 * Copyright (C) 2011, Patrick Plenefisch and FIRST Robotics Team 451 "The Cat Attack"
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System451.Communication.Dashboard.WPF.Controls.Designer;

namespace System451.Communication.Dashboard.WPF.Controls.Designers
{
    public class TriggerListenersDesigner : DesignerBase
    {
        Label t;
        public override FrameworkElement GetProperyField()
        {
            var pan = new DockPanel();
            pan.Width = 110;//TODO: hard coded from Toolbox.xaml
            t = new Label();
            try
            {
                t.Content = GetValue();
            }
            catch { }//Null value
            var btn = new Button();
            btn.Content = "...";
            btn.Click += delegate
            {
                t.Content = new TriggerListenersDesignerWindow().UpdateTLs((t.Content??"").ToString());
            };
            DockPanel.SetDock(btn, Dock.Right);
            pan.ToolTip = t.Content;
            pan.Children.Add(btn);
            pan.Children.Add(t);
            return pan;
        }

        public override bool IsDefaultValue()
        {
            return GetValue() == "";
        }
    }
}
