using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ComponentsLibrary
{
    [DefaultEvent("TextChanged")]
    public partial class EditorTextArea : UserControl
    {
        public int EditorLines { get; private set; }

        public Panel LineNumbersPanel => panelLineNumbers;
        public RichTextBox EditorArea => txtEditor;
        public int LineHeight { get; private set; }

        [Browsable(true)]
        [Category("Action")]
        [Description("Invoked when user changes text in the editor")]
        public event EventHandler EditorTextChanged;

        public bool AcceptsTab
        {
            get => txtEditor.AcceptsTab;
            set => txtEditor.AcceptsTab = value;
        }

        public bool AutoWordSelection
        {
            get => txtEditor.AutoWordSelection;
            set => txtEditor.AutoWordSelection = value;
        }

        public EditorTextArea()
        {
            InitializeComponent();
        }

        private void EditorTextArea_Load(object sender, EventArgs e)
        {
            EditorArea.WordWrap = false;
            EditorArea.TextChanged += new EventHandler(editorTextArea_EditorTextChanged);

            using Graphics g = txtEditor.CreateGraphics();
            LineHeight = TextRenderer.MeasureText(g, "T", txtEditor.Font).Height;
            EditorLines = EditorArea.Lines.Length;

            SetUpLabels();
        }

        private void editorTextArea_EditorTextChanged(object sender, EventArgs e)
        {
            if (EditorLines != EditorArea.Lines.Length)
            {
                UpdateLineNumbersPanel();
            }
        }

        public void UpdateLineNumbersPanel()
        {
            // Create number labels if they are not setup
            if (LineNumbersPanel.Controls.OfType<Label>().Count() <= 0)
            {
                SetUpLabels();
            }

            //get the first visible char index
            int firstVisibleChar = EditorArea.GetCharIndexFromPosition(new Point(0, 0));
            //    //get the line index from the char index
            int lineIndex = EditorArea.GetLineFromCharIndex(firstVisibleChar);

            int index = LineNumbersPanel.Controls.Count;
            foreach (Label label in LineNumbersPanel.Controls.OfType<Label>())
            {
                label.Text = $"{lineIndex + index}.";
                index--;
            }
        }

        private void SetUpLabels()
        {
            LineNumbersPanel.Controls.Clear();

            int possiblLabels = EditorArea.Height / LineHeight;

            for (int i = 0; i <= possiblLabels + 2; i++)
            {
                Label newLineNumber = new Label()
                {
                    TextAlign = ContentAlignment.MiddleRight,
                    Height = (LineHeight - 2),
                    Text = "",
                    Dock = DockStyle.Top,
                    ForeColor = Color.Gray
                };

                LineNumbersPanel.Controls.Add(newLineNumber);
                newLineNumber.BringToFront();
                EditorLines++;
            }
        }

        private void txtEditor_SizeChanged(object sender, EventArgs e)
        {
            if(LineHeight != 0)
            {
                SetUpLabels();
                UpdateLineNumbersPanel();
            }
        }

        //public void UpdateLineNumbersPanel()
        //{
        //    //get the first visible char index
        //    int firstVisibleChar = EditorArea.GetCharIndexFromPosition(new Point(0, 0));
        //    //get the line index from the char index
        //    int lineIndex = EditorArea.GetLineFromCharIndex(firstVisibleChar);

        //    // Means label spots are full and it's only needed to change the text inside
        //    if (lineIndex != 0)
        //    {
        //        int index = LineNumbersPanel.Controls.Count;
        //        foreach (Control label in LineNumbersPanel.Controls)
        //        {
        //            if (label is Label)
        //            {
        //                ((Label)label).Text = $"{lineIndex + index}.";
        //                index--;
        //            }
        //        }
        //        return;
        //    }

        //    else if (EditorLines < EditorArea.Lines.Length)
        //    {
        //        while (EditorLines != EditorArea.Lines.Length)
        //        {
        //            Label newLineNumber = new Label()
        //            {
        //                TextAlign = ContentAlignment.MiddleRight,
        //                Height = (LineHeight - 2),
        //                Text = $"{LineNumbersPanel.Controls.Count + 1}. ",
        //                Dock = DockStyle.Top,
        //                ForeColor = Color.Gray
        //            };

        //            LineNumbersPanel.Controls.Add(newLineNumber);
        //            newLineNumber.BringToFront();
        //            EditorLines++;
        //        }
        //    }
        //    else
        //    {
        //        while (EditorLines != EditorArea.Lines.Length)
        //        {
        //            LineNumbersPanel.Controls.RemoveAt(0);
        //            EditorLines--;
        //        }
        //    }
        //}
    }
}
