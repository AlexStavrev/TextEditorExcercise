using System.Drawing;
using System.Windows.Forms;

namespace TextEditor2
{
    public class CustomColorTable : ProfessionalColorTable
    {
        //a bunch of other overrides...
        public Color MainColor { get; set; }

        public CustomColorTable()
        {
            MainColor = Color.FromArgb(64, 64, 64);
        }

        public override Color ToolStripDropDownBackground => MainColor;
        public override Color MenuBorder => MainColor;
        public override Color SeparatorDark => Color.DimGray;
        public override Color ImageMarginGradientMiddle => Color.Transparent;
        public override Color ImageMarginGradientBegin => Color.Transparent;
        public override Color ImageMarginGradientEnd => Color.Transparent;
        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(40, 40, 38);
        public override Color MenuItemSelectedGradientEnd => MainColor;
        public override Color MenuItemBorder => Color.FromArgb(94, 16, 188);
        public override Color MenuItemPressedGradientBegin => Color.FromArgb(40, 40, 38);
        public override Color MenuItemPressedGradientEnd => MainColor;
    }
}
