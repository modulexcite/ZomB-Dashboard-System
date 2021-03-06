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
using System.Drawing;
using System.Windows.Forms;
using System451.Communication.Dashboard.Properties;

namespace System451.Communication.Dashboard.Controls
{
    [ToolboxBitmap(typeof(icofinds), "System451.Communication.Dashboard.TBB.Taco.png")]
    public partial class TacoMeter : ZomBControl
    {
        float speedval = 0;

        public TacoMeter()
        {
            InitializeComponent();
            speedval = 0;
            ControlName = "taco";
        }

        [DefaultValue("0"), Category("ZomB"), Description("The Value of the taco Meter")]
        public float Value
        {
            get
            {
                return speedval;
            }
            set
            {
                speedval = value;
                this.Invalidate();
            }
        }

        public override void UpdateControl(ZomBDataObject value)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Utils.ZomBDataFunction(UpdateControl), value);
            }
            else
            {
                this.Value = float.Parse(value);
            }
        }

        private void TacoMeter_Paint(object sender, PaintEventArgs e)
        {
            if (Math.Abs(Value) > 1)
            {
                if (Value == -999.99)
                {
                }
                else
                {
                    Value = (Value < -1) ? -1 : 1;
                }
            }

            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bicubic;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.DrawImage(Resources.TacoShell, 0, 0, this.Width, this.Height);
            e.Graphics.ScaleTransform((float)this.Width / 400f, (float)this.Height / 300f);

            using (TextureBrush fullTaco = new TextureBrush(Resources.TacoFull_small21))
                e.Graphics.FillPie(fullTaco, 0f, 0f, 400f, 434f, -180f, (Value * 90f) + 90);
        }
    }
}
