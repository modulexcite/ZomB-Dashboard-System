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
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace System451.Communication.Dashboard.WPF.Controls
{
    public class DashboardDataCanvas : Canvas, IZomBDashboardDataHubConsumer
    {
        public DashboardDataCanvas()
        {
            DashboardDataHub = new DashboardDataHub();
            AutoStart = true;
            this.Loaded += delegate
            {
                ReloadControls();
                if (AutoStart)
                    Start();
            };
        }

        protected DashboardDataCanvas(bool init)
        {
            if (init)
                DashboardDataHub = new DashboardDataHub();
            this.Loaded += delegate
            {
                ReloadControls();
                if (AutoStart)
                    Start();
            };
        }

        ~DashboardDataCanvas()
        {
            if (DashboardDataHub != null)
                DashboardDataHub.Stop();
        }

        public DashboardDataHub DashboardDataHub { get; set; }

        /// <summary>
        /// Start the DashboardDataHub when we load the form?
        /// </summary>
        public bool AutoStart { get; set; }

        /// <summary>
        /// Re-iterate and find all controls
        /// </summary>
        public void ReloadControls()
        {
            AddControls(this.Children);
        }

        /// <summary>
        /// Start the DashboardDataHub
        /// </summary>
        public void Start()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                DashboardDataHub.Start();
            }
        }

        /// <summary>
        /// Restart the DashboardDataHub
        /// </summary>
        public void Restart()
        {
            Stop();
            Start();
        }

        /// <summary>
        /// Stop the DashboardDataHub
        /// </summary>
        public void Stop()
        {
            DashboardDataHub.Stop();
        }

        /// <summary>
        /// What the DDH will load as sources when it start()'s
        /// </summary>
        [Category("ZomB"), Description("What the DDH will load as sources when it start()'s")]
        public StartSources DefaultSources
        {
            get
            {
                if (DashboardDataHub != null)
                    return DashboardDataHub.StartSource;
                return hiddends;
            }
            set
            {
                if (DashboardDataHub != null)
                    DashboardDataHub.StartSource = value;
                else
                    hiddends = value;
            }
        }
        StartSources hiddends;

        /// <summary>
        /// What to do when an invalid packet is recieved
        /// </summary>
        [Category("ZomB"), Description("What the DDH will do when an invalid packet is recieved")]
        public InvalidPacketActions InvalidPacketAction
        {
            get
            {
                if (DashboardDataHub != null)
                    return DashboardDataHub.InvalidPacketAction;
                return hiddenipa;
            }
            set
            {
                if (DashboardDataHub != null)
                    DashboardDataHub.InvalidPacketAction = value;
                else
                    hiddenipa = value;
            }
        }
        InvalidPacketActions hiddenipa;

        /// <summary>
        /// Your team number
        /// </summary>
        [Category("ZomB"), Description("Your team number. Used for the camera and TCP connection auto configure.")]
        public int Team
        {
            get
            {
                if (DashboardDataHub != null)
                    return DashboardDataHub.Team;
                return hiddent;
            }
            set
            {
                if (DashboardDataHub != null)
                    DashboardDataHub.Team = value;
                else
                    hiddent = value;
            }
        }
        int hiddent;

        //make wpf happy, and me unhappy
        public static readonly DependencyProperty DefaultSourcesProperty =
            DependencyProperty.Register("DefaultSources", typeof(StartSources), typeof(DashboardDataCanvas), new UIPropertyMetadata(
                new PropertyChangedCallback((s, e) => (s as DashboardDataCanvas).DefaultSources = (StartSources)e.NewValue)));

        public static readonly DependencyProperty InvalidPacketActionProperty =
                    DependencyProperty.Register("InvalidPacketAction", typeof(InvalidPacketActions), typeof(DashboardDataCanvas), new UIPropertyMetadata(
                        new PropertyChangedCallback((s, e) => (s as DashboardDataCanvas).InvalidPacketAction = (InvalidPacketActions)e.NewValue)));

        public static readonly DependencyProperty TeamProperty =
                    DependencyProperty.Register("Team", typeof(InvalidPacketActions), typeof(DashboardDataCanvas), new UIPropertyMetadata(
                        new PropertyChangedCallback((s, e) => (s as DashboardDataCanvas).Team = (int)e.NewValue)));



        private void AddControls(IEnumerable controlCollection)
        {
            foreach (UIElement item in controlCollection)
            {
                if (item is IZomBControl)
                {
                    DashboardDataHub.Add((IZomBControl)item);
                }

                if (item is IZomBControlGroup)
                {
                    DashboardDataHub.Add((IZomBControlGroup)item);
                }
                if (item is IZomBMonitor)
                {
                    DashboardDataHub.Add((IZomBMonitor)item);
                }

                try
                {
                    //If panel or has other controls, find those
                    foreach (UIElement e in LogicalTreeHelper.GetChildren(item))
                    {
                        AddControls(LogicalTreeHelper.GetChildren(item));
                        break;
                    }
                }
                catch { }
            }
        }
    }
}
