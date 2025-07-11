﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UAssetAPI;
using UAssetAPI.ExportTypes;
using UAssetAPI.PropertyTypes.Objects;
using UAssetAPI.PropertyTypes.Structs;
using UAssetAPI.UnrealTypes;

namespace WaccaSongBrowser
{
    public partial class Message : UserControl
    {
        static string? messageFolder = null;
        public Message(string folderName)
        {
            InitializeComponent();
            messageFolder = folderName;
        }
        public void ProcessFile(string assetPath, bool inject = false, bool waccaTxt = false)
        {
            // Load the asset
            bool replace_strings = false;
            UAsset asset = new UAsset(assetPath, EngineVersion.VER_UE4_19);
            string po = assetPath.Substring(0, assetPath.Length - 6) + "po";
            Dictionary<string, string> translations = new Dictionary<string, string>();
            if (File.Exists(messageFolder + "/Wacca.txt") && waccaTxt && inject)
            {
                replace_strings = true;
                string allText = File.ReadAllText(messageFolder + "/Wacca.txt", System.Text.Encoding.UTF8);
                string vanillaText = File.ReadAllText(messageFolder + "/WaccaVanilla.txt", System.Text.Encoding.UTF8);
                string[] vanilla = vanillaText.Split('$');
                string[] t = allText.Split('$');
                if (vanilla.Length != t.Length)
                {
                    outputMessage.Text = "Wacca.txt and WaccaVanilla.txt do not contain the same number of $ !!!! This characher must only be used as a separator!!!";
                    return;
                }
                for (int i = 0; i < vanilla.Length; i++)
                {
                    translations[vanilla[i].Trim()] = t[i].Trim();
                }
            }

            if (File.Exists(po) && inject)
            {
                replace_strings = true;
                Console.WriteLine($"{po} found alongside uasset. Using it to replace strings");
                // Load the PO file and populate the translations dictionary
                string[] poLines = File.ReadAllLines(po, System.Text.Encoding.UTF8);
                string? currentMsgId = null;

                foreach (string line in poLines)
                {
                    if (line.StartsWith("msgid \""))
                    {
                        currentMsgId = line.Substring(7, line.Length - 8);
                    }
                    else if (line.StartsWith("msgstr \"") && currentMsgId != null)
                    {
                        string translatedValue = line.Substring(8, line.Length - 9);
                        translations[currentMsgId] = translatedValue;
                        currentMsgId = null;
                    }
                }
            }

            string Language = "fr_BE";
            string text = "msgid \"\"\r\nmsgstr \"\"\r\n\"Language: " + Language + "\\n\"\r\n\"Content-Type: text/plain; charset=UTF-8\\n\"\r\n";
            if (waccaTxt && !replace_strings)
            {
                text = "";
            }
            // Iterate through exports
            foreach (var export in asset.Exports)
            {
                if (export is DataTableExport dataTableExport)
                {
                    Console.WriteLine($"DataTable Name: {export.ObjectName.Value.Value}");

                    // Process each row
                    foreach (var row in dataTableExport.Table.Data)
                    {
                        // Update JapaneseMessage
                        foreach (var property in row.Value)
                        {
                            if (property is StrPropertyData strProp &&
                                strProp.Name != null &&
                                strProp.Name.Value != null &&
                                strProp.Name.Value.ToString() == "JapaneseMessage")
                            {
                                string originalValue = strProp.Value?.ToString();
                                if (!string.IsNullOrEmpty(originalValue))
                                {
                                    if (waccaTxt && !replace_strings)
                                    {
                                        text += originalValue + "$\n";
                                    }
                                    else if (replace_strings)
                                    {
                                        // Check if a translation exists for the original value
                                        if (translations.TryGetValue(originalValue, out string? translatedValue))
                                        {
                                            Console.WriteLine($"{originalValue} => {translatedValue}");
                                            // Update the property value
                                            strProp.Value = (FString)translatedValue;
                                        }
                                        else
                                        {
                                            Console.WriteLine($"No translation found for: {originalValue}");
                                        }
                                    }
                                    else
                                    {
                                        text += "msgid \"" + originalValue.Replace("\"", "\\\"") + "\"\r\nmsgstr \"\"\r\n";
                                    }
                                }
                            }
                            else
                            {
                                Console.WriteLine("Property is not a valid StrPropertyData or does not have the expected Name.");
                            }
                        }
                    }
                }
            }

            if (waccaTxt && !replace_strings)
            {
                // Write txt file with text as UTF-8
                File.AppendAllText(messageFolder + "/Wacca.txt", text, System.Text.Encoding.UTF8);
                File.AppendAllText(messageFolder + "/WaccaVanilla.txt", text, System.Text.Encoding.UTF8);
            }
            else if (replace_strings)
            {
                // Define the subfolder path
                string subfolderPath = Path.Combine(Path.GetDirectoryName(assetPath), "Translated");

                // Ensure the subfolder exists
                Directory.CreateDirectory(subfolderPath);

                // Create the full output path inside the subfolder
                string outputPath = Path.Combine(subfolderPath, Path.GetFileName(assetPath));

                // Save the updated asset
                asset.Write(outputPath);
                Console.WriteLine($"Translated asset saved to: {outputPath}");
                outputMessage.Text = $"Translated asset saved to: {outputPath}";
            }
            else
            {
                // Write Po file with text as UTF-8
                string poOutputPath = assetPath.Substring(0, assetPath.Length - 6) + "po";
                File.WriteAllText(poOutputPath, text, System.Text.Encoding.UTF8);
                Console.WriteLine($"Po file saved to: {poOutputPath}");

            }
        }
        static void PrintEverything(UAsset asset)
        {
            // Iterate through exports
            foreach (var export in asset.Exports)
            {
                if (export is DataTableExport dataTableExport)
                {
                    Console.WriteLine($"DataTable Name: {export.ObjectName.Value.Value}");
                    foreach (var row in dataTableExport.Table.Data)
                    {
                        Console.WriteLine($"Row Name: {row.Value}");
                        foreach (var property in row.Value)
                        {
                            ExtractTextFromProperty(property);
                        }
                    }
                }
                else if (export is NormalExport normalExport)
                {
                    Console.WriteLine($"Normal Export: {export.ObjectName.Value.Value}");
                    foreach (var property in normalExport.Data)
                    {
                        ExtractTextFromProperty(property);
                    }
                }
            }
        }

