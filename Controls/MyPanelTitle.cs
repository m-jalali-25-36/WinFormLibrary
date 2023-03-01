using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace WinFormLibrary.Controls
{
    public partial class MyPanelTitle : UserControl
    {
        private bool active = false;
        private Form myForm;
        private ToolTip toolTip;
        private bool iconEnable = true;
        private bool textEnable = true;
        private bool closeButtonEnable = true;
        private int maxSize;

        public MyPanelTitle()
        {
            InitializeComponent();
        }
        public MyPanelTitle(Form form) : this()
        {
            MyForm = form;
        }
        public new string Text
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        public Icon Icon
        {
            set { picbIcon.Image = value.ToBitmap(); }
        }

        public ToolTip ToolTip
        {
            get
            {
                return toolTip;
            }
            set
            {
                toolTip = value;
                setToolTipe();
            }
        }

        public bool CloseButtonEnable
        {
            get => closeButtonEnable;
            set
            {
                closeButtonEnable = value;
                button1.Enabled = value;
                button1.Visible = value;
            }
        }
        public bool TextEnable
        {
            get => textEnable;
            set
            {
                textEnable = value;
                label1.Enabled = value;
                label1.Visible = value;
            }
        }
        public bool IconEnable
        {
            get => iconEnable;
            set
            {
                iconEnable = value;
                picbIcon.Enabled = value;
                picbIcon.Visible = value;
            }
        }
        public int MaxSize { get => maxSize; set => maxSize = value; }
        //public bool AutoSize { get; set; }

        public event EventHandler ClickClose
        {
            add { button1.Click += value; }
            remove { button1.Click -= value; }
        }
        public new event EventHandler Click
        {
            add { label1.Click += value; picbIcon.Click += value; base.Click += value; }
            remove { label1.Click -= value; picbIcon.Click -= value; base.Click -= value; }
        }
        public bool Active
        {
            get
            {
                return active;
            }
            set
            {
                active = value;
                if (active)
                {
                    base.BackColor = Color.White;
                }
                else
                {
                    base.BackColor = System.Drawing.SystemColors.Control;
                }
            }
        }

        public new Size Size { get; set; }

        public Form MyForm
        {
            get { return myForm; }
            set
            {
                myForm = value;
                if (myForm != null)
                {
                    Text = myForm.Text;
                    picbIcon.Image = myForm.Icon.ToBitmap();
                    myForm.TextChanged += (o, e) =>
                    {
                        Text = myForm.Text;
                        setToolTipe();
                    };
                    button1.Click += (o, s) =>
                    {
                        MyForm.Close();
                    };
                }
            }
        }
        public int Index { get; set; }

        private void setToolTipe()
        {
            if (toolTip == null)
                return;
            toolTip.SetToolTip(button1, "بستن پنجره");
            toolTip.SetToolTip(label1, Text);
            toolTip.SetToolTip(picbIcon, Text);
        }
    }
}
