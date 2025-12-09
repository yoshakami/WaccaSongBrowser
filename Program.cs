namespace WaccaSongBrowser
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new WaccaSongBrowser());

        }
    }
    public class UnlockData
    {
        public int MusicId { get; set; }
        public long ItemActivateStartTime { get; set; }
    }
    public class SongData
    {
        public uint UniqueID { get; set; }
        public string MusicMessage { get; set; }
        public string ArtistMessage { get; set; }
        public string CopyrightMessage { get; set; }
        public uint Version { get; set; }
        public string AssetDirectory { get; set; }
        public string MovieAssetName { get; set; }
        public string MovieAssetNameHard { get; set; }
        public string MovieAssetNameExpert { get; set; }
        public string MovieAssetNameInferno { get; set; }
        public string JacketAssetName { get; set; }
        public string Rubi { get; set; }

        public bool ValidCulture_ja_JP { get; set; }
        public bool ValidCulture_en_US { get; set; }
        public bool ValidCulture_zh_Hant_TW { get; set; }
        public bool ValidCulture_en_HK { get; set; }
        public bool ValidCulture_en_SG { get; set; }
        public bool ValidCulture_ko_KR { get; set; }
        public bool ValidCulture_h_Hans_CN_Guest { get; set; }
        public bool ValidCulture_h_Hans_CN_GeneralMember { get; set; }
        public bool ValidCulture_h_Hans_CN_VipMember { get; set; }
        public bool ValidCulture_Offline { get; set; }
        public bool ValidCulture_NoneActive { get; set; }

        public bool Recommend { get; set; }  // beginner
        public int WaccaPointCost { get; set; }
        public byte bCollaboration { get; set; }
        public byte bWaccaOriginal { get; set; }
        public byte TrainingLevel { get; set; }
        // public byte Reserved { get; set; }
        public string Bpm { get; set; }
        public string HashTag { get; set; }

        public string NotesDesignerNormal { get; set; }
        public string NotesDesignerHard { get; set; }
        public string NotesDesignerExpert { get; set; }
        public string NotesDesignerInferno { get; set; }

        public float DifficultyNormalLv { get; set; }
        public float DifficultyHardLv { get; set; }
        public float DifficultyExpertLv { get; set; }
        public float DifficultyInfernoLv { get; set; }

        public float ClearRateNormal { get; set; }
        public float ClearRateHard { get; set; }
        public float ClearRateExpert { get; set; }
        public float ClearRateInferno { get; set; }
        public float PreviewBeginTime { get; set; }
        public float PreviewSeconds { get; set; }
        public int ScoreGenre { get; set; }  // category
        public int bingo0 { get; set; }
        public int bingo1 { get; set; }
        public int bingo2 { get; set; }
        public int bingo3 { get; set; }
        public int bingo4 { get; set; }
        public int bingo5 { get; set; }
        public int bingo6 { get; set; }
        public int bingo7 { get; set; }
        public int bingo8 { get; set; }
        public int bingo9 { get; set; }
        public ulong WorkBuffer { get; set; }
        //public string AssetFullPath { get; set; }
    };
    public class ConditionData
    {
        public int ConditionId { get; set; }
        public bool bConditionLimitNowSeason { get; set; }
        public int ConditionType { get; set; }
        public string Value1 { get; set; }
        public string Value2 { get; set; }
        public string Value3 { get; set; }
        public string Value4 { get; set; }
        public string Value5 { get; set; }
    }
    public class IconData
    {
        public int IconId { get; set; }
        public string IconTextureName { get; set; }
        public sbyte IconRarity { get; set; }
        public string NameTag { get; set; }
        public string ExplanationTextTag { get; set; }
        public long ItemActivateStartTime { get; set; }
        public long ItemActivateEndTime { get; set; }
        public bool bIsInitItem { get; set; }
        public int GainWaccaPoint { get; set; }
    }
    public class GradeData
    {
        public int GradeId { get; set; }
        public int GradePartsId01 { get; set; }
        public int GradePartsId02 { get; set; }
        public int GradePartsId03 { get; set; }
        public byte GradeRarity { get; set; }
        public string NameTag { get; set; }
        public string ExplanationTextTag { get; set; }
        public long ItemActivateStartTime { get; set; }
        public long ItemActivateEndTime { get; set; }
        public bool bIsInitItem { get; set; }
        public int GainWaccaPoint { get; set; }
    }
    public class GradePartsTableData
    {
        public int GradePartsId { get; set; }
        public int GradePartsType { get; set; }
        public string NameTag { get; set; }
        public string ExplanationTextTag { get; set; }
        public long ItemActivateStartTime { get; set; }
        public long ItemActivateEndTime { get; set; }
        public bool bIsInitItem { get; set; }
        public int GainWaccaPoint { get; set; }
    }
    public class MessageData
    {
        public string JapaneseMessage { get; set; }
        public string EnglishMessageUSA { get; set; }
        public string EnglishMessageSG { get; set; }
        public string TraditionalChineseMessageTW { get; set; }
        public string TraditionalChineseMessageHK { get; set; }
        public string SimplifiedChineseMessage { get; set; }
        public string KoreanMessage { get; set; }
    }
        public class TotalResultItemJudgementData
    {
        public int ItemId { get; set; }
        public long ConditionGetableStartTime { get; set; }
        public long ConditionGetableEndTime { get; set; }
        public List<string> ConditionKeys { get; set; } = new List<string>();
    }

    // Define your mapping once (dictionary)
    public static class Conditions
    {
        public static Dictionary<int, string> Types = new Dictionary<int, string>
        {
        { -2, "INVALID" },
        { -1, "SYSTEM" },
        { 0, "INIT" },
        { 1, "LEVEL_UP_COUNT" },
        { 2, "TOTAL_WACCA_POINT" },
        { 3, "TOTAL_SCORE" },
        { 4, "GRADE_COUNT" },
        { 5, "ICON_COUNT" },
        { 6, "SYMBOL_COLOR_COUNT" },
        { 7, "INPUT_SE_COUNT" },
        { 8, "UNLOCK_MUSIC_COUNT" },
        { 9, "PLAY_COUNT" },
        { 10, "PLAY_COUNT_RATE" },
        { 11, "PLAY_MUSIC_COUNT" },
        { 12, "PLAY_MUSIC_COUNT_RATE" },
        { 13, "PLAY_MUSIC_SCORE_COUNT" },
        { 14, "PLAY_MUSIC_SCORE_COUNT_RATE" },
        { 15, "STAGE_UP" },
        { 16, "GAME_PLAY_COUNT" },
        { 17, "MULTI_PLAY_COUNT" },
        { 18, "GAME_PLAY_COUNT_MODE" },
        { 19, "PLAY_MUSIC_TAG_ALL_DIFFICULTY" },
        { 20, "PLAY_MUSIC_TAG_ALL_DIFFICULTY_RATE" },
        { 21, "1PLAY_MUSIC" },
        { 22, "1PLAY_MUSIC_RATE" },
        { 23, "PLAY_MUSIC_TAG" },
        { 24, "RATE_COUNT_LOW" },
        { 25, "TROPHY_COMPLETE" },
        { 26, "OPEN_MUSIC_TAG" },
        { 27, "PLAY_MUSIC_ID" },
        { 28, "USE_ICON_NUM" },
        { 29, "MUSIC_CONTINUE_PLAY" },
        { 30, "MUSIC_SELECT_CANCEL" },
        { 31, "DATE" },
        { 32, "PREFECTURES" },
        { 33, "PLAY_AREA" },
        { 34, "PLAY_AREA_COMPLETE" },
        { 35, "GRADE_CUSTOMIZE_PLAY" },
        { 36, "MUSIC_ID_DIFFICULTY_FILL" },
        { 37, "PLAY_MUSIC_ID_RATE" },
        { 38, "LOGIN" },
        { 39, "PLAY_MUSIC_TAG_DIFFICULTY_RATE" },
        { 40, "CONTINUOUS_LOGIN" },
        { 41, "TOTAL_LOGIN" },
        { 42, "COMBO" },
        { 43, "1MISS_ALL_MARVELOUS" },
        { 44, "GRADE_GET" },
        { 45, "HIGH_SPEED" },
        { 46, "RATING" },
        { 47, "USER_PLATE_COUNT" },
        { 48, "GACHA_COUNT" },
        { 49, "GET_ITEM" },
        { 50, "SCORE_MULTIPLE" },
        { 51, "SCORE_LAST_DIGITS" },
        { 52, "1PLAY_MUSIC_TAG_STATUS" },
        { 53, "TOTAL_GATE_POINT" },
        { 54, "TOTAL_USER_LEVEL" },
        { 55, "FRIEND_COUNT" },
        { 56, "BINGO_LINE_NUM" },
        { 57, "BINGO_SHEET_NUM" },
        { 58, "GALLERY_MODE_PLAY" },
        { 59, "MUSIC_ID_DIFFICULTY_RATE" },
        { 60, "MUSIC_ID_DIFFICULTY_STATUS" },
        { 61, "MUSIC_TAG_1PLAY" },
        { 62, "LEVEL_TOTAL_1PLAY" },
        { 63, "MUSIC_ID_RATE_1PLAY" },
        { 64, "MUSIC_ID_STATUS_1PLAY" },
    };
    }

}

