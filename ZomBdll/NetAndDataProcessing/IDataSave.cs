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
using System.IO;

namespace System451.Communication.Dashboard
{
    public interface ISavableZomBData
    {
        /// <summary>
        /// Gets the value of the data as a MemoryStream
        /// </summary>
        MemoryStream DataValue { get; }

        /// <summary>
        /// Notifies the Saver when new data is present
        /// This should call a fast enqueue function to the string
        /// </summary>
        event EventHandler DataUpdated;
    }

    public interface IZomBDataSaver
    {
        void Add(ISavableZomBData DataSource, string file);
        void StartSave();
        void EndSave();
    }
}
