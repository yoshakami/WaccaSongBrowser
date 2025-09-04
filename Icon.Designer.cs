namespace WaccaSongBrowser
{
    partial class Icon
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
            createNewIcon = new Button();
            artistTextBox = new TextBox();
            artistLabel = new Label();
            musicTextBox = new TextBox();
            musicLabel = new Label();
            progressBar = new ProgressBar();
            label1 = new Label();
            textBox1 = new TextBox();
            label2 = new Label();
            textBox2 = new TextBox();
            label3 = new Label();
            textBox5 = new TextBox();
            label6 = new Label();
            textBox6 = new TextBox();
            label7 = new Label();
            bIsInitItem = new CheckBox();
            label4 = new Label();
            textBox3 = new TextBox();
            label5 = new Label();
            textBox4 = new TextBox();
            label8 = new Label();
            label9 = new Label();
            textBox7 = new TextBox();
            label10 = new Label();
            textBox8 = new TextBox();
            label11 = new Label();
            button1 = new Button();
            SuspendLayout();
            // 
            // createNewIcon
            // 
            createNewIcon.Location = new Point(22, 248);
            createNewIcon.Name = "createNewIcon";
            createNewIcon.Size = new Size(284, 116);
            createNewIcon.TabIndex = 3;
            createNewIcon.Text = "Add All Missing Icons from /UI/Textures/USERICON/S* subfolders";
            createNewIcon.UseVisualStyleBackColor = true;
            // 
            // artistTextBox
            // 
            artistTextBox.Location = new Point(380, 125);
            artistTextBox.Margin = new Padding(3, 2, 3, 2);
            artistTextBox.Name = "artistTextBox";
            artistTextBox.Size = new Size(407, 23);
            artistTextBox.TabIndex = 6;
            // 
            // artistLabel
            // 
            artistLabel.AutoSize = true;
            artistLabel.Location = new Point(380, 108);
            artistLabel.Name = "artistLabel";
            artistLabel.Size = new Size(305, 15);
            artistLabel.TabIndex = 7;
            artistLabel.Text = "IconTextureName    <- example: S03/uT_UICN_S03_06_26";
            // 
            // musicTextBox
            // 
            musicTextBox.Location = new Point(380, 75);
            musicTextBox.Margin = new Padding(3, 2, 3, 2);
            musicTextBox.Name = "musicTextBox";
            musicTextBox.Size = new Size(407, 23);
            musicTextBox.TabIndex = 4;
            // 
            // musicLabel
            // 
            musicLabel.AutoSize = true;
            musicLabel.Location = new Point(380, 58);
            musicLabel.Name = "musicLabel";
            musicLabel.Size = new Size(354, 15);
            musicLabel.TabIndex = 5;
            musicLabel.Text = "IconId   <- I'd suggest not going below 400000 to avoid duplicates";
            // 
            // progressBar
            // 
            progressBar.BackColor = SystemColors.ActiveCaptionText;
            progressBar.ForeColor = SystemColors.Desktop;
            progressBar.Location = new Point(333, 22);
            progressBar.Margin = new Padding(0);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(19, 625);
            progressBar.TabIndex = 80;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(550, 22);
            label1.Name = "label1";
            label1.Size = new Size(72, 15);
            label1.TabIndex = 81;
            label1.Text = "Manual Add";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(380, 225);
            textBox1.Margin = new Padding(3, 2, 3, 2);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(407, 23);
            textBox1.TabIndex = 84;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(380, 208);
            label2.Name = "label2";
            label2.Size = new Size(372, 15);
            label2.TabIndex = 85;
            label2.Text = "ItemActivateStartTime   <- AAAAMMDDHH  set to 0 for no restriction";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(380, 175);
            textBox2.Margin = new Padding(3, 2, 3, 2);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(407, 23);
            textBox2.TabIndex = 82;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(380, 158);
            label3.Name = "label3";
            label3.Size = new Size(180, 15);
            label3.TabIndex = 83;
            label3.Text = "IconRarity   <- 1, 2, 3, or 4 is used";
            // 
            // textBox5
            // 
            textBox5.Location = new Point(380, 325);
            textBox5.Margin = new Padding(3, 2, 3, 2);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(407, 23);
            textBox5.TabIndex = 88;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(380, 308);
            label6.Name = "label6";
            label6.Size = new Size(183, 15);
            label6.TabIndex = 89;
            label6.Text = "GainWaccaPoint    <- default: 500";
            // 
            // textBox6
            // 
            textBox6.Location = new Point(380, 275);
            textBox6.Margin = new Padding(3, 2, 3, 2);
            textBox6.Name = "textBox6";
            textBox6.Size = new Size(407, 23);
            textBox6.TabIndex = 86;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(380, 258);
            label7.Name = "label7";
            label7.Size = new Size(368, 15);
            label7.TabIndex = 87;
            label7.Text = "ItemActivateEndTime   <- AAAAMMDDHH  set to 0 for no restriction";
            // 
            // bIsInitItem
            // 
            bIsInitItem.AutoSize = true;
            bIsInitItem.Location = new Point(380, 565);
            bIsInitItem.Name = "bIsInitItem";
            bIsInitItem.Size = new Size(288, 34);
            bIsInitItem.TabIndex = 90;
            bIsInitItem.Text = "bIsInitItem   <- unlocked by default for new users.\nwill not unlock this item for existing users";
            bIsInitItem.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 408);
            label4.Name = "label4";
            label4.Size = new Size(308, 30);
            label4.TabIndex = 91;
            label4.Text = "in order to grant yourself a new icon, you need to edit\n wacca_item in artemis, and add item id and type 6 (icon)";
            // 
            // textBox3
            // 
            textBox3.Location = new Point(380, 425);
            textBox3.Margin = new Padding(3, 2, 3, 2);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(407, 23);
            textBox3.TabIndex = 94;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(380, 408);
            label5.Name = "label5";
            label5.Size = new Size(407, 15);
            label5.TabIndex = 95;
            label5.Text = "Icon Acquisition Way    <- shown in my room. ex: Play Dive With U in expert";
            // 
            // textBox4
            // 
            textBox4.Location = new Point(380, 375);
            textBox4.Margin = new Padding(3, 2, 3, 2);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(407, 23);
            textBox4.TabIndex = 92;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(380, 358);
            label8.Name = "label8";
            label8.Size = new Size(196, 15);
            label8.TabIndex = 93;
            label8.Text = "Icon Name    <- shown in My Room";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(36, 470);
            label9.Name = "label9";
            label9.Size = new Size(79, 15);
            label9.TabIndex = 96;
            label9.Text = "no file loaded";
            // 
            // textBox7
            // 
            textBox7.Enabled = false;
            textBox7.Location = new Point(380, 525);
            textBox7.Margin = new Padding(3, 2, 3, 2);
            textBox7.Name = "textBox7";
            textBox7.Size = new Size(407, 23);
            textBox7.TabIndex = 99;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(380, 508);
            label10.Name = "label10";
            label10.Size = new Size(108, 15);
            label10.TabIndex = 100;
            label10.Text = "ExplanationTextTag";
            // 
            // textBox8
            // 
            textBox8.Enabled = false;
            textBox8.Location = new Point(380, 475);
            textBox8.Margin = new Padding(3, 2, 3, 2);
            textBox8.Name = "textBox8";
            textBox8.Size = new Size(407, 23);
            textBox8.TabIndex = 97;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(380, 458);
            label11.Name = "label11";
            label11.Size = new Size(57, 15);
            label11.TabIndex = 98;
            label11.Text = "NameTag";
            // 
            // button1
            // 
            button1.Location = new Point(840, 248);
            button1.Name = "button1";
            button1.Size = new Size(284, 116);
            button1.TabIndex = 101;
            button1.Text = "Inject NEW";
            button1.UseVisualStyleBackColor = true;
            // 
            // Icon
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(button1);
            Controls.Add(textBox7);
            Controls.Add(label10);
            Controls.Add(textBox8);
            Controls.Add(label11);
            Controls.Add(label9);
            Controls.Add(textBox3);
            Controls.Add(label5);
            Controls.Add(textBox4);
            Controls.Add(label8);
            Controls.Add(label4);
            Controls.Add(bIsInitItem);
            Controls.Add(textBox5);
            Controls.Add(label6);
            Controls.Add(textBox6);
            Controls.Add(label7);
            Controls.Add(textBox1);
            Controls.Add(label2);
            Controls.Add(textBox2);
            Controls.Add(label3);
            Controls.Add(label1);
            Controls.Add(progressBar);
            Controls.Add(artistTextBox);
            Controls.Add(artistLabel);
            Controls.Add(musicTextBox);
            Controls.Add(musicLabel);
            Controls.Add(createNewIcon);
            Name = "Icon";
            Size = new Size(1200, 700);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button createNewIcon;
        private TextBox artistTextBox;
        private Label artistLabel;
        private TextBox musicTextBox;
        private Label musicLabel;
        private ProgressBar progressBar;
        private Label label1;
        private TextBox textBox1;
        private Label label2;
        private TextBox textBox2;
        private Label label3;
        private TextBox textBox5;
        private Label label6;
        private TextBox textBox6;
        private Label label7;
        private CheckBox bIsInitItem;
        private Label label4;
        private TextBox textBox3;
        private Label label5;
        private TextBox textBox4;
        private Label label8;
        private Label label9;
        private TextBox textBox7;
        private Label label10;
        private TextBox textBox8;
        private Label label11;
        private Button button1;
    }
}
