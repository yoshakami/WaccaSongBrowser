namespace WaccaSongBrowser
{
    public partial class Menu : UserControl
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void injectUserRate_Click(object sender, EventArgs e)
        {
            Message.injectUserRateButton_Click(null, null);
        }
    }
}
