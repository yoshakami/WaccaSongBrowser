using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.PropertyTypes.Structs;
using UAssetAPI;

namespace WaccaSongList
{
    public partial class WaccaSongBrowser : Form
    {
        public WaccaSongBrowser()
        {
            InitializeComponent();
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
        private void WaccaSongBrowser_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (var file in files)
                {
                    Read(file);
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
                currentSongId = nextMatch.UniqueID;
                Load(nextMatch);
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
                currentSongId = nextMatch.UniqueID;
                Load(nextMatch);
            }
        }

        private void filterGenreButton_Click(object sender, EventArgs e)
        {
            int filter = filterGenre.SelectedIndex;

            if (allSongs.Count == 0)
                return;

            // Filter songs by name
            var matchingSongs = allSongs
                .Where(song => song.ScoreGenre == filter)
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
                if (song.ScoreGenre == filter)
                {
                    nextMatch = song;
                    break;
                }
            }

            if (nextMatch != null)
            {
                currentSongId = nextMatch.UniqueID;
                Load(nextMatch);
            }
            FilterByIntProperty("ScoreGenre", filterGenre.SelectedIndex);
        }
        private void filterVersionButton_Click(object sender, EventArgs e)
        {
            FilterByIntProperty("Version", filterVersion.SelectedIndex);
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
                    currentSongId = song.UniqueID;
                    Load(song);
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
                    currentSongId = song.UniqueID;
                    Load(song);
                    break;
                }
            }
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
            FilterByBoolProperty("ValidCulture_zh_Hans_CN_Guest", filterkokrCheckBox.Checked);
        }
        private void filtercngeButton_Click(object sender, EventArgs e)
        {
            FilterByBoolProperty("ValidCulture_zh_Hans_CN_GeneralMember", filterkokrCheckBox.Checked);
        }
        private void filterfiltercnvipButton_Click(object sender, EventArgs e)
        {
            FilterByBoolProperty("ValidCulture_zh_Hans_CN_VipMember", filterkokrCheckBox.Checked);
        }
        private void filternotAvailableButton_Click(object sender, EventArgs e)
        {
            FilterByBoolProperty("ValidCulture_NoneActive", filterkokrCheckBox.Checked);
        }

        private void filterofflineButton_Click(object sender, EventArgs e)
        {
            FilterByBoolProperty("ValidCulture_Offline", filterofflineCheckBox.Checked);
        }

        private void filterBeginnerButton_Click(object sender, EventArgs e)
        {
            FilterByBoolProperty("Recommend", filterBeginnerCheckBox.Checked);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {

        }

        uint currentSongId;
        private void songidButton_Click(object sender, EventArgs e)
        {
            if ()
            uint.TryParse(songid.Text, out currentSongId);
            uint.TryParse(songidTextBox.Text, out currentSongId);

            int currentIndex = allSongs.FindIndex(s => s.UniqueID == currentSongId);
            Load(allSongs[currentIndex]);
        }
        private void Load(SongData song)
        {

        }

        private void autoSaveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (autoSaveCheckBox.Checked)
            {
                saveButton_Click(null, null);
            }
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
        List<SongData> allSongs;

        public static void Read(string file)
        {
            string uassetPath = file;

            // Load the asset (assumes .uexp is in the same folder)
            var asset = new UAsset(uassetPath, UAssetAPI.UnrealTypes.EngineVersion.VER_UE4_19);

            // Go through each export to find the DataTable
            foreach (var export in asset.Exports)
            {
                if (export is DataTableExport dataTable)
                {
                    Console.WriteLine("Reading rows from DataTable...\n");

                    foreach (var row in dataTable.Table.Data)
                    {
                        if (row is StructPropertyData rowStruct)
                        {
                            var data = new SongData
                            {
                                UniqueID = GetFieldValue<uint>(rowStruct, "UniqueID"),
                                MusicMessage = GetFieldValue<string>(rowStruct, "MusicMessage"),
                                ArtistMessage = GetFieldValue<string>(rowStruct, "ArtistMessage"),
                                //CopyrightMessage = GetFieldValue<string>(rowStruct, "CopyrightMessage"),
                                Version = GetFieldValue<string>(rowStruct, "Version"),
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
                                ValidCulture_zh_Hans_CN_Guest = GetFieldValue<bool>(rowStruct, "bValidCulture_zh_Hans_CN_Guest"),
                                ValidCulture_zh_Hans_CN_GeneralMember = GetFieldValue<bool>(rowStruct, "bValidCulture_zh_Hans_CN_GeneralMember"),
                                ValidCulture_zh_Hans_CN_VipMember = GetFieldValue<bool>(rowStruct, "bValidCulture_zh_Hans_CN_VipMember"),
                                ValidCulture_Offline = GetFieldValue<bool>(rowStruct, "bValidCulture_Offline"),
                                ValidCulture_NoneActive = GetFieldValue<bool>(rowStruct, "bValidCulture_NoneActive"),

                                Recommend = GetFieldValue<bool>(rowStruct, "bRecommend"),
                                WaccaPointCost = GetFieldValue<int>(rowStruct, "WaccaPointCost"),
                                //Collaboration = GetFieldValue<byte>(rowStruct, "Collaboration"),
                                //WaccaOriginal = GetFieldValue<byte>(rowStruct, "WaccaOriginal"),
                                //TrainingLevel = GetFieldValue<byte>(rowStruct, "TrainingLevel"),
                                //Reserved = GetFieldValue<byte>(rowStruct, "Reserved"),

                                Bpm = GetFieldValue<float>(rowStruct, "Bpm"),
                                //HashTag = GetFieldValue<string>(rowStruct, "HashTag"),

                                NotesDesignerNormal = GetFieldValue<string>(rowStruct, "NotesDesignerNormal"),
                                NotesDesignerHard = GetFieldValue<string>(rowStruct, "NotesDesignerHard"),
                                NotesDesignerExpert = GetFieldValue<string>(rowStruct, "NotesDesignerExpert"),
                                NotesDesignerInferno = GetFieldValue<string>(rowStruct, "NotesDesignerInferno"),

                                DifficultyNormalLv = GetFieldValue<float>(rowStruct, "DifficultyNormalLv"),
                                DifficultyHardLv = GetFieldValue<float>(rowStruct, "DifficultyHardLv"),
                                DifficultyExpertLv = GetFieldValue<float>(rowStruct, "DifficultyExpertLv"),
                                DifficultyInfernoLv = GetFieldValue<float>(rowStruct, "DifficultyInfernoLv"),

                                ClearRateNormal = GetFieldValue<float>(rowStruct, "ClearNormaRateNormal"),
                                ClearRateHard = GetFieldValue<float>(rowStruct, "ClearNormaRateHard"),
                                ClearRateExpert = GetFieldValue<float>(rowStruct, "ClearNormaRateExpert"),
                                ClearRateInferno = GetFieldValue<float>(rowStruct, "ClearNormaRateInferno"),

                                PreviewBeginTime = GetFieldValue<float>(rowStruct, "PreviewBeginTime"),
                                PreviewSeconds = GetFieldValue<float>(rowStruct, "PreviewSeconds"),

                                ScoreGenre = GetFieldValue<int>(rowStruct, "ScoreGenre"),
                                bingo0 = GetFieldValue<int>(rowStruct, "bingo0"),
                                bingo1 = GetFieldValue<int>(rowStruct, "bingo1"),
                                bingo2 = GetFieldValue<int>(rowStruct, "bingo2"),
                                bingo3 = GetFieldValue<int>(rowStruct, "bingo3"),
                                bingo4 = GetFieldValue<int>(rowStruct, "bingo4"),
                                bingo5 = GetFieldValue<int>(rowStruct, "bingo5"),
                                bingo6 = GetFieldValue<int>(rowStruct, "bingo6"),
                                bingo7 = GetFieldValue<int>(rowStruct, "bingo7"),
                                bingo8 = GetFieldValue<int>(rowStruct, "bingo8"),
                                bingo9 = GetFieldValue<int>(rowStruct, "bingo9"),
                                //public ulong WorkBuffer { get; set; }
                                AssetFullPath = GetFieldValue<string>(rowStruct, "AssetFullPath"),
                            };
                        }
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
