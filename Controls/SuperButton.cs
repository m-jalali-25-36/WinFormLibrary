using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace WinFormLibrary.Controls
{
    public partial class SuperButton : UserControl
    {
        private int cornerRadius = 10;
        public SuperButton()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            //using (var gp = new GraphicsPath())
            //{
            //    gp.AddEllipse(new Rectangle(0, 0, this.Width - 1, this.Height - 1));

            //    this.Region = new Region(gp);
            //}
            float penWidth = 5F;
            Pen myPen = new Pen(Color.Black, penWidth);
            e.Graphics.DrawEllipse(myPen, new RectangleF(new PointF(-2, -2), new
            SizeF((float)(this.Width + 2), this.Height + 2)));
            var graphicsObj = this.CreateGraphics();
            graphicsObj.DrawLine(myPen, 20, 20, 200, 210);
            myPen.Dispose();
            //this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, cornerRadius, cornerRadius));
            //Region.GetBounds(graphicsObj);
            System.Drawing.Drawing2D.GraphicsPath buttonPath =
        new System.Drawing.Drawing2D.GraphicsPath();

            // Set a new rectangle to the same size as the button's 
            // ClientRectangle property.
            System.Drawing.Rectangle newRectangle = this.ClientRectangle;
            var q = Region.FromHrgn(CreateRoundRectRgn(0, 0, this.Width, this.Height, cornerRadius, cornerRadius));

            // Decrease the size of the rectangle.
            //newRectangle.Inflate(-10, -10);

            // Draw the button's border.
            //e.Graphics.DrawEllipse(System.Drawing.Pens.Blue, newRectangle);
            e.Graphics.DrawLines(Pens.Blue, new Point[] { new Point(0, 0), new Point(Width, 0), new Point(0, 0), new Point(0, Height), new Point(Width, Height), new Point(0, Height), new Point(Width, Height), new Point(Width, 0) });
            // Increase the size of the rectangle to include the border.
            //newRectangle.Inflate(1, 1);

            // Create a circle within the new rectangle.
            buttonPath.AddEllipse(newRectangle);
            buttonPath.AddLine(new Point(0, 0), new Point(Width, 0));
            //System.Windows.Forms.ControlPaint.DrawBorder(e.Graphics, newRectangle,
            //Color.Blue, 4, ButtonBorderStyle.Solid
            //, Color.Blue, 4, ButtonBorderStyle.Solid
            //, Color.Blue, 4, ButtonBorderStyle.Solid
            //, Color.Blue, 4, ButtonBorderStyle.Solid);
            //ControlPaint.DrawBorder3D(e.Graphics, this.ClientRectangle,
            //                Color.Black,2, ButtonBorderStyle.Solid);
            // Set the button's Region property to the newly created 
            // circle region.
            this.Region = new System.Drawing.Region(buttonPath);
        }


        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthElips,
            int nHeightElips);
    }
}
