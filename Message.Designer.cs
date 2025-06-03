namespace WaccaSongBrowser
{
    partial class Message
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
            label1 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(72, 73);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 0;
            label1.Text = "label1";
            // 
            // Message
            // 
            //AutoScaleDimensions = new SizeF(50F, 50F);
            //AutoScaleMode = AutoScaleMode.Font;
            Width = 1600;
            Height = 900;
            Controls.Add(label1);
            Name = "Message";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
    }
}