        static void ExtractTextFromProperty(PropertyData property)
        {
            switch (property)
            {
                case StrPropertyData strProp:
                    Console.WriteLine($"    String Property: {strProp.Name.Value} -> {strProp.Value}");
                    break;
                case TextPropertyData textProp:
                    Console.WriteLine($"    Text Property: {textProp.Name.Value} -> {textProp.Value}");
                    break;
                case NamePropertyData nameProp:
                    Console.WriteLine($"    Name Property: {nameProp.Name.Value} -> {nameProp.Value}");
                    break;
                case StructPropertyData structProp:
                    Console.WriteLine($"    Struct Property: {structProp.Name.Value}");
                    foreach (var innerProp in structProp.Value)
                    {
                        ExtractTextFromProperty(innerProp);
                    }
                    break;
                case ArrayPropertyData arrayProp:
                    Console.WriteLine($"    Array Property: {arrayProp.Name.Value}");
                    foreach (var element in arrayProp.Value)
                    {
                        ExtractTextFromProperty(element);
                    }
                    break;
                default:
                    Console.WriteLine($"    Unhandled Property: {property.GetType().Name}");
                    break;
            }
        }

        private void createPo_Click(object sender, EventArgs e)
        {
            outputMessage.Text = "Processing...";
            string[] files = Directory.GetFiles(messageFolder, "*.uasset"); // scan inside for the .uasset file:
            foreach (string file in files)
            {
                ProcessFile(file);
            }
            outputMessage.Text = $".po files saved";
        }

        private void injectPo_Click(object sender, EventArgs e)
        {
            outputMessage.Text = "Processing...";
            string[] files = Directory.GetFiles(messageFolder, "*.uasset"); // scan inside for the .uasset file:
            foreach (string file in files)
            {
                ProcessFile(file, true);
            }
        }

        private void createWacca_Click(object sender, EventArgs e)
        {
            outputMessage.Text = "Processing...";
            if (File.Exists(messageFolder + "/Wacca.txt"))
            {
                outputMessage.Text = "Wacca.txt already exists in the Message folder";
            }
            else if (File.Exists(messageFolder + "/WaccaVanilla.txt"))
            {
                outputMessage.Text = "WaccaVanilla.txt already exists in the Message folder";
            }
            else
            {
                string[] files = Directory.GetFiles(messageFolder, "*.uasset"); // scan inside for the .uasset file:
                foreach (string file in files)
                {
                    ProcessFile(file, false, true);
                }
                outputMessage.Text = "Wacca.txt and WaccaVanilla.txt created in the Message folder. Do not edit the vanilla txt!!!!";
            }
        }

        private void injectWacca_Click(object sender, EventArgs e)
        {
            outputMessage.Text = "Processing...";
            if (!File.Exists(messageFolder + "/Wacca.txt"))
            {
                outputMessage.Text = "Wacca.txt does not exists in the Message folder";
            }
            else if (!File.Exists(messageFolder + "/WaccaVanilla.txt"))
            {
                outputMessage.Text = "WaccaVanilla.txt does not exists in the Message folder";
            }
            else
            {
                string[] files = Directory.GetFiles(messageFolder, "*.uasset"); // scan inside for the .uasset file:
                foreach (string file in files)
                {
                    ProcessFile(file, true, true);
                }
                outputMessage.Text = "Wacca.txt and WaccaVanilla.txt created in the Message folder. Do not edit the vanilla txt!!!!";
            }
        }
    }
}

