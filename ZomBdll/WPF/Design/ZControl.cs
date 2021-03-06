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
using System.Windows.Media;
using System.Windows;

[assembly: System451.Communication.Dashboard.WPF.Design.ZomBZamlNamespace("System451.Communication.Dashboard.WPF.Controls", "ZomB")]

namespace System451.Communication.Dashboard.WPF.Design
{
    [global::System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class ZomBControlAttribute : Attribute
    {
        public ZomBControlAttribute(string name)
        {
            Name = name;
            Star = false;
        }

        public string Name { get; private set; }
        public string Description { get; set; }
        public Type Type { get; internal set; }
        public ZomBDataTypeHint TypeHints { get; set; }
        public ImageSource Icon { get; set; }
        public string IconName { get; set; }
        public bool Star { get; set; }

        public ZomBControlAttribute Clone()
        {
            return (ZomBControlAttribute)MemberwiseClone();
        }
    }

    [global::System.AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class ZomBDesignerVerbAttribute : Attribute
    {
        public ZomBDesignerVerbAttribute(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
        public string Description { get; set; }
    }

    [global::System.AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
    public sealed class ZomBZamlNamespaceAttribute : Attribute
    {
        public ZomBZamlNamespaceAttribute(string clrNamespace, string xmlNamespace)
        {
            this.ClrNamespace = clrNamespace;
            this.XmlNamespace = xmlNamespace;
        }

        public string ClrNamespace { get; private set; }
        public string XmlNamespace { get; private set; }
    }
}
