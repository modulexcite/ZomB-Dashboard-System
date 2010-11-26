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
using System.IO;
using System.Windows;
using System;
using System.Diagnostics;

namespace System451.Communication.Dashboard.ViZ
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            LoadAssembliesGeneric();
            var args = e.Args;
            if (args.Length >0)
            {
                if (File.Exists(args[0]))
                    new Run(File.ReadAllText(args[0])).Show();
                else
                {
                    switch (args[0])
                    {
                        case "-extract":
                            Utils.InstallUtils.ExtractAll();
                            this.Shutdown();
                            break;
                        case "-install":
                            Utils.InstallUtils.NGen();
                            this.Shutdown();
                            break;
                        default:
                            MessageBox.Show("Invalid CLI arguments. Valid arguments:\r\n -extract  Extract all embedded dll's\r\n [fileName]  Run this Zaml file");
                            new Designer().Show();
                            break;
                    }
                }
            }
            else
                new Designer().Show();
        }

        private void LoadAssembliesGeneric()
        {
            foreach (var item in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    ResourceDictionary MyResourceDictionary = new ResourceDictionary();
     MyResourceDictionary.Source = new Uri("pack://application:,,,/"+item.FullName+";component/Themes/Generic.xaml");
     App.Current.Resources.MergedDictionaries.Add(MyResourceDictionary);
                }
                catch
                {
                    //must not have Themes/Generic.xaml
                }
            }
        }
    }
}
