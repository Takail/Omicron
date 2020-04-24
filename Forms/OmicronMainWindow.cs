using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace OmicronMain
{
    public partial class OmicronMainWindow : Form
    {
        public OmicronMainWindow()
        {
            InitializeComponent();
        }
        string modeFor914Var = "DroppedAndPlayerTeleport";
        private void omicronPicBox_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://github.com/k0vac/omicron");
        private void donateLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) => System.Diagnostics.Process.Start("https://paypal.me/Veljko61");
        private void maxPlayersNUpDown_ValueChanged_1(object sender, EventArgs e) => instabilityLabel.Visible = maxPlayersNUpDown.Value > 25;
        private void scpNadeDmgTrackBar_Scroll(object sender, EventArgs e) => scpNadeDmgLabel.Text = (scpNadeDmgTrackBar.Value / 10.00) + "x";
        private void humanNadeDmbTrackBar_Scroll(object sender, EventArgs e) => humanNadeDmbLabel.Text = (humanNadeDmgTrackBar.Value / 10.00) + "x";
        private void pdTrackBar_Scroll(object sender, EventArgs e) => pdExitLabel.Text = (pdTrackBar.Value + "");
        private void MainForm_Load(object sender, EventArgs e) => nineFourteenModeCheckBox.SelectedItem = "Dropped + Player Teleport";
        private void kickRateLimitCheckBox_CheckedChanged(object sender, EventArgs e) => ddosWarnLabel.Visible = !kickRateLimitedCheckBox.Checked;
        private void onlineModeCheckBox_CheckedChanged(object sender, EventArgs e) => offlineModeLabel.Visible = !onlineModeCheckBox.Checked;
        private void contactEmailTextBox_Leave(object sender, EventArgs e) => ((TextBox)sender).SelectionLength = 0;
        private void generateConfigButton_Click(object sender, EventArgs e) => saveFileDialog1.ShowDialog();
        private void pasteIdTextBox_TextChanged(object sender, EventArgs e)
        {
            if (!pasteIdTextBox.Text.Contains("pastebin.com")) return;
            string[] pBinID = pasteIdTextBox.Text.Split('/');
            pasteIdTextBox.Text = pBinID[pBinID.Length - 1];
            pasteSyntaxLabel.Visible = true;
        }
        private void afkKickCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!afkKickCheckBox.Checked)
            {
                afktimeNUpDown.Enabled = false;
                afktimeNUpDown.Value = 0;
                return;
            }
            afktimeNUpDown.Enabled = true;
            afktimeNUpDown.Value = 90;           
        }
        private void enableQueryCheckBox13_CheckedChanged(object sender, EventArgs e)
        {
            if (!enableQueryCheckBox.Checked)
            {
                queryPortShiftNUpDown.Enabled = false;
                queryPwTextBox.Enabled = false;
                ipv6QueryCheckBox.Enabled = false;
                return;
            }
            queryPortShiftNUpDown.Enabled = true;
            queryPwTextBox.Enabled = true;
            ipv6QueryCheckBox.Enabled = true;
        }
        private void spawnProtCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!spawnProtCheckBox.Checked)
            {
                spawnProtTimeNUpDown.Enabled = false;
                spawnProtTimeNUpDown.Value = 0;
                return;
            }
            spawnProtTimeNUpDown.Enabled = true;
            spawnProtTimeNUpDown.Value = 30;
        }
        private void nineFourteenModeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (nineFourteenModeCheckBox.SelectedItem)
            {
                case "Dropped(No Player Teleport)":
                    modeFor914Var = "Dropped";
                    break;
                case "Dropped + Held":
                    modeFor914Var = "DroppedAndHeld";
                    break;
                case "Dropped + Inventory":
                    modeFor914Var = "DroppedAndInventory";
                    break;
                case "Dropped + Player Teleport":
                    modeFor914Var = "DroppedAndPlayerTeleport";
                    break;
                case "Held":
                    modeFor914Var = "Held";
                    break;
                case "Inventory":
                    modeFor914Var = "Inventory";
                    break;
            }
        }
        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            StreamWriter sw = new StreamWriter(((SaveFileDialog)sender).FileName);
            sw.WriteLine($"server_name: {serverNameTextBox.Text}");
            sw.WriteLine($"player_list_title: {serverTitleTextBox.Text}");
            sw.WriteLine($"player_list_rate: default");
            sw.WriteLine($"serverinfo_pastebin_id: {pasteIdTextBox.Text}");
            sw.WriteLine($"server_ip: {ipTextBox.Text}");
            sw.WriteLine($"max_players: {maxPlayersNUpDown.Value}");
            sw.WriteLine($"use_reserved_slots: {reservedSlotsCheckBox.Checked}");
            sw.WriteLine($"lobby_waiting_time: default");
            sw.WriteLine($"ipv4_bind_ip: {bindIpTextbox.Text}");
            sw.WriteLine($"ipv6_bind_ip: ::"); //Planned for Implementation
            sw.WriteLine($"contact_email: {contactEmailTextBox.Text}");
            progressBar1.Value = 20;
            sw.WriteLine($"minimum_MTF_time_to_spawn: {minSpawnTimeNUpDown.Value}");
            sw.WriteLine($"maximum_MTF_time_to_spawn: {maxSpawnTimeNUpDown.Value}");
            sw.WriteLine($"maximum_MTF_respawn_amount: {mtfSpawnSizeNUpDown.Value}");
            sw.WriteLine($"maximum_CI_respawn_amount: {ciSpawnSizeNUpDown.Value}");
            sw.WriteLine($"priority_mtf_respawn: true");
            sw.WriteLine($"use_crypto_rng: false");
            sw.WriteLine($"ci_respawn_percent: {ciSpawnChanceNUpDown.Value}");
            sw.WriteLine($"ci_on_start_percent: {ciSpawnChanceAtStartNUpDown.Value}");
            sw.WriteLine($"team_respawn_queue: 40143140314414041340");
            progressBar1.Value = 50;
            sw.WriteLine($"server_forced_class: -1");
            sw.WriteLine($"map_seed: -1");
            sw.WriteLine($"afk_time: {afktimeNUpDown.Value}");
            sw.WriteLine($"intercom_cooldown: {icomCdNUpDown.Value}");
            sw.WriteLine($"intercom_max_speech_time: {icomSpeechTimeNUpDown.Value}");
            sw.WriteLine($"auto_round_restart_time: 10");
            sw.WriteLine($"friendly_fire: {friendlyFireCheckBox.Checked}");
            sw.WriteLine($"warhead_tminus_start_duration: {icomSpeechTimeNUpDown.Value}");
            sw.WriteLine($"human_grenade_multiplier: {humanNadeDmgTrackBar.Value / 10.00}");
            sw.WriteLine($"scp_grenade_multiplier: {scpNadeDmgTrackBar.Value / 10.00}");
            sw.WriteLine($"lock_gates_on_countdown: true");
            sw.WriteLine($"isolate_zones_on_countdown: false");
            sw.WriteLine($"open_doors_on_countdown: true");
            sw.WriteLine($"keep_items_after_escaping: {keepItemsOnEscCheckBox.Checked}");
            sw.WriteLine($"disable_decontamination: {disableDecontCheckBox.Checked}");
            sw.WriteLine($"no_holidays: false");
            sw.WriteLine($"allow_disarmed_interaction: false");
            sw.WriteLine($"914_mode: {modeFor914Var}");
            sw.WriteLine($"pd_exit_count: {pdTrackBar.Value}");
            sw.WriteLine($"pd_random_exit_rids:");
            sw.WriteLine($"pd_random_exit_rids_after_decontamination:");
            sw.WriteLine($"pd_refresh_exit: false");
            progressBar1.Value = 60;
            sw.WriteLine($"spawn_protect_disable: {!spawnProtCheckBox.Checked}");
            sw.WriteLine($"spawn_protect_time: {spawnProtTimeNUpDown.Value}");
            sw.WriteLine($"spawn_protect_team: [1, 2]");
            sw.WriteLine($"spawn_protect_allow_dmg: {allProtDmgCheckBox.Checked}");
            sw.WriteLine($"grenade_chain_limit: 10");
            sw.WriteLine($"grenade_chain_length_limit: 4");
            sw.WriteLine($"online_mode: {onlineModeCheckBox.Checked}");
            sw.WriteLine($"ip_banning: true");
            sw.WriteLine($"enable_whitelist: {whitelistCheckBox.Checked }");
            sw.WriteLine($"hide_from_public_list: {hideFromListCheckBox.Checked}");
            sw.WriteLine($"forward_ports: {forwardPortsCheckBox.Checked}");
            sw.WriteLine($"enable_query: {enableQueryCheckBox.Checked}");
            sw.WriteLine($"query_port_shift: {queryPortShiftNUpDown.Value}");
            sw.WriteLine($"query_use_IPv6: {ipv6QueryCheckBox.Checked}");
            sw.WriteLine(queryPwTextBox.Text != "" ? $"administator_query_password : {queryPwTextBox.Text}" : "administator_query_password : none");
            sw.WriteLine($"enable_sync_command_binding: false");
            sw.WriteLine($"anti_player_wallhack: default #Deprecated"); //DEPRECATED
            sw.WriteLine($"ratelimit_kick: {kickRateLimitedCheckBox.Checked}");
            sw.WriteLine($"enforce_same_ip: {blockProxyCheckBox.Checked}");
            sw.WriteLine($"no_enforcement_for_local_ip_addresses: true");
            sw.WriteLine($"enforce_same_asn: {blockProxyCheckBox.Checked}");
            sw.WriteLine($"enable_ip_ratelimit: {rateLimitCheckBox.Checked}");
            sw.WriteLine($"enable_userid_ratelimit: {rateLimitCheckBox.Checked}");
            sw.WriteLine($"ip_ratelimit_window: {rateLimitNumericUpDown.Value}");
            sw.WriteLine($"userid_ratelimit_window: {rateLimitNumericUpDown.Value}");
            sw.WriteLine($"class_d_personnel_defaultammo: [0, 0, 0]");
            sw.WriteLine($"nine_tailed_fox_scientist_defaultammo: [120, 20, 20]");
            sw.WriteLine($"scientist_defaultammo: [0, 0, 0]");
            sw.WriteLine($"chaos_insurgency_defaultammo: [0, 100, 0]");
            sw.WriteLine($"nine_tailed_fox_lieutenant_defaultammo: [120, 0, 100]");
            sw.WriteLine($"nine_tailed_fox_commander_defaultammo: [120, 0, 100]");
            sw.WriteLine($"nine_tailed_fox_cadet_defaultammo: [40, 0, 100]");
            sw.WriteLine($"tutorial_defaultammo: [0, 0, 0]");
            sw.WriteLine($"facility_guard_defaultammo: [0, 35, 0]");
            string[] portsList = portQueueTextBox.Text.Split(',');
            progressBar1.Value = 75;
            sw.WriteLine("port_queue:");
            for (int i = 0; i < portsList.Length; i++)
            {
                sw.WriteLine(" - " + portsList[i]);
            }
            sw.WriteLine($"custom_whitelist: false"); //Planned "Others" Tab
            sw.WriteLine($"server_access_restrictions: false"); //Planned "Others" Tab
            sw.WriteLine($"allow_playing_as_tutorial: true"); //Planned for implementation
            sw.WriteLine($"096_destroy_locked_doors: true"); //Planned for implementation
            sw.WriteLine($"sinkhole_slow_amount: 30"); //Planned for implementation
            sw.WriteLine($"sinkhole_spawn_chance: 0"); //Planned for implementation
            sw.WriteLine($"mtf_can_cuff_researchers: true"); //Planned for implementation
            sw.WriteLine($"ci_can_cuff_class_d: true"); //Planned for implementation
            sw.WriteLine($"cuffed_escapee_change_team: true"); //Planned for implementation
            sw.WriteLine($"gban_ban_ip: default");
            sw.WriteLine($"noclip_protection_output: false");
            sw.WriteLine($"speedhack_protection_output: false");
            sw.WriteLine($"broadcast_kicks: false"); //Planned "Others" Tab
            sw.WriteLine($"broadcast_kick_text: %nick% has been kicked."); //Planned "Others" Tab
            sw.WriteLine($"broadcast_kick_duration: 5"); //Planned "Others" Tab
            sw.WriteLine($"broadcast_bans: true"); //Planned "Others" Tab
            sw.WriteLine($"broadcast_ban_text: %nick% has been banned."); //Planned "Others" Tab
            sw.WriteLine($"broadcast_ban_duration: 5"); //Planned "Others" Tab
            sw.WriteLine($"geoblocking_whitelist:"); //Planned "Others" Tab
            sw.WriteLine($" - AA");
            sw.WriteLine($" - AB");
            sw.WriteLine($" - AC");
            sw.WriteLine($"geoblocking_blacklist:"); //Planned "Others" Tab
            sw.WriteLine($" - AA");
            sw.WriteLine($" - AB");
            sw.WriteLine($" - AC");
            sw.Flush();
            sw.Close();
            progressBar1.Value = 100;
            generateLabel.Visible = true;
            generateLabel.Text = "Files have been generated.";
        }

        private void OmicronMainWindow_FormClosed(object sender, FormClosedEventArgs e) => Application.Exit();
    }
}