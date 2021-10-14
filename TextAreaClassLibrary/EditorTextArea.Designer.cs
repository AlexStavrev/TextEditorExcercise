
namespace TextAreaClassLibrary
{
    partial class EditorTextArea
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelLineNumbers = new System.Windows.Forms.Panel();
            this.txtEditor = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // panelLineNumbers
            // 
            this.panelLineNumbers.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLineNumbers.Location = new System.Drawing.Point(0, 0);
            this.panelLineNumbers.Name = "panelLineNumbers";
            this.panelLineNumbers.Size = new System.Drawing.Size(34, 507);
            this.panelLineNumbers.TabIndex = 0;
            // 
            // txtEditor
            // 
            this.txtEditor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEditor.Location = new System.Drawing.Point(34, 0);
            this.txtEditor.Name = "txtEditor";
            this.txtEditor.Size = new System.Drawing.Size(783, 507);
            this.txtEditor.TabIndex = 1;
            this.txtEditor.Text = "";
            // 
            // EditorTextArea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtEditor);
            this.Controls.Add(this.panelLineNumbers);
            this.Name = "EditorTextArea";
            this.Size = new System.Drawing.Size(817, 507);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelLineNumbers;
        private System.Windows.Forms.RichTextBox txtEditor;
    }
}
