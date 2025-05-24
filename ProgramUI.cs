using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.PropertyTypes.Structs;
using UAssetAPI;
using UAssetAPI.UnrealTypes;

namespace WaccaSongBrowser
{
    public partial class WaccaSongBrowser : Form
    {
        public WaccaSongBrowser()
        {
            InitializeComponent();
            genre.Items.Add("0: Anime/Pop");
            genre.Items.Add("1: Vocaloid");
            genre.Items.Add("2: Touhou");
            genre.Items.Add("3: 2.5D");
            genre.Items.Add("4: Variety");
            genre.Items.Add("5: Original");
            genre.Items.Add("6: Tano*C");
            filterGenre.Items.Add("0: Anime/Pop");
            filterGenre.Items.Add("1: Vocaloid");
            filterGenre.Items.Add("2: Touhou");
            filterGenre.Items.Add("3: 2.5D");
            filterGenre.Items.Add("4: Variety");
            filterGenre.Items.Add("5: Original");
            filterGenre.Items.Add("6: Tano*C");
            version.Items.Add("1: Wacca");
            version.Items.Add("2: Wacca S");
            version.Items.Add("3: Wacca Lily");
            version.Items.Add("4: Wacca Lily R");
            version.Items.Add("5: Wacca Reverse");
            filterVersion.Items.Add("1: Wacca");
            filterVersion.Items.Add("2: Wacca S");
            filterVersion.Items.Add("3: Wacca Lily");
            filterVersion.Items.Add("4: Wacca Lily R");
            filterVersion.Items.Add("5: Wacca Reverse");
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
        static string openedFileName;
        private void WaccaSongBrowser_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var file in files)
                {
                    try
                    {
                        allSongs.Clear();
                        songid.Items.Clear();
                        Read(file);
                        openedFileName = file;
                        consoleLabel.Text = "file loaded successfully! -> will overwrite on next save";
                        return;
                    }
                    catch { }
                }
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

