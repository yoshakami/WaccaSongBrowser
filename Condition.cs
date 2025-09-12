using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using UAssetAPI;
using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.PropertyTypes.Structs;
using UAssetAPI.UnrealTypes;

namespace WaccaSongBrowser
{
    public partial class Condition : UserControl
    {
        static string conditionFile;
        public Condition()
        {
            InitializeComponent();
            filterConditionType.DataSource = new BindingSource(Conditions.Types, null);
            filterConditionType.DisplayMember = "Value"; // text shown
            filterConditionType.ValueMember = "Key";     // the ID
            conditionType.DataSource = new BindingSource(Conditions.Types, null);
            conditionType.DisplayMember = "Value"; // text shown
            conditionType.ValueMember = "Key";     // the ID
            filterResultItemType.Items.Add("Background");
            filterResultItemType.Items.Add("Color");
            filterResultItemType.Items.Add("Icon");
            filterResultItemType.Items.Add("Navigator");
            filterResultItemType.Items.Add("SFX");
            filterResultItemType.Items.Add("Title");
            filterResultItemType.Items.Add("Trophy");
            filterResultItemType.Items.Add("Other");
            // if (Read(fileName) != -1) // already called as static!!!!!!
            // fill UI elements since these can't be in a static method
            foreach (ConditionData data in allConditions)
            {
                conditionid.Items.Add(data.ConditionId.ToString());
            }
            if (resultDict != null) resultDict.Clear();
            resultDict = allResult
            .SelectMany(r => r.ConditionKeys.Select(k => new { k, r }))
            .GroupBy(x => x.k)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.r).ToList()
            );
            loadedFilesLabel.Text = "Loaded Files: ";
            if (IconTable != null)
                loadedFilesLabel.Text += "\nIconTable";
            if (SESetTable != null)
                loadedFilesLabel.Text += "\nSESetTable";
            if (GradeTable != null)
                loadedFilesLabel.Text += "\nGradeTable";
            if (TrophyTable != null)
                loadedFilesLabel.Text += "\nTrophyTable";
            loadedFilesLabel.Text += "\nConditionTable";
            if (UserPlateBackgroundTable != null)
                loadedFilesLabel.Text += "\nUserPlateBackgroundTable";
            if (TouchPanelSymbolColorTable != null)
                loadedFilesLabel.Text += "\nTouchPanelSymbolColorTable";
            if (TotalResultItemJudgementTable != null)
            {
                // enable everything
                loadedFilesLabel.Text += "\nTotalResultItemJudgementTable";
                resultConditionTextBox.Enabled = true;
                resultItemIdTextBox.Enabled = true;
                resultStartTimeTextBox.Enabled = true;
                resultEndTimeTextBox.Enabled = true;
            }
            else
            {
                // disable everything
                resultConditionTextBox.Enabled = false;
                resultItemIdTextBox.Enabled = false;
                resultStartTimeTextBox.Enabled = false;
                resultEndTimeTextBox.Enabled = false;
            }
            if (IconMessage != null)
                loadedFilesLabel.Text += "\n../Message/IconMessage";
            if (SESetMessage != null)
                loadedFilesLabel.Text += "\n../Message/AttackSEMessage";
            if (UserPlateBackgroundMessage != null)
                loadedFilesLabel.Text += "\n../Message/UserPlateBackgroundMessage";
            if (ColorMessage != null)
                loadedFilesLabel.Text += "\n../Message/TouchPanelSymbolColorMessage";

            nextButton_Click(null, null); // read returns -1 if allconditions is empty

