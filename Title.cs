using System.Text;
using UAssetAPI;
using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Structs;

namespace WaccaSongBrowser
{
    public partial class Title : UserControl
    {
        public Title()
        {
            InitializeComponent();
            filteritemActivateStartTimeCheckBox_CheckedChanged(null, null);
            filteritemActivateEndTimeCheckBox_CheckedChanged(null, null);
            filterGainWaccaPointCheckBox_CheckedChanged(null, null);
            filterNameTagCheckBox_CheckedChanged(null, null);
            filterexplanationTextTagCheckBox_CheckedChanged(null, null);
            filterbIsInitItemEnableCb_CheckedChanged(null, null);
            filtergradePartsId01CheckBox_CheckedChanged(null, null);
            filtergradePartsId02CheckBox_CheckedChanged(null, null);
            filtergradePartsId03CheckBox_CheckedChanged(null, null);
            filtergradeRarityCheckBox_CheckedChanged(null, null);
            filtergradeAcquisitionWayCheckBox_CheckedChanged(null, null);
        }
        static UAsset GradeTable;
        static List<string> text = new List<string>();
        static List<string> textVanilla = new List<string>();
        static string messageFolder;
        static string filePath;
        public static sbyte ReadGrade(string uassetPath)
        {
            filePath = uassetPath;
            text.Clear();
            if (!File.Exists(uassetPath)) return -1;
            // Load the asset (assumes .uexp is in the same folder)
            GradeTable = new UAsset(uassetPath, UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_19);
            int id;
            // Go through each export to find the DataTable
            foreach (var export in GradeTable.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    foreach (var row in dataTable.Table.Data)
                    {
                        string ConditionId = row.Name.ToString();              // <-- "010010101"
                        if (row is StructPropertyData rowStruct)
                        {
                            id = WaccaSongBrowser.GetFieldValue<int>(rowStruct, "GradeId");
                            if (id == 0) return -1;
                            text.Add(WaccaSongBrowser.GetFieldValue<string>(rowStruct, "NameTag"));
                        }
                    }
                }
            }
            return 0;
        }
        private void injectWaccaGradeButton_Click(object sender, EventArgs e)
        {
            saveLabel.Text = "Processing...";
            messageFolder = Path.GetDirectoryName(filePath);
            string titlesPath = Path.Combine(messageFolder, "Titles.txt");
            string titlesVanillaPath = Path.Combine(messageFolder, "TitlesVanilla.txt");

            if (File.Exists(titlesPath) && File.Exists(titlesVanillaPath))
            {
                text = File.ReadAllLines(titlesPath, Encoding.UTF8).ToList();
                textVanilla = File.ReadAllLines(titlesVanillaPath, Encoding.UTF8).ToList();

                if (GradeTable == null)
                {
                    saveLabel.Text = "GradeTable is not loaded. Please run ReadGrade first.";
                    return;
                }

                foreach (var export in GradeTable.Exports)
                {
                    if (export is DataTableExport dataTable)
                    {
                        int index = 0;
                        foreach (var row in dataTable.Table.Data)
                        {
                            if (row is StructPropertyData rowStruct)
                            {
                                string currentTag = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "NameTag");
                                string vanillaTag = index < textVanilla.Count ? textVanilla[index] : null;
                                string newTag = index < text.Count ? text[index] : currentTag;
                                WaccaSongBrowser.SetFieldValue(rowStruct, "GradePartsId01", 0);
                                WaccaSongBrowser.SetFieldValue(rowStruct, "GradePartsId02", 0);
                                WaccaSongBrowser.SetFieldValue(rowStruct, "GradePartsId03", 0);
                                // Replace only if the current tag matches the vanilla one
                                if (vanillaTag != null && currentTag == vanillaTag)
                                {
                                    WaccaSongBrowser.SetFieldValue(rowStruct, "NameTag", newTag);
                                }
                                index++;
                            }
                        }
                    }
                }

                GradeTable.Write(Path.Combine(messageFolder, "GradeTableNew.uasset"));
                saveLabel.Text = "Successfully injected Titles.txt into GradeTableNew.uasset";
            }
            else
            {
                saveLabel.Text = "Missing Titles.txt or TitlesVanilla.txt. Please click the create button first.";
            }
        }
        private void filteritemActivateStartTimeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filteritemActivateStartTimeTextBox.Enabled = filteritemActivateStartTimeCheckBox.Checked;
        }

        private void filteritemActivateEndTimeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filteritemActivateEndTimeTextBox.Enabled = filteritemActivateEndTimeCheckBox.Checked;
        }

        private void filterGainWaccaPointCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterGainWaccaPointTextBox.Enabled = filterGainWaccaPointCheckBox.Checked;
        }
        private void filterNameTagCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterNameTagTextBox.Enabled = filterNameTagCheckBox.Checked;
        }

        private void filterexplanationTextTagCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterexplanationTextTagTextBox.Enabled = filterexplanationTextTagCheckBox.Checked;
        }

        private void filterbIsInitItemEnableCb_CheckedChanged(object sender, EventArgs e)
        {
            filterbIsInitItemCheckBox.Enabled = filterbIsInitItemEnableCb.Checked;
        }

        private void filtergradePartsId01CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filtergradePartsId01TextBox.Enabled = filtergradePartsId01CheckBox.Checked;
        }

        private void filtergradePartsId02CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filtergradePartsId02TextBox.Enabled = filtergradePartsId02CheckBox.Checked;
        }

        private void filtergradePartsId03CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filtergradePartsId03TextBox.Enabled = filtergradePartsId03CheckBox.Checked;
        }

        private void filtergradeRarityCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filtergradeRarityTextBox.Enabled = filtergradeRarityCheckBox.Checked;
        }
        private void filtergradeAcquisitionWayCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filtergradeAcquisitionWayTextBox.Enabled = filtergradeAcquisitionWayCheckBox.Checked;
        }
        private void gainWaccaPointfreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (gainWaccaPointfreezeCheckBox.CheckState == CheckState.Checked)
            {
                gainWaccaPointTextBox.BackColor = Color.LightBlue;
                gainWaccaPointfreezeCheckBox.BackColor = Color.LightBlue;
            }
            else
            {
                gainWaccaPointTextBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                gainWaccaPointfreezeCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
            }
        }
        private void nameTagfreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (nameTagfreezeCheckBox.CheckState == CheckState.Checked)
            {
                nameTagTextBox.BackColor = Color.LightBlue;
                nameTagfreezeCheckBox.BackColor = Color.LightBlue;
            }
            else
            {
                nameTagTextBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                nameTagfreezeCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
            }
        }
        private void bIsInitItemfreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (bIsInitItemfreezeCheckBox.CheckState == CheckState.Checked)
            {
                bIsInitItem.BackColor = Color.LightBlue;
                bIsInitItemfreezeCheckBox.BackColor = Color.LightBlue;
            }
            else
            {
                bIsInitItem.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                bIsInitItemfreezeCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
            }
        }

        private void gradePartsId01freezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (gradePartsId01freezeCheckBox.CheckState == CheckState.Checked)
            {
                gradePartsId01TextBox.BackColor = Color.LightBlue;
                gradePartsId01freezeCheckBox.BackColor = Color.LightBlue;
            }
            else
            {
                gradePartsId01TextBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                gradePartsId01freezeCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
            }
        }

        private void gradePartsId02freezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (gradePartsId02freezeCheckBox.CheckState == CheckState.Checked)
            {
                gradePartsId02TextBox.BackColor = Color.LightBlue;
                gradePartsId02freezeCheckBox.BackColor = Color.LightBlue;
            }
            else
            {
                gradePartsId02TextBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                gradePartsId02freezeCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
            }
        }

        private void gradePartsId03freezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (gradePartsId03freezeCheckBox.CheckState == CheckState.Checked)
            {
                gradePartsId03TextBox.BackColor = Color.LightBlue;
                gradePartsId03freezeCheckBox.BackColor = Color.LightBlue;
            }
            else
            {
                gradePartsId03TextBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                gradePartsId03freezeCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
            }
        }

        private void gradeRarityfreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (gradeRarityfreezeCheckBox.CheckState == CheckState.Checked)
            {
                gradeRarityTextBox.BackColor = Color.LightBlue;
                gradeRarityfreezeCheckBox.BackColor = Color.LightBlue;
            }
            else
            {
                gradeRarityTextBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                gradeRarityfreezeCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
            }
        }


        private void ExplanationTextTagfreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ExplanationTextTagfreezeCheckBox.CheckState == CheckState.Checked)
            {
                ExplanationTextTagTextBox.BackColor = Color.LightBlue;
                ExplanationTextTagfreezeCheckBox.BackColor = Color.LightBlue;
            }
            else
            {
                ExplanationTextTagTextBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                ExplanationTextTagfreezeCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
            }
        }

        private void gradeAcquisitionWayfreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (gradeAcquisitionWayfreezeCheckBox.CheckState == CheckState.Checked)
            {
                gradeAcquisitionWayTextBox.BackColor = Color.LightBlue;
                gradeAcquisitionWayfreezeCheckBox.BackColor = Color.LightBlue;
            }
            else
            {
                gradeAcquisitionWayTextBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                gradeAcquisitionWayfreezeCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
            }
        }

        private void itemActivateStartTimefreezeTextBox_CheckedChanged(object sender, EventArgs e)
        {
            if (itemActivateStartTimefreezeCheckBox.CheckState == CheckState.Checked)
            {
                itemActivateStartTimeTextBox.BackColor = Color.LightBlue;
                itemActivateStartTimefreezeCheckBox.BackColor = Color.LightBlue;
            }
            else
            {
                itemActivateStartTimeTextBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                itemActivateStartTimefreezeCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
            }
        }

        private void itemActivateEndTimefreezeTextBox_CheckedChanged(object sender, EventArgs e)
        {
            if (itemActivateEndTimefreezeCheckBox.CheckState == CheckState.Checked)
            {
                itemActivateEndTimeTextBox.BackColor = Color.LightBlue;
                itemActivateEndTimefreezeCheckBox.BackColor = Color.LightBlue;
            }
            else
            {
                itemActivateEndTimeTextBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                itemActivateEndTimefreezeCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
            }
        }


        private void validateButton_Click(object sender, EventArgs e)
        {

        }

        private void nextButton_Click(object sender, EventArgs e)
        {

        }

        private void previousButton_Click(object sender, EventArgs e)
        {

        }

        private void searchButton_Click(object sender, EventArgs e)
        {

        }

        private void searchNextButton_Click(object sender, EventArgs e)
        {

        }

        private void searchPreviousButton_Click(object sender, EventArgs e)
        {

        }

        private void injectNewButton_Click(object sender, EventArgs e)
        {

        }

        private void createNewIconButton_Click(object sender, EventArgs e)
        {

        }

        private void ramSaveCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void saveButton_Click(object sender, EventArgs e)
        {

        }

        private void autoSaveCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void createWaccaGradeButton_Click(object sender, EventArgs e)
        {
            saveLabel.Text = "Processing...";
            messageFolder = Path.GetDirectoryName(filePath);
            if (File.Exists(Path.Combine(messageFolder, "Titles.txt")))
            {
                saveLabel.Text = "Titles.txt already exists in the current folder";
            }
            else if (File.Exists(Path.Combine(messageFolder, "TitlesVanilla.txt")))
            {
                saveLabel.Text = "TitlesVanilla.txt already exists in the current folder";
            }
            else
            {
                File.WriteAllLines(Path.Combine(messageFolder, "Titles.txt"), text, Encoding.UTF8);
                File.WriteAllLines(Path.Combine(messageFolder, "TitlesVanilla.txt"), text);
                saveLabel.Text = "Titles.txt and TitlesVanilla.txt created in the Message folder. Do not edit the vanilla txt!!!!";
            }
        }
    }
}
