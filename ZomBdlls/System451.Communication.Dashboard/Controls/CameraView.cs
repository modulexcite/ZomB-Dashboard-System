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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SmashTcpDashboard;
using System.IO;
using System.Collections.ObjectModel;

namespace System451.Communication.Dashboard
{
    public partial class CameraView : UserControl, ISavableZomBData, IZomBControlGroup
    {
        ZomBControlCollection targs = new ZomBControlCollection();
        Image view;
        Receiver Receiver;
        delegate void UpdaterDelegate();

        public CameraView()
        {
            view = new Bitmap(10, 10);
            EnableAutoReset = false;
            InitializeComponent();
            if (!this.DesignMode)
            {
                Receiver = null;
                //Receiver.OnImageUpdate += new Receiver.ImageUpdate(Receiver_OnImageUpdate);
                //Start();// we don't know the IP
                timer1.Enabled = false;
            }
            else
            {
                timer1.Enabled = false;
            }
        }
        private string ip = "10.0.0.2";
        [Category("ZomB"), Description("Target IP (10.xx.xx.2, where xxxx is your team number)")]
        public string IPAddress
        {
            get { return ip; }
            set
            {
                ip = value;
                if (!DesignMode)
                {
                    
                    if (Receiver != null)
                    {
                        Receiver.Stop();
                    }
                    Receiver = new TcpReceiver(ip);
                    Receiver.OnImageUpdate += new Receiver.ImageUpdate(Receiver_OnImageUpdate);
                    Start();
                }
            }
        }
        [Category("ZomB"), Description("Auto Reset the Camera"), DefaultValue(false)]
        public bool EnableAutoReset { get; set; }
        public void Restart()
        {
            Stop();
            Start();
        }


        public void Start()
        {
            if (!this.DesignMode && Receiver != null && this.CanFocus)
            {
                Receiver.Start();
                timer1.Enabled = EnableAutoReset;
            }
        }
        public void Stop()
        {
            Receiver.Stop();
        }

        void Receiver_OnImageUpdate()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new UpdaterDelegate(Receiver_OnImageUpdate));
            }
            else
            {
                
                view = Bitmap.FromStream(new MemoryStream(Receiver.ImageData));
                if (this.dataUpdatedEvent != null)
                    dataUpdatedEvent(this, new EventArgs());
                this.Invalidate();

                //ZomBSaver takes care of this now
                //File.WriteAllBytes(@"C:\Program Files\FRC Dashboard\img\ZomEye"+(calls++)+".jpg", Receiver.ImageData);
            }
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(640, 480);
            }
        }
        private void UserControl2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(view, 0, 0, this.Width, this.Height);
            if (this.Targets.Count > 0)
            {
                e.Graphics.ScaleTransform((this.Width / view.Width), (this.Height / view.Height));
                using (Pen pn = new Pen(TargetColor, TargetWidth))
                {
                    pn.LineJoin = System.Drawing.Drawing2D.LineJoin.Round;

                    using (Brush br = new SolidBrush(TargetFillColor))
                    {
                        foreach (KeyValuePair<string,IZomBControl> ttarget in Targets)
                        {
                            TargetInfo target = (TargetInfo)((object)ttarget);
                            if (!target.Target.IsEmpty)
                            {
                                PointF[] pts = new PointF[] { new PointF(target.Target.Top, target.Target.Left),
                                new PointF(target.Target.Top, target.Target.Right),
                                new PointF(target.Target.Bottom, target.Target.Right),
                                new PointF(target.Target.Bottom, target.Target.Left)};

                                e.Graphics.FillPolygon(br, pts);

                                e.Graphics.DrawPolygon(pn, pts);
                            }
                        }

                    }
                }
            }
        }
        private Color boxColor = Color.Green;
        [DefaultValue(typeof(Color), "Green"), Category("ZomB"), Description("Target bounding box color")]
        public Color TargetColor
        {
            get { return boxColor; }
            set { boxColor = value; }
        }

        private float width = 2.5f;
        [DefaultValue(2.5f), Category("ZomB"), Description("Target bounding box width")]
        public float TargetWidth
        {
            get { return width; }
            set { width = value; }
        }

        private Color fillColor = Color.FromArgb(100, Color.Green);
        [DefaultValue(typeof(Color), "Color [A=100, R=0, G=127, B=0]"), Category("ZomB"), Description("Target bounding box color")]
        public Color TargetFillColor
        {
            get { return fillColor; }
            set { fillColor = value; }
        }

        [DefaultValue(true), Category("ZomB"), Description("Show the Auto-Reset check box?")]
        public bool ShowAutoReset
        {
            get { return checkBox1.Visible; }
            set { checkBox1.Visible = value; }
        }

        [Category("ZomB"), Description("The Targets")]
        public ZomBControlCollection Targets
        {
            get { return targs; }
        }

        // ticks every 1750ms
        // checks if the video has timeed out
        // also (re)starts the video stream
        private void timer1_Tick(object sender, EventArgs e)
        {

            try
            {
                if (Receiver != null)
                {
                    if (!Receiver.Running)
                    {
                        Start();
                    }
                }
                else
                {
                }
            }
            catch
            {
            }

        }


        #region ISavableZomBData Members

        TypeConverter ISavableZomBData.GetTypeConverter()
        {
             return new BitmapConverter();
        }

        string ISavableZomBData.DataValue
        {
            get { return (new BitmapConverter()).ConvertToString(this.view); }
        }
        private EventHandler dataUpdatedEvent;

        event EventHandler ISavableZomBData.DataUpdated
        {
            add { dataUpdatedEvent += value; }
            remove { dataUpdatedEvent -= value; }
        }
        #endregion

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = EnableAutoReset = checkBox1.Checked;
        }

        #region IZomBControlGroup Members

        ZomBControlCollection IZomBControlGroup.GetControls()
        {
            return this.targs;
        }

        #endregion
    }
    public class TargetInfo : IZomBControl
    {

        public TargetInfo()
        {

        }
        public TargetInfo(string parm)
        {
            ControlName = parm;
        }

        private RectangleF itarget;
        public RectangleF Target
        {
            get { return itarget; }
            set { itarget = value; }
        }




        private RectangleF RectParse(string value)
        {
            RectangleF rf = new RectangleF();
            if (value != null && value != "")
            {
                //format widthxheigth+xpos,ypos
                rf.Width = float.Parse(value.Substring(0, value.IndexOf('x')));
                rf.Height = float.Parse(value.Substring(value.IndexOf('x') + 1, (value.IndexOf('+') - (value.IndexOf('x') + 1))));
                rf.X = float.Parse(value.Substring(value.IndexOf('+') + 1, (value.IndexOf(',') - (value.IndexOf('+') + 1))));
                rf.Y = float.Parse(value.Substring(value.IndexOf(',') + 1));
            }
            return rf;
        }



        #region IZomBControl Members
        public bool IsMultiWatch
        {
            get { return false; }
        }

        public string ControlName
        {
            get;
            set;
        }

        public void UpdateControl(string value)
        {
            Target = RectParse(value);

        }

        public void ControlAdded(object sender, ZomBControlAddedEventArgs e)
        {
            
        }

        #endregion
    }
}
