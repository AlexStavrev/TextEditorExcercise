using ComponentsLibrary;
using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextEditor2
{
    public partial class MainForm : Form
    {
        private bool isDirty;
        public bool IsDirty
        {
            get { return isDirty; }
            set
            {
                if (value)
                {
                    lblFileName.Text = $"- {Path.GetFileName(saveFileName)}*";
                }
                else
                {
                    lblFileName.Text = $"- {Path.GetFileName(saveFileName)}";
                }

                isDirty = value;
            }
        }

        public string saveFileName
        {
            get
            {
                if(saveFileDialog1.FileName.Equals(""))
                {
                    return "Untitled";
                }
                return saveFileDialog1.FileName;
            }
            private set { }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnDispose_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                WindowState = FormWindowState.Normal;
            }
            else
            {
                WindowState = FormWindowState.Maximized;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // Makes the form movable with the custom border
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void OnBorderMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, 0x112, 0xf012, 0);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OnLoad();
        }

        private void OnLoad()
        {
            editorTextArea.LineNumbersPanel.Width = 35;
            editorTextArea.LineNumbersPanel.Font = new Font("Arial Narrow", 10.5F, FontStyle.Bold, GraphicsUnit.Point);
            editorTextArea.EditorArea.BackColor = Color.FromArgb(60, 60, 60);
            editorTextArea.EditorArea.ForeColor = Color.LightGray;
            editorTextArea.EditorArea.TextChanged += new EventHandler(editorTextArea_EditorTextChanged);
            editorTextArea.EditorArea.KeyPress += new KeyPressEventHandler(editorTextArea_KeyPress);

            editorTextArea.EditorArea.SelectionChanged += new EventHandler(editorTextArea_SelectionChanged);
            editorTextArea.EditorArea.VScroll += new EventHandler(editorTextArea_Scroll);

            menuStrip.RenderMode = ToolStripRenderMode.Professional;
            menuStrip.Renderer = new ToolStripProfessionalRenderer(new CustomColorTable());
        }

        private void editorTextArea_EditorTextChanged(object sender, EventArgs e)
        {
            IsDirty = true;
            toolStripStatusLabelCharacters.Text = $"chars. {editorTextArea.EditorArea.Text.Length}";
        }

        private async void editorTextArea_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsControl(e.KeyChar))
            {
                if(e.KeyChar != (char)Keys.Back)
                {
                    editorTextArea.UpdateLineNumbersPanel();
                }
                return;
            }
            await UpdateColor(e, editorTextArea.EditorArea.SelectionStart, true, true);
            editorTextArea.EditorArea.SelectionStart++;

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileAs();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Exit();
        }

        private void closeFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseFile();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Delete();
        }

        private void uppercaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Uppercase();
        }

        private void lowercaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Lowercase();
        }

        private void reverseCasingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Reversecase();
        }

        private void insertDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InsertDate();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void OpenFile()
        {
            if (IsDirty)
            {
                string fileName = openFileDialog1.FileName.Equals("openFileDialog1") ? "Untitled" : openFileDialog1.FileName;
                switch (MessageBox.Show($"Do you want to save changes before closing {fileName}?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning))
                {
                    case DialogResult.Yes:
                        SaveFile();
                        break;
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.No:
                        break;
                }

            }

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog1.FileName;
                try
                {
                    editorTextArea.EditorArea.Text = File.ReadAllText(filePath);
                    saveFileDialog1.FileName = openFileDialog1.FileName;
                    RecolorTextArea();
                    IsDirty = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening the file \"{filePath}\"!\n\nError was \"{ex.Message}\"", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveFile()
        {
            string filePath = saveFileDialog1.FileName;
            if (filePath.Equals(""))
            {
                SaveFileAs();
            }
            else
            {
                try
                {
                    File.WriteAllText(filePath, editorTextArea.EditorArea.Text);
                    IsDirty = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving the file \"{filePath}\"!\n\nError was \"{ex.Message}\"", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SaveFileAs()
        {
            saveFileDialog1.Title = "Save as...";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog1.FileName;

                if (!File.Exists(filePath) || (File.Exists(filePath)) && MessageBox.Show($"File \"{filePath}\" exists.\nWould you like to overwrite it?", "Overwrite File?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        File.WriteAllText(filePath, editorTextArea.EditorArea.Text);
                        IsDirty = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving as for file \"{filePath}\"!\n\nError was \"{ex.Message}\"", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            saveFileDialog1.Title = "Save file...";
        }

        private void CloseFile()
        {
            if (IsDirty && MessageBox.Show("Do you want to close the file without saving your changes?", "Save unsaved changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }
            saveFileDialog1.FileName = "";
            openFileDialog1.FileName = "";
            editorTextArea.EditorArea.Text = "";
            IsDirty = false;
        }

        private void Exit()
        {
            if (IsDirty && MessageBox.Show("Do you want to exit without saving your changes?", "Save unsaved changes?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }
            Application.Exit();
            Dispose();
        }

        private void Cut()
        {
            editorTextArea.EditorArea.Cut();
        }

        private void Copy()
        {
            editorTextArea.EditorArea.Copy();
        }

        private void Paste()
        {
            int start = editorTextArea.EditorArea.SelectionStart;
            editorTextArea.EditorArea.SelectedText = Clipboard.GetData("Text").ToString();
            int end = editorTextArea.EditorArea.SelectionStart;
            RecolorTextArea(start, end, true);
        }

        private void Delete()
        {
            editorTextArea.EditorArea.SelectedText = "";
        }

        private void Uppercase()
        {
            editorTextArea.EditorArea.SelectedText = editorTextArea.EditorArea.SelectedText.ToUpper();
        }

        private void Lowercase()
        {
            editorTextArea.EditorArea.SelectedText = editorTextArea.EditorArea.SelectedText.ToLower();
        }

        private void Reversecase()
        {
            StringBuilder reversedCaseString = new();
            foreach (char letter in editorTextArea.EditorArea.SelectedText)
            {
                if (Char.IsLetter(letter))
                {
                    reversedCaseString.Append((Char.IsUpper(letter)) ? Char.ToLower(letter) : Char.ToUpper(letter));
                }
                else
                {
                    reversedCaseString.Append(letter);
                }
            }
            int start = editorTextArea.EditorArea.SelectionStart;
            editorTextArea.EditorArea.SelectedText = reversedCaseString.ToString();
            int end = editorTextArea.EditorArea.SelectionStart;
            RecolorTextArea(start, end);
            editorTextArea.EditorArea.SelectionStart = start;
            editorTextArea.EditorArea.SelectionLength = end - start;
        }

        private void InsertDate()
        {
            editorTextArea.EditorArea.SelectedText = DateTime.Now.ToString();
        }

        private void About()
        {
            MessageBox.Show("Text Editor by Alex\n\nVersion: 1.0", "About", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        private void Undo()
        {
            editorTextArea.EditorArea.Undo();
        }

        private void Redo()
        {
            editorTextArea.EditorArea.Redo();
        }

        public void RecolorTextArea(int start = 0, int end = 0, bool checkLetters = false)
        {
            if (end == 0)
            {
                end = editorTextArea.EditorArea.Text.Length;
            }
            for (int i = start; i < end; i++)
            {
                char c = editorTextArea.EditorArea.Text[i];
                if (checkLetters || !Char.IsLetter(c))
                {
                    UpdateColor(new KeyPressEventArgs(c), i, checkLetters);
                }
            }
        }

        public async Task UpdateColor(KeyPressEventArgs e, int index, bool shouldDelay = false, bool checkLetters = false)
        {
            if (shouldDelay)
            {
                await Task.Delay(1);
            }

            if (Char.IsDigit(e.KeyChar))
            {
                editorTextArea.EditorArea.ColorChatAtIndex(index, Color.MediumPurple);
            }
            else if (e.KeyChar is '%')
            {
                editorTextArea.EditorArea.ColorChatAtIndex(index, Color.Violet);
            }
            else if (Char.IsSymbol(e.KeyChar) || Char.IsPunctuation(e.KeyChar))
            {
                editorTextArea.EditorArea.ColorChatAtIndex(index, Color.Magenta);
            }
            else if (checkLetters)
            {
                editorTextArea.EditorArea.ColorChatAtIndex(index, editorTextArea.EditorArea.ForeColor);
            }
        }

        private void editorTextArea_Scroll(object sender, EventArgs e)
        {
            OnScroll();
        }

        private void OnScroll()
        {
            editorTextArea.UpdateLineNumbersPanel();
        }

        private void editorTextArea_SelectionChanged(object sender, EventArgs e)
        {
            OnCaretPossitionChange();
        }

        private void OnCaretPossitionChange()
        {
            int selectionStart = editorTextArea.EditorArea.SelectionStart;

            int lineFromCharIndex = editorTextArea.EditorArea.GetLineFromCharIndex(selectionStart);
            int charIndexFromLine = editorTextArea.EditorArea.GetFirstCharIndexFromLine(lineFromCharIndex);

            int currentLine = editorTextArea.EditorArea.GetLineFromCharIndex(selectionStart) + 1;
            int currentCol = editorTextArea.EditorArea.SelectionStart - charIndexFromLine + 1;

            toolStripStatusLabelCaretPossition.Text = $"Ln {currentLine}, Col {currentCol}";
        }
    }
}


