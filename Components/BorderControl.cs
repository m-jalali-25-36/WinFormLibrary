using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormLibrary.Components
{
    public partial class BorderControl : Component
    {
        public BorderControl()
        {
            InitializeComponent();
        }

        public BorderControl(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private int lineWidth;

        public int LineWidth
        {
            get { return lineWidth; }
            set { lineWidth = value; }
        }

        private Color lineColor;

        public Color LineColor
        {
            get { return lineColor; }
            set { lineColor = value; }
        }

        private ButtonBorderStyle borderStyle;

        public ButtonBorderStyle BorderStyle
        {
            get { return borderStyle; }
            set { borderStyle = value; }
        }




        private Control targetControl;

        public Control TargetControl
        {
            get { return targetControl; }
            set
            {
                targetControl = value;
                if (targetControl != null)
                    targetControl.Paint += control_Paint;
            }
        }

        private void control_Paint(object o, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, targetControl.ClientRectangle,
                lineColor, lineWidth, borderStyle,
                lineColor, lineWidth, borderStyle,
                lineColor, lineWidth, borderStyle,
                lineColor, lineWidth, borderStyle);
        }

    }
}
