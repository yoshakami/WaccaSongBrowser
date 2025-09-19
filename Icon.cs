using System.IO;
using UAssetAPI;
using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Structs;

namespace WaccaSongBrowser
{
    public partial class IconTab : UserControl
    {
        public IconTab()
        {
            InitializeComponent();
            filtericonTextureNameCheckBox_CheckedChanged(null, null);
            filtericonRarityCheckBox_CheckedChanged(null, null);
            filteritemActivateStartTimeCheckBox_CheckedChanged(null, null);
            filteritemActivateEndTimeCheckBox_CheckedChanged(null, null);
            filterGainWaccaPointCheckBox_CheckedChanged(null, null);
            filtericonNameCheckBox_CheckedChanged(null, null);
            filtericonAcquisitionWayCheckBox_CheckedChanged(null, null);
            filterNameTagCheckBox_CheckedChanged(null, null);
            filterexplanationTextTagCheckBox_CheckedChanged(null, null);
            filterbIsInitItemEnableCb_CheckedChanged(null, null);
            foreach (IconData data in allIcons)
            {
                iconid.Items.Add(data.IconId.ToString());
            }
            nextButton_Click(null, null); // read returns -1 if allconditions is empty
        }
        static UAsset IconTable;
        static string filePath;
        static List<IconData> allIcons = new List<IconData>();
        public static sbyte ReadIcon(string uassetPath)
        {
            filePath = uassetPath;
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
                            IconData data = new IconData
                            {
                                IconId = id,
                                IconTextureName = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "IconTextureName"),
                                IconRarity = WaccaSongBrowser.GetFieldValue<byte>(rowStruct, "IconRarity"),
                                NameTag = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "NameTag"),
                                ExplanationTextTag = WaccaSongBrowser.GetFieldValue<string>(rowStruct, "ExplanationTextTag"),
                                ItemActivateStartTime = WaccaSongBrowser.GetFieldValue<long>(rowStruct, "ItemActivateStartTime"),
                                ItemActivateEndTime = WaccaSongBrowser.GetFieldValue<long>(rowStruct, "ItemActivateEndTime"),
                                bIsInitItem = WaccaSongBrowser.GetFieldValue<bool>(rowStruct, "bIsInitItem"),
                                GainWaccaPoint = WaccaSongBrowser.GetFieldValue<int>(rowStruct, "GainWaccaPoint"),
                            };
                            allIcons.Add(data);
                        }
                    }
                }
            }
            return 0;
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
        static int currentIconId;
        private void validateButton_Click(object sender, EventArgs e)
        {
            if (allIcons.Count == 0)
                return;
            saveLabel.Text = "";
            saveChanges();
            int.TryParse(iconid.Text, out currentIconId);
            if (currentIconId == 0)
            {
                saveLabel.Text = "ID 0 cannot be used";
                saveLabel.Text = "";
                return;
            }
            int currentIndex = allIcons.FindIndex(s => s.IconId == currentIconId);
            if (currentIndex == -1)
            {
                // consoleLabel.Text = "Creating new song ID. Will be saved upon next click on save, or on next Song ID change if autosave is on"; // use WSongInject instead
                saveLabel.Text = $"ID {currentIconId} not found";
                saveLabel.Text = "";
                return;
            }
            LoadUI(allIcons[currentIndex]);
        }
        private void LoadUI(IconData icon)
        {
            iconIdTextBox.Text = icon.IconId.ToString();
            if (!iconTextureNamefreezeCheckBox.Checked)
                iconTextureNameTextBox.Text = icon.IconTextureName;
            if (!iconRarityfreezeCheckBox.Checked)
                iconRarityTextBox.Text = icon.IconRarity.ToString();
            if (!nameTagfreezeCheckBox.Checked)
                nameTagTextBox.Text = icon.NameTag;
            if (!explanationTextTagfreezeCheckBox.Checked)
                explanationTextTagTextBox.Text = icon.ExplanationTextTag;
            if (!itemActivateStartTimefreezeChechBox.Checked)
                itemActivateStartTimeTextBox.Text = icon.ItemActivateStartTime.ToString();
            if (!ItemActivateEndTimefreezeCheckBox.Checked)
                itemActivateEndTimeTextBox.Text = icon.ItemActivateEndTime.ToString();
            if (!bIsInitItemfreezeCheckBox.Checked)
                bIsInitItem.Checked = icon.bIsInitItem;
            if (!gainWaccaPointfreezeCheckBox.Checked)
                gainWaccaPointTextBox.Text = icon.GainWaccaPoint.ToString();
            path = $"{execPath}icons/{iconTextureNameTextBox.Text.Substring(iconTextureNameTextBox.Text.IndexOf('/') + 1)}.png";
            if (File.Exists(path))
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var ms = new MemoryStream())
                {
                    fs.CopyTo(ms);                  // Copy file into memory
                    ms.Position = 0;                // Reset position to beginning
                    iconPictureBox.Image = Image.FromStream(ms); // Load image from memory
                }
            }
        }
        static readonly string execPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/");
        static string path;
        private void nextButton_Click(object sender, EventArgs e)
        {

            if (iconid.SelectedIndex >= iconid.Items.Count - 1)
                iconid.SelectedIndex = 0;
            else
                iconid.SelectedIndex += 1;
            validateButton_Click(null, null);
        }

        private void previousButton_Click(object sender, EventArgs e)
        {
            if (iconid.SelectedIndex <= 0)
                iconid.SelectedIndex = iconid.Items.Count - 1;
            else
                iconid.SelectedIndex -= 1;
            validateButton_Click(null, null);
        }
        private void injectNewButton_Click(object sender, EventArgs e)
        {

        }

        private void createNewIconButton_Click(object sender, EventArgs e)
        {

        }
        string filter;
        int gainF; // temp memory allocated so the program avoids memory leak, there is no other func than search who use them.
        byte rarityF;
        long timeF;
        bool isinitF;
        private void searchButton_Click(object sender, EventArgs e)
        {
            IEnumerable<IconData> filtered = allIcons;

            // --- text filters ---
            if (filtericonTextureNameCheckBox.Checked)
            {
                filter = filtericonTextureNameTextBox.Text.Trim().ToLower();
                if (!string.IsNullOrEmpty(filter))
                    filtered = filtered.Where(song => song.IconTextureName != null &&
                                                      song.IconTextureName.ToLower().Contains(filter));
            }
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
            if (filtericonRarityCheckBox.Checked)
                filtered = filtered.Where(song => byte.TryParse(filtericonRarityTextBox.Text, out rarityF) && song.IconRarity == rarityF);
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
            currentIconId = firstSong.IconId;
            LoadUI(firstSong);


            // Option B: keep resultList for navigation (next/previous)
            // store in a field:
            filteredSongs = resultList;
            filteredSongsSelectedIndex = 0;
            showingItemLabel.Text = $"Showing Result 1/{filteredSongs.Count}";
        }

        private void filterInvertMatchesButton_Click(object sender, EventArgs e)
        {
            IEnumerable<IconData> filtered = allIcons;

            // Exclude songs already in filteredSongs
            filtered = filtered.Where(song => !filteredSongs.Any(fs => fs.IconId == song.IconId));

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
            currentIconId = firstSong.IconId;
            LoadUI(firstSong);

            // Option B: keep resultList for navigation (next/previous)
            filteredSongs = resultList;
            filteredSongsSelectedIndex = 0;
            showingItemLabel.Text = $"Showing Result 1/{filteredSongs.Count}";
        }

        static List<IconData> filteredSongs = new List<IconData>();
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
            currentIconId = filteredSongs[filteredSongsSelectedIndex].IconId;
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
            currentIconId = filteredSongs[filteredSongsSelectedIndex].IconId;
            LoadUI(filteredSongs[filteredSongsSelectedIndex]);
            showingItemLabel.Text = $"Showing Result {filteredSongsSelectedIndex + 1}/{filteredSongs.Count}";
        }
        private void saveChangesInRam()
        { 
        }
        private void ramSaveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ramSaveCheckBox.Checked)
            {
                saveChangesInRam();
            }
        }

        static int savecount = 0;
        private void saveButton_Click(object sender, EventArgs e)
        {
            saveChangesInRam();
            saveLabel.Text = $"Saved {++savecount} times";
            IconTable.Write(filePath);
        }
        private void autoSaveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autoSaveCheckBox.Checked == true)
            {
                saveChanges();
            }
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
