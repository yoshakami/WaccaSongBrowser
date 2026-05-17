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
            Title = new Label();
            injectWaccaTrophyButton = new Button();
            createWaccaTrophyButton = new Button();
            injectWaccaGradeButton = new Button();
            createWaccaGradeButton = new Button();
            mergeENSGbutton = new Button();
            injectUserRateButton = new Button();
            messageFolderToMergeInTextBox = new TextBox();
            pathToUserRateCoefficientTabletextBox = new TextBox();
            pathToUserRateLabel = new Label();
            destMessageFolderLabel = new Label();
            injectUS = new Button();
            CreateUS = new Button();
            injectSG = new Button();
            CreateSG = new Button();
            injectTW = new Button();
            CreateTW = new Button();
            injectHK = new Button();
            CreateHK = new Button();
            injectCN = new Button();
            CreateCN = new Button();
            injectKO = new Button();
            CreateKO = new Button();
            languageAcomboBox = new ComboBox();
            languageAlabel = new Label();
            languageBlabel = new Label();
            languageBcomboBox = new ComboBox();
            SuspendLayout();
            // 
            // outputMessage
            // 
            outputMessage.AutoSize = true;
            outputMessage.Location = new Point(546, 619);
            outputMessage.Name = "outputMessage";
            outputMessage.Size = new Size(48, 15);
            outputMessage.TabIndex = 0;
            outputMessage.Text = "Output:";
            // 
            // createPo
            // 
            createPo.Location = new Point(421, 254);
            createPo.Name = "createPo";
            createPo.Size = new Size(227, 26);
            createPo.TabIndex = 1;
            createPo.Text = "Create .po file from Ja for all .uasset";
            createPo.UseVisualStyleBackColor = true;
            createPo.Click += createPo_Click;
            // 
            // createWacca
            // 
            createWacca.Location = new Point(421, 337);
            createWacca.Name = "createWacca";
            createWacca.Size = new Size(227, 28);
            createWacca.TabIndex = 2;
            createWacca.Text = "Create Wacca.txt from Japanese";
            createWacca.UseVisualStyleBackColor = true;
            createWacca.Click += createWacca_Click;
            // 
            // injectWacca
            // 
            injectWacca.Location = new Point(752, 337);
            injectWacca.Name = "injectWacca";
            injectWacca.Size = new Size(227, 28);
            injectWacca.TabIndex = 3;
            injectWacca.Text = "Inject Wacca.txt into Japanese";
            injectWacca.UseVisualStyleBackColor = true;
            injectWacca.Click += injectWacca_Click;
            // 
            // injectPo
            // 
            injectPo.Location = new Point(752, 254);
            injectPo.Name = "injectPo";
            injectPo.Size = new Size(227, 26);
            injectPo.TabIndex = 4;
            injectPo.Text = "Inject all .po into Ja for all .uasset";
            injectPo.UseVisualStyleBackColor = true;
            injectPo.Click += injectPo_Click;
            // 
            // Title
            // 
            Title.AutoSize = true;
            Title.Location = new Point(636, 49);
            Title.Name = "Title";
            Title.Size = new Size(127, 15);
            Title.TabIndex = 5;
            Title.Text = "Message management";
            // 
            // injectWaccaTrophyButton
            // 
            injectWaccaTrophyButton.Location = new Point(752, 286);
            injectWaccaTrophyButton.Name = "injectWaccaTrophyButton";
            injectWaccaTrophyButton.Size = new Size(227, 47);
            injectWaccaTrophyButton.TabIndex = 7;
            injectWaccaTrophyButton.Text = "Inject Trophy.txt for TrophyTable";
            injectWaccaTrophyButton.UseVisualStyleBackColor = true;
            injectWaccaTrophyButton.Click += injectWaccaTrophyButton_Click;
            // 
            // createWaccaTrophyButton
            // 
            createWaccaTrophyButton.Location = new Point(421, 286);
            createWaccaTrophyButton.Name = "createWaccaTrophyButton";
            createWaccaTrophyButton.Size = new Size(227, 47);
            createWaccaTrophyButton.TabIndex = 6;
            createWaccaTrophyButton.Text = "Create Trophy.txt";
            createWaccaTrophyButton.UseVisualStyleBackColor = true;
            createWaccaTrophyButton.Click += createWaccaTrophyButton_Click;
            // 
            // injectWaccaGradeButton
            // 
            injectWaccaGradeButton.Location = new Point(752, 202);
            injectWaccaGradeButton.Name = "injectWaccaGradeButton";
            injectWaccaGradeButton.Size = new Size(227, 47);
            injectWaccaGradeButton.TabIndex = 9;
            injectWaccaGradeButton.Text = "Inject Titles.txt for GradeTable";
            injectWaccaGradeButton.UseVisualStyleBackColor = true;
            // 
            // createWaccaGradeButton
            // 
            createWaccaGradeButton.Location = new Point(421, 202);
            createWaccaGradeButton.Name = "createWaccaGradeButton";
            createWaccaGradeButton.Size = new Size(227, 47);
            createWaccaGradeButton.TabIndex = 8;
            createWaccaGradeButton.Text = "Create Titles.txt";
            createWaccaGradeButton.UseVisualStyleBackColor = true;
            // 
            // mergeENSGbutton
            // 
            mergeENSGbutton.Location = new Point(421, 96);
            mergeENSGbutton.Name = "mergeENSGbutton";
            mergeENSGbutton.Size = new Size(227, 47);
            mergeENSGbutton.TabIndex = 10;
            mergeENSGbutton.Text = "Take Language A from source and Overwrite Language B from dest folder";
            mergeENSGbutton.UseVisualStyleBackColor = true;
            mergeENSGbutton.Click += mergeButton_Click;
            // 
            // injectUserRateButton
            // 
            injectUserRateButton.Location = new Point(752, 96);
            injectUserRateButton.Name = "injectUserRateButton";
            injectUserRateButton.Size = new Size(227, 47);
            injectUserRateButton.TabIndex = 11;
            injectUserRateButton.Text = "Inject User Rate";
            injectUserRateButton.UseVisualStyleBackColor = true;
            injectUserRateButton.Click += injectUserRateButton_Click;
            // 
            // messageFolderToMergeInTextBox
            // 
            messageFolderToMergeInTextBox.Location = new Point(421, 173);
            messageFolderToMergeInTextBox.Name = "messageFolderToMergeInTextBox";
            messageFolderToMergeInTextBox.Size = new Size(227, 23);
            messageFolderToMergeInTextBox.TabIndex = 12;
            // 
            // pathToUserRateCoefficientTabletextBox
            // 
            pathToUserRateCoefficientTabletextBox.Location = new Point(752, 173);
            pathToUserRateCoefficientTabletextBox.Name = "pathToUserRateCoefficientTabletextBox";
            pathToUserRateCoefficientTabletextBox.Size = new Size(227, 23);
            pathToUserRateCoefficientTabletextBox.TabIndex = 13;
            // 
            // pathToUserRateLabel
            // 
            pathToUserRateLabel.AutoSize = true;
            pathToUserRateLabel.Location = new Point(756, 152);
            pathToUserRateLabel.Name = "pathToUserRateLabel";
            pathToUserRateLabel.Size = new Size(216, 15);
            pathToUserRateLabel.TabIndex = 14;
            pathToUserRateLabel.Text = "Path To UserRateCoefficientTable.uasset";
            // 
            // destMessageFolderLabel
            // 
            destMessageFolderLabel.AutoSize = true;
            destMessageFolderLabel.Location = new Point(342, 152);
            destMessageFolderLabel.Name = "destMessageFolderLabel";
            destMessageFolderLabel.Size = new Size(379, 15);
            destMessageFolderLabel.TabIndex = 15;
            destMessageFolderLabel.Text = "dest Message Folder (will be merged with Language A from source dir)";
            // 
            // injectUS
            // 
            injectUS.Location = new Point(752, 371);
            injectUS.Name = "injectUS";
            injectUS.Size = new Size(227, 26);
            injectUS.TabIndex = 17;
            injectUS.Text = "Inject Wacca.txt into EnUS";
            injectUS.UseVisualStyleBackColor = true;
            injectUS.Click += injectUS_Click;
            // 
            // CreateUS
            // 
            CreateUS.Location = new Point(421, 371);
            CreateUS.Name = "CreateUS";
            CreateUS.Size = new Size(227, 26);
            CreateUS.TabIndex = 16;
            CreateUS.Text = "Create Wacca.txt from EnUS";
            CreateUS.UseVisualStyleBackColor = true;
            CreateUS.Click += CreateUS_Click;
            // 
            // injectSG
            // 
            injectSG.Location = new Point(752, 403);
            injectSG.Name = "injectSG";
            injectSG.Size = new Size(227, 26);
            injectSG.TabIndex = 19;
            injectSG.Text = "Inject Wacca.txt into SG";
            injectSG.UseVisualStyleBackColor = true;
            injectSG.Click += injectSG_Click;
            // 
            // CreateSG
            // 
            CreateSG.Location = new Point(421, 403);
            CreateSG.Name = "CreateSG";
            CreateSG.Size = new Size(227, 26);
            CreateSG.TabIndex = 18;
            CreateSG.Text = "Create Wacca.txt from SG";
            CreateSG.UseVisualStyleBackColor = true;
            CreateSG.Click += CreateSG_Click;
            // 
            // injectTW
            // 
            injectTW.Location = new Point(752, 435);
            injectTW.Name = "injectTW";
            injectTW.Size = new Size(227, 26);
            injectTW.TabIndex = 21;
            injectTW.Text = "Inject Wacca.txt into TW";
            injectTW.UseVisualStyleBackColor = true;
            injectTW.Click += injectTW_Click;
            // 
            // CreateTW
            // 
            CreateTW.Location = new Point(421, 435);
            CreateTW.Name = "CreateTW";
            CreateTW.Size = new Size(227, 26);
            CreateTW.TabIndex = 20;
            CreateTW.Text = "Create Wacca.txt from TW";
            CreateTW.UseVisualStyleBackColor = true;
            CreateTW.Click += CreateTW_Click;
            // 
            // injectHK
            // 
            injectHK.Location = new Point(752, 467);
            injectHK.Name = "injectHK";
            injectHK.Size = new Size(227, 26);
            injectHK.TabIndex = 23;
            injectHK.Text = "Inject Wacca.txt into HK";
            injectHK.UseVisualStyleBackColor = true;
            injectHK.Click += injectHK_Click;
            // 
            // CreateHK
            // 
            CreateHK.Location = new Point(421, 467);
            CreateHK.Name = "CreateHK";
            CreateHK.Size = new Size(227, 26);
            CreateHK.TabIndex = 22;
            CreateHK.Text = "Create Wacca.txt from HK";
            CreateHK.UseVisualStyleBackColor = true;
            CreateHK.Click += CreateHK_Click;
            // 
            // injectCN
            // 
            injectCN.Location = new Point(752, 499);
            injectCN.Name = "injectCN";
            injectCN.Size = new Size(227, 26);
            injectCN.TabIndex = 25;
            injectCN.Text = "Inject Wacca.txt into Chinese";
            injectCN.UseVisualStyleBackColor = true;
            injectCN.Click += injectCN_Click;
            // 
            // CreateCN
            // 
            CreateCN.Location = new Point(421, 499);
            CreateCN.Name = "CreateCN";
            CreateCN.Size = new Size(227, 26);
            CreateCN.TabIndex = 24;
            CreateCN.Text = "Create Wacca.txt from Chinese";
            CreateCN.UseVisualStyleBackColor = true;
            CreateCN.Click += CreateCN_Click;
            // 
            // injectKO
            // 
            injectKO.Location = new Point(752, 531);
            injectKO.Name = "injectKO";
            injectKO.Size = new Size(227, 26);
            injectKO.TabIndex = 27;
            injectKO.Text = "Inject Wacca.txt into Korean";
            injectKO.UseVisualStyleBackColor = true;
            injectKO.Click += injectKO_Click;
            // 
            // CreateKO
            // 
            CreateKO.Location = new Point(421, 531);
            CreateKO.Name = "CreateKO";
            CreateKO.Size = new Size(227, 26);
            CreateKO.TabIndex = 26;
            CreateKO.Text = "Create Wacca.txt from Korean";
            CreateKO.UseVisualStyleBackColor = true;
            CreateKO.Click += CreateKO_Click;
            // 
            // languageAcomboBox
            // 
            languageAcomboBox.BackColor = SystemColors.Window;
            languageAcomboBox.FormattingEnabled = true;
            languageAcomboBox.Location = new Point(216, 120);
            languageAcomboBox.Margin = new Padding(3, 2, 3, 2);
            languageAcomboBox.Name = "languageAcomboBox";
            languageAcomboBox.Size = new Size(199, 23);
            languageAcomboBox.TabIndex = 28;
            // 
            // languageAlabel
            // 
            languageAlabel.AutoSize = true;
            languageAlabel.Location = new Point(281, 96);
            languageAlabel.Name = "languageAlabel";
            languageAlabel.Size = new Size(70, 15);
            languageAlabel.TabIndex = 29;
            languageAlabel.Text = "Language A";
            // 
            // languageBlabel
            // 
            languageBlabel.AutoSize = true;
            languageBlabel.Location = new Point(281, 190);
            languageBlabel.Name = "languageBlabel";
            languageBlabel.Size = new Size(69, 15);
            languageBlabel.TabIndex = 31;
            languageBlabel.Text = "Language B";
            // 
            // languageBcomboBox
            // 
            languageBcomboBox.BackColor = SystemColors.Window;
            languageBcomboBox.FormattingEnabled = true;
            languageBcomboBox.Location = new Point(216, 214);
            languageBcomboBox.Margin = new Padding(3, 2, 3, 2);
            languageBcomboBox.Name = "languageBcomboBox";
            languageBcomboBox.Size = new Size(199, 23);
            languageBcomboBox.TabIndex = 30;
            // 
            // Message
            // 
            Controls.Add(languageBlabel);
            Controls.Add(languageBcomboBox);
            Controls.Add(languageAlabel);
            Controls.Add(languageAcomboBox);
            Controls.Add(injectKO);
            Controls.Add(CreateKO);
            Controls.Add(injectCN);
            Controls.Add(CreateCN);
            Controls.Add(injectHK);
            Controls.Add(CreateHK);
            Controls.Add(injectTW);
            Controls.Add(CreateTW);
            Controls.Add(injectSG);
            Controls.Add(CreateSG);
            Controls.Add(injectUS);
            Controls.Add(CreateUS);
            Controls.Add(destMessageFolderLabel);
            Controls.Add(pathToUserRateLabel);
            Controls.Add(pathToUserRateCoefficientTabletextBox);
            Controls.Add(messageFolderToMergeInTextBox);
            Controls.Add(injectUserRateButton);
            Controls.Add(mergeENSGbutton);
            Controls.Add(injectWaccaGradeButton);
            Controls.Add(createWaccaGradeButton);
            Controls.Add(injectWaccaTrophyButton);
            Controls.Add(createWaccaTrophyButton);
            Controls.Add(Title);
            Controls.Add(injectPo);
            Controls.Add(injectWacca);
            Controls.Add(createWacca);
            Controls.Add(createPo);
            Controls.Add(outputMessage);
            Name = "Message";
            Size = new Size(1338, 681);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label outputMessage;
        private Button createPo;
        private Button createWacca;
        private Button injectWacca;
        private Button injectPo;
        private Label Title;
        private Button injectWaccaTrophyButton;
        private Button createWaccaTrophyButton;
        private Button injectWaccaGradeButton;
        private Button createWaccaGradeButton;
        private Button mergeENSGbutton;
        private Button injectUserRateButton;
        private TextBox messageFolderToMergeInTextBox;
        private TextBox pathToUserRateCoefficientTabletextBox;
        private Label pathToUserRateLabel;
        private Label destMessageFolderLabel;
        private Button injectUS;
        private Button CreateUS;
        private Button injectSG;
        private Button CreateSG;
        private Button injectTW;
        private Button CreateTW;
        private Button injectHK;
        private Button CreateHK;
        private Button injectCN;
        private Button CreateCN;
        private Button injectKO;
        private Button CreateKO;
        private ComboBox languageAcomboBox;
        private Label languageAlabel;
        private Label languageBlabel;
        private ComboBox languageBcomboBox;
    }
}
