namespace WaccaSongBrowser
{
    partial class Menu
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
            labal2 = new Label();
            label2 = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(543, 116);
            label1.Name = "label1";
            label1.Size = new Size(231, 15);
            label1.TabIndex = 0;
            label1.Text = "Drag and Drop any of these to start editing";
            // 
            // labal2
            // 
            labal2.AutoSize = true;
            labal2.Location = new Point(595, 193);
            labal2.Name = "labal2";
            labal2.Size = new Size(119, 15);
            labal2.TabIndex = 1;
            labal2.Text = "The \"Message\" folder";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(574, 303);
            label2.Name = "label2";
            label2.Size = new Size(156, 15);
            label2.TabIndex = 2;
            label2.Text = "MusicParameterTable.uasset";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(595, 253);
            label3.Name = "label3";
            label3.Size = new Size(100, 15);
            label3.TabIndex = 3;
            label3.Text = "The \"Table\" folder";
            // 
            // Menu
            // 
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(labal2);
            Controls.Add(label1);
            Name = "Menu";
            Size = new Size(1600, 900);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label labal2;
        private Label label2;
        private Label label3;
    }
}
