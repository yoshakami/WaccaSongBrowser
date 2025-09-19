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

namespace WaccaSongBrowser
{
    public partial class Icon : UserControl
    {
        public Icon()
        {
            InitializeComponent();
        }
        static UAsset IconTable;
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
            IconTable = new UAsset(uassetPath, UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_19);
            int id;
            // Go through each export to find the DataTable
            foreach (var export in IconTable.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    foreach (var row in dataTable.Table.Data)
                    {
                        string ConditionId = row.Name.ToString();              // <-- "010010101"
                        if (row is StructPropertyData rowStruct)
                        {
                            id = WaccaSongBrowser.GetFieldValue<int>(rowStruct, "IconId");
                            if (id == 0) return -1;
                            //text.Add(WaccaSongBrowser.GetFieldValue<string>(rowStruct, "NameTag"));
                            //TODO: fill list
                        }
                    }
                }
            }
            return 0;
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

        private void ramSaveCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void saveButton_Click(object sender, EventArgs e)
        {

        }

        private void autoSaveCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void injectNewButton_Click(object sender, EventArgs e)
        {

        }

        private void createNewIconButton_Click(object sender, EventArgs e)
        {

        }

        private void filtericonTextureNameCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filtericonTextureNameTextBox.Enabled = filtericonTextureNameCheckBox.Checked;
        }

        private void filtericonRarityCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filtericonRarityTextBox.Enabled = filtericonRarityCheckBox.Checked;
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

        private void filtericonNameCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filtericonNameTextBox.Enabled = filtericonNameCheckBox.Checked;
        }

        private void filtericonAcquisitionWayCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filtericonAcquisitionWayTextBox.Enabled = filtericonAcquisitionWayCheckBox.Checked;
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

        private void iconTextureNamefreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (iconTextureNamefreezeCheckBox.CheckState == CheckState.Checked)
            {
                iconTextureNameTextBox.BackColor = Color.LightBlue;
                iconTextureNamefreezeCheckBox.BackColor = Color.LightBlue;
            }
            else
            {
                iconTextureNameTextBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                iconTextureNamefreezeCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
            }
        }

        private void iconRarityfreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (iconRarityfreezeCheckBox.CheckState == CheckState.Checked)
            {
                iconRarityTextBox.BackColor = Color.LightBlue;
                iconRarityfreezeCheckBox.BackColor = Color.LightBlue;
            }
            else
            {
                iconRarityTextBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                iconRarityfreezeCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
            }
        }

        private void itemActivateStartTimefreezeChechBox_CheckedChanged(object sender, EventArgs e)
        {
            if (itemActivateStartTimefreezeChechBox.CheckState == CheckState.Checked)
            {
                itemActivateStartTimeTextBox.BackColor = Color.LightBlue;
                itemActivateStartTimefreezeChechBox.BackColor = Color.LightBlue;
            }
            else
            {
                itemActivateStartTimeTextBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                itemActivateStartTimefreezeChechBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
            }
        }

        private void ItemActivateEndTimefreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ItemActivateEndTimefreezeCheckBox.CheckState == CheckState.Checked)
            {
                itemActivateEndTimeTextBox.BackColor = Color.LightBlue;
                ItemActivateEndTimefreezeCheckBox.BackColor = Color.LightBlue;
            }
            else
            {
                itemActivateEndTimeTextBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                ItemActivateEndTimefreezeCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
            }
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

        private void iconNamefreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (iconNamefreezeCheckBox.CheckState == CheckState.Checked)
            {
                iconNameTextBox.BackColor = Color.LightBlue;
                iconNamefreezeCheckBox.BackColor = Color.LightBlue;
            }
            else
            {
                iconNameTextBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                iconNamefreezeCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
            }
        }

        private void iconAcquisitionWayfreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (iconAcquisitionWayfreezeCheckBox.CheckState == CheckState.Checked)
            {
                iconAcquisitionWayTextBox.BackColor = Color.LightBlue;
                iconAcquisitionWayfreezeCheckBox.BackColor = Color.LightBlue;
            }
            else
            {
                iconAcquisitionWayTextBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                iconAcquisitionWayfreezeCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
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

        private void explanationTextTagfreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (explanationTextTagfreezeCheckBox.CheckState == CheckState.Checked)
            {
                explanationTextTagTextBox.BackColor = Color.LightBlue;
                explanationTextTagfreezeCheckBox.BackColor = Color.LightBlue;
            }
            else
            {
                explanationTextTagTextBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                explanationTextTagfreezeCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
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
    }
}
