using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WinFormLibrary.Controls
{
    public class MyTextBox : TextBox
    {
        private Label linkLable;

        public Label LinkLable
        {
            get { return linkLable; }
            set
            {
                linkLable = value;
                if (linkLable != null)
                    linkLable.Click += (o, e) =>
                    {
                        this.Focus();
                        this.Select();
                    };
            }
        }
    }
}