using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace WinFormLibrary.Components
{
    public partial class ElipseControl : Component
    {
        public ElipseControl()
        {
            InitializeComponent();
        }

        public ElipseControl(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthElips,
            int nHeightElips);

        private Control control;

        public Control TargetControl
        {
            get { return control; }
            set
            {
                control = value;
                if (control != null)
                    control.SizeChanged += (o, e) => control.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, control.Width, control.Height, cornerRadius, cornerRadius));
            }
        }
        private int cornerRadius;

        public int CornerRadius
        {
            get { return cornerRadius; }
            set
            {
                cornerRadius = value;
                if (control != null)
                    control.SizeChanged += (o, e) => control.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, control.Width, control.Height, cornerRadius, cornerRadius));
            }
        }


    }
}
