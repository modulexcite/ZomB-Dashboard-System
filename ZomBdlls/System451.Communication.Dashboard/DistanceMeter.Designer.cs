﻿namespace System451.Communication.Dashboard
{
    partial class DistanceMeter
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DistanceMeter
            // 
            this.DoubleBuffered = true;
            this.Name = "DistanceMeter";
            this.Size = new System.Drawing.Size(100, 150);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.DistanceMeter_Paint);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
