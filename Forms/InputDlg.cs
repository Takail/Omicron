using System;
using System.Windows.Forms;

namespace Omicron_Pi
{
    public partial class InputDlg : Form
    {
        public InputDlg()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if(Globals.inputResult.Length == 32)
                Globals.inputResult = inputBox.Text;
            else
                Globals.inputResult = inputBox.Text.ToLower();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void inputDlg_Load(object sender, EventArgs e)
        {
            string textToFill = "";
            switch (Globals.inputType)
            {
                case "role":
                    textToFill = "Please enter a new role name";
                    break;
                case "userid":
                    textToFill = "Please enter a new userid";
                    Globals.inputResult = "";
                    break;
                case "steamAPIKey":
                    textToFill = "Please your SteamAPI key.";
                    helpButton.Show();
                    Globals.inputResult = "";
                    break;
            }
            Text = textToFill;
            label1.Text = textToFill;
            inputBox.Text = Globals.inputResult;
        }

        private void inputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                e.Handled = true;
                Globals.inputResult = inputBox.Text.ToLower();
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Would you like to open the webpage where you can get your SteamAPIKey?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start("https://steamcommunity.com/dev/apikey");
            }
        }
    }
}
