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
            Application.Run(new Form1());

        }
        static void Read(string file)
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
                            string id = GetFieldValue(rowStruct, "UniqueID");
                            string jacket = GetFieldValue(rowStruct, "JacketAssetName");
                            string music = GetFieldValue(rowStruct, "MusicMessage");

                            Console.WriteLine($"ID: {id}");
                            Console.WriteLine($"  JacketAssetName: {jacket}");
                            Console.WriteLine($"  MusicMessage: {music}");
                            Console.WriteLine();
                        }
                    }
                }
            }
            Console.ReadLine();
        }

        static string GetFieldValue(StructPropertyData structData, string fieldName)
        {
            var prop = structData.Value.FirstOrDefault(p => p.Name.Value.ToString() == fieldName);

            if (prop is StrPropertyData strProp)
            {
                return strProp.Value?.ToString() ?? "(null)";
            }
            else if (prop is IntPropertyData intProp)
            {
                return intProp.Value.ToString();
            }
            else if (prop is BoolPropertyData boolProp)
            {
                return boolProp.Value.ToString();
            }
            else
            {
                return "(unsupported or missing)";
            }
        }
    }

}

