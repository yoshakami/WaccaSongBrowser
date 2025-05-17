using System.Collections.Generic;
using System;
using System.Linq;
using UAssetAPI;
using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.PropertyTypes.Structs;

namespace WaccaSongList
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
    public class SongData
    {
        public uint UniqueID { get; set; }
        public string MusicMessage { get; set; }
        public string ArtistMessage { get; set; }
        //public string CopyrightMessage { get; set; }
        public string Version { get; set; }
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
        public bool ValidCulture_zh_Hans_CN_Guest { get; set; }
        public bool ValidCulture_zh_Hans_CN_GeneralMember { get; set; }
        public bool ValidCulture_zh_Hans_CN_VipMember { get; set; }
        public bool ValidCulture_Offline { get; set; }
        public bool ValidCulture_NoneActive { get; set; }

        public bool Recommend { get; set; }  // beginner
        public int WaccaPointCost { get; set; }
        //public byte Collaboration { get; set; }
        //public byte WaccaOriginal { get; set; }
        //public byte TrainingLevel { get; set; }
        //public byte Reserved { get; set; }
        public float Bpm { get; set; }
        //public string HashTag { get; set; }

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
        //public ulong WorkBuffer { get; set; }
        public string AssetFullPath { get; set; }
    }

}

