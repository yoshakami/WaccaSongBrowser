using System.Globalization;
using System.Text;
using UAssetAPI;
using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.PropertyTypes.Structs;
using UAssetAPI.UnrealTypes;

namespace WaccaSongBrowser
{
    public partial class WaccaSongBrowser : Form
    {
        //string[] WaccaVersions = { "1: Wacca", "2: Wacca S", "3: Wacca Lily", "4: Wacca Lily R", "5: Wacca Reverse", "6: Hidden" };
        string[] WaccaVersions = { "Luxance", "Takahashi", "Tiger", "Snowball", "Twis", "6: Hidden" };

        // string[] ScoreGenre = { "0: Anime/Pop", "1: Vocaloid", "2: Touhou", "3: 2.5D", "4: Variety", "5: Original", "6: Tano*C" };
        string[] ScoreGenre = { "0: Anime/Pop", "1: Vocaloid", "2: Touhou", "3: Hot Picks", "4: Variety", "5: Nanahira", "6: Tano*C", "7: Hidden" };

        public WaccaSongBrowser()
        {
            // I use UserControls for other pages. they are inside panelMainContainer
            // see right under this function
            InitializeComponent();
            FillLists();
            this.Size = new Size(1354, 720);
            // load the "main page"
            LoadPage(new Menu());
        }
        private void LoadPage(UserControl page)
        {
            panelMainContainer.Controls.Clear();
            page.Dock = DockStyle.Fill;
            panelMainContainer.Controls.Add(page);
        }
        static string openedFileName;
        private void WaccaSongBrowser_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
                string path2;
                foreach (var path in paths)
                {
                    try
                    {
                        if (Directory.Exists(path))
                        {
                            // It's a folder
                            string folderName = new DirectoryInfo(path).Name;
                            //MessageBox.Show("Dropped a folder: " + folderName);
                            if (folderName.ToLower() == "message")
                            {
                                panelMainContainer.Visible = true;
                                panelMainContainer.Enabled = true;
                                LoadPage(new Message(path));
                            }
                            else // if (folderName.ToLower() == "table")
                            {
                                var files = Directory.GetFiles(path, "MusicParameterTable.uasset"); // scan inside for the .uasset file:
                                if (files.Length > 0)
                                {
                                    string file = files[0]; // or let user choose
                                    if (Read(file) == -1)
                                    {
                                        return;
                                    }
                                    openedFileName = file;
                                    consoleLabel.Text = $"Folder dropped: {folderName}, loaded: {Path.GetFileName(file)}";
                                    panelMainContainer.Visible = false;
                                    panelMainContainer.Enabled = false;
                                    return;
                                }
                            }
                        }
                        else if (File.Exists(path))
                        {
                            path2 = path;
                            // It's a file
                            allSongs.Clear();
                            songid.Items.Clear();
                            if (path2.EndsWith(".uexp"))
                                path2 = path2.Substring(0, path2.Length - 4) + "uasset";
                            if (Read(path2) != -1)  // it is MusicParameterTable!!!! (or UnlockMusicTable)
                            {
                                openedFileName = path2;
                                consoleLabel.Text = "File loaded successfully! -> will overwrite on next save";
                                panelMainContainer.Visible = false;
                                panelMainContainer.Enabled = false;
                            }
                            // read ConditionTable
                            else if (Condition.Read(path2) != -1)
                            {
                                panelMainContainer.Visible = true;
                                panelMainContainer.Enabled = true;
                                LoadPage(new Condition());  // Load Condition UI
                            }
                            else if (Message.ReadTrophy(path2) != -1)
                            {
                                panelMainContainer.Visible = true;
                                panelMainContainer.Enabled = true;
                                LoadPage(new Message(path2, "trophy"));
                            }
                            else if (Message.ReadGrade(path2) != -1)
                            {
                                panelMainContainer.Visible = true;
                                panelMainContainer.Enabled = true;
                                LoadPage(new Message(path2, "grade"));
                            }
                            else
                            {


                                // TODO:
                                // read IconTable
                                panelMainContainer.Visible = true;
                                panelMainContainer.Enabled = true;
                                LoadPage(new Message(path2));
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        panelMainContainer.Visible = true;
                        panelMainContainer.Enabled = true;
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
            }
        }

        private void WaccaSongBrowser_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void checkAllCorrectBoxesButton_Click(object sender, EventArgs e)
        {
            offlineCheckBox.Checked = true;
            jaCheckBox.Checked = true;
            usaCheckBox.Checked = true;
            zhtwCheckBox.Checked = true;
            enhkCheckBox.Checked = true;
            ensgCheckBox.Checked = true;
            kokrCheckBox.Checked = true;
            cnguCheckBox.Checked = true;
            cngeCheckBox.Checked = true;
            cnvipCheckBox.Checked = true;
            notAvailableCheckBox.Checked = false;
        }
        private void filterMusicButton_Click(object sender, EventArgs e)
        {
            string filter = filterMusicTextBox.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(filter) || allSongs.Count == 0)
                return;

            // Filter songs by name
            var matchingSongs = allSongs
                .Where(song => song.MusicMessage != null && song.MusicMessage.ToLower().Contains(filter))
                .ToList();

            if (matchingSongs.Count == 0)
                return; // No match found

            // Find the index of the currently loaded song in the global list
            int currentIndex = allSongs.FindIndex(s => s.UniqueID == currentSongId);

            // Try to find the next matching song after the current one
            SongData nextMatch = null;

            for (int i = 1; i <= allSongs.Count; i++)
            {
                // Loop through indices starting from currentIndex + 1, wrapping around
                int index = (currentIndex + i) % allSongs.Count;

                var song = allSongs[index];
                if (song.MusicMessage != null && song.MusicMessage.ToLower().Contains(filter))
                {
                    nextMatch = song;
                    break;
                }
            }

            if (nextMatch != null)
            {
                saveChanges();
                currentSongId = nextMatch.UniqueID;
                LoadUI(nextMatch);
            }
        }

        private void filterArtistButton_Click(object sender, EventArgs e)
        {
            string filter = filterArtistTextBox.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(filter) || allSongs.Count == 0)
                return;

            // Filter songs by name
            var matchingSongs = allSongs
                .Where(song => song.ArtistMessage != null && song.ArtistMessage.ToLower().Contains(filter))
                .ToList();

            if (matchingSongs.Count == 0)
                return; // No match found

            // Find the index of the currently loaded song in the global list
            int currentIndex = allSongs.FindIndex(s => s.UniqueID == currentSongId);

            // Try to find the next matching song after the current one
            SongData nextMatch = null;

            for (int i = 1; i <= allSongs.Count; i++)
            {
                // Loop through indices starting from currentIndex + 1, wrapping around
                int index = (currentIndex + i) % allSongs.Count;

                var song = allSongs[index];
                if (song.ArtistMessage != null && song.ArtistMessage.ToLower().Contains(filter))
                {
                    nextMatch = song;
                    break;
                }
            }

            if (nextMatch != null)
            {
                saveChanges();
                currentSongId = nextMatch.UniqueID;
                LoadUI(nextMatch);
            }
        }

        private void filterGenreButton_Click(object sender, EventArgs e)
        {
            FilterByIntProperty("ScoreGenre", filterGenre.SelectedIndex);
        }
        private void filterVersionButton_Click(object sender, EventArgs e)
        {
            FilterByuIntProperty("Version", (uint)filterVersion.SelectedIndex + 1);
        }
        private void FilterByuIntProperty(string propertyName, uint expectedValue)
        {
            if (allSongs.Count == 0)
                return;

            // Find the current index in the list
            int currentIndex = allSongs.FindIndex(s => s.UniqueID == currentSongId);

            for (int i = 1; i <= allSongs.Count; i++)
            {
                int index = (currentIndex + i) % allSongs.Count;
                var song = allSongs[index];

                uint? propValue = GetuIntProperty(song, propertyName);

                if (propValue == expectedValue)
                {
                    saveChanges();
                    currentSongId = song.UniqueID;
                    LoadUI(song);
                    break;
                }
            }
        }
        private uint? GetuIntProperty(SongData song, string propertyName)
        {
            var propInfo = typeof(SongData).GetProperty(propertyName);
            if (propInfo != null && propInfo.PropertyType == typeof(uint))
            {
                return (uint)propInfo.GetValue(song);
            }

            var fieldInfo = typeof(SongData).GetField(propertyName);
            if (fieldInfo != null && fieldInfo.FieldType == typeof(uint))
            {
                return (uint)fieldInfo.GetValue(song);
            }

            return null; // Not found or not an int
        }
        private void FilterByIntProperty(string propertyName, int expectedValue)
        {
            if (allSongs.Count == 0)
                return;

            // Find the current index in the list
            int currentIndex = allSongs.FindIndex(s => s.UniqueID == currentSongId);

            for (int i = 1; i <= allSongs.Count; i++)
            {
                int index = (currentIndex + i) % allSongs.Count;
                var song = allSongs[index];

                int? propValue = GetIntProperty(song, propertyName);

                if (propValue == expectedValue)
                {
                    saveChanges();
                    currentSongId = song.UniqueID;
                    LoadUI(song);
                    break;
                }
            }
        }
        private int? GetIntProperty(SongData song, string propertyName)
        {
            var propInfo = typeof(SongData).GetProperty(propertyName);
            if (propInfo != null && propInfo.PropertyType == typeof(int))
            {
                return (int)propInfo.GetValue(song);
            }

            var fieldInfo = typeof(SongData).GetField(propertyName);
            if (fieldInfo != null && fieldInfo.FieldType == typeof(int))
            {
                return (int)fieldInfo.GetValue(song);
            }

            return null; // Not found or not an int
        }

