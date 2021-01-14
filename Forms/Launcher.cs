using Omicron_Pi;
using System;
using System.Windows.Forms;

namespace OmicronMain
{
    public partial class Launcher : Form
    {
        public static string gameplayConfigPath = null;
        public static string remoteAdmConfigPath = null;
        public Launcher()
        {
            InitializeComponent();
        }

        private void selectOmicron_Click(object sender, EventArgs e)
        {
            OmicronMainWindow omiMain = new OmicronMainWindow();
            omiMain.Show();
            Hide();
        }

        private void selectOmicronPi_Click(object sender, EventArgs e)
        {
            OmicronPiMainWindow omiPiMain = new OmicronPiMainWindow();
            omiPiMain.Show();
            Hide();
        }

        private void omicronGithub_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://github.com/Takail/Omicron/");

        private void Launcher_FormClosing(object sender, FormClosingEventArgs e) => Application.Exit();
    }
}
