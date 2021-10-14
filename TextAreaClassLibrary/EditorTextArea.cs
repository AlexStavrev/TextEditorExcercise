using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextAreaClassLibrary
{
    public partial class EditorTextArea : UserControl
    {
        public Panel LineNumbersPanel => panelLineNumbers;
        public RichTextBox EditorArea => txtEditor;

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
    }
}
