using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinFormLibrary.Controls
{
    public partial class MyPanel : UserControl
    {

        public delegate void funcShowForm();
        private List<PanelForm> forms;
        private ControlCollection titles { get { return viewScroller1.Controls; } }
        public MyPanel()
        {
            InitializeComponent();
            forms = new List<PanelForm>();
        }
        public int Count { get => forms.Count(); }
        private int CounterIndex = 0;
        public int IndexActiveForm { get; private set; }

        public new RightToLeft RightToLeft
        {
            get
            {
                return viewScroller1.RightToLeft;
            }
            set
            {
                base.RightToLeft = value;
                viewScroller1.RightToLeft = value;
                panel1.RightToLeft = value;
                tableLayoutPanel1.RightToLeft = value;
            }
        }

        private void ShowForm(PanelForm panelForm)
        {
            ShowForm(panelForm.UnitName);
        }
        public void ShowForm(Form form)
        {
            ShowForm(forms.FirstOrDefault(q => q.Form == form));
        }
        private void ShowForm(string unitName)
        {
            try
            {
                var f = forms.FirstOrDefault(q => q.UnitName == unitName);
                if (f != null)
                {
                    panel1.Controls.Clear();
                    panel1.AutoScroll = f.Form.Dock != DockStyle.Fill;
                    if (f.Form.Dock != DockStyle.Fill && RightToLeft == RightToLeft.Yes && panel1.Size.Width > f.Form.Size.Width)
                        f.Form.Location = new System.Drawing.Point(panel1.Size.Width - f.Form.Size.Width - f.Form.Location.X, f.Form.Location.Y);
                    panel1.Controls.Add(f.Form);
                    if (f.FuncShow != null)
                        f.FuncShow();
                    else
                        f.Form.Show();
                    ActiveTitle(f.Form);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void ShowForm(int index)
        {
            try
            {
                if (index >= 0 && index < forms.Count())
                {
                    panel1.Controls.Clear();
                    panel1.AutoScroll = forms[index].Form.Dock != DockStyle.Fill;
                    if (forms[index].Form.Dock != DockStyle.Fill && RightToLeft == RightToLeft.Yes && panel1.Size.Width > forms[index].Form.Size.Width)
                        forms[index].Form.Location = new Point(panel1.Size.Width - forms[index].Form.Size.Width - forms[index].Form.Location.X - 10, forms[index].Form.Location.Y);
                    panel1.Controls.Add(forms[index].Form);
                    if (forms[index].FuncShow != null)
                        forms[index].FuncShow();
                    else
                        forms[index].Form.Show();
                    ActiveTitle(forms[index].Form);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void AddForm(Form form) => AddForm(form, null, false);
        public void AddForm(Form form, funcShowForm funcShow) => AddForm(form, funcShow, false);
        public void AddForm(Form form, funcShowForm funcShow = null, bool DockFill = false, bool IconTitleEnable = true, bool CloseButtonEnable = true, bool TextTitleEnable = true)
        {
            try
            {
                form.TopLevel = false;
                form.TopMost = true;
                if (form.MaximizeBox || DockFill)
                    form.Dock = DockStyle.Fill;
                form.StartPosition = FormStartPosition.CenterParent;
                form.FormBorderStyle = FormBorderStyle.None;
                var pf = new PanelForm(form, form.Name, $"{form.Name}-{CounterIndex++}");
                pf.FuncShow = funcShow;
                pf.IconTitleEnable = IconTitleEnable;
                pf.TextTitleEnable = TextTitleEnable;
                pf.CloseButtonEnable = CloseButtonEnable;
                form.FormClosed += (o, e) =>
                {
                    panel1.Controls.Remove(form);
                    viewScroller1.Controls.Remove(pf.Title);
                    var temp = findIndexForm(pf);
                    //RemoveForm(form);
                    forms.Remove(pf);
                    if (IndexActiveForm == temp)
                    {
                        if (IndexActiveForm == 0)
                            if (Count == 0)
                                IndexActiveForm = -1;
                            else
                                ShowForm(0);
                        else if (IndexActiveForm > 0)
                            ShowForm(IndexActiveForm - 1);
                    }
                };
                CreateTitleForm(pf);
                forms.Add(pf);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public void AddShowForm(Form form) => AddShowForm(form, null, false);
        public void AddShowForm(Form form, funcShowForm funcShow) => AddShowForm(form, funcShow, false);
        public void AddShowForm(Form form, funcShowForm funcShow = null, bool DockFill = false, bool IconTitleEnable = true, bool CloseButtonEnable = true, bool TextTitleEnable = true)
        {
            try
            {
                AddForm(form, funcShow, DockFill, IconTitleEnable, CloseButtonEnable, TextTitleEnable);
                panel1.Controls.Clear();
                panel1.AutoScroll = form.Dock != DockStyle.Fill;
                if (form.Dock != DockStyle.Fill && RightToLeft == RightToLeft.Yes && panel1.Size.Width > form.Size.Width)
                    form.Location = new Point(panel1.Size.Width - form.Size.Width - form.Location.X - 10, form.Location.Y);
                panel1.Controls.Add(form);
                var pf = forms.FirstOrDefault(q => q.Form == form);
                pf.FuncShow = funcShow;
                if (funcShow == null)
                    form.Show();
                else
                    funcShow();
                ActiveTitle(pf);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ActiveTitle(Form form)
        {
            ActiveTitle(forms.FirstOrDefault(q => q.Form == form));
        }
        private void ActiveTitle(PanelForm panelForm)
        {
            try
            {
                foreach (var item in titles)
                {
                    if (!(item is MyPanelTitle))
                        continue;
                    MyPanelTitle t = item as MyPanelTitle;
                    t.Active = t.Name == panelForm.UnitName;
                }

                IndexActiveForm = findIndexForm(panelForm);
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void CreateTitleForm(PanelForm pf)
        {
            try
            {
                MyPanelTitle title = new MyPanelTitle(pf.Form);
                pf.Title = title;
                title.IconEnable = pf.IconTitleEnable;
                title.TextEnable = pf.TextTitleEnable;
                title.CloseButtonEnable = pf.CloseButtonEnable;
                title.ToolTip = toolTip1;
                title.Active = false;
                title.AutoSize = true;
                title.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                title.Height = 29;
                title.Name = pf.UnitName;
                viewScroller1.Controls.Add(title);
                title.Click += (o, e) =>
                {
                    ShowForm(title.Name);
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        private int findIndexForm(Form form)
        {
            return findIndexForm(forms.FirstOrDefault(q => q.Form == form));
        }
        private int findIndexForm(PanelForm panelform)
        {
            return findIndexForm(panelform.UnitName);
        }
        private int findIndexForm(string unitName)
        {
            try
            {
                var t = forms.Select((q, i) => (q, i)).FirstOrDefault(q => q.q.UnitName == unitName);
                if (t.q == null)
                    return -1;
                return t.i;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void RemoveForm(Form form)
        {
            try
            {
                forms.Remove(forms.FirstOrDefault(q => q.Form == form));
                form.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void CloseForm(Form form)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }

        class PanelForm
        {
            public PanelForm()
            {

            }
            public PanelForm(Form form)
            {
                this.Form = form;
            }
            public PanelForm(Form form, string publicName, string unitName)
            {
                this.Form = form;
                this.PublicName = publicName;
                this.UnitName = unitName;
            }
            public Form Form { get; set; }
            public int Index { get; set; }
            public MyPanelTitle Title { get; set; }
            public string PublicName { get; set; }
            public string UnitName { get; set; }
            public bool MultiTab { get; set; } = true;
            public funcShowForm FuncShow { get; set; } = null;
            public bool IconTitleEnable { get; set; } = true;
            public bool TextTitleEnable { get; set; } = true;
            public bool CloseButtonEnable { get; set; } = true;
        }

        private void panel1_SizeChanged(object sender, EventArgs e)
        {
            if (IndexActiveForm >= 0 && IndexActiveForm < forms.Count && forms[IndexActiveForm].Form.Dock != DockStyle.Fill && RightToLeft == RightToLeft.Yes && panel1.Size.Width > forms[IndexActiveForm].Form.Size.Width)
                forms[IndexActiveForm].Form.Location = new Point(panel1.Size.Width - forms[IndexActiveForm].Form.Size.Width - forms[IndexActiveForm].Form.Location.X - 10, forms[IndexActiveForm].Form.Location.Y);
        }

    }
}
