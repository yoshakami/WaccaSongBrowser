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
            outputMessage = new Label();
            createPo = new Button();
            createWacca = new Button();
            injectWacca = new Button();
            injectPo = new Button();
            label1 = new Label();
            injectWaccaTrophyButton = new Button();
            createWaccaTrophyButton = new Button();
            injectWaccaGradeButton = new Button();
            createWaccaGradeButton = new Button();
            SuspendLayout();
            // 
            // outputMessage
            // 
            outputMessage.AutoSize = true;
            outputMessage.Location = new Point(546, 517);
            outputMessage.Name = "outputMessage";
            outputMessage.Size = new Size(48, 15);
            outputMessage.TabIndex = 0;
            outputMessage.Text = "Output:";
            // 
            // createPo
            // 
            createPo.Location = new Point(421, 254);
            createPo.Name = "createPo";
            createPo.Size = new Size(227, 47);
            createPo.TabIndex = 1;
            createPo.Text = "Create .po file for all .uasset";
            createPo.UseVisualStyleBackColor = true;
            createPo.Click += createPo_Click;
            // 
            // createWacca
            // 
            createWacca.Location = new Point(421, 355);
            createWacca.Name = "createWacca";
            createWacca.Size = new Size(227, 47);
            createWacca.TabIndex = 2;
            createWacca.Text = "Create Wacca.txt";
            createWacca.UseVisualStyleBackColor = true;
            createWacca.Click += createWacca_Click;
            // 
            // injectWacca
            // 
            injectWacca.Location = new Point(752, 355);
            injectWacca.Name = "injectWacca";
            injectWacca.Size = new Size(227, 47);
            injectWacca.TabIndex = 3;
            injectWacca.Text = "Inject Wacca.txt for all .uasset";
            injectWacca.UseVisualStyleBackColor = true;
            injectWacca.Click += injectWacca_Click;
            // 
            // injectPo
            // 
            injectPo.Location = new Point(752, 254);
            injectPo.Name = "injectPo";
            injectPo.Size = new Size(227, 47);
            injectPo.TabIndex = 4;
            injectPo.Text = "Inject all .po for all .uasset";
            injectPo.UseVisualStyleBackColor = true;
            injectPo.Click += injectPo_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(633, 170);
            label1.Name = "label1";
            label1.Size = new Size(127, 15);
            label1.TabIndex = 5;
            label1.Text = "Message management";
            // 
            // injectWaccaTrophyButton
            // 
            injectWaccaTrophyButton.Location = new Point(752, 304);
            injectWaccaTrophyButton.Name = "injectWaccaTrophyButton";
            injectWaccaTrophyButton.Size = new Size(227, 47);
            injectWaccaTrophyButton.TabIndex = 7;
            injectWaccaTrophyButton.Text = "Inject Trophy.txt for TrophyTable";
            injectWaccaTrophyButton.UseVisualStyleBackColor = true;
            injectWaccaTrophyButton.Click += injectWaccaTrophyButton_Click;
            // 
            // createWaccaTrophyButton
            // 
            createWaccaTrophyButton.Location = new Point(421, 304);
            createWaccaTrophyButton.Name = "createWaccaTrophyButton";
            createWaccaTrophyButton.Size = new Size(227, 47);
            createWaccaTrophyButton.TabIndex = 6;
            createWaccaTrophyButton.Text = "Create Trophy.txt";
            createWaccaTrophyButton.UseVisualStyleBackColor = true;
            createWaccaTrophyButton.Click += createWaccaTrophyButton_Click;
            // 
            // injectWaccaGradeButton
            // 
            injectWaccaGradeButton.Location = new Point(752, 407);
            injectWaccaGradeButton.Name = "injectWaccaGradeButton";
            injectWaccaGradeButton.Size = new Size(227, 47);
            injectWaccaGradeButton.TabIndex = 9;
            injectWaccaGradeButton.Text = "Inject Titles.txt for GradeTable";
            injectWaccaGradeButton.UseVisualStyleBackColor = true;
            // 
            // createWaccaGradeButton
            // 
            createWaccaGradeButton.Location = new Point(421, 407);
            createWaccaGradeButton.Name = "createWaccaGradeButton";
            createWaccaGradeButton.Size = new Size(227, 47);
            createWaccaGradeButton.TabIndex = 8;
            createWaccaGradeButton.Text = "Create Titles.txt";
            createWaccaGradeButton.UseVisualStyleBackColor = true;
            // 
            // Message
            // 
            Controls.Add(injectWaccaGradeButton);
            Controls.Add(createWaccaGradeButton);
            Controls.Add(injectWaccaTrophyButton);
            Controls.Add(createWaccaTrophyButton);
            Controls.Add(label1);
            Controls.Add(injectPo);
            Controls.Add(injectWacca);
            Controls.Add(createWacca);
            Controls.Add(createPo);
            Controls.Add(outputMessage);
            Name = "Message";
            Size = new Size(1364, 720);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label outputMessage;
        private Button createPo;
        private Button createWacca;
        private Button injectWacca;
        private Button injectPo;
        private Label label1;
        private Button injectWaccaTrophyButton;
        private Button createWaccaTrophyButton;
        private Button injectWaccaGradeButton;
        private Button createWaccaGradeButton;
    }
}
