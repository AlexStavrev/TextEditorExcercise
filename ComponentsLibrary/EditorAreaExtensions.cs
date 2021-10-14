using System.Drawing;
using System.Windows.Forms;

namespace ComponentsLibrary
{
    public static class EditorAreaExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;
        }

        public static void ColorChatAtIndex(this RichTextBox box, int charIndex, Color color)
        {
            box.SelectionStart = charIndex;
            box.SelectionLength = 1;
            box.SelectionColor = color;

            box.SelectionLength = 0;
            box.SelectionStart = charIndex;
            box.SelectionColor = box.ForeColor;
        }
    }
}