        private void FilterByBoolProperty(string propertyName, bool expectedValue)
        {
            if (allSongs.Count == 0)
                return;

            // Find current index
            int currentIndex = allSongs.FindIndex(s => s.UniqueID == currentSongId);

            for (int i = 1; i <= allSongs.Count; i++)
            {
                int index = (currentIndex + i) % allSongs.Count;
                var song = allSongs[index];

                // Use reflection or a dictionary to get the Bool value for that field
                bool? propValue = GetBoolProperty(song, propertyName);

                if (propValue == expectedValue)
                {
                    saveChanges();
                    currentSongId = song.UniqueID;
                    LoadUI(song);
                    break;
                }
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
        private bool? GetBoolProperty(SongData song, string propertyName)
        {
            var propInfo = typeof(SongData).GetProperty(propertyName);
            if (propInfo != null && propInfo.PropertyType == typeof(bool))
            {
                return (bool)propInfo.GetValue(song);
            }

            var fieldInfo = typeof(SongData).GetField(propertyName);
            if (fieldInfo != null && fieldInfo.FieldType == typeof(bool))
            {
                return (bool)fieldInfo.GetValue(song);
            }

            return null; // Not found or not a bool
        }
        private void filterjaButton_Click(object sender, EventArgs e)
        {
            FilterByBoolProperty("ValidCulture_ja_JP", filterjaCheckBox.Checked);
        }

        private void filterusaButton_Click(object sender, EventArgs e)
        {
            FilterByBoolProperty("ValidCulture_en_US", filterusaCheckBox.Checked);
        }

        private void filterzhtwButton_Click(object sender, EventArgs e)
        {
            FilterByBoolProperty("ValidCulture_zh_Hant_TW", filterzhtwCheckBox.Checked);
        }
        private void filterenhkButton_Click(object sender, EventArgs e)
        {
            FilterByBoolProperty("ValidCulture_en_HK", filterenhkCheckBox.Checked);
        }
        private void filterensgButton_Click(object sender, EventArgs e)
        {
            FilterByBoolProperty("ValidCulture_en_SG", filterensgCheckBox.Checked);
        }
        private void filterkokrButton_Click(object sender, EventArgs e)
        {
            FilterByBoolProperty("ValidCulture_ko_KR", filterkokrCheckBox.Checked);
        }
        private void filtercnguButton_Click(object sender, EventArgs e)
        {
            FilterByBoolProperty("ValidCulture_h_Hans_CN_Guest", filtercnguCheckBox.Checked);
        }
        private void filtercngeButton_Click(object sender, EventArgs e)
        {
            FilterByBoolProperty("ValidCulture_h_Hans_CN_GeneralMember", filtercngeCheckBox.Checked);
        }
        private void filterfiltercnvipButton_Click(object sender, EventArgs e)
        {
            FilterByBoolProperty("ValidCulture_h_Hans_CN_VipMember", filtercnvipCheckBox.Checked);
        }
        private void filterSearchButton_Click(object sender, EventArgs e)
        {
            IEnumerable<SongData> filtered = allSongs;

            // --- Text filters ---
            if (filterMusicEnableCheckBox.Checked)
            {
                string filter = filterMusicTextBox.Text.Trim().ToLower();
                if (!string.IsNullOrEmpty(filter))
                    filtered = filtered.Where(song => song.MusicMessage != null &&
                                                      song.MusicMessage.ToLower().Contains(filter));
            }

            if (filterArtistEnableCheckBox.Checked)
            {
                string filter = filterArtistTextBox.Text.Trim().ToLower();
                if (!string.IsNullOrEmpty(filter))
                    filtered = filtered.Where(song => song.ArtistMessage != null &&
                                                      song.ArtistMessage.ToLower().Contains(filter));
            }

            // --- Int/Uint filters ---
            if (filterGenreEnableCheckBox.Checked)
                filtered = filtered.Where(song => song.ScoreGenre == filterGenre.SelectedIndex);

            if (filterVersionEnableCheckBox.Checked)
                filtered = filtered.Where(song => song.Version == (uint)(filterVersion.SelectedIndex + 1));

            // --- Unlock/New filter ---
            if (filterNewEnableCheckBox.Checked)
            {
                filtered = filtered.Where(song =>
                    musicUnlockStatus.TryGetValue((int)song.UniqueID, out bool isNew) &&
                    isNew == filterNewCheckBox.Checked
                );
            }


            // --- Beginner filter ---
            if (filterBeginnerEnableCheckBox.Checked)
                filtered = filtered.Where(song => song.Recommend == filterBeginnerCheckBox.Checked);

            // --- has inferno
            if (filterInfernoEnableCheckBox.Checked)
            {
                if (filterInfernoCheckBox.Checked)
                    filtered = filtered.Where(song => song.DifficultyInfernoLv != 0);
                else
                    filtered = filtered.Where(song => song.DifficultyInfernoLv == 0);

            }
            // --- Offline=
            if (filterOfflineEnableCheckBox.Checked)
                filtered = filtered.Where(song => GetBoolProperty(song, "ValidCulture_Offline") == filterofflineCheckBox.Checked);
            if (filterjaEnableCheckBox.Checked)
                filtered = filtered.Where(song => GetBoolProperty(song, "ValidCulture_ja_JP") == filterjaCheckBox.Checked);


            // --- chart creatorzzzzzzzz
            if (filterCreatorNormalCheckBox.Checked)
            {
                string filter = filterCreatorNormalTextBox.Text.Trim().ToLower();
                if (!string.IsNullOrEmpty(filter))
                    filtered = filtered.Where(song => song.NotesDesignerNormal != null && song.NotesDesignerNormal.ToLower().Contains(filter));
            }
            if (filterCreatorHardCheckBox.Checked)
            {
                string filter = filterCreatorHardTextBox.Text.Trim().ToLower();
                if (!string.IsNullOrEmpty(filter))
                    filtered = filtered.Where(song => song.NotesDesignerHard != null && song.NotesDesignerHard.ToLower().Contains(filter));
            }
            if (filterCreatorExtremeCheckBox.Checked)
            {
                string filter = filterCreatorExtremeTextBox.Text.Trim().ToLower();
                if (!string.IsNullOrEmpty(filter))
                    filtered = filtered.Where(song => song.NotesDesignerExpert != null && song.NotesDesignerExpert.ToLower().Contains(filter));
            }
            if (filterCreatorInfernoCheckBox.Checked)
            {
                string filter = filterCreatorInfernoTextBox.Text.Trim().ToLower();
                if (!string.IsNullOrEmpty(filter))
                    filtered = filtered.Where(song => song.NotesDesignerInferno != null && song.NotesDesignerInferno.ToLower().Contains(filter));
            }
            // --- movie asset name normal
            if (filterMovieNormalCheckBox.Checked)
            {
                string filter = filterMovieNormalTextBox.Text.Trim().ToLower();
                if (!string.IsNullOrEmpty(filter))
                    filtered = filtered.Where(song => song.MovieAssetName != null && song.MovieAssetName.ToLower().Contains(filter));
            }
            // --- PointCost
            if (filterPointCostCheckBox.Checked)
            {
                if (int.TryParse(filterPointCostTextBox.Text.Trim().ToLower(), out int cost))
                {
                    filtered = filtered.Where(song => song.WaccaPointCost == cost);
                }
            }

            // --- Culture filters ---
            if (filterusaEnableCheckBox.Checked)
                filtered = filtered.Where(song => GetBoolProperty(song, "ValidCulture_en_US") == filterusaCheckBox.Checked);

            if (filterzhtwEnableCheckBox.Checked)
                filtered = filtered.Where(song => GetBoolProperty(song, "ValidCulture_zh_Hant_TW") == filterzhtwCheckBox.Checked);

            if (filterenhkEnableCheckBox.Checked)
                filtered = filtered.Where(song => GetBoolProperty(song, "ValidCulture_en_HK") == filterenhkCheckBox.Checked);

            if (filterensgEnableCheckBox.Checked)
                filtered = filtered.Where(song => GetBoolProperty(song, "ValidCulture_en_SG") == filterensgCheckBox.Checked);

            if (filterkokrEnableCheckBox.Checked)
                filtered = filtered.Where(song => GetBoolProperty(song, "ValidCulture_ko_KR") == filterkokrCheckBox.Checked);

            if (filtercnguEnableCheckBox.Checked)
                filtered = filtered.Where(song => GetBoolProperty(song, "ValidCulture_h_Hans_CN_Guest") == filtercnguCheckBox.Checked);

            if (filtercngeEnableCheckBox.Checked)
                filtered = filtered.Where(song => GetBoolProperty(song, "ValidCulture_h_Hans_CN_GeneralMember") == filtercngeCheckBox.Checked);

            if (filtercnvipEnableCheckBox.Checked)
                filtered = filtered.Where(song => GetBoolProperty(song, "ValidCulture_h_Hans_CN_VipMember") == filtercnvipCheckBox.Checked);

            if (filternotAvailableEnableCheckBox.Checked)
                filtered = filtered.Where(song => GetBoolProperty(song, "ValidCulture_NoneActive") == filternotAvailableCheckBox.Checked);

            // --- bingo filters ---
            if (filterBingoCheckBox0.Checked)
            {
                if (int.TryParse(filterBingoTextBox0.Text.Trim().ToLower(), out int bingo0))
                    filtered = filtered.Where(song => song.bingo0 == bingo0);
            }
            if (filterBingoCheckBox1.Checked)
            {
                if (int.TryParse(filterBingoTextBox1.Text.Trim().ToLower(), out int bingo1))
                    filtered = filtered.Where(song => song.bingo1 == bingo1);
            }
            if (filterBingoCheckBox2.Checked)
            {
                if (int.TryParse(filterBingoTextBox2.Text.Trim().ToLower(), out int bingo2))
                    filtered = filtered.Where(song => song.bingo2 == bingo2);
            }
            if (filterBingoCheckBox3.Checked)
            {
                if (int.TryParse(filterBingoTextBox3.Text.Trim().ToLower(), out int bingo3))
                    filtered = filtered.Where(song => song.bingo3 == bingo3);
            }
            if (filterBingoCheckBox4.Checked)
            {
                if (int.TryParse(filterBingoTextBox4.Text.Trim().ToLower(), out int bingo4))
                    filtered = filtered.Where(song => song.bingo4 == bingo4);
            }
            if (filterBingoCheckBox5.Checked)
            {
                if (int.TryParse(filterBingoTextBox5.Text.Trim().ToLower(), out int bingo5))
                    filtered = filtered.Where(song => song.bingo5 == bingo5);
            }
            if (filterBingoCheckBox6.Checked)
            {
                if (int.TryParse(filterBingoTextBox6.Text.Trim().ToLower(), out int bingo6))
                    filtered = filtered.Where(song => song.bingo6 == bingo6);
            }
            if (filterBingoCheckBox7.Checked)
            {
                if (int.TryParse(filterBingoTextBox7.Text.Trim().ToLower(), out int bingo7))
                    filtered = filtered.Where(song => song.bingo7 == bingo7);
            }
            if (filterBingoCheckBox8.Checked)
            {
                if (int.TryParse(filterBingoTextBox8.Text.Trim().ToLower(), out int bingo8))
                    filtered = filtered.Where(song => song.bingo8 == bingo8);
            }
            if (filterBingoCheckBox9.Checked)
            {
                if (int.TryParse(filterBingoTextBox9.Text.Trim().ToLower(), out int bingo9))
                    filtered = filtered.Where(song => song.bingo9 == bingo9);
            }

            // --- Final result ---
            var resultList = filtered.ToList();
            if (resultList.Count == 0)
            {
                searchResultLabel.Text = "No match.";
                searchSectionLabel.Text = $"Showing Result 0/0";
                return;
            }

            // Option A: Show first match immediately
            var firstSong = resultList.First();
            if (resultList.Count > 1)
                searchResultLabel.Text = $"{resultList.Count} matches!";
            else
                searchResultLabel.Text = $"{resultList.Count} match!";
            saveChanges();
            currentSongId = firstSong.UniqueID;
            LoadUI(firstSong);


            // Option B: keep resultList for navigation (next/previous)
            // store in a field:
            filteredSongs = resultList;
            filteredSongsSelectedIndex = 0;
            searchSectionLabel.Text = $"Showing Result 1/{filteredSongs.Count}";
        }

        private void filterInvertMatchesButton_Click(object sender, EventArgs e)
        {
            IEnumerable<SongData> filtered = allSongs;

            // Exclude songs already in filteredSongs
            filtered = filtered.Where(song => !filteredSongs.Any(fs => fs.UniqueID == song.UniqueID));

            // --- Final result ---
            var resultList = filtered.ToList();
            if (resultList.Count == 0)
            {
                searchResultLabel.Text = "No match.";
                searchSectionLabel.Text = $"Showing Result 0/0";
                return;
            }

            // Option A: Show first match immediately
            var firstSong = resultList.First();
            if (resultList.Count > 1)
                searchResultLabel.Text = $"{resultList.Count} matches!";
            else
                searchResultLabel.Text = $"{resultList.Count} match!";

            saveChanges();
            currentSongId = firstSong.UniqueID;
            LoadUI(firstSong);

            // Option B: keep resultList for navigation (next/previous)
            filteredSongs = resultList;
            filteredSongsSelectedIndex = 0;
            searchSectionLabel.Text = $"Showing Result 1/{filteredSongs.Count}";
        }

        static List<SongData> filteredSongs = new List<SongData>();
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
            currentSongId = filteredSongs[filteredSongsSelectedIndex].UniqueID;
            LoadUI(filteredSongs[filteredSongsSelectedIndex]);
            searchSectionLabel.Text = $"Showing Result {filteredSongsSelectedIndex + 1}/{filteredSongs.Count}";
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
            currentSongId = filteredSongs[filteredSongsSelectedIndex].UniqueID;
            LoadUI(filteredSongs[filteredSongsSelectedIndex]);
            searchSectionLabel.Text = $"Showing Result {filteredSongsSelectedIndex + 1}/{filteredSongs.Count}";
        }
        void FillLists()
        {
            panelMainContainer.Dock = DockStyle.Fill;
            genre.Items.Add(ScoreGenre[0]);
            genre.Items.Add(ScoreGenre[1]);
            genre.Items.Add(ScoreGenre[2]);
            genre.Items.Add(ScoreGenre[3]);
            genre.Items.Add(ScoreGenre[4]);
            genre.Items.Add(ScoreGenre[5]);
            genre.Items.Add(ScoreGenre[6]);
            genre.Items.Add(ScoreGenre[7]);
            filterGenre.Items.Add(ScoreGenre[0]);
            filterGenre.Items.Add(ScoreGenre[1]);
            filterGenre.Items.Add(ScoreGenre[2]);
            filterGenre.Items.Add(ScoreGenre[3]);
            filterGenre.Items.Add(ScoreGenre[4]);
            filterGenre.Items.Add(ScoreGenre[5]);
            filterGenre.Items.Add(ScoreGenre[6]);
            filterGenre.Items.Add(ScoreGenre[7]);
            version.Items.Add(WaccaVersions[0]);
            version.Items.Add(WaccaVersions[1]);
            version.Items.Add(WaccaVersions[2]);
            version.Items.Add(WaccaVersions[3]);
            version.Items.Add(WaccaVersions[4]);
            version.Items.Add(WaccaVersions[5]);
            filterVersion.Items.Add(WaccaVersions[0]);
            filterVersion.Items.Add(WaccaVersions[1]);
            filterVersion.Items.Add(WaccaVersions[2]);
            filterVersion.Items.Add(WaccaVersions[3]);
            filterVersion.Items.Add(WaccaVersions[4]);
            filterVersion.Items.Add(WaccaVersions[5]);

            filterMusicEnableCheckBox_CheckedChanged(null, null);
            filterArtistEnableCheckBox_CheckedChanged(null, null);
            filterGenreEnableCheckBox_CheckedChanged(null, null);
            filterVersionEnableCheckBox_CheckedChanged(null, null);
            filterNewEnableCheckBox_CheckedChanged(null, null);
            filterBeginnerEnableCheckBox_CheckedChanged(null, null);
            filterOfflineEnableCheckBox_CheckedChanged(null, null);
            filterjaEnableCheckBox_CheckedChanged(null, null);
            filterusaEnableCheckBox_CheckedChanged(null, null);
            filterzhtwEnableCheckBox_CheckedChanged(null, null);
            filterenhkEnableCheckBox_CheckedChanged(null, null);
            filterensgEnableCheckBox_CheckedChanged(null, null);
            filterkokrEnableCheckBox_CheckedChanged(null, null);
            filtercnguEnableCheckBox_CheckedChanged(null, null);
            filtercngeEnableCheckBox_CheckedChanged(null, null);
            filtercnvipEnableCheckBox_CheckedChanged(null, null);
            filternotAvailableEnableCheckBox_CheckedChanged(null, null);
            filterBingoCheckBox0_CheckedChanged(null, null);
            filterBingoCheckBox1_CheckedChanged(null, null);
            filterBingoCheckBox2_CheckedChanged(null, null);
            filterBingoCheckBox3_CheckedChanged(null, null);
            filterBingoCheckBox4_CheckedChanged(null, null);
            filterBingoCheckBox5_CheckedChanged(null, null);
            filterBingoCheckBox6_CheckedChanged(null, null);
            filterBingoCheckBox7_CheckedChanged(null, null);
            filterBingoCheckBox8_CheckedChanged(null, null);
            filterBingoCheckBox9_CheckedChanged(null, null);
            filterPointCostCheckBox_CheckedChanged(null, null);
            filterMovieNormalCheckBox_CheckedChanged(null, null);
            filterCreatorNormalCheckBox_CheckedChanged(null, null);
            filterInfernoEnableCheckBox_CheckedChanged(null, null);
            filterCreatorHardCheckBox_CheckedChanged(null, null);
            filterCreatorExtremeCheckBox_CheckedChanged(null, null);
            filterCreatorInfernoCheckBox_CheckedChanged(null, null);

        }
        private void filternotAvailableButton_Click(object sender, EventArgs e)
        {
            FilterByBoolProperty("ValidCulture_NoneActive", filternotAvailableCheckBox.Checked);
        }

        private void filterofflineButton_Click(object sender, EventArgs e)
        {
            FilterByBoolProperty("ValidCulture_Offline", filterofflineCheckBox.Checked);
        }

        private void filterBeginnerButton_Click(object sender, EventArgs e)
        {
            FilterByBoolProperty("Recommend", filterBeginnerCheckBox.Checked);
        }
        private void FilterByUnlockStatus(bool expectedValue)
        {
            if (allSongs.Count == 0)
                return;

            // Find current index
            int currentIndex = allSongs.FindIndex(s => s.UniqueID == currentSongId);

            // Walk through the list starting AFTER the current song, wrapping around
            for (int i = 1; i <= allSongs.Count; i++)
            {
                int index = (currentIndex + i) % allSongs.Count;
                var song = allSongs[index];

                // Lookup unlock status
                bool isUnlocked = musicUnlockStatus.TryGetValue((int)song.UniqueID, out bool value) && value;

                if (isUnlocked == expectedValue)
                {
                    saveChanges();
                    currentSongId = song.UniqueID;
                    LoadUI(song);
                    break; // stop at the first match
                }
            }
        }


        private void filterNewButton_Click(object sender, EventArgs e)
        {
            // only show "new" songs (unlocked within 28 days)
            FilterByUnlockStatus(filterNewCheckBox.Checked);
        }


        private void saveSongData(SongData songData)
        {
            songData.MusicMessage = musicTextBox.Text;
            songData.ArtistMessage = artistTextBox.Text;
            songData.Version = (uint)(version.SelectedIndex + 1);
            songData.AssetDirectory = merTextBox.Text;
            songData.MovieAssetName = movieNormalTextBox.Text;
            songData.MovieAssetNameHard = movieHardTextBox.Text;
            songData.MovieAssetNameExpert = movieExtremeTextBox.Text;
            songData.MovieAssetNameInferno = movieInfernoTextBox.Text;
            songData.JacketAssetName = jacketTextBox.Text;
            songData.Rubi = rubiTextBox.Text;
            songData.ValidCulture_ja_JP = jaCheckBox.Checked;
            songData.ValidCulture_en_US = usaCheckBox.Checked;
            songData.ValidCulture_zh_Hant_TW = zhtwCheckBox.Checked;
            songData.ValidCulture_en_HK = enhkCheckBox.Checked;
            songData.ValidCulture_en_SG = ensgCheckBox.Checked;
            songData.ValidCulture_ko_KR = kokrCheckBox.Checked;
            songData.ValidCulture_h_Hans_CN_Guest = cnguCheckBox.Checked;
            songData.ValidCulture_h_Hans_CN_GeneralMember = cngeCheckBox.Checked;
            songData.ValidCulture_h_Hans_CN_VipMember = cnvipCheckBox.Checked;
            songData.ValidCulture_Offline = offlineCheckBox.Checked;
            songData.ValidCulture_NoneActive = notAvailableCheckBox.Checked;
            songData.Recommend = beginnerCheckBox.Checked;
            int inti;
            if (int.TryParse(pointCostTextBox.Text, out inti))
                songData.WaccaPointCost = inti;
            songData.Bpm = bpmTextBox.Text;
            songData.NotesDesignerNormal = creatorNormalTextBox.Text;
            songData.NotesDesignerHard = creatorHardTextBox.Text;
            songData.NotesDesignerExpert = creatorExtremeTextBox.Text;
            songData.NotesDesignerInferno = creatorInfernoTextBox.Text;

            float f;
            if (float.TryParse(diffNormalTextBox.Text, out f))
                songData.DifficultyNormalLv = f;
            if (float.TryParse(diffHardTextBox.Text, out f))
                songData.DifficultyHardLv = f;
            if (float.TryParse(diffExtremeTextBox.Text, out f))
                songData.DifficultyExpertLv = f;
            if (float.TryParse(diffInfernoTextBox.Text, out f))
                songData.DifficultyInfernoLv = f;

            if (float.TryParse(crNormalTextBox.Text, out f))
                songData.ClearRateNormal = f;
            if (float.TryParse(crHardTextBox.Text, out f))
                songData.ClearRateHard = f;
            if (float.TryParse(crExtremeTextBox.Text, out f))
                songData.ClearRateExpert = f;
            if (float.TryParse(crInfernoTextBox.Text, out f))
                songData.ClearRateInferno = f;

            if (float.TryParse(previewTimeTextBox.Text, out f))
                songData.PreviewBeginTime = f;
            if (float.TryParse(previewSecTextBox.Text, out f))
                songData.PreviewSeconds = f;

            songData.ScoreGenre = genre.SelectedIndex;
            if (int.TryParse(bingo0TextBox.Text, out inti))
                songData.bingo0 = inti;
            if (int.TryParse(bingo1TextBox.Text, out inti))
                songData.bingo1 = inti;
            if (int.TryParse(bingo2TextBox.Text, out inti))
                songData.bingo2 = inti;
            if (int.TryParse(bingo3TextBox.Text, out inti))
                songData.bingo3 = inti;
            if (int.TryParse(bingo4TextBox.Text, out inti))
                songData.bingo4 = inti;
            if (int.TryParse(bingo5TextBox.Text, out inti))
                songData.bingo5 = inti;
            if (int.TryParse(bingo6TextBox.Text, out inti))
                songData.bingo6 = inti;
            if (int.TryParse(bingo7TextBox.Text, out inti))
                songData.bingo7 = inti;
            if (int.TryParse(bingo8TextBox.Text, out inti))
                songData.bingo8 = inti;
            if (int.TryParse(bingo9TextBox.Text, out inti))
                songData.bingo9 = inti;
        }
        static int savecount = 0;
        private void saveButton_Click(object sender, EventArgs e)
        {
            saveChangesInRam();
            SaveAllUnlockMusic();
            saveLabel.Text = $"Saved {++savecount} times";
            consoleLabel.Text = "";
            MusicParameterTable.Write(openedFileName);
            if (UnlockMusicTableFilePath != null)
            {
                UnlockMusicTable.Write(UnlockMusicTableFilePath);
            }
            if (UnlockInfernoTableFilePath != null)
            {
                UnlockInfernoTable.Write(UnlockInfernoTableFilePath);
            }
        }
        private void ramSaveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ramSaveCheckBox.Checked)
            {
                saveChangesInRam();
            }
        }
        public static void SaveAllUnlockMusic()
        {
            if (UnlockMusicTable == null || musicUnlockStatus == null)
                return;

            foreach (var export in UnlockMusicTable.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    var structRows = dataTable.Table.Data.OfType<StructPropertyData>().ToList();

                    foreach (var kvp in musicUnlockStatus)
                    {
                        int musicId = kvp.Key;
                        bool isNew = kvp.Value;

                        // Find existing row
                        var existingRow = structRows
                            .FirstOrDefault(r => GetFieldValue<int>(r, "MusicId") == musicId);

                        long newStartTime = 0;
                        if (isNew)
                        {
                            // Use yesterday (24h ago) for safety with timezones
                            DateTime yesterday = DateTime.Now.AddDays(-1);
                            string formatted = yesterday.ToString("yyyyMMddHH");
                            newStartTime = long.Parse(formatted);
                        }

                        if (existingRow != null)
                        {
                            // Update existing entry
                            SetFieldValue(existingRow, "ItemActivateStartTime", newStartTime);
                        }
                        else
                        {
                            // --- Create new row ---
                            var newRow = new StructPropertyData(new FName(UnlockMusicTable, $"Data_{musicId}"))
                            {
                                Value = new List<PropertyData>
                                {
                                    new IntPropertyData(new FName(UnlockMusicTable, "MusicId")) { Value = musicId },
                                    new Int64PropertyData(new FName(UnlockMusicTable, "AdaptStartTime")) { Value = 0 },
                                    new Int64PropertyData(new FName(UnlockMusicTable, "AdaptEndTime")) { Value = 0 },
                                    new BoolPropertyData(new FName(UnlockMusicTable, "bRequirePurchase")) { Value = false },
                                    new IntPropertyData(new FName(UnlockMusicTable, "RequiredMusicOpenWaccaPoint")) { Value = 0 },
                                    new BoolPropertyData(new FName(UnlockMusicTable, "bVipPreOpen")) { Value = false },
                                    new StrPropertyData(new FName(UnlockMusicTable, "NameTag")) { Value = null },
                                    new StrPropertyData(new FName(UnlockMusicTable, "ExplanationTextTag")) { Value = null },
                                    new Int64PropertyData(new FName(UnlockMusicTable, "ItemActivateStartTime")) { Value = newStartTime },
                                    new Int64PropertyData(new FName(UnlockMusicTable, "ItemActivateEndTime")) { Value = 0 },
                                    new BoolPropertyData(new FName(UnlockMusicTable, "bIsInitItem")) { Value = true },
                                    new IntPropertyData(new FName(UnlockMusicTable, "GainWaccaPoint")) { Value = 0 }
                                }
                            };
                            dataTable.Table.Data.Add(newRow);
                        }
                    }
                }
            }
        }
        void SaveCurentUnlockInferno()
        {
            if (UnlockInfernoTable == null || !int.TryParse(songid.Text, out int newId) || diffInfernoTextBox.Text == "0")
                return;

            foreach (var export in UnlockInfernoTable.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    var structRows = dataTable.Table.Data.OfType<StructPropertyData>().ToList();


                    // Find existing row
                    var existingRow = structRows
                        .FirstOrDefault(r => GetFieldValue<int>(r, "MusicId") == newId);

                    long newStartTime = 0;
                    if (checkBoxNew.Checked)
                    {
                        // Use yesterday (24h ago) for safety with timezones
                        DateTime yesterday = DateTime.Now.AddDays(-1);
                        string formatted = yesterday.ToString("yyyyMMddHH");
                        newStartTime = long.Parse(formatted);
                    }

                    if (existingRow != null)
                    {
                        // Update existing entry
                        SetFieldValue(existingRow, "ItemActivateStartTime", newStartTime);
                    }
                    else
                    {
                        // --- Create new row ---
                        var newRow = new StructPropertyData(new FName(UnlockMusicTable, newId.ToString()))
                        {
                            Value = new List<PropertyData>
                                {
                                    new IntPropertyData(new FName(UnlockMusicTable, "MusicId")) { Value = newId },
                                    new BoolPropertyData(new FName(UnlockMusicTable, "bRequirePurchase")) { Value = false },
                                    new IntPropertyData(new FName(UnlockMusicTable, "RequiredInfernoOpenWaccaPoint")) { Value = 0 },
                                    new BoolPropertyData(new FName(UnlockMusicTable, "bVipPreOpen")) { Value = false },
                                    new StrPropertyData(new FName(UnlockMusicTable, "NameTag")) { Value = null },
                                    new StrPropertyData(new FName(UnlockMusicTable, "ExplanationTextTag")) { Value = null },
                                    new Int64PropertyData(new FName(UnlockMusicTable, "ItemActivateStartTime")) { Value = newStartTime },
                                    new Int64PropertyData(new FName(UnlockMusicTable, "ItemActivateEndTime")) { Value = 0 },
                                    new BoolPropertyData(new FName(UnlockMusicTable, "bIsInitItem")) { Value = true },
                                    new IntPropertyData(new FName(UnlockMusicTable, "GainWaccaPoint")) { Value = 0 }
                                }
                        };
                        dataTable.Table.Data.Add(newRow);
                    }
                }
            }
        }


        private bool saveChangesInRam()
        {
            if (currentSongId == 0)
                return false;
            // Go through each export to find the DataTable
            foreach (var export in MusicParameterTable.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    var songData = allSongs.FirstOrDefault(s => s.UniqueID == currentSongId);
                    if (songData == null)
                    {
                        return false;
                    }

                    saveSongData(songData);

                    foreach (var row in dataTable.Table.Data)
                    {
                        if (row is StructPropertyData rowStruct)
                        {
                            uint id = GetFieldValue<uint>(rowStruct, "UniqueID");
                            if (id != currentSongId)
                            {
                                continue;
                            }
                            musicUnlockStatus[(int)id] = checkBoxNew.Checked;
                            SaveCurentUnlockInferno();
                            // Lookup the song data you want to save (replace this with your data source)
                            SetFieldValue(rowStruct, "MusicMessage", songData.MusicMessage);
                            SetFieldValue(rowStruct, "ArtistMessage", songData.ArtistMessage);
                            //SetFieldValue(rowStruct, "CopyrightMessage", songData.CopyrightMessage);
                            SetFieldValue(rowStruct, "VersionNo", songData.Version);
                            SetFieldValue(rowStruct, "AssetDirectory", songData.AssetDirectory);
                            SetFieldValue(rowStruct, "MovieAssetName", songData.MovieAssetName);
                            SetFieldValue(rowStruct, "MovieAssetNameHard", songData.MovieAssetNameHard);
                            SetFieldValue(rowStruct, "MovieAssetNameExpert", songData.MovieAssetNameExpert);
                            SetFieldValue(rowStruct, "MovieAssetNameInferno", songData.MovieAssetNameInferno);
                            SetFieldValue(rowStruct, "JacketAssetName", songData.JacketAssetName);
                            SetFieldValue(rowStruct, "Rubi", songData.Rubi);

                            SetFieldValue(rowStruct, "bValidCulture_ja_JP", songData.ValidCulture_ja_JP);
                            SetFieldValue(rowStruct, "bValidCulture_en_US", songData.ValidCulture_en_US);
                            SetFieldValue(rowStruct, "bValidCulture_zh_Hant_TW", songData.ValidCulture_zh_Hant_TW);
                            SetFieldValue(rowStruct, "bValidCulture_en_HK", songData.ValidCulture_en_HK);
                            SetFieldValue(rowStruct, "bValidCulture_en_SG", songData.ValidCulture_en_SG);
                            SetFieldValue(rowStruct, "bValidCulture_ko_KR", songData.ValidCulture_ko_KR);
                            SetFieldValue(rowStruct, "bValidCulture_h_Hans_CN_Guest", songData.ValidCulture_h_Hans_CN_Guest);
                            SetFieldValue(rowStruct, "bValidCulture_h_Hans_CN_GeneralMember", songData.ValidCulture_h_Hans_CN_GeneralMember);
                            SetFieldValue(rowStruct, "bValidCulture_h_Hans_CN_VipMember", songData.ValidCulture_h_Hans_CN_VipMember);
                            SetFieldValue(rowStruct, "bValidCulture_Offline", songData.ValidCulture_Offline);
                            SetFieldValue(rowStruct, "bValidCulture_NoneActive", songData.ValidCulture_NoneActive);
                            SetFieldValue(rowStruct, "bRecommend", songData.Recommend);
                            SetFieldValue(rowStruct, "WaccaPointCost", songData.WaccaPointCost);
                            //Collaboration = GetFieldValue<byte>(rowStruct, "bCollaboration"),
                            //WaccaOriginal = GetFieldValue<byte>(rowStruct, "bWaccaOriginal"),
                            //TrainingLevel = GetFieldValue<byte>(rowStruct, "TrainingLevel"),
                            //Reserved = GetFieldValue<byte>(rowStruct, "Reserved"),
                            SetFieldValue(rowStruct, "Bpm", songData.Bpm);
                            //HashTag = GetFieldValue<string>(rowStruct, "HashTag"),
                            SetFieldValue(rowStruct, "NotesDesignerNormal", songData.NotesDesignerNormal);
                            SetFieldValue(rowStruct, "NotesDesignerHard", songData.NotesDesignerHard);
                            SetFieldValue(rowStruct, "NotesDesignerExpert", songData.NotesDesignerExpert);
                            SetFieldValue(rowStruct, "NotesDesignerInferno", songData.NotesDesignerInferno);
                            SetFieldValue(rowStruct, "DifficultyNormalLv", songData.DifficultyNormalLv);
                            SetFieldValue(rowStruct, "DifficultyHardLv", songData.DifficultyHardLv);
                            SetFieldValue(rowStruct, "DifficultyExtremeLv", songData.DifficultyExpertLv);
                            SetFieldValue(rowStruct, "DifficultyInfernoLv", songData.DifficultyInfernoLv);
                            SetFieldValue(rowStruct, "ClearNormaRateNormal", songData.ClearRateNormal);
                            SetFieldValue(rowStruct, "ClearNormaRateHard", songData.ClearRateHard);
                            SetFieldValue(rowStruct, "ClearNormaRateExtreme", songData.ClearRateExpert);
                            SetFieldValue(rowStruct, "ClearNormaRateInferno", songData.ClearRateInferno);
                            SetFieldValue(rowStruct, "PreviewBeginTime", songData.PreviewBeginTime);
                            SetFieldValue(rowStruct, "PreviewSeconds", songData.PreviewSeconds);
                            SetFieldValue(rowStruct, "ScoreGenre", songData.ScoreGenre);
                            SetFieldValue(rowStruct, "MusicTagForUnlock0", songData.bingo0);
                            SetFieldValue(rowStruct, "MusicTagForUnlock1", songData.bingo1);
                            SetFieldValue(rowStruct, "MusicTagForUnlock2", songData.bingo2);
                            SetFieldValue(rowStruct, "MusicTagForUnlock3", songData.bingo3);
                            SetFieldValue(rowStruct, "MusicTagForUnlock4", songData.bingo4);
                            SetFieldValue(rowStruct, "MusicTagForUnlock5", songData.bingo5);
                            SetFieldValue(rowStruct, "MusicTagForUnlock6", songData.bingo6);
                            SetFieldValue(rowStruct, "MusicTagForUnlock7", songData.bingo7);
                            SetFieldValue(rowStruct, "MusicTagForUnlock8", songData.bingo8);
                            SetFieldValue(rowStruct, "MusicTagForUnlock9", songData.bingo9);
                            //public ulong WorkBuffer { get; set; }
                            //SetFieldValue(rowStruct, "AssetFullPath", songData.AssetFullPath);
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public static void SetFieldValue(StructPropertyData structData, string fieldName, object newValue)
        {
            var prop = structData.Value.FirstOrDefault(p => p.Name.Value.ToString() == fieldName);

            if (prop is StrPropertyData strProp && newValue is string strVal)
            {
                strProp.Value = (strVal == "null") ? null : (UAssetAPI.UnrealTypes.FString)strVal;
            }
            else if (prop is IntPropertyData intProp && newValue is int intVal)
            {
                intProp.Value = intVal;
            }
            else if (prop is Int64PropertyData int64Prop && newValue is long int64Val)
            {
                int64Prop.Value = int64Val;
            }
            else if (prop is UInt32PropertyData uintProp && newValue is uint uintVal)
            {
                uintProp.Value = uintVal;
            }
            else if (prop is BoolPropertyData boolProp && newValue is bool boolVal)
            {
                boolProp.Value = boolVal;
            }
            else if (prop is FloatPropertyData floatProp && newValue is float floatVal)
            {
                floatProp.Value = floatVal;
            }
            else if (prop is ArrayPropertyData arrProp && newValue is List<string> stringList)
            {
                // Build a new array of StrPropertyData
                var newArray = new PropertyData[stringList.Count];
                for (int i = 0; i < stringList.Count; i++)
                {
                    newArray[i] = new StrPropertyData
                    {
                        Name = arrProp.Name, // preserve the name
                        Value = (UAssetAPI.UnrealTypes.FString)stringList[i]
                    };
                }

                arrProp.Value = newArray;
            }
            else
            {
                Console.WriteLine($"Unsupported or mismatched type for field '{fieldName}'");
            }
        }

        private void nextSongButton_Click(object sender, EventArgs e)
        {
            if (songid.SelectedIndex >= songid.Items.Count - 1)
                songid.SelectedIndex = 0;
            else
                songid.SelectedIndex += 1;
            songidButton_Click(null, null);
        }

        private void previousSongButton_Click(object sender, EventArgs e)
        {
            if (songid.SelectedIndex <= 0)
                songid.SelectedIndex = songid.Items.Count - 1;
            else
                songid.SelectedIndex -= 1;
            songidButton_Click(null, null);
        }

        static uint currentSongId;
        private void songidButton_Click(object sender, EventArgs e)
        {
            if (allSongs.Count == 0)
                return;
            consoleLabel.Text = "";
            saveChanges();
            uint.TryParse(songid.Text, out currentSongId);
            if (currentSongId == 0)
            {
                consoleLabel.Text = "ID 0 cannot be used";
                saveLabel.Text = "";
                return;
            }
            int currentIndex = allSongs.FindIndex(s => s.UniqueID == currentSongId);
            if (currentIndex == -1)
            {
                consoleLabel.Text = $"ID {currentSongId} not found";
                saveLabel.Text = "";
                return;
            }
            LoadUI(allSongs[currentIndex]);
        }
        /**
         * you need to set curentSongId first before calling this function!!!!!
         * this function fills all UI elements with the data from song
         */
        private void LoadUI(SongData song)
        {
            musicTextBox.Text = song.MusicMessage;
            artistTextBox.Text = song.ArtistMessage;
            if (checkBoxNew.Enabled && freezeNewCheckBox.Checked == false)
            {
                checkBoxNew.Checked = musicUnlockStatus[(int)song.UniqueID];
            }
            if (song.ScoreGenre < genre.Items.Count && freezeGenreCheckBox.Checked == false)
            {
                genre.SelectedIndex = song.ScoreGenre;
            }
            rubiTextBox.Text = song.Rubi;
            if (freezePointCostCheckBox.Checked == false)
            {
                pointCostTextBox.Text = song.WaccaPointCost.ToString();
            }
            merTextBox.Text = song.AssetDirectory;
            jacketTextBox.Text = song.JacketAssetName;
            bpmTextBox.Text = song.Bpm.ToString();
            previewTimeTextBox.Text = song.PreviewBeginTime.ToString();
            previewSecTextBox.Text = song.PreviewSeconds.ToString();
            if (song.Version < 1)
            {
                version.SelectedIndex = 0;
            }
            else if (song.Version >= version.Items.Count)  // allows adding more versions just by editing the items at the top of the code
            {
                version.SelectedIndex = version.Items.Count - 1;
            }
            else if (freezeVersionCheckBox.Checked == false)
            {
                version.SelectedIndex = (int)(song.Version - 1);
            }
            diffNormalTextBox.Text = song.DifficultyNormalLv.ToString();
            diffHardTextBox.Text = song.DifficultyHardLv.ToString();
            diffExtremeTextBox.Text = song.DifficultyExpertLv.ToString();
            diffInfernoTextBox.Text = song.DifficultyInfernoLv.ToString();
            crNormalTextBox.Text = song.ClearRateNormal.ToString();
            crHardTextBox.Text = song.ClearRateHard.ToString();
            crExtremeTextBox.Text = song.ClearRateExpert.ToString();
            crInfernoTextBox.Text = song.ClearRateInferno.ToString();
            movieNormalTextBox.Text = song.MovieAssetName;
            movieHardTextBox.Text = song.MovieAssetNameHard;
            movieExtremeTextBox.Text = song.MovieAssetNameExpert;
            movieInfernoTextBox.Text = song.MovieAssetNameInferno;
            creatorNormalTextBox.Text = song.NotesDesignerNormal;
            creatorHardTextBox.Text = song.NotesDesignerHard;
            creatorExtremeTextBox.Text = song.NotesDesignerExpert;
            creatorInfernoTextBox.Text = song.NotesDesignerInferno;
            bingo0TextBox.Text = song.bingo0.ToString();
            bingo1TextBox.Text = song.bingo1.ToString();
            bingo2TextBox.Text = song.bingo2.ToString();
            bingo3TextBox.Text = song.bingo3.ToString();
            bingo4TextBox.Text = song.bingo4.ToString();
            bingo5TextBox.Text = song.bingo5.ToString();
            bingo6TextBox.Text = song.bingo6.ToString();
            bingo7TextBox.Text = song.bingo7.ToString();
            bingo8TextBox.Text = song.bingo8.ToString();
            bingo9TextBox.Text = song.bingo9.ToString();
            if (freezeAvailableCheckBox.Checked == false)
            {
                offlineCheckBox.Checked = song.ValidCulture_Offline;
                jaCheckBox.Checked = song.ValidCulture_ja_JP;
                usaCheckBox.Checked = song.ValidCulture_en_US;
                zhtwCheckBox.Checked = song.ValidCulture_zh_Hant_TW;
                enhkCheckBox.Checked = song.ValidCulture_en_HK;
                ensgCheckBox.Checked = song.ValidCulture_en_SG;
                kokrCheckBox.Checked = song.ValidCulture_ko_KR;
                cnguCheckBox.Checked = song.ValidCulture_h_Hans_CN_Guest;
                cngeCheckBox.Checked = song.ValidCulture_h_Hans_CN_GeneralMember;
                cnvipCheckBox.Checked = song.ValidCulture_h_Hans_CN_VipMember;
                notAvailableCheckBox.Checked = song.ValidCulture_NoneActive;
            }
            if (freezeBeginnerCheckBox.Checked == false)
            {
                beginnerCheckBox.Checked = song.Recommend;
            }

            songid.Text = song.UniqueID.ToString();

            string path = execPath + song.JacketAssetName + ".png";
            if (File.Exists(path))
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var ms = new MemoryStream())
                {
                    fs.CopyTo(ms);                  // Copy file into memory
                    ms.Position = 0;                // Reset position to beginning
                    jacketPictureBox.Image = Image.FromStream(ms); // Load image from memory
                }
            }
        }

        static readonly string execPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/");
        private void autoSaveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autoSaveCheckBox.Checked == true)
            {
                saveChanges();
            }
        }

        public static T GetFieldValue<T>(StructPropertyData structData, string fieldName)
        {
            var prop = structData.Value.FirstOrDefault(p => p.Name.Value.ToString() == fieldName);

            return prop switch
            {
                StrPropertyData strProp when typeof(T) == typeof(string) => (T)(object)(strProp.Value?.ToString() ?? "null"),
                IntPropertyData intProp when typeof(T) == typeof(int) => (T)(object)intProp.Value,
                UInt32PropertyData uintProp when typeof(T) == typeof(uint) => (T)(object)uintProp.Value,
                Int64PropertyData longProp when typeof(T) == typeof(long) => (T)(object)longProp.Value,
                BoolPropertyData boolProp when typeof(T) == typeof(bool) => (T)(object)boolProp.Value,
                FloatPropertyData floatProp when typeof(T) == typeof(float) => (T)(object)floatProp.Value,
                BytePropertyData byteProp when typeof(T) == typeof(byte) => (T)(object)byteProp.Value,
                _ => default
            };
        }
        static List<SongData> allSongs = new List<SongData>();
        static UAsset MusicParameterTable;
        int Read(string file)
        {
            string uassetPath = file;
            string directory;
            string newPath;

            // Get just the file name
            string fileName = Path.GetFileName(uassetPath);

            if (fileName.StartsWith("UnlockMusicTable", StringComparison.OrdinalIgnoreCase))
            {
                // swap file if it's UnlockMusicTable.uasset
                // Get the directory part
                directory = Path.GetDirectoryName(uassetPath);

                // Combine with the new filename
                uassetPath = Path.Combine(directory, "MusicParameterTable.uasset");
            }

            if (!File.Exists(uassetPath)) return -1;

            // Load the asset (assumes .uexp is in the same folder)
            MusicParameterTable = new UAsset(uassetPath, UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_19);

            // Go through each export to find the DataTable
            foreach (var export in MusicParameterTable.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    foreach (var row in dataTable.Table.Data)
                    {
                        if (row is StructPropertyData rowStruct)
                        {
                            SongData data = new SongData
                            {
                                UniqueID = GetFieldValue<uint>(rowStruct, "UniqueID"),
                                MusicMessage = GetFieldValue<string>(rowStruct, "MusicMessage"),
                                ArtistMessage = GetFieldValue<string>(rowStruct, "ArtistMessage"),
                                //CopyrightMessage = GetFieldValue<string>(rowStruct, "CopyrightMessage"),
                                Version = GetFieldValue<uint>(rowStruct, "VersionNo"),
                                AssetDirectory = GetFieldValue<string>(rowStruct, "AssetDirectory"),
                                MovieAssetName = GetFieldValue<string>(rowStruct, "MovieAssetName"),
                                MovieAssetNameHard = GetFieldValue<string>(rowStruct, "MovieAssetNameHard"),
                                MovieAssetNameExpert = GetFieldValue<string>(rowStruct, "MovieAssetNameExpert"),
                                MovieAssetNameInferno = GetFieldValue<string>(rowStruct, "MovieAssetNameInferno"),
                                JacketAssetName = GetFieldValue<string>(rowStruct, "JacketAssetName"),
                                Rubi = GetFieldValue<string>(rowStruct, "Rubi"),

                                ValidCulture_ja_JP = GetFieldValue<bool>(rowStruct, "bValidCulture_ja_JP"),
                                ValidCulture_en_US = GetFieldValue<bool>(rowStruct, "bValidCulture_en_US"),
                                ValidCulture_zh_Hant_TW = GetFieldValue<bool>(rowStruct, "bValidCulture_zh_Hant_TW"),
                                ValidCulture_en_HK = GetFieldValue<bool>(rowStruct, "bValidCulture_en_HK"),
                                ValidCulture_en_SG = GetFieldValue<bool>(rowStruct, "bValidCulture_en_SG"),
                                ValidCulture_ko_KR = GetFieldValue<bool>(rowStruct, "bValidCulture_ko_KR"),
                                ValidCulture_h_Hans_CN_Guest = GetFieldValue<bool>(rowStruct, "bValidCulture_h_Hans_CN_Guest"),
                                ValidCulture_h_Hans_CN_GeneralMember = GetFieldValue<bool>(rowStruct, "bValidCulture_h_Hans_CN_GeneralMember"),
                                ValidCulture_h_Hans_CN_VipMember = GetFieldValue<bool>(rowStruct, "bValidCulture_h_Hans_CN_VipMember"),
                                ValidCulture_Offline = GetFieldValue<bool>(rowStruct, "bValidCulture_Offline"),
                                ValidCulture_NoneActive = GetFieldValue<bool>(rowStruct, "bValidCulture_NoneActive"),

                                Recommend = GetFieldValue<bool>(rowStruct, "bRecommend"),
                                WaccaPointCost = GetFieldValue<int>(rowStruct, "WaccaPointCost"),
                                //Collaboration = GetFieldValue<byte>(rowStruct, "bCollaboration"),
                                //WaccaOriginal = GetFieldValue<byte>(rowStruct, "bWaccaOriginal"),
                                //TrainingLevel = GetFieldValue<byte>(rowStruct, "TrainingLevel"),
                                //Reserved = GetFieldValue<byte>(rowStruct, "Reserved"),

                                Bpm = GetFieldValue<string>(rowStruct, "Bpm"),
                                //HashTag = GetFieldValue<string>(rowStruct, "HashTag"),

                                NotesDesignerNormal = GetFieldValue<string>(rowStruct, "NotesDesignerNormal"),
                                NotesDesignerHard = GetFieldValue<string>(rowStruct, "NotesDesignerHard"),
                                NotesDesignerExpert = GetFieldValue<string>(rowStruct, "NotesDesignerExpert"),
                                NotesDesignerInferno = GetFieldValue<string>(rowStruct, "NotesDesignerInferno"),

                                DifficultyNormalLv = GetFieldValue<float>(rowStruct, "DifficultyNormalLv"),
                                DifficultyHardLv = GetFieldValue<float>(rowStruct, "DifficultyHardLv"),
                                DifficultyExpertLv = GetFieldValue<float>(rowStruct, "DifficultyExtremeLv"),
                                DifficultyInfernoLv = GetFieldValue<float>(rowStruct, "DifficultyInfernoLv"),

                                ClearRateNormal = GetFieldValue<float>(rowStruct, "ClearNormaRateNormal"),
                                ClearRateHard = GetFieldValue<float>(rowStruct, "ClearNormaRateHard"),
                                ClearRateExpert = GetFieldValue<float>(rowStruct, "ClearNormaRateExtreme"),
                                ClearRateInferno = GetFieldValue<float>(rowStruct, "ClearNormaRateInferno"),

                                PreviewBeginTime = GetFieldValue<float>(rowStruct, "PreviewBeginTime"),
                                PreviewSeconds = GetFieldValue<float>(rowStruct, "PreviewSeconds"),

                                ScoreGenre = GetFieldValue<int>(rowStruct, "ScoreGenre"),
                                bingo0 = GetFieldValue<int>(rowStruct, "MusicTagForUnlock0"),
                                bingo1 = GetFieldValue<int>(rowStruct, "MusicTagForUnlock1"),
                                bingo2 = GetFieldValue<int>(rowStruct, "MusicTagForUnlock2"),
                                bingo3 = GetFieldValue<int>(rowStruct, "MusicTagForUnlock3"),
                                bingo4 = GetFieldValue<int>(rowStruct, "MusicTagForUnlock4"),
                                bingo5 = GetFieldValue<int>(rowStruct, "MusicTagForUnlock5"),
                                bingo6 = GetFieldValue<int>(rowStruct, "MusicTagForUnlock6"),
                                bingo7 = GetFieldValue<int>(rowStruct, "MusicTagForUnlock7"),
                                bingo8 = GetFieldValue<int>(rowStruct, "MusicTagForUnlock8"),
                                bingo9 = GetFieldValue<int>(rowStruct, "MusicTagForUnlock9"),
                                //public ulong WorkBuffer { get; set; }
                                //AssetFullPath = GetFieldValue<string>(rowStruct, "AssetFullPath"),
                            };

                            if (data.UniqueID.ToString() == "0")
                            {
                                return -1;
                            }
                            songid.Items.Add(data.UniqueID.ToString());
                            allSongs.Add(data);
                        }
                    }
                }
            }

            // Get the directory part
            directory = Path.GetDirectoryName(uassetPath);

            // Combine with the new filename
            newPath = Path.Combine(directory, "UnlockMusicTable.uasset");
            if (File.Exists(newPath))
            {
                UnlockMusicTableFilePath = newPath;
                ReadUnlockMusic();
            }
            else
            {
                UnlockMusicTableFilePath = null;
                UnlockMusicTable = null;
                musicUnlockStatus = null;
                checkBoxNew.BackColor = Color.FromArgb(0xff, 0xAA, 0xAA, 0xAA);
                checkBoxNew.Enabled = false;
            }

            // Combine with the new filename
            newPath = Path.Combine(directory, "UnlockInfernoTable.uasset");
            if (File.Exists(newPath))
            {
                UnlockInfernoTableFilePath = newPath;
                UnlockInfernoTable = new UAsset(UnlockInfernoTableFilePath, UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_19);
                //ReadUnlockMusic();
            }
            else
            {
                UnlockInfernoTableFilePath = null;
                UnlockInfernoTable = null;
                checkBoxNew.BackColor = Color.FromArgb(0xff, 0xAA, 0xAA, 0xAA);
                checkBoxNew.Enabled = false;
            }
            nextSongButton_Click(null, null);
            return 0;
        }
        static string UnlockMusicTableFilePath;
        static UAsset UnlockMusicTable;
        static Dictionary<int, bool> musicUnlockStatus;
        static string UnlockInfernoTableFilePath;
        static UAsset UnlockInfernoTable;
        public static int ReadUnlockMusic()
        {
            musicUnlockStatus = new Dictionary<int, bool>();

            UnlockMusicTable = new UAsset(UnlockMusicTableFilePath, UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_19);

            foreach (var export in UnlockMusicTable.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    foreach (var row in dataTable.Table.Data)
                    {
                        if (row is StructPropertyData rowStruct)
                        {
                            UnlockData data = new UnlockData
                            {
                                MusicId = GetFieldValue<int>(rowStruct, "MusicId"),
                                ItemActivateStartTime = GetFieldValue<long>(rowStruct, "ItemActivateStartTime"),
                            };

                            if (data.MusicId == 0)
                                continue;

                            bool isWithin28Days = CheckWithin28Days(data.ItemActivateStartTime);
                            musicUnlockStatus[data.MusicId] = isWithin28Days;
                        }
                    }
                }
            }
            return 0;
        }

        public static bool CheckWithin28Days(long itemActivateStartTime)
        {
            if (itemActivateStartTime == 0) return false;

            DateTime currentDate = DateTime.Now;
            DateTime startTime;

            // Convert YYYYMMDDHH (e.g., 2020012307) to DateTime
            string timeString = itemActivateStartTime.ToString();
            if (timeString.Length < 10) return false;

            try
            {
                int year = int.Parse(timeString.Substring(0, 4));
                int month = int.Parse(timeString.Substring(4, 2));
                int day = int.Parse(timeString.Substring(6, 2));
                int hour = int.Parse(timeString.Substring(8, 2));
                startTime = new DateTime(year, month, day, hour, 0, 0);
            }
            catch
            {
                return false;
            }

            return (currentDate - startTime).TotalDays <= 28;
        }
        static bool finished = true;
        private void rubiTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!finished || !rubiAutoFixCheckBox.Checked)
                return;

            finished = false;

            int caretIndex = rubiTextBox.SelectionStart;

            string input = musicTextBox.Text ?? "0";

            // If '|' exists, take only the part after it
            int pipeIndex = input.IndexOf('|');
            if (pipeIndex < 0)
                pipeIndex = input.IndexOf('｜'); // U+FF5C
            if (pipeIndex >= 0 && pipeIndex < input.Length - 1)
            {
                input = input.Substring(pipeIndex + 1);
            }

            // Convert to uppercase
            input = input.ToUpperInvariant();

            // Keep only A–Z and 0–9
            var sb = new StringBuilder(input.Length);
            foreach (char c in input)
            {
                if ((c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9'))
                    sb.Append((char)(c + 0xFEE0));
            }

            rubiTextBox.Text = sb.ToString();
            if (rubiTextBox.Text == "")
                rubiTextBox.Text = ((char)('0' + 0xFEE0)).ToString();

            rubiTextBox.SelectionStart = Math.Min(caretIndex, rubiTextBox.Text.Length);
            rubiTextBox.SelectionLength = 0;

            finished = true;
        }


        private void songid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the "ding" sound
                songidButton_Click(sender, e);
            }
        }

        private void filterMusicTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the "ding" sound
                filterMusicButton_Click(sender, e);
            }
        }

        private void filterArtistTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the "ding" sound
                filterArtistButton_Click(sender, e);
            }
        }

        private void filterGenre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the "ding" sound
                filterGenreButton_Click(sender, e);
            }
        }

        private void filterNewCheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the "ding" sound
                filterNewButton_Click(sender, e);
            }
        }

        private void filterBeginnerCheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the "ding" sound
                filterBeginnerButton_Click(sender, e);
            }
        }

        private void filterVersion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the "ding" sound
                filterVersionButton_Click(sender, e);
            }
        }

        private void filterofflineCheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the "ding" sound
                filterofflineButton_Click(sender, e);
            }
        }

        private void filterjaCheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the "ding" sound
                filterjaButton_Click(sender, e);
            }
        }

        private void filterusaCheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the "ding" sound
                filterusaButton_Click(sender, e);
            }
        }

        private void filterzhtwCheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the "ding" sound
                filterzhtwButton_Click(sender, e);
            }
        }

        private void filterenhkCheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the "ding" sound
                filterenhkButton_Click(sender, e);
            }
        }

        private void filterensgCheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the "ding" sound
                filterensgButton_Click(sender, e);
            }
        }

        private void filterkokrCheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the "ding" sound
                filterkokrButton_Click(sender, e);
            }
        }

        private void filtercnguCheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the "ding" sound
                filtercnguButton_Click(sender, e);
            }
        }

        private void filtercngeCheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the "ding" sound
                filtercngeButton_Click(sender, e);
            }
        }

        private void filtercnvipCheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the "ding" sound
                filterfiltercnvipButton_Click(sender, e);
            }
        }

        private void filternotAvailableCheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Prevent the "ding" sound
                filternotAvailableButton_Click(sender, e);
            }
        }

        private void freezeGenreCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (freezeGenreCheckBox.CheckState == CheckState.Checked)
            {
                genre.BackColor = Color.LightBlue;
                freezeGenreCheckBox.BackColor = Color.LightBlue;
            }
            else
            {
                genre.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                freezeGenreCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
            }
        }

        private void freezeVersionCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (freezeVersionCheckBox.CheckState == CheckState.Checked)
            {
                version.BackColor = Color.LightBlue;
                freezeVersionCheckBox.BackColor = Color.LightBlue;
            }
            else
            {
                version.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                freezeVersionCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
            }
        }

        private void freezePointCostCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (freezePointCostCheckBox.CheckState == CheckState.Checked)
            {
                pointCostTextBox.BackColor = Color.LightBlue;
                freezePointCostCheckBox.BackColor = Color.LightBlue;
            }
            else
            {
                pointCostTextBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                freezePointCostCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
            }
        }

        private void freezeNewCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxNew.Enabled)
            {
                if (freezeNewCheckBox.CheckState == CheckState.Checked)
                {
                    checkBoxNew.BackColor = Color.LightBlue;
                    freezeNewCheckBox.BackColor = Color.LightBlue;
                }
                else
                {
                    checkBoxNew.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                    freezeNewCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                }
            }
        }

        private void freezeBeginnerCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (freezeBeginnerCheckBox.CheckState == CheckState.Checked)
            {
                beginnerCheckBox.BackColor = Color.LightBlue;
                freezeBeginnerCheckBox.BackColor = Color.LightBlue;
            }
            else
            {
                beginnerCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                freezeBeginnerCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
            }
        }

        private void freezeAvailableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (freezeAvailableCheckBox.CheckState == CheckState.Checked)
            {
                freezeAvailableCheckBox.BackColor = Color.LightBlue;
                offlineCheckBox.BackColor = Color.LightBlue;
                jaCheckBox.BackColor = Color.LightBlue;
                usaCheckBox.BackColor = Color.LightBlue;
                zhtwCheckBox.BackColor = Color.LightBlue;
                enhkCheckBox.BackColor = Color.LightBlue;
                ensgCheckBox.BackColor = Color.LightBlue;
                kokrCheckBox.BackColor = Color.LightBlue;
                cnguCheckBox.BackColor = Color.LightBlue;
                cngeCheckBox.BackColor = Color.LightBlue;
                cnvipCheckBox.BackColor = Color.LightBlue;
                notAvailableCheckBox.BackColor = Color.LightBlue;
            }
            else
            {
                freezeAvailableCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                offlineCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                jaCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                usaCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                zhtwCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                enhkCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                ensgCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                kokrCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                cnguCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                cngeCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                cnvipCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
                notAvailableCheckBox.BackColor = Color.FromArgb(0xff, 0xf0, 0xf0, 0xf0);
            }
        }
        private void filterMusicEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterMusicLabel.Enabled = filterMusicEnableCheckBox.Checked;
            filterMusicTextBox.Enabled = filterMusicEnableCheckBox.Checked;
        }

        private void filterArtistEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterArtistLabel.Enabled = filterArtistEnableCheckBox.Checked;
            filterArtistTextBox.Enabled = filterArtistEnableCheckBox.Checked;
        }

        private void filterGenreEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterGenreLabel.Enabled = filterGenreEnableCheckBox.Checked;
            filterGenre.Enabled = filterGenreEnableCheckBox.Checked;
        }

        private void filterVersionEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterVersionLabel.Enabled = filterVersionEnableCheckBox.Checked;
            filterVersion.Enabled = filterVersionEnableCheckBox.Checked;
        }

        private void filterNewEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterNewCheckBox.Enabled = filterNewEnableCheckBox.Checked;
        }

        private void filterBeginnerEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterBeginnerCheckBox.Enabled = filterBeginnerEnableCheckBox.Checked;
        }

        private void filterOfflineEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterofflineCheckBox.Enabled = filterOfflineEnableCheckBox.Checked;
        }

        private void filterjaEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterjaCheckBox.Enabled = filterjaEnableCheckBox.Checked;
        }

        private void filterusaEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterusaCheckBox.Enabled = filterusaEnableCheckBox.Checked;
        }

        private void filterzhtwEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterzhtwCheckBox.Enabled = filterzhtwEnableCheckBox.Checked;
        }

        private void filterenhkEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterenhkCheckBox.Enabled = filterenhkEnableCheckBox.Checked;
        }

        private void filterensgEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterensgCheckBox.Enabled = filterensgEnableCheckBox.Checked;
        }

        private void filterkokrEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterkokrCheckBox.Enabled = filterkokrEnableCheckBox.Checked;
        }

        private void filtercnguEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filtercnguCheckBox.Enabled = filtercnguEnableCheckBox.Checked;
        }

        private void filtercngeEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filtercngeCheckBox.Enabled = filtercngeEnableCheckBox.Checked;
        }

        private void filtercnvipEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filtercnvipCheckBox.Enabled = filtercnvipEnableCheckBox.Checked;
        }

        private void filternotAvailableEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filternotAvailableCheckBox.Enabled = filternotAvailableEnableCheckBox.Checked;
        }

        private void filterBingoCheckBox0_CheckedChanged(object sender, EventArgs e)
        {
            filterBingoTextBox0.Enabled = filterBingoCheckBox0.Checked;
            filterBingoLabel0.Enabled = filterBingoCheckBox0.Checked;
        }

        private void filterBingoCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            filterBingoTextBox1.Enabled = filterBingoCheckBox1.Checked;
            filterBingoLabel1.Enabled = filterBingoCheckBox1.Checked;
        }

        private void filterBingoCheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            filterBingoTextBox2.Enabled = filterBingoCheckBox2.Checked;
            filterBingoLabel2.Enabled = filterBingoCheckBox2.Checked;
        }

        private void filterBingoCheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            filterBingoTextBox3.Enabled = filterBingoCheckBox3.Checked;
            filterBingoLabel3.Enabled = filterBingoCheckBox3.Checked;
        }

        private void filterBingoCheckBox4_CheckedChanged(object sender, EventArgs e)
        {
            filterBingoTextBox4.Enabled = filterBingoCheckBox4.Checked;
            filterBingoLabel4.Enabled = filterBingoCheckBox4.Checked;
        }

        private void filterBingoCheckBox5_CheckedChanged(object sender, EventArgs e)
        {
            filterBingoTextBox5.Enabled = filterBingoCheckBox5.Checked;
            filterBingoLabel5.Enabled = filterBingoCheckBox5.Checked;
        }

        private void filterBingoCheckBox6_CheckedChanged(object sender, EventArgs e)
        {

            filterBingoTextBox6.Enabled = filterBingoCheckBox6.Checked;
            filterBingoLabel6.Enabled = filterBingoCheckBox6.Checked;
        }

        private void filterBingoCheckBox7_CheckedChanged(object sender, EventArgs e)
        {

            filterBingoTextBox7.Enabled = filterBingoCheckBox7.Checked;
            filterBingoLabel7.Enabled = filterBingoCheckBox7.Checked;
        }

        private void filterBingoCheckBox8_CheckedChanged(object sender, EventArgs e)
        {

            filterBingoTextBox8.Enabled = filterBingoCheckBox8.Checked;
            filterBingoLabel8.Enabled = filterBingoCheckBox8.Checked;
        }

        private void filterBingoCheckBox9_CheckedChanged(object sender, EventArgs e)
        {

            filterBingoTextBox9.Enabled = filterBingoCheckBox9.Checked;
            filterBingoLabel9.Enabled = filterBingoCheckBox9.Checked;
        }

        private void filterInfernoEnableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterInfernoCheckBox.Enabled = filterInfernoEnableCheckBox.Checked;
        }

        private void filterCreatorNormalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterCreatorNormalLabel.Enabled = filterCreatorNormalCheckBox.Checked;
            filterCreatorNormalTextBox.Enabled = filterCreatorNormalCheckBox.Checked;
        }

        private void filterCreatorHardCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterCreatorHardLabel.Enabled = filterCreatorHardCheckBox.Checked;
            filterCreatorHardTextBox.Enabled = filterCreatorHardCheckBox.Checked;
        }

        private void filterCreatorExtremeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterCreatorExtremeLabel.Enabled = filterCreatorExtremeCheckBox.Checked;
            filterCreatorExtremeTextBox.Enabled = filterCreatorExtremeCheckBox.Checked;
        }

        private void filterCreatorInfernoCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterCreatorInfernoLabel.Enabled = filterCreatorInfernoCheckBox.Checked;
            filterCreatorInfernoTextBox.Enabled = filterCreatorInfernoCheckBox.Checked;
        }

        private void filterMovieNormalCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterMovieNormalLabel.Enabled = filterMovieNormalCheckBox.Checked;
            filterMovieNormalTextBox.Enabled = filterMovieNormalCheckBox.Checked;
        }

        private void filterPointCostCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filterPointCostLabel.Enabled = filterPointCostCheckBox.Checked;
            filterPointCostTextBox.Enabled = filterPointCostCheckBox.Checked;
        }
        // ---------- Click handlers ----------
        private void sortAllArtistButton_Click(object sender, EventArgs e)
        {
            SortMusicParameterTableByField("ArtistMessage", numeric: false, descending: false, caseInsensitive: true);
        }

        private void sortAllIDsmallButton_Click(object sender, EventArgs e)
        {
            SortMusicParameterTableByField("UniqueID", numeric: true, descending: false);
        }

        private void sortAllBigButton_Click(object sender, EventArgs e)
        {
            SortMusicParameterTableByField("UniqueID", numeric: true, descending: true);
        }

        private void sortAllBPMsmallButton_Click(object sender, EventArgs e)
        {
            SortMusicParameterTableByField("Bpm", numeric: true, descending: false);
        }

        private void sortAllBPMbigButton_Click(object sender, EventArgs e)
        {
            SortMusicParameterTableByField("Bpm", numeric: true, descending: true);
        }

        // ---------- Main sorter ----------
        private void SortMusicParameterTableByField(string fieldName, bool numeric, bool descending, bool caseInsensitive = false)
        {
            saveChanges();
            // Find the DataTableExport inside MusicParameterTable
            foreach (var export in MusicParameterTable.Exports)
            {
                if (!(export is DataTableExport dataTable))
                    continue;

                // Extract StructPropertyData rows (these represent each song row)
                var structRows = dataTable.Table.Data.OfType<StructPropertyData>().ToList();
                if (structRows.Count == 0)
                    return; // nothing to sort

                List<StructPropertyData> sortedRows;

                if (numeric)
                {
                    Func<StructPropertyData, double> numericKey = row =>
                    {
                        var raw = GetStructFieldValueRaw(row, fieldName);
                        if (raw == null)
                            return descending ? double.MinValue : double.MaxValue;

                        if (raw is uint u) return (double)u;
                        if (raw is int i) return (double)i;
                        if (raw is long l) return (double)l;
                        if (raw is double dval) return dval;
                        if (raw is float fval) return (double)fval;

                        var s = raw.ToString();
                        if (double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out double d))
                            return d;

                        return descending ? double.MinValue : double.MaxValue;
                    };

                    sortedRows = descending
                        ? structRows.OrderByDescending(numericKey).ToList()
                        : structRows.OrderBy(numericKey).ToList();
                }
                else
                {
                    Func<StructPropertyData, string> stringKey = row =>
                    {
                        var raw = GetStructFieldValueRaw(row, fieldName);
                        var s = raw?.ToString() ?? string.Empty;
                        return caseInsensitive ? s.ToLowerInvariant() : s;
                    };

                    sortedRows = descending
                        ? structRows.OrderByDescending(stringKey).ToList()
                        : structRows.OrderBy(stringKey).ToList();
                }

                // --- FIXED PART ---
                // We must replace the Table.Data contents with StructPropertyData items.
                // table.Data is List<StructPropertyData>, so add only StructPropertyData objects.
                dataTable.Table.Data.Clear();
                dataTable.Table.Data.AddRange(sortedRows);
                // --- END FIXED PART ---

                // Reorder your in-memory allSongs to match the new table order (by UniqueID)
                var newAllSongs = new List<SongData>();
                var songsById = allSongs.ToDictionary(s => s.UniqueID);

                foreach (var row in sortedRows)
                {
                    var idRaw = GetStructFieldValueRaw(row, "UniqueID");
                    uint id = 0;
                    if (idRaw is uint uid) id = uid;
                    else if (idRaw is int iid) id = (uint)iid;
                    else if (idRaw is long lid) id = (uint)lid;
                    else
                    {
                        var s = idRaw?.ToString();
                        if (!string.IsNullOrEmpty(s) && uint.TryParse(s, out uint parsed)) id = parsed;
                    }

                    if (songsById.TryGetValue(id, out var song))
                    {
                        newAllSongs.Add(song);
                    }
                }

                foreach (var s in allSongs)
                    if (!newAllSongs.Contains(s))
                        newAllSongs.Add(s);

                allSongs = newAllSongs;

                // Refresh UI and persist
                RefreshSongListUI();
                saveChanges();

                return; // handled first DataTableExport found
            }
        }


        // ---------- Helper: read property value from StructPropertyData ----------
        private static object GetStructFieldValueRaw(StructPropertyData structData, string fieldName)
        {
            var prop = structData.Value.FirstOrDefault(p => p.Name.Value.ToString() == fieldName);
            if (prop == null) return null;

            // match the property types you use in save code
            if (prop is StrPropertyData strProp)
            {
                // StrPropertyData.Value is an FString sometimes - ToString() is safe
                return strProp.Value?.ToString();
            }
            if (prop is UInt32PropertyData u32Prop)
            {
                return u32Prop.Value;
            }
            if (prop is IntPropertyData iProp)
            {
                return iProp.Value;
            }
            if (prop is Int64PropertyData i64Prop)
            {
                return i64Prop.Value;
            }
            if (prop is UInt64PropertyData u64Prop)
            {
                return u64Prop.Value;
            }
            if (prop is FloatPropertyData fProp)
            {
                return fProp.Value;
            }
            if (prop is BoolPropertyData bProp)
            {
                return bProp.Value;
            }

            // fallback
            return prop.ToString();
        }
        private void RefreshSongListUI()
        {
            // adapt to your existing UI code if needed
            songid.Items.Clear();
            foreach (var song in allSongs)
                songid.Items.Add(song.UniqueID.ToString());

            if (allSongs.Count > 0)
            {
                currentSongId = allSongs[0].UniqueID;
                LoadUI(allSongs[0]);
            }
            else
            {
                searchResultLabel.Text = "No songs.";
            }
        }
        private void injectNewIDButton_Click(object sender, EventArgs e)
        {
            // Parse new ID from the UI
            if (!uint.TryParse(songid.Text, out uint newId))
            {
                consoleLabel.Text = "Invalid song ID.";
                return;
            }
            if (newId == 0)
            {
                consoleLabel.Text = "ID 0 is reserved.";
                return;
            }

            // Prevent duplicates
            if (allSongs.Any(s => s.UniqueID == newId))
            {
                consoleLabel.Text = $"ID {newId} already exists.";
                return;
            }

            // Find the MusicParameterTable DataTableExport
            foreach (var export in MusicParameterTable.Exports)
            {
                if (!(export is DataTableExport dataTable))
                    continue;

                // Create a new SongData and fill fields from the current UI using your helper
                var songData = new SongData();
                saveSongData(songData); // populate fields from UI; UniqueID set below
                songData.UniqueID = newId;

                // Build a new StructPropertyData row (same fields as in your saveChangesInRam hint)
                var newRow = new StructPropertyData
                {
                    Name = new FName(MusicParameterTable, new FString(newId.ToString())),
                    StructType = new FName(MusicParameterTable, new FString("MusicParameterTableData")),
                    Value = new List<PropertyData>()
                };

                // Required fields (copied from your pattern)
                newRow.Value.Add(new UInt32PropertyData(new FName(MusicParameterTable, "UniqueID")) { Value = songData.UniqueID });
                newRow.Value.Add(new StrPropertyData(new FName(MusicParameterTable, "MusicMessage")) { Value = (FString)songData.MusicMessage });
                newRow.Value.Add(new StrPropertyData(new FName(MusicParameterTable, "ArtistMessage")) { Value = (FString)songData.ArtistMessage });
                newRow.Value.Add(new StrPropertyData(new FName(MusicParameterTable, "CopyrightMessage")) { Value = (FString)"-" });
                newRow.Value.Add(new UInt32PropertyData(new FName(MusicParameterTable, "VersionNo")) { Value = songData.Version });
                newRow.Value.Add(new StrPropertyData(new FName(MusicParameterTable, "AssetDirectory")) { Value = (FString)songData.AssetDirectory });
                newRow.Value.Add(new StrPropertyData(new FName(MusicParameterTable, "MovieAssetName")) { Value = (FString)songData.MovieAssetName });
                newRow.Value.Add(new StrPropertyData(new FName(MusicParameterTable, "MovieAssetNameHard")) { Value = (FString)songData.MovieAssetNameHard });
                newRow.Value.Add(new StrPropertyData(new FName(MusicParameterTable, "MovieAssetNameExpert")) { Value = (FString)songData.MovieAssetNameExpert });
                newRow.Value.Add(new StrPropertyData(new FName(MusicParameterTable, "MovieAssetNameInferno")) { Value = (FString)songData.MovieAssetNameInferno });
                newRow.Value.Add(new StrPropertyData(new FName(MusicParameterTable, "JacketAssetName")) { Value = (FString)songData.JacketAssetName });
                newRow.Value.Add(new StrPropertyData(new FName(MusicParameterTable, "Rubi")) { Value = (FString)songData.Rubi });

                newRow.Value.Add(new BoolPropertyData(new FName(MusicParameterTable, "bValidCulture_ja_JP")) { Value = songData.ValidCulture_ja_JP });
                newRow.Value.Add(new BoolPropertyData(new FName(MusicParameterTable, "bValidCulture_en_US")) { Value = songData.ValidCulture_en_US });
                newRow.Value.Add(new BoolPropertyData(new FName(MusicParameterTable, "bValidCulture_zh_Hant_TW")) { Value = songData.ValidCulture_zh_Hant_TW });
                newRow.Value.Add(new BoolPropertyData(new FName(MusicParameterTable, "bValidCulture_en_HK")) { Value = songData.ValidCulture_en_HK });
                newRow.Value.Add(new BoolPropertyData(new FName(MusicParameterTable, "bValidCulture_en_SG")) { Value = songData.ValidCulture_en_SG });
                newRow.Value.Add(new BoolPropertyData(new FName(MusicParameterTable, "bValidCulture_ko_KR")) { Value = songData.ValidCulture_ko_KR });
                newRow.Value.Add(new BoolPropertyData(new FName(MusicParameterTable, "bValidCulture_h_Hans_CN_Guest")) { Value = songData.ValidCulture_h_Hans_CN_Guest });
                newRow.Value.Add(new BoolPropertyData(new FName(MusicParameterTable, "bValidCulture_h_Hans_CN_GeneralMember")) { Value = songData.ValidCulture_h_Hans_CN_GeneralMember });
                newRow.Value.Add(new BoolPropertyData(new FName(MusicParameterTable, "bValidCulture_h_Hans_CN_VipMember")) { Value = songData.ValidCulture_h_Hans_CN_VipMember });
                newRow.Value.Add(new BoolPropertyData(new FName(MusicParameterTable, "bValidCulture_Offline")) { Value = songData.ValidCulture_Offline });
                newRow.Value.Add(new BoolPropertyData(new FName(MusicParameterTable, "bValidCulture_NoneActive")) { Value = songData.ValidCulture_NoneActive });

                newRow.Value.Add(new BoolPropertyData(new FName(MusicParameterTable, "bRecommend")) { Value = songData.Recommend });
                newRow.Value.Add(new IntPropertyData(new FName(MusicParameterTable, "WaccaPointCost")) { Value = songData.WaccaPointCost });
                newRow.Value.Add(new BytePropertyData(new FName(MusicParameterTable, "bCollaboration"))
                {
                    EnumType = new FName(MusicParameterTable, "None"),
                    Value = 0
                });
                newRow.Value.Add(new BytePropertyData(new FName(MusicParameterTable, "bWaccaOriginal"))
                {
                    EnumType = new FName(MusicParameterTable, "None"),
                    Value = 0
                });
                newRow.Value.Add(new BytePropertyData(new FName(MusicParameterTable, "TrainingLevel"))
                {
                    EnumType = new FName(MusicParameterTable, "None"),
                    Value = 3
                });
                newRow.Value.Add(new BytePropertyData(new FName(MusicParameterTable, "Reserved"))
                {
                    EnumType = new FName(MusicParameterTable, "None"),
                    Value = 0
                });

                newRow.Value.Add(new StrPropertyData(new FName(MusicParameterTable, "Bpm")) { Value = (FString)songData.Bpm });
                newRow.Value.Add(new StrPropertyData(new FName(MusicParameterTable, "HashTag")) { Value = (FString)"Yosh" });

                newRow.Value.Add(new StrPropertyData(new FName(MusicParameterTable, "NotesDesignerNormal")) { Value = (FString)songData.NotesDesignerNormal });
                newRow.Value.Add(new StrPropertyData(new FName(MusicParameterTable, "NotesDesignerHard")) { Value = (FString)songData.NotesDesignerHard });
                newRow.Value.Add(new StrPropertyData(new FName(MusicParameterTable, "NotesDesignerExpert")) { Value = (FString)songData.NotesDesignerExpert });
                newRow.Value.Add(new StrPropertyData(new FName(MusicParameterTable, "NotesDesignerInferno")) { Value = (FString)songData.NotesDesignerInferno });

                newRow.Value.Add(new FloatPropertyData(new FName(MusicParameterTable, "DifficultyNormalLv")) { Value = songData.DifficultyNormalLv });
                newRow.Value.Add(new FloatPropertyData(new FName(MusicParameterTable, "DifficultyHardLv")) { Value = songData.DifficultyHardLv });
                newRow.Value.Add(new FloatPropertyData(new FName(MusicParameterTable, "DifficultyExtremeLv")) { Value = songData.DifficultyExpertLv });
                newRow.Value.Add(new FloatPropertyData(new FName(MusicParameterTable, "DifficultyInfernoLv")) { Value = songData.DifficultyInfernoLv });

                newRow.Value.Add(new FloatPropertyData(new FName(MusicParameterTable, "ClearNormaRateNormal")) { Value = songData.ClearRateNormal });
                newRow.Value.Add(new FloatPropertyData(new FName(MusicParameterTable, "ClearNormaRateHard")) { Value = songData.ClearRateHard });
                newRow.Value.Add(new FloatPropertyData(new FName(MusicParameterTable, "ClearNormaRateExtreme")) { Value = songData.ClearRateExpert });
                newRow.Value.Add(new FloatPropertyData(new FName(MusicParameterTable, "ClearNormaRateInferno")) { Value = songData.ClearRateInferno });

                newRow.Value.Add(new FloatPropertyData(new FName(MusicParameterTable, "PreviewBeginTime")) { Value = songData.PreviewBeginTime });
                newRow.Value.Add(new FloatPropertyData(new FName(MusicParameterTable, "PreviewSeconds")) { Value = songData.PreviewSeconds });

                newRow.Value.Add(new IntPropertyData(new FName(MusicParameterTable, "ScoreGenre")) { Value = songData.ScoreGenre });
                newRow.Value.Add(new IntPropertyData(new FName(MusicParameterTable, "MusicTagForUnlock0")) { Value = songData.bingo0 });
                newRow.Value.Add(new IntPropertyData(new FName(MusicParameterTable, "MusicTagForUnlock1")) { Value = songData.bingo1 });
                newRow.Value.Add(new IntPropertyData(new FName(MusicParameterTable, "MusicTagForUnlock2")) { Value = songData.bingo2 });
                newRow.Value.Add(new IntPropertyData(new FName(MusicParameterTable, "MusicTagForUnlock3")) { Value = songData.bingo3 });
                newRow.Value.Add(new IntPropertyData(new FName(MusicParameterTable, "MusicTagForUnlock4")) { Value = songData.bingo4 });
                newRow.Value.Add(new IntPropertyData(new FName(MusicParameterTable, "MusicTagForUnlock5")) { Value = songData.bingo5 });
                newRow.Value.Add(new IntPropertyData(new FName(MusicParameterTable, "MusicTagForUnlock6")) { Value = songData.bingo6 });
                newRow.Value.Add(new IntPropertyData(new FName(MusicParameterTable, "MusicTagForUnlock7")) { Value = songData.bingo7 });
                newRow.Value.Add(new IntPropertyData(new FName(MusicParameterTable, "MusicTagForUnlock8")) { Value = songData.bingo8 });
                newRow.Value.Add(new IntPropertyData(new FName(MusicParameterTable, "MusicTagForUnlock9")) { Value = songData.bingo9 });
                newRow.Value.Add(new UInt64PropertyData(new FName(MusicParameterTable, "WorkBuffer")) { Value = 0 });
                newRow.Value.Add(new StrPropertyData(new FName(MusicParameterTable, "AssetFullPath")) { Value = (FString)$"D:/project/Mercury/Mercury/Content//MusicData/{songData.AssetDirectory}" });

                // Insert at the beginning of the DataTable
                dataTable.Table.Data.Insert(0, newRow);

                // Update in-memory lists / UI
                songid.Items.Insert(0, songData.UniqueID.ToString());
                allSongs.Insert(0, songData);

                // Attempt to set unlock status; ignore if out of range
                try
                {
                    musicUnlockStatus[(int)newId] = checkBoxNew.Checked;
                    //SaveAllUnlockMusic();
                    SaveCurentUnlockInferno();
                }
                catch
                {
                    // If musicUnlockStatus is an array and newId is out of bounds we ignore here.
                    // You may want to resize the array/list if needed.
                }

                // Set current selection and show it
                currentSongId = songData.UniqueID;
                consoleLabel.Text = $"Injected new ID {newId}.";
                LoadUI(songData);

                // Persist according to user mode (auto/ram/manual)
                saveChanges();

                return; // done
            }

            // If we reach here, no DataTableExport found
            consoleLabel.Text = "MusicParameterTable export not found.";
        }

        // --- Helpers ---
        private void WriteListToFile(IEnumerable<string> lines, string filename)
        {
            try
            {
                File.WriteAllLines(filename, lines.Where(l => !string.IsNullOrWhiteSpace(l)));
                consoleLabel.Text = $"Wrote {lines.Count()} lines to {filename}";
            }
            catch (Exception ex)
            {
                consoleLabel.Text = $"Error writing {filename}: {ex.Message}";
            }
        }

        // --- Buttons ---

        private void writeChartersNormalButton_Click(object sender, EventArgs e)
        {
            var lines = allSongs.Select(s => s.NotesDesignerNormal);
            WriteListToFile(lines, "ChartersNormal.txt");
        }

        private void writeChartersHardButton_Click(object sender, EventArgs e)
        {
            var lines = allSongs.Select(s => s.NotesDesignerHard);
            WriteListToFile(lines, "ChartersHard.txt");
        }

        private void writeChartersExtremeButton_Click(object sender, EventArgs e)
        {
            var lines = allSongs.Select(s => s.NotesDesignerExpert);
            WriteListToFile(lines, "ChartersExtreme.txt");
        }

        private void writeChartersInfernoButton_Click(object sender, EventArgs e)
        {
            var lines = allSongs.Select(s => s.NotesDesignerInferno);
            WriteListToFile(lines, "ChartersInferno.txt");
        }

        private void writeMusicButton_Click(object sender, EventArgs e)
        {
            var lines = allSongs.Select(s => s.MusicMessage);
            WriteListToFile(lines, "MusicTitle.txt");
        }

        private void writeArtistButton_Click(object sender, EventArgs e)
        {
            var lines = allSongs.Select(s => s.ArtistMessage);
            WriteListToFile(lines, "MusicArtist.txt");
        }


        private void next999Button_Click(object sender, EventArgs e)
        {
            for (short i = 0; i < 1000; i++)
            {
                nextSongButton_Click(null, null);
            }
        }

        private void searchNext999Button_Click(object sender, EventArgs e)
        {
            for (short i = 0; i < 1000; i++)
            {
                searchNextButton_Click(null, null);
            }
        }

        private void rubiAutoFixCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (rubiAutoFixCheckBox.Checked)
            {
                rubiTextBox_TextChanged(null, null);
            }
        }
    }
}