        private void saveSongData(SongData songData)
        {

            songData.MusicMessage = musicTextBox.Text;
            songData.ArtistMessage = artistTextBox.Text;
            if (!doNotChangeVersionBecauseItIsNegative)
                songData.Version = (uint)version.SelectedIndex;
            songData.AssetDirectory = merTextBox.Text;
            songData.MovieAssetName = movieNormalTextBox.Text;
            songData.MovieAssetName = movieHardTextBox.Text;
            songData.MovieAssetName = movieExtremeTextBox.Text;
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
            if (saveChangesInRam())
            {
                saveLabel.Text = $"Saved {++savecount} times";
                MusicParameterTable.Write(openedFileName);
            }
            else
            {
                saveLabel.Text = "file not saved.";
            }
        }
        private void ramSaveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ramSaveCheckBox.Checked)
            {
                saveChangesInRam();
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
                    Console.WriteLine("Reading rows from DataTable...\n");
                    var songData = allSongs.FirstOrDefault(s => s.UniqueID == currentSongId);

                    if (songData == null)  // user added a new song id
                    {
                        songData = new SongData();
                        saveSongData(songData);
                        // Create a new StructPropertyData row
                        var newRow = new StructPropertyData
                        {
                            Name = new FName(MusicParameterTable, new FString(currentSongId.ToString())), // peak code moment
                            StructType = new FName(MusicParameterTable, new FString("MusicParameterTableData")),
                            Value = new List<PropertyData>()
                        };

                        // regex is my friend
                        newRow.Value.Add(new UInt32PropertyData(new FName((INameMap)(FString)"UniqueID")) { Value = songData.UniqueID });
                        newRow.Value.Add(new StrPropertyData(new FName((INameMap)(FString)"MusicMessage")) { Value = (FString)songData.MusicMessage });
                        newRow.Value.Add(new StrPropertyData(new FName((INameMap)(FString)"ArtistMessage")) { Value = (FString)songData.ArtistMessage });
                        newRow.Value.Add(new StrPropertyData(new FName((INameMap)(FString)"CopyrightMessage")) { Value = (FString)"-" });
                        newRow.Value.Add(new UInt32PropertyData(new FName((INameMap)(FString)"VersionNo")) { Value = songData.Version });
                        newRow.Value.Add(new StrPropertyData(new FName((INameMap)(FString)"AssetDirectory")) { Value = (FString)songData.AssetDirectory });
                        newRow.Value.Add(new StrPropertyData(new FName((INameMap)(FString)"MovieAssetName")) { Value = (FString)songData.MovieAssetName });
                        newRow.Value.Add(new StrPropertyData(new FName((INameMap)(FString)"MovieAssetNameHard")) { Value = (FString)songData.MovieAssetNameHard });
                        newRow.Value.Add(new StrPropertyData(new FName((INameMap)(FString)"MovieAssetNameExpert")) { Value = (FString)songData.MovieAssetNameExpert });
                        newRow.Value.Add(new StrPropertyData(new FName((INameMap)(FString)"MovieAssetNameInferno")) { Value = (FString)songData.MovieAssetNameInferno });
                        newRow.Value.Add(new StrPropertyData(new FName((INameMap)(FString)"JacketAssetName")) { Value = (FString)songData.JacketAssetName });
                        newRow.Value.Add(new StrPropertyData(new FName((INameMap)(FString)"Rubi")) { Value = (FString)songData.Rubi });

                        newRow.Value.Add(new BoolPropertyData(new FName((INameMap)(FString)"bValidCulture_ja_JP")) { Value = songData.ValidCulture_ja_JP });
                        newRow.Value.Add(new BoolPropertyData(new FName((INameMap)(FString)"bValidCulture_en_US")) { Value = songData.ValidCulture_en_US });
                        newRow.Value.Add(new BoolPropertyData(new FName((INameMap)(FString)"bValidCulture_zh_Hant_TW")) { Value = songData.ValidCulture_zh_Hant_TW });
                        newRow.Value.Add(new BoolPropertyData(new FName((INameMap)(FString)"bValidCulture_en_HK")) { Value = songData.ValidCulture_en_HK });
                        newRow.Value.Add(new BoolPropertyData(new FName((INameMap)(FString)"bValidCulture_en_SG")) { Value = songData.ValidCulture_en_SG });
                        newRow.Value.Add(new BoolPropertyData(new FName((INameMap)(FString)"bValidCulture_ko_KR")) { Value = songData.ValidCulture_ko_KR });
                        newRow.Value.Add(new BoolPropertyData(new FName((INameMap)(FString)"bValidCulture_h_Hans_CN_Guest")) { Value = songData.ValidCulture_h_Hans_CN_Guest });
                        newRow.Value.Add(new BoolPropertyData(new FName((INameMap)(FString)"bValidCulture_h_Hans_CN_GeneralMember")) { Value = songData.ValidCulture_h_Hans_CN_GeneralMember });
                        newRow.Value.Add(new BoolPropertyData(new FName((INameMap)(FString)"bValidCulture_h_Hans_CN_VipMember")) { Value = songData.ValidCulture_h_Hans_CN_VipMember });
                        newRow.Value.Add(new BoolPropertyData(new FName((INameMap)(FString)"bValidCulture_Offline")) { Value = songData.ValidCulture_Offline });
                        newRow.Value.Add(new BoolPropertyData(new FName((INameMap)(FString)"bValidCulture_NoneActive")) { Value = songData.ValidCulture_NoneActive });

                        newRow.Value.Add(new BoolPropertyData(new FName((INameMap)(FString)"bRecommend")) { Value = songData.Recommend });
                        newRow.Value.Add(new IntPropertyData(new FName((INameMap)(FString)"WaccaPointCost")) { Value = songData.WaccaPointCost });
                        newRow.Value.Add(new BytePropertyData(new FName((INameMap)(FString)"bCollaboration"))
                        {
                            EnumType = new FName((INameMap)(FString)"None"), // Optional if "None" is needed
                            Value = 0
                        }); ;
                        newRow.Value.Add(new BytePropertyData(new FName((INameMap)(FString)"bWaccaOriginal"))
                        {
                            EnumType = new FName((INameMap)(FString)"None"), // Optional if "None" is needed
                            Value = 0
                        });
                        newRow.Value.Add(new BytePropertyData(new FName((INameMap)(FString)"TrainingLevel"))
                        {
                            EnumType = new FName((INameMap)(FString)"None"), // Optional if "None" is needed
                            Value = 3 // Sets Value 2 to 3
                        });
                        newRow.Value.Add(new BytePropertyData(new FName((INameMap)(FString)"Reserved"))
                        {
                            EnumType = new FName((INameMap)(FString)"None"), // Optional if "None" is needed
                            Value = 0
                        });

                        newRow.Value.Add(new StrPropertyData(new FName((INameMap)(FString)"Bpm")) { Value = (FString)songData.Bpm });
                        newRow.Value.Add(new StrPropertyData(new FName((INameMap)(FString)"HashTag")) { Value = (FString)"Yosh" });

                        newRow.Value.Add(new StrPropertyData(new FName((INameMap)(FString)"NotesDesignerNormal")) { Value = (FString)songData.NotesDesignerNormal });
                        newRow.Value.Add(new StrPropertyData(new FName((INameMap)(FString)"NotesDesignerHard")) { Value = (FString)songData.NotesDesignerHard });
                        newRow.Value.Add(new StrPropertyData(new FName((INameMap)(FString)"NotesDesignerExpert")) { Value = (FString)songData.NotesDesignerExpert });
                        newRow.Value.Add(new StrPropertyData(new FName((INameMap)(FString)"NotesDesignerInferno")) { Value = (FString)songData.NotesDesignerInferno });

                        newRow.Value.Add(new FloatPropertyData(new FName((INameMap)(FString)"DifficultyNormalLv")) { Value = songData.DifficultyNormalLv });
                        newRow.Value.Add(new FloatPropertyData(new FName((INameMap)(FString)"DifficultyHardLv")) { Value = songData.DifficultyHardLv });
                        newRow.Value.Add(new FloatPropertyData(new FName((INameMap)(FString)"DifficultyExtremeLv")) { Value = songData.DifficultyExpertLv });
                        newRow.Value.Add(new FloatPropertyData(new FName((INameMap)(FString)"DifficultyInfernoLv")) { Value = songData.DifficultyInfernoLv });

                        newRow.Value.Add(new FloatPropertyData(new FName((INameMap)(FString)"ClearNormaRateNormal")) { Value = songData.ClearRateNormal });
                        newRow.Value.Add(new FloatPropertyData(new FName((INameMap)(FString)"ClearNormaRateHard")) { Value = songData.ClearRateHard });
                        newRow.Value.Add(new FloatPropertyData(new FName((INameMap)(FString)"ClearNormaRateExtreme")) { Value = songData.ClearRateExpert });
                        newRow.Value.Add(new FloatPropertyData(new FName((INameMap)(FString)"ClearNormaRateInferno")) { Value = songData.ClearRateInferno });

                        newRow.Value.Add(new FloatPropertyData(new FName((INameMap)(FString)"PreviewBeginTime")) { Value = songData.PreviewBeginTime });
                        newRow.Value.Add(new FloatPropertyData(new FName((INameMap)(FString)"PreviewSeconds")) { Value = songData.PreviewSeconds });

                        newRow.Value.Add(new IntPropertyData(new FName((INameMap)(FString)"ScoreGenre")) { Value = songData.ScoreGenre });
                        newRow.Value.Add(new IntPropertyData(new FName((INameMap)(FString)"MusicTagForUnlock0")) { Value = songData.bingo0 });
                        newRow.Value.Add(new IntPropertyData(new FName((INameMap)(FString)"MusicTagForUnlock1")) { Value = songData.bingo1 });
                        newRow.Value.Add(new IntPropertyData(new FName((INameMap)(FString)"MusicTagForUnlock2")) { Value = songData.bingo2 });
                        newRow.Value.Add(new IntPropertyData(new FName((INameMap)(FString)"MusicTagForUnlock3")) { Value = songData.bingo3 });
                        newRow.Value.Add(new IntPropertyData(new FName((INameMap)(FString)"MusicTagForUnlock4")) { Value = songData.bingo4 });
                        newRow.Value.Add(new IntPropertyData(new FName((INameMap)(FString)"MusicTagForUnlock5")) { Value = songData.bingo5 });
                        newRow.Value.Add(new IntPropertyData(new FName((INameMap)(FString)"MusicTagForUnlock6")) { Value = songData.bingo6 });
                        newRow.Value.Add(new IntPropertyData(new FName((INameMap)(FString)"MusicTagForUnlock7")) { Value = songData.bingo7 });
                        newRow.Value.Add(new IntPropertyData(new FName((INameMap)(FString)"MusicTagForUnlock8")) { Value = songData.bingo8 });
                        newRow.Value.Add(new IntPropertyData(new FName((INameMap)(FString)"MusicTagForUnlock9")) { Value = songData.bingo9 });
                        newRow.Value.Add(new UInt64PropertyData(new FName((INameMap)(FString)"WorkBuffer")) { Value = 0 });
                        newRow.Value.Add(new StrPropertyData(new FName((INameMap)(FString)"AssetFullPath")) { Value = (FString)$"D:/project/Mercury/Mercury/Content/MusicData/{songData.AssetDirectory}" });
                        // all fields are there.

                        // Finally, add it to the DataTable
                        dataTable.Table.Data.Add(newRow);
                        return true;
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
        void SetFieldValue(StructPropertyData structData, string fieldName, object newValue)
        {
            var prop = structData.Value.FirstOrDefault(p => p.Name.Value.ToString() == fieldName);

            if (prop is StrPropertyData strProp && newValue is string strVal)
            {
                strProp.Value = (UAssetAPI.UnrealTypes.FString)strVal;
            }
            else if (prop is IntPropertyData intProp && newValue is int intVal)
            {
                intProp.Value = intVal;
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
            else
            {
                Console.WriteLine($"Unsupported or mismatched type for field '{fieldName}'");
            }
        }


        uint currentSongId;
        private void songidButton_Click(object sender, EventArgs e)
        {
            if (allSongs.Count == 0)
                return;
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
                consoleLabel.Text = "Creating new song ID. Will be saved upon next click on save, or on next Song ID change if autosave is on";
                saveLabel.Text = "";
                return;
            }
            LoadUI(allSongs[currentIndex]);
        }
        static bool doNotChangeVersionBecauseItIsNegative = false;
        private void LoadUI(SongData song)
        {
            musicTextBox.Text = song.MusicMessage;
            artistTextBox.Text = song.ArtistMessage;
            genre.SelectedIndex = song.ScoreGenre;
            rubiTextBox.Text = song.Rubi;
            pointCostTextBox.Text = song.WaccaPointCost.ToString();
            merTextBox.Text = song.AssetDirectory;
            jacketTextBox.Text = song.JacketAssetName;
            bpmTextBox.Text = song.Bpm.ToString();
            previewTimeTextBox.Text = song.PreviewBeginTime.ToString();
            previewSecTextBox.Text = song.PreviewSeconds.ToString();
            if ((int)(song.Version - 1) < 0)
                doNotChangeVersionBecauseItIsNegative = true;
            else
                version.SelectedIndex = (int)(song.Version - 1);
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
            beginnerCheckBox.Checked = song.Recommend;

            //filters
            filterofflineCheckBox.Checked = song.ValidCulture_Offline;
            filterjaCheckBox.Checked = song.ValidCulture_ja_JP;
            filterusaCheckBox.Checked = song.ValidCulture_en_US;
            filterzhtwCheckBox.Checked = song.ValidCulture_zh_Hant_TW;
            filterenhkCheckBox.Checked = song.ValidCulture_en_HK;
            filterensgCheckBox.Checked = song.ValidCulture_en_SG;
            filterkokrCheckBox.Checked = song.ValidCulture_ko_KR;
            filtercnguCheckBox.Checked = song.ValidCulture_h_Hans_CN_Guest;
            filtercngeCheckBox.Checked = song.ValidCulture_h_Hans_CN_GeneralMember;
            filtercnvipCheckBox.Checked = song.ValidCulture_h_Hans_CN_VipMember;
            filternotAvailableCheckBox.Checked = song.ValidCulture_NoneActive;
            filterBeginnerCheckBox.Checked = song.Recommend;

            songid.Text = song.UniqueID.ToString();
            filterMusicTextBox.Text = song.MusicMessage;
            filterArtistTextBox.Text = song.ArtistMessage;
            filterGenre.SelectedIndex = song.ScoreGenre;
            filterVersion.SelectedIndex = (int)(song.Version - 1);

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
            saveChanges();
        }

        public static T GetFieldValue<T>(StructPropertyData structData, string fieldName)
        {
            var prop = structData.Value.FirstOrDefault(p => p.Name.Value.ToString() == fieldName);

            return prop switch
            {
                StrPropertyData strProp when typeof(T) == typeof(string) => (T)(object)(strProp.Value?.ToString() ?? "(null)"),
                IntPropertyData intProp when typeof(T) == typeof(int) => (T)(object)intProp.Value,
                UInt32PropertyData uintProp when typeof(T) == typeof(uint) => (T)(object)uintProp.Value,
                BoolPropertyData boolProp when typeof(T) == typeof(bool) => (T)(object)boolProp.Value,
                FloatPropertyData floatProp when typeof(T) == typeof(float) => (T)(object)floatProp.Value,
                BytePropertyData byteProp when typeof(T) == typeof(byte) => (T)(object)byteProp.Value,
                _ => default
            };
        }
        static List<SongData> allSongs = new List<SongData>();
        static UAsset MusicParameterTable;
        void Read(string file)
        {
            string uassetPath = file;

            // Load the asset (assumes .uexp is in the same folder)
            MusicParameterTable = new UAsset(uassetPath, UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_19);

            // Go through each export to find the DataTable
            foreach (var export in MusicParameterTable.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    Console.WriteLine("Reading rows from DataTable...\n");

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
                            songid.Items.Add(data.UniqueID.ToString());
                            allSongs.Add(data);
                        }
                    }
                }
            }
        }
    }
}
