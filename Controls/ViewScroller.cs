using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormLibrary.Controls
{
    public partial class ViewScroller : UserControl
    {
        private int scrollStep = 2;
        private int buttonWidth = 19;
        private bool scroll = false;
        private int step;
        private bool left;
        private int minStep = 1;
        private int maxStep = 20;
        private int mousePosition;

        public ViewScroller()
        {
            InitializeComponent();
            this.MouseWheel += this.ViewScroller_Scroll;
            base.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Clear();
            this.btnLeft.MouseDown += new MouseEventHandler(this.btnLeft_MouseDown);
            this.btnLeft.MouseUp += new MouseEventHandler(this.btnLeft_MouseUp);
            this.btnRight.MouseDown += new MouseEventHandler(this.btnRight_MouseDown);
            this.btnRight.MouseUp += new MouseEventHandler(this.btnRight_MouseUp);
            this.flp1.ControlAdded += this.flpLocationSet;
            this.flp1.ControlRemoved += this.flpLocationSet;
        }

        public new RightToLeft RightToLeft
        {
            get => base.RightToLeft;
            set
            {
                base.RightToLeft = value;
                if (base.RightToLeft == RightToLeft.Yes)
                {
                    ComponentResourceManager resources = new ComponentResourceManager(typeof(ViewScroller));
                    object obj = resources.GetObject("btnLeft.Image");
                    if (obj != null)
                        this.btnRight.Image = (Image)obj;
                    obj = resources.GetObject("btnRight.Image");
                    if (obj != null)
                        this.btnLeft.Image = (Image)obj;
                    this.btnRight.MouseDown += new MouseEventHandler(this.btnLeft_MouseDown);
                    this.btnRight.MouseUp += new MouseEventHandler(this.btnLeft_MouseUp);
                    this.btnLeft.MouseDown += new MouseEventHandler(this.btnRight_MouseDown);
                    this.btnLeft.MouseUp += new MouseEventHandler(this.btnRight_MouseUp);
                    this.btnLeft.MouseDown -= new MouseEventHandler(this.btnLeft_MouseDown);
                    this.btnLeft.MouseUp -= new MouseEventHandler(this.btnLeft_MouseUp);
                    this.btnRight.MouseDown -= new MouseEventHandler(this.btnRight_MouseDown);
                    this.btnRight.MouseUp -= new MouseEventHandler(this.btnRight_MouseUp);
                    flpLocationSet(null, null);
                    this.SizeChanged += this.flpLocationSet;
                    this.panel1.SizeChanged += this.flpLocationSet;
                }
                else
                {
                    ComponentResourceManager resources = new ComponentResourceManager(typeof(ViewScroller));
                    object obj = resources.GetObject("btnLeft.Image");
                    if (obj != null)
                        this.btnLeft.Image = (Image)obj;
                    obj = resources.GetObject("btnRight.Image");
                    if (obj != null)
                        this.btnRight.Image = (Image)obj;
                    this.btnRight.MouseDown -= new MouseEventHandler(this.btnLeft_MouseDown);
                    this.btnRight.MouseUp -= new MouseEventHandler(this.btnLeft_MouseUp);
                    this.btnLeft.MouseDown -= new MouseEventHandler(this.btnRight_MouseDown);
                    this.btnLeft.MouseUp -= new MouseEventHandler(this.btnRight_MouseUp);
                    this.btnLeft.MouseDown += new MouseEventHandler(this.btnLeft_MouseDown);
                    this.btnLeft.MouseUp += new MouseEventHandler(this.btnLeft_MouseUp);
                    this.btnRight.MouseDown += new MouseEventHandler(this.btnRight_MouseDown);
                    this.btnRight.MouseUp += new MouseEventHandler(this.btnRight_MouseUp);
                    this.SizeChanged -= this.flpLocationSet;
                    this.panel1.SizeChanged -= this.flpLocationSet;
                }
            }
        }

        public int ButtonWidth
        {
            get { return buttonWidth; }
            set
            {
                buttonWidth = value;
                btnLeft.Height = value;
                btnRight.Height = value;
            }
        }

        public int ScrollStep
        {
            get => scrollStep;
            set
            {
                if (value > 0)
                    scrollStep = value;
            }
        }

        public new ControlCollection Controls { get => flp1.Controls; }

        public void AddControl(Control control)
        {
            flp1.Controls.Add(control);
        }

        public bool FocusControl(Control control)
        {
            try
            {
                if (!flp1.Controls.Contains(control))
                    return false;
                var con = flp1.Controls[flp1.Controls.IndexOf(control)];
                if (con == null)
                    return false;
                if (con.Location.X > panel1.Width - flp1.Location.X)
                {
                    flp1.Location = new Point(panel1.Width - con.Location.X - con.Width, 0);
                }
                else if (con.Location.X < flp1.Location.X * -1)
                {
                    flp1.Location = new Point(con.Location.X * -1, 0);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void btnLeft_MouseDown(object sender, MouseEventArgs e)
        {
            step = scrollStep;
            left = true;
            mousePosition = MousePosition.X;
            scroll = true;
            timer1.Start();
        }

        private void btnLeft_MouseUp(object sender, MouseEventArgs e)
        {
            step = 0;
            scroll = false;
            timer1.Stop();
        }

        private void btnRight_MouseDown(object sender, MouseEventArgs e)
        {
            step = scrollStep;
            left = false;
            mousePosition = MousePosition.X;
            scroll = true;
            timer1.Start();
        }

        private void btnRight_MouseUp(object sender, MouseEventArgs e)
        {
            step = 0;
            scroll = false;
            timer1.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if (scroll)
            {
                int m = (MousePosition.X - mousePosition);
                if (m > 2 || m < -2)
                {
                    m = !left ? m : -1 * m;
                    step += m > 0 ? 1 : -1;
                    if (step > maxStep)
                        step = maxStep;
                    else if (step < minStep)
                        step = minStep;
                }
                if (!left)
                {
                    if (flp1.Location.X < 0)
                        flp1.Location = new Point(flp1.Location.X + step, 0);
                }
                else
                {
                    if (flp1.Width + flp1.Location.X > panel1.Width)
                        flp1.Location = new Point(flp1.Location.X - step, 0);
                }
                mousePosition = MousePosition.X;
            }
        }

        private void flpLocationSet(object o, EventArgs e)
        {
            if (base.RightToLeft == RightToLeft.Yes)
            {
                if (panel1.Width > flp1.Width)
                    flp1.Location = new Point(panel1.Width - flp1.Width, 0);
                else if (flp1.Location.X > 0)
                    flp1.Location = new Point(0, 0);
            }
            else
            {
                if (flp1.Width < panel1.Width)
                    flp1.Location = new Point(0, 0);
                else if (flp1.Width + flp1.Location.X < panel1.Width)
                    flp1.Location = new Point(flp1.Width - panel1.Width, 0);
            }

        }

        private void ViewScroller_Scroll(object sender, MouseEventArgs e)
        {
            if (flp1.Width > panel1.Width)
                if (flp1.Width + flp1.Location.X + e.Delta < panel1.Width)
                    flp1.Location = new Point(panel1.Width - flp1.Width, 0);
                else if (flp1.Location.X + e.Delta > 0)
                    flp1.Location = new Point(0, 0);
                else
                    flp1.Location = new Point(flp1.Location.X + e.Delta, 0);
        }
    }
}
