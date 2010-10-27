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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Forms;

namespace System451.Communication.Dashboard
{
    namespace WPF.Design
    {
        public static class Reflector
        {
            public static IEnumerable<Type> GetZomBDesignableClasses()
            {
                bool tried = false;
                retry:
                try
                {
                    var retTypes = new List<Type>();
                    var asms = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (var asm in asms)
                    {
                        var types = asm.GetTypes();
                        foreach (var type in types)
                        {
                            foreach (var atr in type.GetCustomAttributes(typeof(ZomBControlAttribute), false))
                            {
                                retTypes.Add(type);
                                break;
                            }
                        }
                    }
                    return retTypes;
                }
                catch (Exception ex)
                {
                    if (!tried)
                    {
                        //need to load modules
                        Utils.AutoExtractor.Extract(System451.Communication.Dashboard.Utils.AutoExtractor.Files.All);
                        tried = true;
                        goto retry;
                    }
                    else
                    {
                        //darn, need to restart
                        if (System.Windows.Forms.MessageBox.Show("An exception occured while searching for controls. This may be caused by a failure to properly load modules. If this is the first time you get this error message, it is reccomended to restart. Would you like to restart?"
                            , "Error", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Error, System.Windows.Forms.MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
                        {
                            Application.Restart();
                        }
                        else
                        {
                            MessageBox.Show("Aborting: "+ex.ToString());
                            Application.Exit();
                        }
                    }
                    return null;
                }
            }

            public static IEnumerable<ZomBControlAttribute> GetZomBDesignableInfos(IEnumerable<Type> types)
            {
                return (from t in types
                        let info = (t.GetCustomAttributes(typeof(ZomBControlAttribute), false)[0] as ZomBControlAttribute)
                        where (((info.Type = t) != null) &&
                        ((info.Icon = (ImageSource)System.Windows.Application.Current.FindResource(info.IconName ?? "DefaultControlImage")) != null))
                        orderby info.Name
                        select info);
            }

            public static object Inflate(Type t)
            {
                return t.GetConstructor(Type.EmptyTypes).Invoke(null);
            }
        }
    }
}
