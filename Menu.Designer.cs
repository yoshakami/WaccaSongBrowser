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
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            injectUserRate = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(574, 134);
            label1.Name = "label1";
            label1.Size = new Size(231, 15);
            label1.TabIndex = 0;
            label1.Text = "Drag and Drop any of these to start editing";
            // 
            // labal2
            // 
            labal2.AutoSize = true;
            labal2.Location = new Point(615, 203);
            labal2.Name = "labal2";
            labal2.Size = new Size(119, 15);
            labal2.TabIndex = 1;
            labal2.Text = "The \"Message\" folder";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(763, 271);
            label2.Name = "label2";
            label2.Size = new Size(156, 15);
            label2.TabIndex = 2;
            label2.Text = "MusicParameterTable.uasset";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(626, 271);
            label3.Name = "label3";
            label3.Size = new Size(100, 15);
            label3.TabIndex = 3;
            label3.Text = "The \"Table\" folder";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(626, 541);
            label4.Name = "label4";
            label4.Size = new Size(93, 15);
            label4.TabIndex = 4;
            label4.Text = "IconTable.uasset";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(612, 347);
            label5.Name = "label5";
            label5.Size = new Size(123, 15);
            label5.TabIndex = 5;
            label5.Text = "ConditionTable.uasset";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(965, 271);
            label6.Name = "label6";
            label6.Size = new Size(139, 15);
            label6.TabIndex = 6;
            label6.Text = "UnlockMusicTable.uasset";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(765, 347);
            label7.Name = "label7";
            label7.Size = new Size(210, 15);
            label7.TabIndex = 7;
            label7.Text = "TotalResultItemJudgementTable.uasset";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(587, 602);
            label8.Name = "label8";
            label8.Size = new Size(183, 15);
            label8.TabIndex = 8;
            label8.Text = "UserPlateBackgroundTable.uasset";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(624, 470);
            label9.Name = "label9";
            label9.Size = new Size(106, 15);
            label9.TabIndex = 10;
            label9.Text = "TrophyTable.uasset";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(626, 410);
            label10.Name = "label10";
            label10.Size = new Size(101, 15);
            label10.TabIndex = 9;
            label10.Text = "GradeTable.uasset";
            // 
            // injectUserRate
            // 
            injectUserRate.Location = new Point(302, 271);
            injectUserRate.Name = "injectUserRate";
            injectUserRate.Size = new Size(227, 47);
            injectUserRate.TabIndex = 11;
            injectUserRate.Text = "Inject User Rate (easter egg)";
            injectUserRate.UseVisualStyleBackColor = true;
            injectUserRate.Visible = false;
            injectUserRate.Click += injectUserRate_Click;
            // 
            // Menu
            // 
            Controls.Add(injectUserRate);
            Controls.Add(label9);
            Controls.Add(label10);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(labal2);
            Controls.Add(label1);
            Name = "Menu";
            Size = new Size(1338, 681);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label labal2;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Button injectUserRate;
    }
}
