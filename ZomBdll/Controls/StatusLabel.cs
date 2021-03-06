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
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System451.Communication.Dashboard.Controls
{
    /// <summary>
    /// A ZomB control that displays text at different locations
    /// </summary>
    [ToolboxBitmap(typeof(Label)), Designer(typeof(Design.StatusLabelDesigner))]
    public class StatusLabel : Label, IZomBControlGroup
    {
        ZomBControlCollection innerCollection = new ZomBControlCollection();
        ZomBRemoteControl label = new ZomBRemoteControl();
        ZomBRemoteControl xmove = new ZomBRemoteControl();
        ZomBRemoteControl ymove = new ZomBRemoteControl();
        Timer t;
        Point oldPoint;
        Size oldSize;

        /// <summary>
        /// Creates a new status Label
        /// </summary>
        public StatusLabel()
        {
            this.DoubleBuffered = true;
            LabelName = "lbl1";
            label.ControlUpdated += new ControlUpdatedDelegate(label_ControlUpdated);
            XMoveName = "lblx1";
            xmove.ControlUpdated += new ControlUpdatedDelegate(xmove_ControlUpdated);
            YMoveName = "lbly1";
            ymove.ControlUpdated += new ControlUpdatedDelegate(ymove_ControlUpdated);
            innerCollection.Add(label);
            this.XMax = 100f;
            this.XMin = 0f;
            this.XVMax = 100f;
            this.XVMin = 0f;
            this.YMax = 100f;
            this.YMin = 0f;
            this.YVMax = 100f;
            this.YVMin = 0f;


            this.YValue = 0f;
            this.XValue = 0f;


            if (this.DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                this.LocationChanged += new EventHandler(StatusLabel_LocationChanged);
                this.Width = 100;
                this.Height = 100;
                this.Resize += new EventHandler(StatusLabel_Resize);
                base.AutoSize = false;
                t = new Timer();
                t.Interval = 100;
                t.Tick += new EventHandler(t_Tick);
            }
            else

                base.AutoSize = true;
        }

        void t_Tick(object sender, EventArgs e)
        {
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                if (oldSize != this.Size)
                {
                    StatusLabel_Resize(sender, e);
                    oldSize = this.Size;
                }
                if (oldPoint != this.Location)
                {
                    StatusLabel_LocationChanged(sender, e);
                    oldPoint = this.Location;
                }
            }
        }

        void StatusLabel_Resize(object sender, EventArgs e)
        {
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                Size nl = this.Size;
                XMax = nl.Width + XMin;
                YMax = nl.Height + YMin;
            }
        }
        protected override Size DefaultSize
        {
            get
            {
                return new Size(100, 100);
            }
        }

        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                base.AutoSize = !(DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime);
            }
        }

        void StatusLabel_SizeChanged(object sender, EventArgs e)
        {
            if (DesignMode || LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                Size nl = this.Size;
                XMax = nl.Width + XMin;
                YMax = nl.Height + YMin;
            }
        }

        [DefaultValue("Resize this to change the range of the label")]
        public override string Text
        {
            get
            {
                if (DesignMode)
                    return "Resize this to change the range of the label";
                else
                    return base.Text;
            }
            set
            {
                if (value == "Resize this to change the range of the label" || DesignMode)
                    return;
                //MessageBox.Show("Change this programaticaly, not at design time!");
                else
                    base.Text = value;
            }
        }

        void StatusLabel_LocationChanged(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                float difx, dify;
                difx = XMax - XMin;
                dify = YMax - YMin;
                Point nl = this.Location;
                XMax = (XMin = nl.X) + difx;//Assign the new X and Y (in a very consice manner!)
                YMax = (YMin = nl.Y) + dify;//YMin is the new y point, YMax is the new Y point plus the old difference
            }
        }

        void ymove_ControlUpdated(object sender, ZomBControlUpdatedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ControlUpdatedDelegate(ymove_ControlUpdated), sender, e);
            }
            else
                //Scale virtual to real
                this.Location = new Point(this.Location.X, (int)((1 - (float.Parse(e.Value) - YVMin) / (YVMax - YVMin)) * (YMax - YMin) - YMin));
        }

        void xmove_ControlUpdated(object sender, ZomBControlUpdatedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ControlUpdatedDelegate(xmove_ControlUpdated), sender, e);
            }
            else
                //Scale virtual to real
                this.Location = new Point((int)(((float.Parse(e.Value) - XVMin) / (XVMax - XVMin)) * (XMax - XMin) + XMin), this.Location.Y);
        }

        void label_ControlUpdated(object sender, ZomBControlUpdatedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ControlUpdatedDelegate(label_ControlUpdated), sender, e);
            }
            else
                this.Text = e.Value;
        }

        /// <summary>
        /// The name of this control
        /// </summary>
        [DefaultValue("lbl1"), Category("ZomB"), Description("The name of this control")]
        public string LabelName
        {
            get { return label.ControlName; }
            set { label.ControlName = value; }
        }

        /// <summary>
        /// The name of the x axis movement control
        /// </summary>
        [DefaultValue("lblx1"), Category("ZomB"), Description("The name of the x axis movement control")]
        public string XMoveName
        {
            get { return xmove.ControlName; }
            set { xmove.ControlName = value; }
        }

        /// <summary>
        /// The name of the y axis movement control
        /// </summary>
        [DefaultValue("lbly1"), Category("ZomB"), Description("The name of the y axis movement control")]
        public string YMoveName
        {
            get { return ymove.ControlName; }
            set { ymove.ControlName = value; }
        }

        /// <summary>
        /// Can this control move sideways (x axis)
        /// </summary>
        [DefaultValue(false), Category("ZomB"), Description("Can this control move sideways (x axis)")]
        public bool MovesOnX
        {
            get { return innerCollection.Contains(xmove); }
            set
            {
                if (value && (!innerCollection.Contains(xmove)))
                {
                    innerCollection.Add(xmove);
                }
                else if ((!value) && (innerCollection.Contains(xmove)))
                {
                    innerCollection.Remove(xmove);
                }
            }
        }

        /// <summary>
        /// Can this control move up and down (y axis)
        /// </summary>
        [DefaultValue(false), Category("ZomB"), Description("Can this control move up and down (y axis)")]
        public bool MovesOnY
        {
            get { return innerCollection.Contains(ymove); }
            set
            {
                if (value && (!innerCollection.Contains(ymove)))
                {
                    innerCollection.Add(ymove);
                }
                else if ((!value) && (innerCollection.Contains(ymove)))
                {
                    innerCollection.Remove(ymove);
                }
            }
        }

        /// <summary>
        /// The controls physical Y Min
        /// </summary>
        [DefaultValue(0f), Category("ZomB"), Description("The controls physical Y Min")]
        public float YMin
        {
            get;
            set;
        }
        /// <summary>
        /// The controls physical Y Max
        /// </summary>
        [DefaultValue(100f), Category("ZomB"), Description("The controls physical Y Max")]
        public float YMax
        {
            get;
            set;
        }
        /// <summary>
        /// The controls virtual Y Min
        /// </summary>
        [DefaultValue(0f), Category("ZomB"), Description("The controls virtual Y Min")]
        public float YVMin
        {
            get;
            set;
        }

        /// <summary>
        /// The controls virtual Y Max
        /// </summary>
        [DefaultValue(100f), Category("ZomB"), Description("The controls virtual Y Max")]
        public float YVMax
        {
            get;
            set;
        }

        /// <summary>
        /// The controls physical X Min
        /// </summary>
        [DefaultValue(0f), Category("ZomB"), Description("The controls physical X Min")]
        public float XMin
        {
            get;
            set;
        }

        /// <summary>
        /// The controls physical X Max
        /// </summary>
        [DefaultValue(100f), Category("ZomB"), Description("The controls physical X Max")]
        public float XMax
        {
            get;
            set;
        }

        /// <summary>
        /// The controls virtual X Min
        /// </summary>
        [DefaultValue(0f), Category("ZomB"), Description("The controls virtual X Min")]
        public float XVMin
        {
            get;
            set;
        }

        /// <summary>
        /// The controls virtual X Max
        /// </summary>
        [DefaultValue(100f), Category("ZomB"), Description("The controls virtual X Max")]
        public float XVMax
        {
            get;
            set;
        }

        /// <summary>
        /// The controls X value
        /// </summary>
        [DefaultValue(0f), Category("ZomB"), Description("The controls X value")]
        public float XValue
        {
            get
            {
                return ((this.Location.X - XMin) / (XMax - XMin)) * (XVMax - XVMin) + XVMin;
            }
            set
            {
                //use the mech to be true
                xmove_ControlUpdated(this, new ZomBControlUpdatedEventArgs(value.ToString()));
            }
        }

        /// <summary>
        /// The controls Y value
        /// </summary>
        [DefaultValue(0f), Category("ZomB"), Description("The controls Y value")]
        public float YValue
        {
            get
            {
                return 1 - ((this.Location.Y - YMin) / (YMax - YMin)) * (YVMax - YVMin) - YVMin;
            }
            set
            {
                //use the mech to be true
                ymove_ControlUpdated(this, new ZomBControlUpdatedEventArgs(value.ToString()));
            }
        }

        #region IZomBControlGroup Members

        ZomBControlCollection IZomBControlGroup.GetControls()
        {
            return innerCollection;
        }

        void IZomBControlGroup.ControlAdded(object sender, ZomBControlAddedEventArgs e)
        {

        }

        #endregion
    }
    namespace Design
    {
        /// <summary>
        /// Designer for the StatusLabel
        /// </summary>
        /// <see cref="StatusLabel"/>
        internal class StatusLabelDesigner : ControlDesigner
        {
            public StatusLabelDesigner()
            {

            }

            public override DesignerVerbCollection Verbs
            {
                get
                {
                    return new DesignerVerbCollection(new DesignerVerb[] 
            {
                new DesignerVerb("AutoSize Height", new EventHandler(OnAutoSizeHeight))
            });
                }
            }
            private void OnAutoSizeHeight(object sender, EventArgs e)
            {
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(this.Component);
                Font f = (Font)props["Font"].GetValue(this.Component);
                props["Height"].SetValue(this.Component, f.Height);
            }
        }
    }
}
