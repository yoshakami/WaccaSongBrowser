using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UAssetAPI;
using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Structs;
using static System.Net.Mime.MediaTypeNames;

namespace WaccaSongBrowser
{
    public partial class Title : UserControl
    {
        public Title()
        {
            InitializeComponent();
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


        private void filtergradePartsId01CheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filtergradePartsId02CheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filtergradePartsId03CheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filtergradeRarityCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filterNameTagCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filterexplanationTextTagCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filtergradeAcquisitionWayCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filteritemActivateStartTimeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filteritemActivateEndTimeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filterGainWaccaPointCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filterbIsInitItemEnableCb_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void gradePartsId01freezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void gradePartsId02freezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void gradePartsId03freezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void gradeRarityfreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void nameTagfreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ExplanationTextTagfreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void gradeAcquisitionWayfreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void itemActivateStartTimefreezeTextBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void itemActivateEndTimefreezeTextBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void gainWaccaPointfreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void bIsInitItemfreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

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

        }

        private void injectWaccaGradeButton_Click_1(object sender, EventArgs e)
        {

        }
    }
}
