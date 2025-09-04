using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.PropertyTypes.Structs;
using UAssetAPI;
using UAssetAPI.UnrealTypes;
using System.Text;

namespace WaccaSongBrowser
{
    public partial class Condition : UserControl
    {
        static string conditionFile;
        public Condition()
        {
            InitializeComponent();
        }
        public Condition(string fileName)
        {
            InitializeComponent();
            conditionType.DataSource = new BindingSource(Conditions.Types, null);
            conditionType.DisplayMember = "Value"; // text shown
            conditionType.ValueMember = "Key";     // the ID

        }
        static UAsset ConditionTable;
        static List<ConditionData> allConditions = new List<ConditionData>();
        public static sbyte Read(string file)
        {
            if (!File.Exists(file)) return -1;

            string uassetPath = file;
            string directory;
            string newPath;

            // Get just the file name
            string fileName = Path.GetFileName(uassetPath);

            if (fileName.Equals("TotalResultItemJudgementTable.uasset", StringComparison.OrdinalIgnoreCase))
            {
                // swap file if it's UnlockMusicTable.uasset
                // Get the directory part
                directory = Path.GetDirectoryName(uassetPath);

                // Combine with the new filename
                uassetPath = Path.Combine(directory, "ConditionTable.uasset");
            }

            if (!File.Exists(uassetPath)) return -1;
            conditionFile = uassetPath;

            // Load the asset (assumes .uexp is in the same folder)
            ConditionTable = new UAsset(uassetPath, UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_19);

            // Go through each export to find the DataTable
            foreach (var export in ConditionTable.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    Console.WriteLine("Reading rows from DataTable...\n");

                    foreach (var row in dataTable.Table.Data)
                    {
                        if (row is StructPropertyData rowStruct)
                        {
                            ConditionData data = new ConditionData
                            {
                                ConditionId = WaccaSongBrowser.GetFieldValue<int>(rowStruct, "ConditionId"),  // TODO: Add real condition ID from the table
                                bConditionLimitNowSeason = WaccaSongBrowser.GetFieldValue<bool>(rowStruct, "bConditionLimitNowSeason"),
                                ConditionType = WaccaSongBrowser.GetFieldValue<int>(rowStruct, "ConditionType"),
                                Value1 = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "Value1"),
                                Value2 = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "Value2"),
                                Value3 = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "Value3"),
                                Value4 = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "Value4"),
                                Value5 = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "Value5"),
                            };
                            
                            if (data.ConditionId == 0 && data.ConditionType == 0 && data.Value1 == null && data.Value2 == null && data.Value3 == null && data.Value4 == null && data.Value5 == null && data.bConditionLimitNowSeason == false)
                            {
                                return -1;
                            }
                            // conditionid.Items.Add(data.ConditionId.ToString());
                            allConditions.Add(data);  // breakpoint here to check values for wrong table and return -1 if wrong!!!!!!
                        }
                    }
                }
            }

            // Get the directory part
            directory = Path.GetDirectoryName(uassetPath);

            // Combine with the new filename
            newPath = Path.Combine(directory, "TotalResultItemJudgementTable.uasset");
            if (File.Exists(newPath))
            {
                TotalResultItemJudgementTableFilePath = newPath;
                return ReadTotalResultItemJudgementTable();

            }
            else
            {
                TotalResultItemJudgementTableFilePath = null;
                TotalResultItemJudgementTable = null;
                return 2; // cannot read TotalResultItemJudgementTable 

            }
            //nextSongButton_Click(null, null);


            return 0;
        }
        static string TotalResultItemJudgementTableFilePath;
        static UAsset TotalResultItemJudgementTable;

        static List<TotalResultItemJudgementData> allResult = new List<TotalResultItemJudgementData>();
        private static sbyte ReadTotalResultItemJudgementTable()
        {
            // Load the asset (assumes .uexp is in the same folder)
            TotalResultItemJudgementTable = new UAsset(TotalResultItemJudgementTableFilePath, UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_19);

            // Go through each export to find the DataTable
            foreach (var export in TotalResultItemJudgementTable.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    Console.WriteLine("Reading rows from DataTable...\n");

                    foreach (var row in dataTable.Table.Data)
                    {
                        if (row is StructPropertyData rowStruct)
                        {
                            TotalResultItemJudgementData data = new TotalResultItemJudgementData
                            {
                                ItemId = WaccaSongBrowser.GetFieldValue<int>(rowStruct, "ItemId"),
                                ConditionGetableStartTime = WaccaSongBrowser.GetFieldValue<long>(rowStruct, "ConditionGetableStartTime"),
                                ConditionGetableEndTime = WaccaSongBrowser.GetFieldValue<long>(rowStruct, "ConditionGetableEndTime"),
                                //TODO: get ConditionKeys
                            };
                            /*
                            if (data.ConditionID.ToString() == "0")
                            {
                                return -1;
                            }*/
                            // conditionid.Items.Add(data.ConditionId.ToString());
                            allResult.Add(data);
                        }
                    }
                }
            }
            return 0;
        }
        private void conditionidButton_Click(object sender, EventArgs e)
        {

        }

        private void searchNextButton_Click(object sender, EventArgs e)
        {

        }

        private void searchPreviousButton_Click(object sender, EventArgs e)
        {

        }

        private void filterSearchButton_Click(object sender, EventArgs e)
        {

        }

        private void saveButton_Click(object sender, EventArgs e)
        {

        }

        private void conditionInjectButton_Click(object sender, EventArgs e)
        {

        }

        private void resultInjectButton_Click(object sender, EventArgs e)
        {

        }

        private void filterTypeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filter1checkBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filter2checkBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filter3checkBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filter4checkBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filter5checkBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filterConditionEnablecheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filterResultItemIdCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filterResultStartTimeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void filterResultEndTimeCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
