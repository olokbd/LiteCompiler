using System.Diagnostics;
using System.Drawing;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace LiteCompiler
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            int firstIndex = richTextBox1.GetCharIndexFromPosition(new Point(0, 0));
            int firstLine = richTextBox1.GetLineFromCharIndex(firstIndex);
            int lastIndex = richTextBox1.GetCharIndexFromPosition(new Point(0, richTextBox1.Height));
            int lastLine = richTextBox1.GetLineFromCharIndex(lastIndex);
            int lineHeight = richTextBox1.Font.Height;

            for (int i = firstLine; i <= lastLine + 1; i++)
            {
                Point pos = richTextBox1.GetPositionFromCharIndex(richTextBox1.GetFirstCharIndexFromLine(i));
                e.Graphics.DrawString((i + 1).ToString(), richTextBox1.Font, Brushes.Gray, pictureBox1.Width - e.Graphics.MeasureString((i + 1).ToString(), richTextBox1.Font).Width - 5, pos.Y);
            }
        }

        private void RichTextBox_TextChanged(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void RichTextBox_VScroll(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            dataGridView1.Rows.Clear();
            currentFilePath = null;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var codeText = richTextBox1.Text;

            var compiler = new LiteCompiler();

            var lexer = new Lexer(codeText);

            var tokens = lexer.Tokenize();

            dataGridView1.Rows.Clear();

            foreach (var token in tokens)
            {
                var tokenType = token.Type.ToString();

                if (tokenType == "NEWLINE")
                {

                }
                else if (tokenType == "SHURU" || tokenType == "SHESH" || tokenType == "NAM" || tokenType == "BOL" || tokenType == "BHAI" || tokenType == "JODI" || tokenType == "NAHOLE")
                {
                    dataGridView1.Rows.Add(token.Value, "KEYWORD");
                }
                else
                {
                    dataGridView1.Rows.Add(token.Value, token.Type.ToString());
                }
            }

            string result = compiler.Compile(richTextBox1.Text);

            if (result.StartsWith("Error"))
            {
                textBox1.AppendText("> " + result);
            }
            else
            {
                textBox1.AppendText("> Program compiled successfully!" + Environment.NewLine);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //if (e.Node != null)
            //{
            //    MessageBox.Show($"Selected node: {e.Node.Text}");
            //}
        }

        private void buildToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var codeText = richTextBox1.Text;

            var compiler = new LiteCompiler();

            var lexer = new Lexer(codeText);

            var tokens = lexer.Tokenize();

            dataGridView1.Rows.Clear();

            foreach (var token in tokens)
            {
                var tokenType = token.Type.ToString();

                if (tokenType == "NEWLINE")
                {

                }
                else if (tokenType == "SHURU" || tokenType == "SHESH" || tokenType == "NAM" || tokenType == "BOL" || tokenType == "BHAI" || tokenType == "JODI" || tokenType == "NAHOLE")
                {
                    dataGridView1.Rows.Add(token.Value, "KEYWORD");
                }
                else
                {
                    dataGridView1.Rows.Add(token.Value, token.Type.ToString());
                }
            }

            string result = compiler.Compile(richTextBox1.Text);

            textBox1.AppendText("> " + result);
        }

        private void treeView1_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_Click(object sender, EventArgs e)
        {

        }

        private string currentFilePath = null;
        private void saveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                    sfd.Title = "Save your file";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        currentFilePath = sfd.FileName;
                        File.WriteAllText(currentFilePath, richTextBox1.Text);
                    }
                }
            }
            else
            {
                File.WriteAllText(currentFilePath, richTextBox1.Text);
            }
        }

        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                ofd.Title = "Open a text file";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = ofd.FileName;
                    richTextBox1.Text = File.ReadAllText(currentFilePath);
                }
            }
        }

        private void aboutUsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void aboutUsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            string aboutText =
@"LiteCompiler
--------------------------------
A custom programming language interpreter built in C#, 
combining Bengali-inspired keywords with English syntax.

More info/updates:
https://github.com/olokbd/LiteCompiler

Contributors:
- Md Munawer Mahtab Munna (0242310005101047)
- Md Sifatuzzaman (0242310005101363)
- Niloy Joti Sana (0242310005101191)
- Sanjina Iftasum (0242310005101669)
- Sumona Islam (0242310005101631)
";

            MessageBox.Show(this, aboutText, "About Us", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}



