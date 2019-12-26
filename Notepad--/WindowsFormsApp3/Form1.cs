using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        StatusStrip ts;
        public Form1()
        {
            InitializeComponent();
            ts = new StatusStrip();
            ToolStripStatusLabel tl = new ToolStripStatusLabel();
            ts.Dock = DockStyle.Bottom;
            tl.Text = "Строка: 0 Столбец: 0";
            ts.Items.Add(tl);
            ts.Visible = false;
            this.Controls.Add(ts);

        }
        int doc_count = 1;

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Child child = new Child();
            child.Text = $"Документ {doc_count}";
            child.MdiParent = this;
            child.Show();
            (child.Controls[0] as RichTextBox).SelectionChanged += Form1_SelectionChanged;
            doc_count++;
        }

        private void Form1_SelectionChanged(object sender, EventArgs e)
        {
            строкаСостоянияToolStripMenuItem_Click(new object(), new EventArgs());
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ActiveMdiChild.Close();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Child child = new Child();
                TextBox FullFileName = new TextBox();
                FullFileName.Visible = false;
                FullFileName.Text = openFileDialog1.FileName;
                child.Controls.Add(FullFileName);
                child.Text = openFileDialog1.SafeFileName;
                (child.Controls[0] as RichTextBox).Text = File.ReadAllText(openFileDialog1.FileName, System.Text.Encoding.GetEncoding(1251));
                (child.Controls[0] as RichTextBox).TextChanged += Tb_TextChanged;
                (child.Controls[0] as RichTextBox).SelectionChanged += Form1_SelectionChanged;
                child.MdiParent = this;
                child.Show();
            }
        }

        private void Tb_TextChanged(object sender, EventArgs e)
        {
            if (((sender as RichTextBox).Parent as Child).Text[((sender as RichTextBox).Parent as Child).Text.Length - 1] != '*') ((sender as RichTextBox).Parent as Child).Text += '*';
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                saveFileDialog1.FileName = ActiveMdiChild.Text;
                if (saveFileDialog1.FileName[saveFileDialog1.FileName.Length - 1] == '*')saveFileDialog1.FileName = saveFileDialog1.FileName.Remove(saveFileDialog1.FileName.Length - 1, 1);
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(saveFileDialog1.FileName, (ActiveMdiChild.Controls[0] as RichTextBox).Text);
                }
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                string fn;
                if (ActiveMdiChild.Controls.Count == 2)
                {
                    fn = (ActiveMdiChild.Controls[1] as TextBox).Text;
                }
                else
                {
                    fn = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + $"\\\\{ActiveMdiChild.Text}.txt";
                }
                File.WriteAllText(fn, (ActiveMdiChild.Controls[0] as RichTextBox).Text);
            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void отменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                (ActiveMdiChild.Controls[0] as RichTextBox).Undo();
            }
        }

        private void вырезатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild!= null)
            {
                (ActiveMdiChild.Controls[0] as RichTextBox).Cut();
            }
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                (ActiveMdiChild.Controls[0] as RichTextBox).Copy();
            }
        }

        private void вставитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                (ActiveMdiChild.Controls[0] as RichTextBox).Paste();
            }
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                (ActiveMdiChild.Controls[0] as RichTextBox).Text = (ActiveMdiChild.Controls[0] as RichTextBox).Text.Remove((ActiveMdiChild.Controls[0] as RichTextBox).SelectionStart, (ActiveMdiChild.Controls[0] as RichTextBox).SelectionLength);
            }
        }

        private void выделитьВсеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                (ActiveMdiChild.Controls[0] as RichTextBox).SelectAll();
            }
        }

        private void времяИДатаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                (ActiveMdiChild.Controls[0] as RichTextBox).Text = (ActiveMdiChild.Controls[0] as RichTextBox).Text.Insert((ActiveMdiChild.Controls[0] as RichTextBox).SelectionStart, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());
            }
        }

        private void фонToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                    (ActiveMdiChild.Controls[0] as RichTextBox).BackColor = colorDialog1.Color;
            }
        }

        private void шрифтToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ActiveMdiChild != null)
            {
                if (fontDialog1.ShowDialog() == DialogResult.OK)
                {
                    (ActiveMdiChild.Controls[0] as RichTextBox).SelectionFont = fontDialog1.Font;
                }
            }
        }

        private void строкаСостоянияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ts.Visible = true;
            if (ActiveMdiChild != null)
            {
                int line = (ActiveMdiChild.Controls[0] as RichTextBox).GetLineFromCharIndex((ActiveMdiChild.Controls[0] as RichTextBox).SelectionStart);
                int stolb = (ActiveMdiChild.Controls[0] as RichTextBox).GetFirstCharIndexFromLine(line);
                stolb = (ActiveMdiChild.Controls[0] as RichTextBox).SelectionStart - stolb;
                (ts.Items[0] as ToolStripStatusLabel).Text=$"Строка: {line+1} Столбец: {stolb}";
            }
        }
    }
}
