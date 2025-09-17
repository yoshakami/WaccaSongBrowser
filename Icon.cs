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

        private void filtericonTextureNameCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filtericonRarityCheckBox_CheckedChanged(object sender, EventArgs e)
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

        private void filtericonNameCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filtericonAcquisitionWayCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filterNameTagCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filterexplanationTextTagCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filterbIsInitItemEnableCb_CheckedChanged(object sender, EventArgs e)
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

        private void iconTextureNamefreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void iconRarityfreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void itemActivateStartTimefreezeChechBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ItemActivateEndTimefreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void gainWaccaPointfreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void iconNamefreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void iconAcquisitionWayfreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void nameTagfreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void explanationTextTagfreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void bIsInitItemfreezeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
