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

        private void label1_Click(object sender, EventArgs e)
        {

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


        private void WaccaSongBrowser_DragLeave(object sender, EventArgs e)
        {

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