            filter1checkBox_CheckedChanged(null, null);
            filter2checkBox_CheckedChanged(null, null);
            filter3checkBox_CheckedChanged(null, null);
            filter4checkBox_CheckedChanged(null, null);
            filter5checkBox_CheckedChanged(null, null);
            filterTypeCheckBox_CheckedChanged(null, null);
            filterConditionEnablecheckBox_CheckedChanged(null, null);
            filterResultItemIdCheckBox_CheckedChanged(null, null);
            filterResultItemTypeCheckBox_CheckedChanged(null, null);
            filterResultItemEnableCheckBox_CheckedChanged(null, null);
            filterResultItemRangeCheckBox_CheckedChanged(null, null);
            filterResultStartTimeCheckBox_CheckedChanged(null, null);
            filterResultEndTimeCheckBox_CheckedChanged(null, null);
        }
        static UAsset ConditionTable;
        static List<ConditionData> allConditions = new List<ConditionData>();
        public static sbyte Read(string file)
        {
            allConditions.Clear();
            allResult.Clear();
            if (!File.Exists(file)) return -1;

            string uassetPath = file;
            string directory;
            string newPath;

            // Get just the file name
            string fileName = Path.GetFileName(uassetPath);

            if (fileName.StartsWith("TotalResultItemJudgementTable", StringComparison.OrdinalIgnoreCase))
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
                    foreach (var row in dataTable.Table.Data)
                    {
                        string ConditionId = row.Name.ToString();              // <-- "010010101"
                        if (row is StructPropertyData rowStruct)
                        {
                            ConditionData data = new ConditionData
                            {
                                //ConditionId = WaccaSongBrowser.GetFieldValue<int>(rowStruct, "ConditionId"),  // don't use this, see 020090101
                                ConditionId = int.Parse(ConditionId),
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
            if (allConditions.Count == 0)
            {
                return -1;
            }
            newPath = Path.Combine(directory, "TotalResultItemJudgementTable.uasset");
            if (File.Exists(newPath))
            {
                TotalResultItemJudgementTableFilePath = newPath;
                ReadTotalResultItemJudgementTable();
            }
            else
            {
                TotalResultItemJudgementTableFilePath = null;
                TotalResultItemJudgementTable = null;
                // cannot read TotalResultItemJudgementTable
            }
            newPath = Path.Combine(directory, "IconTable.uasset");
            if (File.Exists(newPath))
            {
                IconTableFilePath = newPath;
                ReadIconTable();
            }
            else
            {
                IconTableFilePath = null;
                IconTable = null;
                // cannot read IconTable
            }
            newPath = Path.Combine(directory, "GradeTable.uasset");
            if (File.Exists(newPath))
            {
                GradeTableFilePath = newPath;
                ReadGradeTable();
            }
            else
            {
                GradeTableFilePath = null;
                GradeTable = null;
                // cannot read GradeTable
            }
            newPath = Path.Combine(directory, "UserPlateBackgroundTable.uasset");
            if (File.Exists(newPath))
            {
                UserPlateBackgroundTableFilePath = newPath;
                ReadUserPlateBackgroundTable();
            }
            else
            {
                UserPlateBackgroundTableFilePath = null;
                UserPlateBackgroundTable = null;
                // cannot read UserPlateBackgroundTable
            }
            newPath = Path.Combine(directory, "TrophyTable.uasset");
            if (File.Exists(newPath))
            {
                TrophyTableFilePath = newPath;
                ReadTrophyTable();
            }
            else
            {
                TrophyTableFilePath = null;
                TrophyTable = null;
                // cannot read TrophyTable
            }
            newPath = Path.Combine(directory, "TouchPanelSymbolColorTable.uasset");
            if (File.Exists(newPath))
            {
                TouchPanelSymbolColorTableFilePath = newPath;
                ReadTouchPanelSymbolColorTable();
            }
            else
            {
                TouchPanelSymbolColorTableFilePath = null;
                TouchPanelSymbolColorTable = null;
                // cannot read TouchPanelSymbolColorTable
            }
            newPath = Path.Combine(directory, "SESetTable.uasset");
            if (File.Exists(newPath))
            {
                SESetTableFilePath = newPath;
                ReadSESetTable();
            }
            else
            {
                SESetTableFilePath = null;
                SESetTable = null;
                // cannot read SESetTable
            }
            newPath = Path.GetFullPath(Path.Combine(directory, "..", "Message", "AttackSEMessage.uasset"));
            if (File.Exists(newPath))
            {
                SESetMessageFilePath = newPath;
                ReadSESetMessage();
            }
            else
            {
                SESetMessageFilePath = null;
                SESetMessage = null;
                // cannot read SESetMessage
            }
            newPath = Path.GetFullPath(Path.Combine(directory, "..", "Message", "TouchPanelSymbolColorMessage.uasset"));
            if (File.Exists(newPath))
            {
                ColorMessageFilePath = newPath;
                ReadColorMessage();
            }
            else
            {
                ColorMessageFilePath = null;
                ColorMessage = null;
                // cannot read SESetMessage
            }
            newPath = Path.GetFullPath(Path.Combine(directory, "..", "Message", "UserPlateBackgroundMessage.uasset"));
            if (File.Exists(newPath))
            {
                UserPlateBackgroundMessageFilePath = newPath;
                ReadUserPlateBackgroundMessage();
            }
            else
            {
                UserPlateBackgroundMessageFilePath = null;
                UserPlateBackgroundMessage = null;
                // cannot read SESetMessage
            }
            newPath = Path.GetFullPath(Path.Combine(directory, "..", "Message", "IconMessage.uasset"));
            if (File.Exists(newPath))
            {
                IconMessageFilePath = newPath;
                ReadIconMessage();
            }
            else
            {
                IconMessageFilePath = null;
                IconMessage = null;
                // cannot read SESetMessage
            }
            //nextSongButton_Click(null, null);
            return 0;
        }
        static string SESetTableFilePath;
        static UAsset SESetTable;  // sound effects
        static string UserPlateBackgroundMessageFilePath;
        static UAsset UserPlateBackgroundMessage;
        static string SESetMessageFilePath;
        static UAsset SESetMessage;  // sound effects
        static string TouchPanelSymbolColorTableFilePath;
        static UAsset TouchPanelSymbolColorTable;  // circle colors
        static string ColorMessageFilePath;
        static UAsset ColorMessage;  // circle colors
        static string TrophyTableFilePath;
        static UAsset TrophyTable;  // trophy
        static string TotalResultItemJudgementTableFilePath;
        static UAsset TotalResultItemJudgementTable;  // link item ID with condition
        static string UserPlateBackgroundTableFilePath;
        static UAsset UserPlateBackgroundTable;  // bg
        static string GradeTableFilePath;
        static UAsset GradeTable;  // title
        static string IconTableFilePath;
        static UAsset IconTable;  // icon
        static string IconMessageFilePath;
        static UAsset IconMessage;  // icon

        // No navigator!!!!!!!! NavigateCharacterTable only has link to the UI, but no link to messages or Voices. so I'll hardcode it for now
        static readonly Dictionary<int, string> navigators = new Dictionary<int, string>
        {
            { 210001, "Elisabeth" },
            { 210002, "Lily" },
            { 310001, "Rune" },
            { 310002, "Evil Rune" },
            { 210054, "Hestia" },
            { 210055, "Ais Wallenstein" },
            { 210056, "Usada Pekora" },
            { 210057, "Sakura Miko" },
            { 210058, "Shirakami Fubuki" },
            { 210059, "Hoshimachi Suisei" },
            { 210060, "Seine" },
            { 210061, "HARDCORE TANO*C CREW" },
        };
        static readonly Dictionary<int, string[]> icon = new Dictionary<int, string[]>();
        static readonly Dictionary<string, int> iconReverse = new Dictionary<string, int>();
        static readonly Dictionary<int, string> grade = new Dictionary<int, string>();
        static readonly Dictionary<int, string[]> bg = new Dictionary<int, string[]>();
        static readonly Dictionary<string, int> bgReverse = new Dictionary<string, int>();
        static readonly Dictionary<int, string> trophy = new Dictionary<int, string>();
        static readonly Dictionary<int, string> colors = new Dictionary<int, string>();
        static readonly Dictionary<string, int> colorsReverse = new Dictionary<string, int>();
        static readonly Dictionary<int, string> seset = new Dictionary<int, string>();
        static readonly Dictionary<string, int> sesetReverse = new Dictionary<string, int>();

        private static void ReadSESetTable()
        {
            // Load the asset (assumes .uexp is in the same folder)
            SESetTable = new UAsset(
                SESetTableFilePath,
                UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_19
            );
            int id;
            foreach (var export in SESetTable.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    foreach (var row in dataTable.Table.Data)
                    {
                        if (row is StructPropertyData rowStruct)
                        {
                            id = WaccaSongBrowser.GetFieldValue<int>(rowStruct, "SESetID");
                            seset[id] = "SFX";
                            sesetReverse[WaccaSongBrowser.GetFieldValue<string>(rowStruct, "NameTag")] = id;
                        }
                    }
                }
            }
        }
        private static void ReadSESetMessage()
        {
            // Load the asset (assumes .uexp is in the same folder)
            SESetMessage = new UAsset(
                SESetMessageFilePath,
                UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_19
            );
            string message;
            int id;
            foreach (var export in SESetMessage.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    foreach (var row in dataTable.Table.Data)
                    {
                        if (row is StructPropertyData rowStruct)
                        {
                            message = row.Name.ToString();
                            if (sesetReverse.TryGetValue(message, out id))
                            {
                                seset[id] = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "JapaneseMessage");
                            }
                        }
                    }
                }
            }
        }
        private static void ReadTouchPanelSymbolColorTable()
        {
            // Load the asset (assumes .uexp is in the same folder)
            TouchPanelSymbolColorTable = new UAsset(
                TouchPanelSymbolColorTableFilePath,
                UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_19
            );
            int id;
            foreach (var export in TouchPanelSymbolColorTable.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    foreach (var row in dataTable.Table.Data)
                    {
                        if (row is StructPropertyData rowStruct)
                        {
                            id = WaccaSongBrowser.GetFieldValue<int>(rowStruct, "SymbolColorId");
                            colors[id] = "Color";
                            colorsReverse[WaccaSongBrowser.GetFieldValue<string>(rowStruct, "NameTag")] = id;
                        }
                    }
                }
            }
        }
        private static void ReadColorMessage()
        {
            // Load the asset (assumes .uexp is in the same folder)
            ColorMessage = new UAsset(
                ColorMessageFilePath,
                UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_19
            );
            string message;
            int id;
            foreach (var export in ColorMessage.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    foreach (var row in dataTable.Table.Data)
                    {
                        if (row is StructPropertyData rowStruct)
                        {
                            message = row.Name.ToString();
                            if (colorsReverse.TryGetValue(message, out id))
                            {
                                colors[id] = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "JapaneseMessage");
                            }
                        }
                    }
                }
            }
        }
        private static void ReadTrophyTable()
        {
            // Load the asset (assumes .uexp is in the same folder)
            TrophyTable = new UAsset(
                TrophyTableFilePath,
                UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_19
            );
            foreach (var export in TrophyTable.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    foreach (var row in dataTable.Table.Data)
                    {
                        if (row is StructPropertyData rowStruct)
                        {
                            trophy[WaccaSongBrowser.GetFieldValue<int>(rowStruct, "TrophyId")] = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "NameTag");
                        }
                    }
                }
            }
        }
        private static void ReadUserPlateBackgroundTable()
        {
            // Load the asset (assumes .uexp is in the same folder)
            UserPlateBackgroundTable = new UAsset(
                UserPlateBackgroundTableFilePath,
                UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_19
            );
            int id;
            string[] namePath = { "", "" };
            foreach (var export in UserPlateBackgroundTable.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    foreach (var row in dataTable.Table.Data)
                    {
                        if (row is StructPropertyData rowStruct)
                        {
                            id = WaccaSongBrowser.GetFieldValue<int>(rowStruct, "UserPlateBackgroundId");
                            namePath[0] = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "NameTag");
                            namePath[1] = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "UserPlateBacgroundTextureName");
                            bg[id] = namePath.ToArray();
                            bgReverse[namePath[0]] = id;
                        }
                    }
                }
            }
        }
        private static void ReadUserPlateBackgroundMessage()
        {
            // Load the asset (assumes .uexp is in the same folder)
            UserPlateBackgroundMessage = new UAsset(
                UserPlateBackgroundMessageFilePath,
                UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_19
            );
            int id;
            string[] namePath;
            foreach (var export in UserPlateBackgroundMessage.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    foreach (var row in dataTable.Table.Data)
                    {
                        if (row is StructPropertyData rowStruct)
                        {
                            if (bgReverse.TryGetValue(row.Name.ToString(), out id))
                            {
                                if (bg.TryGetValue(id, out namePath))
                                {
                                    namePath[0] = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "JapaneseMessage");
                                }
                            }
                        }
                    }
                }
            }
        }

        private static sbyte ReadIconTable()
        {
            // Load the asset (assumes .uexp is in the same folder)
            IconTable = new UAsset(
                IconTableFilePath,
                UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_19
            );
            int id;
            string[] namePath = { "", "" };
            foreach (var export in IconTable.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    foreach (var row in dataTable.Table.Data)
                    {
                        if (row is StructPropertyData rowStruct)
                        {
                            id = WaccaSongBrowser.GetFieldValue<int>(rowStruct, "IconId");
                            namePath[0] = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "NameTag");
                            namePath[1] = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "IconTextureName");
                            icon[id] = namePath.ToArray();
                            iconReverse[namePath[0]] = id;
                        }
                    }
                }
            }
            return 0;
        }
        private static sbyte ReadIconMessage()
        {
            // Load the asset (assumes .uexp is in the same folder)
            IconMessage = new UAsset(
                IconMessageFilePath,
                UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_19
            );
            int id;
            string[] namePath;
            foreach (var export in IconMessage.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    foreach (var row in dataTable.Table.Data)
                    {
                        if (row is StructPropertyData rowStruct)
                        {
                            if (iconReverse.TryGetValue(row.Name.ToString(), out id))
                            {
                                if (icon.TryGetValue(id, out namePath))
                                {
                                    namePath[0] = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "JapaneseMessage");
                                }
                            }
                        }
                    }
                }
            }
            return 0;
        }
        private static sbyte ReadGradeTable()
        {
            // Load the asset (assumes .uexp is in the same folder)
            GradeTable = new UAsset(
                GradeTableFilePath,
                UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_19
            );

            foreach (var export in GradeTable.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    foreach (var row in dataTable.Table.Data)
                    {
                        if (row is StructPropertyData rowStruct)
                        {
                            grade[WaccaSongBrowser.GetFieldValue<int>(rowStruct, "GradeId")] = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "NameTag");
                        }
                    }
                }
            }
            return 0;
        }

        static List<TotalResultItemJudgementData> allResult = new List<TotalResultItemJudgementData>();
        static Dictionary<string, List<TotalResultItemJudgementData>> resultDict;
        private static sbyte ReadTotalResultItemJudgementTable()
        {
            // Load the asset (assumes .uexp is in the same folder)
            TotalResultItemJudgementTable = new UAsset(
                TotalResultItemJudgementTableFilePath,
                UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_19
            );

            foreach (var export in TotalResultItemJudgementTable.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    foreach (var row in dataTable.Table.Data)
                    {
                        if (row is StructPropertyData rowStruct)
                        {
                            TotalResultItemJudgementData data = new TotalResultItemJudgementData
                            {
                                ItemId = WaccaSongBrowser.GetFieldValue<int>(rowStruct, "ItemId"),
                                ConditionGetableStartTime = WaccaSongBrowser.GetFieldValue<long>(rowStruct, "ConditionGetableStartTime"),
                                ConditionGetableEndTime = WaccaSongBrowser.GetFieldValue<long>(rowStruct, "ConditionGetableEndTime"),
                            };

                            // Read the ConditionKeys array
                            var keysArray = rowStruct.Value.FirstOrDefault(
                                v => v.Name.ToString() == "ConditionKeys"
                            ) as ArrayPropertyData;

                            if (keysArray != null)
                            {
                                foreach (var entry in keysArray.Value)
                                {
                                    if (entry is StrPropertyData strProp)
                                    {
                                        data.ConditionKeys.Add(strProp.Value.ToString());
                                    }
                                }
                            }
                            allResult.Add(data);
                        }
                    }
                }
            }
            return 0;
        }

        int currentSongId = 0;
        private void conditionidButton_Click(object sender, EventArgs e)
        {
            if (allConditions.Count == 0)
                return;
            saveLabel.Text = "";
            saveChanges();
            int.TryParse(conditionid.Text, out currentSongId);
            if (currentSongId == 0)
            {
                saveLabel.Text = "ID 0 cannot be used";
                saveLabel.Text = "";
                return;
            }
            int currentIndex = allConditions.FindIndex(s => s.ConditionId == currentSongId);
            if (currentIndex == -1)
            {
                // consoleLabel.Text = "Creating new song ID. Will be saved upon next click on save, or on next Song ID change if autosave is on"; // use WSongInject instead
                saveLabel.Text = $"ID {currentSongId} not found";
                saveLabel.Text = "";
                return;
            }
            LoadUI(allConditions[currentIndex]);
        }
        int navId;
        /**!
         * this function fills all UI elements with the data from song
         */
        private void LoadUI(ConditionData song)
        {
            currentSongId = song.ConditionId;
            conditionIdTextBox.Text = song.ConditionId.ToString();
            conditionCheckBox.Checked = song.bConditionLimitNowSeason;
            condition1textBox.Text = song.Value1;
            condition2textBox.Text = song.Value2;
            condition3textBox.Text = song.Value3;
            condition4textBox.Text = song.Value4;
            condition5textBox.Text = song.Value5;
            conditionTypeTextBox.Text = song.ConditionType.ToString();
            count = 0;
            string key = song.ConditionId.ToString().PadLeft(9, '0');
            bgPictureBox.Visible = false;
            itemPictureBox.Visible = true;
            if (resultDict.TryGetValue(key, out var items))
            {
                int id = 0;
                foreach (var totalResultItemData in items)
                {
                    count++;
                    resultConditionTextBox.Text = totalResultItemData.ConditionKeys[0];
                    resultItemIdTextBox.Text = totalResultItemData.ItemId.ToString();
                    id = totalResultItemData.ItemId;
                    resultStartTimeTextBox.Text = totalResultItemData.ConditionGetableStartTime.ToString();
                    resultEndTimeTextBox.Text = totalResultItemData.ConditionGetableEndTime.ToString();
                }
                resultSearchLabel.Text = $"Showing item {count}/{count}";
                current = count;
                UpdateImage(id);
            }
            else
            {
                // key not found
                resultSearchLabel.Text = "No results found ❌";
                current = 0;
                resultConditionTextBox.Text = "";
                resultItemIdTextBox.Text = "";
                resultStartTimeTextBox.Text = "";
                resultEndTimeTextBox.Text = "";
                itemTextBox.Text = "";
                itemPictureBox.Image = null;
                bgPictureBox.Image = null;
            }
        }
        private void UpdateImage(int itemId)
        {
            string name;
            string[] namePath;
            string path;
            if (navigators.TryGetValue(itemId, out name))
            {
                itemTextBox.Text = "Navigator: " + name;
                path = $"{execPath}navigators/{itemId}.png";
                if (File.Exists(path))
                {
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (var ms = new MemoryStream())
                    {
                        fs.CopyTo(ms);                  // Copy file into memory
                        ms.Position = 0;                // Reset position to beginning
                        itemPictureBox.Image = Image.FromStream(ms); // Load image from memory
                    }
                }
            }
            else if (seset.TryGetValue(itemId, out name))
            {
                itemTextBox.Text = "SFX: " + name;
                itemPictureBox.Image = null;
            }
            else if (icon.TryGetValue(itemId, out namePath))
            {
                itemTextBox.Text = "Icon: " + namePath[0];
                path = $"{execPath}icons/{namePath[1].Substring(namePath[1].IndexOf('/') + 1)}.png";
                if (File.Exists(path))
                {
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (var ms = new MemoryStream())
                    {
                        fs.CopyTo(ms);                  // Copy file into memory
                        ms.Position = 0;                // Reset position to beginning
                        itemPictureBox.Image = Image.FromStream(ms); // Load image from memory
                    }
                }
            }
            else if (grade.TryGetValue(itemId, out name))
            {
                itemTextBox.Text = "Title: " + name;
                itemPictureBox.Image = null;
            }
            else if (colors.TryGetValue(itemId, out name))
            {
                itemTextBox.Text = "Color: " + name;
                itemPictureBox.Image = null;
            }
            else if (trophy.TryGetValue(itemId, out name))
            {
                itemTextBox.Text = "Trophy: " + name;
                itemPictureBox.Image = null;
            }
            else if (bg.TryGetValue(itemId, out namePath))
            {
                bgPictureBox.Visible = true;
                itemPictureBox.Visible = false;
                itemTextBox.Text = "Bg: " + namePath[0];
                path = $"{execPath}bg/{namePath[1]}.png";
                if (File.Exists(path))
                {
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (var ms = new MemoryStream())
                    {
                        fs.CopyTo(ms);                  // Copy file into memory
                        ms.Position = 0;                // Reset position to beginning
                        bgPictureBox.Image = Image.FromStream(ms); // Load image from memory
                    }
                }
            }
            else
            {
                itemTextBox.Text = "";
                itemPictureBox.Image = null;
                bgPictureBox.Image = null;
            }
        }
        static readonly string execPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/");
        static int current;
        static int count;
        private void resultNextButton_Click(object sender, EventArgs e)
        {
            if (count < 2) return;
            if (current >= count)
                current = 1;
            else
                current += 1;
            count = 0;
            string key = currentSongId.ToString().PadLeft(9, '0');
            if (resultDict.TryGetValue(key, out var items))
            {
                foreach (var totalResultItemData in items)
                {
                    count += 1;
                    if (current == count)
                    {
                        resultConditionTextBox.Text = totalResultItemData.ConditionKeys[0];
                        resultItemIdTextBox.Text = totalResultItemData.ItemId.ToString();
                        resultStartTimeTextBox.Text = totalResultItemData.ConditionGetableStartTime.ToString();
                        resultEndTimeTextBox.Text = totalResultItemData.ConditionGetableEndTime.ToString();
                    }
                }
                resultSearchLabel.Text = $"Showing item {current}/{count}";
                int id;
                int.TryParse(resultItemIdTextBox.Text, out id);
                UpdateImage(id);
            }
        }

        private void resultPreviousButton_Click(object sender, EventArgs e)
        {
            if (count < 2) return;
            if (current <= 1)
                current = count;
            else
                current -= 1;
            count = 0;
            string key = currentSongId.ToString().PadLeft(9, '0');
            if (resultDict.TryGetValue(key, out var items))
            {
                foreach (var totalResultItemData in items)
                {
                    count += 1;
                    if (current == count)
                    {
                        resultConditionTextBox.Text = totalResultItemData.ConditionKeys[0];
                        resultItemIdTextBox.Text = totalResultItemData.ItemId.ToString();
                        resultStartTimeTextBox.Text = totalResultItemData.ConditionGetableStartTime.ToString();
                        resultEndTimeTextBox.Text = totalResultItemData.ConditionGetableEndTime.ToString();
                    }
                }
                resultSearchLabel.Text = $"Showing item {current}/{count}";
                int id;
                int.TryParse(resultItemIdTextBox.Text, out id);
                UpdateImage(id);
            }
        }
        private bool? GetBoolProperty(ConditionData song, string propertyName)
        {
            var propInfo = typeof(ConditionData).GetProperty(propertyName);
            if (propInfo != null && propInfo.PropertyType == typeof(bool))
            {
                return (bool)propInfo.GetValue(song);
            }

            var fieldInfo = typeof(ConditionData).GetField(propertyName);
            if (fieldInfo != null && fieldInfo.FieldType == typeof(bool))
            {
                return (bool)fieldInfo.GetValue(song);
            }

            return null; // Not found or not a bool
        }
        private void filterSearchButton_Click(object sender, EventArgs e)
        {
            IEnumerable<ConditionData> filtered = allConditions;


            // --- Int/Uint filters ---
            if (filterTypeCheckBox.Checked)
                filtered = filtered.Where(song => int.TryParse(filterConditionTypeTextBox.Text, out int newType) && song.ConditionType == newType);

            // --- Text filters ---
            if (filter1checkBox.Checked)
            {
                string filter = filter1textBox.Text.Trim().ToLower();
                if (!string.IsNullOrEmpty(filter))
                    filtered = filtered.Where(song => song.Value1 != null &&
                                                      song.Value1.ToLower().Equals(filter));
            }
            if (filter2checkBox.Checked)
            {
                string filter = filter2textBox.Text.Trim().ToLower();
                if (!string.IsNullOrEmpty(filter))
                    filtered = filtered.Where(song => song.Value2 != null &&
                                                      song.Value2.ToLower().Equals(filter));
            }
            if (filter3checkBox.Checked)
            {
                string filter = filter3textBox.Text.Trim().ToLower();
                if (!string.IsNullOrEmpty(filter))
                    filtered = filtered.Where(song => song.Value3 != null &&
                                                      song.Value3.ToLower().Equals(filter));
            }
            if (filter4checkBox.Checked)
            {
                string filter = filter4textBox.Text.Trim().ToLower();
                if (!string.IsNullOrEmpty(filter))
                    filtered = filtered.Where(song => song.Value4 != null &&
                                                      song.Value4.ToLower().Equals(filter));
            }
            if (filter5checkBox.Checked)
            {
                string filter = filter5textBox.Text.Trim().ToLower();
                if (!string.IsNullOrEmpty(filter))
                    filtered = filtered.Where(song => song.Value5 != null &&
                                                      song.Value5.ToLower().Equals(filter));
            }
            // --- LimitSeason filter ---
            if (filterConditionEnablecheckBox.Checked)
            {
                filtered = filtered.Where(song => GetBoolProperty(song, "bConditionLimitNowSeason") == filterConditionCheckBox.Checked);
            }


            IEnumerable<TotalResultItemJudgementData> resuFiltered = allResult;
            if (filterResultItemEnableCheckBox.Checked && !filterResultItemCheckBox.Checked)
            { }
            else
            {
                int min, max;
                // --- ItemID filter ---
                if (filterResultItemIdCheckBox.Checked)
                {
                    resuFiltered = resuFiltered.Where(song => int.TryParse(filterResultItemIdTextBox.Text, out min) && song.ItemId == min);
                }
                if (filterResultItemRangeCheckBox.Checked)
                {
                    int.TryParse(filterResultItemMinTextBox.Text, out min);
                    int.TryParse(filterResultItemMaxTextBox.Text, out max);
                    resuFiltered = resuFiltered.Where(song => min <= song.ItemId && song.ItemId <= max);
                }
                if (filterResultItemTypeCheckBox.Checked)
                {
                    switch (filterResultItemType.SelectedIndex)
                    {
                        case 0:
                            resuFiltered = resuFiltered.Where(song => bg.ContainsKey(song.ItemId));
                            break;
                        case 1:
                            resuFiltered = resuFiltered.Where(song => colors.ContainsKey(song.ItemId));
                            break;
                        case 2:
                            resuFiltered = resuFiltered.Where(song => icon.ContainsKey(song.ItemId));
                            break;
                        case 3:
                            resuFiltered = resuFiltered.Where(song => navigators.ContainsKey(song.ItemId));
                            break;
                        case 4:
                            resuFiltered = resuFiltered.Where(song => seset.ContainsKey(song.ItemId));
                            break;
                        case 5:
                            resuFiltered = resuFiltered.Where(song => grade.ContainsKey(song.ItemId));
                            break;
                        case 6:
                            resuFiltered = resuFiltered.Where(song => trophy.ContainsKey(song.ItemId));
                            break;
                        default:
                            resuFiltered = resuFiltered.Where(song => !trophy.ContainsKey(song.ItemId) && !grade.ContainsKey(song.ItemId) && !seset.ContainsKey(song.ItemId) && !navigators.ContainsKey(song.ItemId) && !icon.ContainsKey(song.ItemId) && !colors.ContainsKey(song.ItemId) && !bg.ContainsKey(song.ItemId));
                            break;
                    }
                }

                // --- Start filter
                if (filterResultStartTimeCheckBox.Checked)
                {
                    resuFiltered = resuFiltered.Where(song => long.TryParse(filterResultStartTimeTextBox.Text, out long start) && song.ConditionGetableStartTime == start);
                }
                // --- End filter
                if (filterResultEndTimeCheckBox.Checked)
                {
                    resuFiltered = resuFiltered.Where(song => long.TryParse(filterResultEndTimeTextBox.Text, out long end) && song.ConditionGetableEndTime == end);
                }

                // Collect all condition IDs from resuFiltered.ConditionKeys
                var validConditionIds = resuFiltered
                    .SelectMany(r => r.ConditionKeys)   // flatten list of keys
                    .Select(k => int.TryParse(k, out var id) ? id : (int?)null)
                    .Where(id => id.HasValue)
                    .Select(id => id.Value)
                    .ToHashSet();

                // Now restrict filtered (ConditionData) by these IDs
                filtered = filtered.Where(song => validConditionIds.Contains(song.ConditionId));
            }
            // --- Final result ---
            var resultList = filtered.ToList();
            if (resultList.Count == 0)
            {
                searchResultLabel.Text = "No match.";
                searchOutputLabel.Text = "Showing Result 0/0";
                return;
            }

            // Option A: Show first match immediately
            var firstSong = resultList.First();
            if (resultList.Count > 1)
                searchResultLabel.Text = $"{resultList.Count} matches!";
            else
                searchResultLabel.Text = $"{resultList.Count} match!";
            saveChanges();
            currentSongId = firstSong.ConditionId;
            LoadUI(firstSong);


            // Option B: keep resultList for navigation (next/previous)
            // store in a field:
            filteredSongs = resultList;
            filteredSongsSelectedIndex = 0;
            searchOutputLabel.Text = $"Showing Result 1/{resultList.Count}";
        }

        private void filterInvertMatchesButton_Click(object sender, EventArgs e)
        {
            IEnumerable<ConditionData> filtered = allConditions;

            // Exclude songs already in filteredSongs
            filtered = filtered.Where(song => !filteredSongs.Any(fs => fs.ConditionId == song.ConditionId));

            // --- Final result ---
            var resultList = filtered.ToList();
            if (resultList.Count == 0)
            {
                searchResultLabel.Text = "No match.";
                searchOutputLabel.Text = $"Showing Result 0/0";
                return;
            }

            // Option A: Show first match immediately
            var firstSong = resultList.First();
            if (resultList.Count > 1)
                searchResultLabel.Text = $"{resultList.Count} matches!";
            else
                searchResultLabel.Text = $"{resultList.Count} match!";

            saveChanges();
            currentSongId = firstSong.ConditionId;
            LoadUI(firstSong);

            // Option B: keep resultList for navigation (next/previous)
            filteredSongs = resultList;
            filteredSongsSelectedIndex = 0;
            searchOutputLabel.Text = $"Showing Result 1/{resultList.Count}";
        }

        static List<ConditionData> filteredSongs = new List<ConditionData>();
        static int filteredSongsSelectedIndex;

        private void searchPreviousButton_Click(object sender, EventArgs e)
        {
            if (filteredSongs.Count == 0) return;
            filteredSongsSelectedIndex--;
            if (filteredSongsSelectedIndex < 0)
            {
                filteredSongsSelectedIndex = filteredSongs.Count - 1;
            }
            saveChanges();
            currentSongId = filteredSongs[filteredSongsSelectedIndex].ConditionId;
            LoadUI(filteredSongs[filteredSongsSelectedIndex]);
            searchOutputLabel.Text = $"Showing Result {filteredSongsSelectedIndex + 1}/{filteredSongs.Count}";
        }

        private void searchNextButton_Click(object sender, EventArgs e)
        {
            if (filteredSongs.Count == 0) return;
            filteredSongsSelectedIndex++;
            if (filteredSongsSelectedIndex >= filteredSongs.Count)
            {
                filteredSongsSelectedIndex = 0;
            }
            saveChanges();
            currentSongId = filteredSongs[filteredSongsSelectedIndex].ConditionId;
            LoadUI(filteredSongs[filteredSongsSelectedIndex]);
            searchOutputLabel.Text = $"Showing Result {filteredSongsSelectedIndex + 1}/{filteredSongs.Count}";
        }

        private void filterTypeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterConditionType.Enabled = filterTypeCheckBox.Checked;
            filterConditionTypeTextBox.Enabled = filterTypeCheckBox.Checked;
        }

        private void filter1checkBox_CheckedChanged(object sender, EventArgs e)
        {
            filter1textBox.Enabled = filter1checkBox.Checked;
        }

        private void filter2checkBox_CheckedChanged(object sender, EventArgs e)
        {
            filter2textBox.Enabled = filter2checkBox.Checked;
        }

        private void filter3checkBox_CheckedChanged(object sender, EventArgs e)
        {
            filter3textBox.Enabled = filter3checkBox.Checked;
        }

        private void filter4checkBox_CheckedChanged(object sender, EventArgs e)
        {
            filter4textBox.Enabled = filter4checkBox.Checked;
        }

        private void filter5checkBox_CheckedChanged(object sender, EventArgs e)
        {
            filter5textBox.Enabled = filter5checkBox.Checked;
        }

        private void filterConditionEnablecheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterConditionCheckBox.Enabled = filterConditionEnablecheckBox.Checked;
        }

        private void filterResultItemIdCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (filterResultItemIdCheckBox.Checked) filterResultItemRangeCheckBox.Checked = false;
            filterResultItemIdTextBox.Enabled = filterResultItemIdCheckBox.Checked;
        }

        private void filterResultStartTimeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterResultStartTimeTextBox.Enabled = filterResultStartTimeCheckBox.Checked;
        }

        private void filterResultEndTimeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterResultEndTimeTextBox.Enabled = filterResultEndTimeCheckBox.Checked;
        }
        private void filterResultItemRangeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (filterResultItemRangeCheckBox.Checked) filterResultItemIdCheckBox.Checked = false;
            filterResultItemMaxTextBox.Enabled = filterResultItemRangeCheckBox.Checked;
            filterResultItemMinTextBox.Enabled = filterResultItemRangeCheckBox.Checked;
        }

        private void filterResultItemEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterResultItemCheckBox.Enabled = filterResultItemEnableCheckBox.Checked;
        }

        private void filterResultItemTypeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterResultItemType.Enabled = filterResultItemTypeCheckBox.Checked;
        }


        private void autoSaveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autoSaveCheckBox.Checked == true)
            {
                saveChanges();
            }
        }

        private void conditionid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the "ding" sound
                conditionidButton_Click(sender, e);
            }
        }
        static int conditionTypeInt;
        static bool doNotChangeType = false;
        private void conditionTypeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (doNotChangeType) return;
            doNotChangeType = true;
            if (int.TryParse(conditionTypeTextBox.Text, out conditionTypeInt))
            {
                if (conditionTypeInt >= -2 && conditionTypeInt <= 64)
                {
                    conditionType.SelectedIndex = conditionTypeInt + 2;
                }
            }
            else // user entered shit
            {
            }
            doNotChangeType = false;
        }

        private void conditionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (doNotChangeType) return;
            doNotChangeType = true;
            conditionTypeTextBox.Text = (conditionType.SelectedIndex - 2).ToString();
            doNotChangeType = false;
        }

        private void filterConditionTypeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (doNotChangeType) return;
            doNotChangeType = true;
            if (int.TryParse(filterConditionTypeTextBox.Text, out conditionTypeInt))
            {
                if (conditionTypeInt >= -2 && conditionTypeInt <= 64)
                {
                    filterConditionType.SelectedIndex = conditionTypeInt + 2;
                }
            }
            else // user entered shit
            {
            }
            doNotChangeType = false;
        }

        private void filterConditionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (doNotChangeType) return;
            doNotChangeType = true;
            filterConditionTypeTextBox.Text = (filterConditionType.SelectedIndex - 2).ToString();
            doNotChangeType = false;
        }
        private void nextButton_Click(object sender, EventArgs e)
        {
            if (conditionid.SelectedIndex >= conditionid.Items.Count - 1)
                conditionid.SelectedIndex = 0;
            else
                conditionid.SelectedIndex += 1;
            conditionidButton_Click(null, null);
        }

        private void previousButton_Click(object sender, EventArgs e)
        {
            if (conditionid.SelectedIndex <= 0)
                conditionid.SelectedIndex = conditionid.Items.Count - 1;
            else
                conditionid.SelectedIndex -= 1;
            conditionidButton_Click(null, null);
        }
        private void saveChanges()
        {
            if (autoSaveCheckBox.Checked)
            {
                saveButton_Click(null, null);
            }
            else if (ramSaveCheckBox.Checked)
            {
                saveChangesInRam();
            }
            // else don't save data
        }
        private void saveSongData(ConditionData songData)
        {
            int i;
            int.TryParse(conditionIdTextBox.Text, out i);
            songData.ConditionId = i;
            int.TryParse(conditionTypeTextBox.Text, out i);
            songData.ConditionType = i;
            songData.bConditionLimitNowSeason = conditionCheckBox.Checked;
            songData.Value1 = condition1textBox.Text;
            songData.Value2 = condition2textBox.Text;
            songData.Value3 = condition3textBox.Text;
            songData.Value4 = condition4textBox.Text;
            songData.Value5 = condition5textBox.Text;
        }
        private void saveResultData(TotalResultItemJudgementData songData)
        {
            int i;
            long k;
            List<string> list = new List<string>();
            int.TryParse(resultItemIdTextBox.Text, out i);
            songData.ItemId = i;
            long.TryParse(resultStartTimeTextBox.Text, out k);
            songData.ConditionGetableStartTime = k;
            long.TryParse(resultEndTimeTextBox.Text, out k);
            songData.ConditionGetableEndTime = k;
            list.Add(resultConditionTextBox.Text);
            songData.ConditionKeys = list;
        }
        private bool saveChangesInRam()
        {
            int.TryParse(conditionIdTextBox.Text, out currentSongId);
            if (currentSongId == 0)
                return false;
            // Go through each export to find the DataTable
            foreach (var export in ConditionTable.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    var songData = allConditions.FirstOrDefault(s => s.ConditionId == currentSongId);
                    if (songData == null)
                    {
                        return false;
                    }

                    saveSongData(songData);
                    int id;
                    foreach (var row in dataTable.Table.Data)
                    {
                        if (row is StructPropertyData rowStruct)
                        {
                            id = WaccaSongBrowser.GetFieldValue<int>(rowStruct, "ConditionId");
                            if (id != currentSongId)
                            {
                                continue;
                            }
                            // Lookup the song data you want to save (replace this with your data source)
                            WaccaSongBrowser.SetFieldValue(rowStruct, "bConditionLimitNowSeason", songData.bConditionLimitNowSeason);
                            WaccaSongBrowser.SetFieldValue(rowStruct, "ConditionType", songData.ConditionType);
                            WaccaSongBrowser.SetFieldValue(rowStruct, "Value1", songData.Value1);
                            WaccaSongBrowser.SetFieldValue(rowStruct, "Value2", songData.Value2);
                            WaccaSongBrowser.SetFieldValue(rowStruct, "Value3", songData.Value3);
                            WaccaSongBrowser.SetFieldValue(rowStruct, "Value4", songData.Value4);
                            WaccaSongBrowser.SetFieldValue(rowStruct, "Value5", songData.Value5);
                        }
                    }
                    if (TotalResultItemJudgementTable != null)
                    {
                        int.TryParse(resultItemIdTextBox.Text, out currentSongId);
                        foreach (var export1 in TotalResultItemJudgementTable.Exports)
                        {
                            if (export1 is DataTableExport dataTable1)
                            {
                                var conditionData = allResult.FirstOrDefault(s => currentSongId == s.ItemId);
                                if (conditionData == null)
                                {
                                    return false;
                                }

                                saveResultData(conditionData);
                                foreach (var row in dataTable1.Table.Data)
                                {
                                    if (row is StructPropertyData rowStruct)
                                    {
                                        id = WaccaSongBrowser.GetFieldValue<int>(rowStruct, "ItemId");
                                        if (id != currentSongId)
                                        {
                                            continue;
                                        }
                                        // Lookup the song data you want to save (replace this with your data source)
                                        WaccaSongBrowser.SetFieldValue(rowStruct, "ConditionGetableStartTime", conditionData.ConditionGetableStartTime);
                                        WaccaSongBrowser.SetFieldValue(rowStruct, "ConditionGetableEndTime", conditionData.ConditionGetableEndTime);
                                        WaccaSongBrowser.SetFieldValue(rowStruct, "ConditionKeys", conditionData.ConditionKeys);
                                    }
                                }
                            }
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        static int savecount = 0;
        private void saveButton_Click(object sender, EventArgs e)
        {
            if (saveChangesInRam())
            {
                saveLabel.Text = $"Saved {++savecount} times";
                ConditionTable.Write(conditionFile);
                if (TotalResultItemJudgementTable != null)
                {
                    TotalResultItemJudgementTable.Write(TotalResultItemJudgementTableFilePath);
                }
            }
            else
            {
                saveLabel.Text = "file not saved.";
            }
        }
        private void conditionInjectButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(conditionIdTextBox.Text, out int newId))
            {
                saveLabel.Text = "Invalid condition ID.";
                return;
            }
            if (newId == 0)
            {
                saveLabel.Text = "ID 0 is reserved.";
                return;
            }

            if (allConditions.Any(s => s.ConditionId == newId))
            {
                saveLabel.Text = $"Condition ID {newId} already exists.";
                return;
            }

            foreach (var export in ConditionTable.Exports)
            {
                if (!(export is DataTableExport dataTable))
                    continue;

                // Create ConditionData from UI
                var conditionData = new ConditionData();
                saveSongData(conditionData);
                conditionData.ConditionId = newId;

                // Build new row
                var newRow = new StructPropertyData
                {
                    Name = new FName(ConditionTable, new FString(newId.ToString())),
                    StructType = new FName(ConditionTable, new FString("ConditionData")),
                    Value = new List<PropertyData>()
                };

                newRow.Value.Add(new IntPropertyData(new FName(ConditionTable, "ConditionId")) { Value = conditionData.ConditionId });
                newRow.Value.Add(new BoolPropertyData(new FName(ConditionTable, "bConditionLimitNowSeason")) { Value = conditionData.bConditionLimitNowSeason });
                newRow.Value.Add(new IntPropertyData(new FName(ConditionTable, "ConditionType")) { Value = conditionData.ConditionType });
                newRow.Value.Add(new StrPropertyData(new FName(ConditionTable, "Value1")) { Value = (FString)conditionData.Value1 });
                newRow.Value.Add(new StrPropertyData(new FName(ConditionTable, "Value2")) { Value = (FString)conditionData.Value2 });
                newRow.Value.Add(new StrPropertyData(new FName(ConditionTable, "Value3")) { Value = (FString)conditionData.Value3 });
                newRow.Value.Add(new StrPropertyData(new FName(ConditionTable, "Value4")) { Value = (FString)conditionData.Value4 });
                newRow.Value.Add(new StrPropertyData(new FName(ConditionTable, "Value5")) { Value = (FString)conditionData.Value5 });

                // Insert at start
                dataTable.Table.Data.Insert(0, newRow);

                // Update in-memory list + UI
                allConditions.Insert(0, conditionData);
                currentSongId = conditionData.ConditionId;
                saveLabel.Text = $"Injected condition ID {newId}.";
                LoadUI(conditionData);
                saveChanges();
                return;
            }

            saveLabel.Text = "ConditionTable export not found.";
        }

        private void resultInjectButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(resultItemIdTextBox.Text, out int newId))
            {
                saveLabel.Text = "Invalid result item ID.";
                return;
            }
            if (newId == 0)
            {
                saveLabel.Text = "ID 0 is reserved.";
                return;
            }

            if (allResult.Any(s => s.ItemId == newId))
            {
                saveLabel.Text = $"Result item ID {newId} already exists.";
                return;
            }

            foreach (var export in TotalResultItemJudgementTable.Exports)
            {
                if (!(export is DataTableExport dataTable))
                    continue;

                // Create ResultData from UI
                var resultData = new TotalResultItemJudgementData();
                saveResultData(resultData);
                resultData.ItemId = newId;

                // Build new row
                var newRow = new StructPropertyData
                {
                    Name = new FName(TotalResultItemJudgementTable, new FString(newId.ToString())),
                    StructType = new FName(TotalResultItemJudgementTable, new FString("TotalResultItemJudgementData")),
                    Value = new List<PropertyData>()
                };

                newRow.Value.Add(new IntPropertyData(new FName(TotalResultItemJudgementTable, "ItemId")) { Value = resultData.ItemId });
                newRow.Value.Add(new Int64PropertyData(new FName(TotalResultItemJudgementTable, "ConditionGetableStartTime")) { Value = resultData.ConditionGetableStartTime });
                newRow.Value.Add(new Int64PropertyData(new FName(TotalResultItemJudgementTable, "ConditionGetableEndTime")) { Value = resultData.ConditionGetableEndTime });

                // Array of ConditionKeys
                var keysArray = new ArrayPropertyData(new FName(TotalResultItemJudgementTable, "ConditionKeys"))
                {
                    ArrayType = new FName(TotalResultItemJudgementTable, "StrProperty"),
                    Value = resultData.ConditionKeys.Select(k => (PropertyData)new StrPropertyData(new FName(TotalResultItemJudgementTable, "ConditionKeys")){ Value = (FString)k }).ToArray()};
                newRow.Value.Add(keysArray);

                // Insert at start
                dataTable.Table.Data.Insert(0, newRow);

                // Update in-memory list + UI
                allResult.Insert(0, resultData);
                currentSongId = resultData.ItemId;
                saveLabel.Text = $"Injected result item ID {newId}.";
                resultConditionTextBox.Text = string.Join(",", resultData.ConditionKeys);
                resultItemIdTextBox.Text = resultData.ItemId.ToString();
                resultStartTimeTextBox.Text = resultData.ConditionGetableStartTime.ToString();
                resultEndTimeTextBox.Text = resultData.ConditionGetableEndTime.ToString();
                resultSearchLabel.Text = "Showing item";
                UpdateImage(newId);
                saveChanges();
                return;
            }

            saveLabel.Text = "TotalResultItemJudgementTable export not found.";
        }


        private void ramSaveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ramSaveCheckBox.Checked)
            {
                saveChangesInRam();
            }
        }
    }
}
