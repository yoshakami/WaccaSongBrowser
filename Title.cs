using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using UAssetAPI;
using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.PropertyTypes.Structs;
using UAssetAPI.UnrealTypes;

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
        static UAsset GradePartsTable;
        static List<string> text = new List<string>();
        static List<string> textVanilla = new List<string>();
        static string messageFolder;
        static string gradeTablePath;
        static string gradePartsTablePath;
        static List<GradeData> allTitles = new List<GradeData>();
        static List<GradePartsTableData> allParts = new List<GradePartsTableData>();
        public static sbyte ReadGrade(string uassetPath)
        {
            gradeTablePath = uassetPath;
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
                            GradeData data = new GradeData
                            {
                                GradeId = id,
                                GradePartsId01 = WaccaSongBrowser.GetFieldValue<int>(rowStruct, "GradePartsId01"),
                                GradePartsId02 = WaccaSongBrowser.GetFieldValue<int>(rowStruct, "GradePartsId02"),
                                GradePartsId03 = WaccaSongBrowser.GetFieldValue<int>(rowStruct, "GradePartsId03"),
                                GradeRarity = WaccaSongBrowser.GetFieldValue<byte>(rowStruct, "GradeRarity"),
                                NameTag = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "NameTag"),
                                ExplanationTextTag = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "ExplanationTextTag"),
                                ItemActivateStartTime = WaccaSongBrowser.GetFieldValue<long>(rowStruct, "ItemActivateStartTime"),
                                ItemActivateEndTime = WaccaSongBrowser.GetFieldValue<long>(rowStruct, "ItemActivateEndTime"),
                                bIsInitItem = WaccaSongBrowser.GetFieldValue<bool>(rowStruct, "bIsInitItem"),
                                GainWaccaPoint = WaccaSongBrowser.GetFieldValue<int>(rowStruct, "GainWaccaPoint"),
                            };
                            allTitles.Add(data);
                        }
                    }
                }
            }
            messageFolder = Path.GetDirectoryName(gradeTablePath);
            gradePartsTablePath = Path.Combine(messageFolder, "GradePartsTable.uasset");
            if (!File.Exists(gradePartsTablePath)) return -1;
            // Load the asset (assumes .uexp is in the same folder)
            GradePartsTable = new UAsset(gradePartsTablePath, UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_19);
            // Go through each export to find the DataTable
            foreach (var export in GradePartsTable.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    foreach (var row in dataTable.Table.Data)
                    {
                        string ConditionId = row.Name.ToString();              // <-- "010010101"
                        if (row is StructPropertyData rowStruct)
                        {
                            id = WaccaSongBrowser.GetFieldValue<int>(rowStruct, "GradePartsId");
                            if (id == 0) return -1;
                            //text.Add(WaccaSongBrowser.GetFieldValue<string>(rowStruct, "NameTag"));
                            GradePartsTableData data = new GradePartsTableData
                            {
                                GradePartsId = id,
                                GradePartsType = WaccaSongBrowser.GetFieldValue<int>(rowStruct, "GradePartsType"),
                                NameTag = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "NameTag"),
                                ExplanationTextTag = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "ExplanationTextTag"),
                                ItemActivateStartTime = WaccaSongBrowser.GetFieldValue<long>(rowStruct, "ItemActivateStartTime"),
                                ItemActivateEndTime = WaccaSongBrowser.GetFieldValue<long>(rowStruct, "ItemActivateEndTime"),
                                bIsInitItem = WaccaSongBrowser.GetFieldValue<bool>(rowStruct, "bIsInitItem"),
                                GainWaccaPoint = WaccaSongBrowser.GetFieldValue<int>(rowStruct, "GainWaccaPoint"),
                            };
                            allParts.Add(data);
                        }
                    }
                }
            }
            return 0;
        }
        static int currentIconId = 0;
        private void validateButton_Click(object sender, EventArgs e)
        {
            if (allTitles.Count == 0)
                return;
            saveLabel.Text = "";
            saveChanges();
            int.TryParse(gradeid.Text, out currentIconId);
            if (currentIconId == 0)
            {
                saveLabel.Text = "ID 0 cannot be used";
                saveLabel.Text = "";
                return;
            }
            int currentIndex = allTitles.FindIndex(s => s.GradeId == currentIconId);
            if (currentIndex == -1)
            {
                // consoleLabel.Text = "Creating new song ID. Will be saved upon next click on save, or on next Song ID change if autosave is on"; // use WSongInject instead
                saveLabel.Text = $"ID {currentIconId} not found";
                saveLabel.Text = "";
                return;
            }
            LoadUI(allTitles[currentIndex]);
        }
        private void LoadUI(GradeData icon)
        {
            gradeidTextBox.Text = icon.GradeId.ToString();
            if (!gradePartsId01freezeCheckBox.Checked)
                gradePartsId01TextBox.Text = icon.GradePartsId01.ToString();
            if (!gradePartsId02freezeCheckBox.Checked)
                gradePartsId02TextBox.Text = icon.GradePartsId02.ToString();
            if (!gradePartsId03freezeCheckBox.Checked)
                gradePartsId03TextBox.Text = icon.GradePartsId03.ToString();
            if (!gradeRarityfreezeCheckBox.Checked)
                gradeRarityTextBox.Text = icon.GradeRarity.ToString();
            if (!nameTagfreezeCheckBox.Checked)
                nameTagTextBox.Text = icon.NameTag;
            if (!ExplanationTextTagfreezeCheckBox.Checked)
                ExplanationTextTagTextBox.Text = icon.ExplanationTextTag;
            if (!itemActivateStartTimefreezeCheckBox.Checked)
                itemActivateStartTimeTextBox.Text = icon.ItemActivateStartTime.ToString();
            if (!itemActivateEndTimefreezeCheckBox.Checked)
                itemActivateEndTimeTextBox.Text = icon.ItemActivateEndTime.ToString();
            if (!bIsInitItemfreezeCheckBox.Checked)
                bIsInitItem.Checked = icon.bIsInitItem;
            if (!gainWaccaPointfreezeCheckBox.Checked)
                gainWaccaPointTextBox.Text = icon.GainWaccaPoint.ToString();
        }

        private void nextButton_Click(object sender, EventArgs e)
        {

            if (gradeid.SelectedIndex >= gradeid.Items.Count - 1)
                gradeid.SelectedIndex = 0;
            else
                gradeid.SelectedIndex += 1;
            validateButton_Click(null, null);
        }

        private void previousButton_Click(object sender, EventArgs e)
        {
            if (gradeid.SelectedIndex <= 0)
                gradeid.SelectedIndex = gradeid.Items.Count - 1;
            else
                gradeid.SelectedIndex -= 1;
            validateButton_Click(null, null);
        }
        string filter;
        int gainF; // temp memory allocated so the program avoids memory leak, there is no other func than search who use them.
        byte rarityF;
        long timeF;
        bool isinitF;
        private void searchButton_Click(object sender, EventArgs e)
        {
            IEnumerable<GradeData> filtered = allTitles;

            // --- text filters ---
            if (filterNameTagCheckBox.Checked)
            {
                filter = filterNameTagTextBox.Text.Trim().ToLower();
                if (!string.IsNullOrEmpty(filter))
                    filtered = filtered.Where(song => song.NameTag != null &&
                                                      song.NameTag.ToLower().Contains(filter));
            }
            if (filterexplanationTextTagCheckBox.Checked)
            {
                filter = filterexplanationTextTagTextBox.Text.Trim().ToLower();
                if (!string.IsNullOrEmpty(filter))
                    filtered = filtered.Where(song => song.ExplanationTextTag != null &&
                                                      song.ExplanationTextTag.ToLower().Contains(filter));
            }

            // --- number filters ---
            if (filtergradePartsId01CheckBox.Checked)
                filtered = filtered.Where(song => int.TryParse(filtergradePartsId01TextBox.Text, out gainF) && song.GradePartsId01 == gainF);
            if (filtergradePartsId02CheckBox.Checked)
                filtered = filtered.Where(song => int.TryParse(filtergradePartsId02TextBox.Text, out gainF) && song.GradePartsId02 == gainF);
            if (filtergradePartsId03CheckBox.Checked)
                filtered = filtered.Where(song => int.TryParse(filtergradePartsId03TextBox.Text, out gainF) && song.GradePartsId03 == gainF);
            if (filtergradeRarityCheckBox.Checked)
                filtered = filtered.Where(song => byte.TryParse(filtergradeRarityTextBox.Text, out rarityF) && song.GradeRarity == rarityF);
            if (filteritemActivateStartTimeCheckBox.Checked)
                filtered = filtered.Where(song => long.TryParse(filteritemActivateStartTimeTextBox.Text, out timeF) && song.ItemActivateStartTime == timeF);
            if (filteritemActivateEndTimeCheckBox.Checked)
                filtered = filtered.Where(song => long.TryParse(filteritemActivateEndTimeTextBox.Text, out timeF) && song.ItemActivateEndTime == timeF);
            if (filterGainWaccaPointCheckBox.Checked)
                filtered = filtered.Where(song => int.TryParse(filterGainWaccaPointTextBox.Text, out gainF) && song.GainWaccaPoint == gainF);
            if (filterbIsInitItemEnableCb.Checked)
                filtered = filtered.Where(song => song.bIsInitItem == filterbIsInitItemCheckBox.Checked);

            // --- Final result ---
            var resultList = filtered.ToList();
            if (resultList.Count == 0)
            {
                searchLabel.Text = "No match.";
                showingItemLabel.Text = $"Showing Result 0/0";
                return;
            }

            // Option A: Show first match immediately
            var firstSong = resultList.First();
            if (resultList.Count > 1)
                searchLabel.Text = $"{resultList.Count} matches!";
            else
                searchLabel.Text = $"{resultList.Count} match!";
            saveChanges();
            currentIconId = firstSong.GradeId;
            LoadUI(firstSong);


            // Option B: keep resultList for navigation (next/previous)
            // store in a field:
            filteredSongs = resultList;
            filteredSongsSelectedIndex = 0;
            showingItemLabel.Text = $"Showing Result 1/{filteredSongs.Count}";
        }

        private void filterInvertMatchesButton_Click(object sender, EventArgs e)
        {
            IEnumerable<GradeData> filtered = allTitles;

            // Exclude songs already in filteredSongs
            filtered = filtered.Where(song => !filteredSongs.Any(fs => fs.GradeId == song.GradeId));

            // --- Final result ---
            var resultList = filtered.ToList();
            if (resultList.Count == 0)
            {
                searchLabel.Text = "No match.";
                showingItemLabel.Text = $"Showing Result 0/0";
                return;
            }

            // Option A: Show first match immediately
            var firstSong = resultList.First();
            if (resultList.Count > 1)
                searchLabel.Text = $"{resultList.Count} matches!";
            else
                searchLabel.Text = $"{resultList.Count} match!";

            saveChanges();
            currentIconId = firstSong.GradeId;
            LoadUI(firstSong);

            // Option B: keep resultList for navigation (next/previous)
            filteredSongs = resultList;
            filteredSongsSelectedIndex = 0;
            showingItemLabel.Text = $"Showing Result 1/{filteredSongs.Count}";
        }

        static List<GradeData> filteredSongs = new List<GradeData>();
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
            currentIconId = filteredSongs[filteredSongsSelectedIndex].GradeId;
            LoadUI(filteredSongs[filteredSongsSelectedIndex]);
            showingItemLabel.Text = $"Showing Result {filteredSongsSelectedIndex + 1}/{filteredSongs.Count}";
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
            currentIconId = filteredSongs[filteredSongsSelectedIndex].GradeId;
            LoadUI(filteredSongs[filteredSongsSelectedIndex]);
            showingItemLabel.Text = $"Showing Result {filteredSongsSelectedIndex + 1}/{filteredSongs.Count}";
        }

        private void injectNewButton_Click(object sender, EventArgs e)
        {

        }
        private bool saveChangesInRam()
        {
            return true;
        }
        private void ramSaveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ramSaveCheckBox.Checked)
            {
                saveChangesInRam();
            }
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
        static int savecount = 0;
        private void saveButton_Click(object sender, EventArgs e)
        {
            saveChangesInRam();
            saveLabel.Text = $"Saved {++savecount} times";
            GradeTable.Write(gradeTablePath);
        }
        private void autoSaveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autoSaveCheckBox.Checked == true)
            {
                saveChanges();
            }
        }
        private void injectWaccaGradeButton_Click(object sender, EventArgs e)
        {
            saveLabel.Text = "Processing...";
            messageFolder = Path.GetDirectoryName(gradeTablePath);
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
                UAsset asset = new UAsset(EngineVersion.VER_UE4_19);

                // Create DataTable object
                var tableObjectPart = new UDataTable();
                var dataTablePart = new DataTableExport(tableObjectPart, asset, Array.Empty<byte>());
                int i = 107001;
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
                                bool bIsInitItem = WaccaSongBrowser.GetFieldValue<bool>(rowStruct, "bIsInitItem");
                                string vanillaTag = index < textVanilla.Count ? textVanilla[index] : null;
                                string newTag = index < text.Count ? text[index] : currentTag;
                                WaccaSongBrowser.SetFieldValue(rowStruct, "GradePartsId01", i);
                                WaccaSongBrowser.SetFieldValue(rowStruct, "GradePartsId02", 0);
                                WaccaSongBrowser.SetFieldValue(rowStruct, "GradePartsId03", 0);
                                WaccaSongBrowser.SetFieldValue(rowStruct, "NameTag", "null");
                                // Replace only if the current tag matches the vanilla one
                                //if (vanillaTag != null && currentTag == vanillaTag)
                                //{
                                //    WaccaSongBrowser.SetFieldValue(rowStruct, "NameTag", newTag);
                                //}
                                index++;


                                var dummy = new FName(asset, "GradePartsTableData");

                                var rowPart = new StructPropertyData(new FName(asset, i.ToString()), new FName(asset, "GradePartsTableData"))
                                {
                                    Value = new List<PropertyData>
                                    {
                                        new IntPropertyData(new FName(asset, "GradePartsId")) { Value = i },
                                        new IntPropertyData(new FName(asset, "GradePartsType")) { Value = 0 },
                                        new StrPropertyData(new FName(asset, "NameTag")) { Value = (FString)newTag },
                                        // Use empty string instead of null to avoid NullReferenceException
                                        new StrPropertyData(new FName(asset, "ExplanationTextTag")) { Value = new FString("") },
                                        new Int64PropertyData(new FName(asset, "ItemActivateStartTime")) { Value = 0 },
                                        new Int64PropertyData(new FName(asset, "ItemActivateEndTime")) { Value = 0 },
                                        new BoolPropertyData(new FName(asset, "bIsInitItem")) { Value = bIsInitItem },
                                        new IntPropertyData(new FName(asset, "GainWaccaPoint")) { Value = 0 }
                                    }
                                };

                                i++;
                                if (i > 407999)
                                {
                                    i = 507001;
                                }
                                else if (i > 307999 && i < 309000)
                                {
                                    i = 407001;
                                }
                                else if (i > 207999 && i < 209000)
                                {
                                    i = 307001;
                                }
                                else if (i > 107999 && i < 109000)
                                {
                                    i = 207001;
                                }
                                dataTablePart.Table.Data.Add(rowPart);
                            }
                        }
                    }
                }

                GradeTable.Write(Path.Combine(messageFolder, "GradeTableNew.uasset"));
                asset.Exports.Add(dataTablePart);
                asset.Write(Path.Combine(messageFolder, "GradePartsTableNew.uasset"));
                saveLabel.Text = "Successfully injected Titles.txt into GradeTableNew.uasset and GradePartsTableNew.uasset";
            }
            else
            {
                saveLabel.Text = "Missing Titles.txt or TitlesVanilla.txt. Please click the create button first.";
            }
        }
        public void CreateGradePartsTable(string outputPath)
        {
            UAsset asset = new UAsset(EngineVersion.VER_UE4_19);

            // Create DataTable object
            var tableObject = new UDataTable();
            var dataTable = new DataTableExport(tableObject, asset, Array.Empty<byte>());

            // Generate rows
            for (int i = 107001; i <= 107999; i++)
            {
                // StructType is set via second constructor argument
                var row = new StructPropertyData(new FName(asset, i.ToString()), new FName(asset, "GradePartsTableData"))
                {
                    Value = new List<PropertyData>
            {
                new IntPropertyData(new FName(asset, "GradePartsId")) { Value = i },
                new IntPropertyData(new FName(asset, "GradePartsType")) { Value = 1 },
                new StrPropertyData(new FName(asset, "NameTag")) { Value = (FString)$"GradePart_{i}" },
                new StrPropertyData(new FName(asset, "ExplanationTextTag")) { Value = (FString)$"GradePartExp_{i}" },
                new Int64PropertyData(new FName(asset, "ItemActivateStartTime")) { Value = 0 },
                new Int64PropertyData(new FName(asset, "ItemActivateEndTime")) { Value = 0 },
                new BoolPropertyData(new FName(asset, "bIsInitItem")) { Value = false },
                new IntPropertyData(new FName(asset, "GainWaccaPoint")) { Value = 0 }
            }
                };

                dataTable.Table.Data.Add(row);
            }

            asset.Exports.Add(dataTable);
            asset.Write(outputPath);
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

        private void createWaccaGradeButton_Click(object sender, EventArgs e)
        {
            saveLabel.Text = "Processing...";
            messageFolder = Path.GetDirectoryName(gradeTablePath);
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
