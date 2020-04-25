using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Net;
using Newtonsoft.Json;

namespace Omicron_Pi
{
    public partial class OmicronPiMainWindow : Form
    {
        private int newGroupNo = 1;
        public string configFilePath;
        public string configFile;
        public string[] configFileArray;
        public static List<TabPage> savedTabPages = new List<TabPage>();

        public List<Group> groupList = new List<Group>();
        public List<string> groupNameList = new List<string>();
        Dictionary<string, string> usersList = new Dictionary<string, string>();
        Dictionary<string, List<string>> permissionsDict = new Dictionary<string, List<string>>();
        Dictionary<string, bool> globalBadgeOptions = new Dictionary<string, bool>();
        private string steamApiKey;

        string finalConfig;
        Group selectedGroup;
        int selectedGroupIndex;

        public OmicronPiMainWindow()
        {
            InitializeComponent();            
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            //Hide tabs that rent General
            foreach (TabPage tab in tabControl.TabPages)
            {
                if (tab == tabControl.TabPages[0]) continue;
                savedTabPages.Add(tab);
                tabControl.TabPages.Remove(tab);
            }

            //Disable save and start again buttons untill a config file is chosen.
            saveCFGButton.Enabled = false;
            startAgainButton.Enabled = false;
        }

        /// <summary>
        /// Open options tabs and close the General Tab then populate opeions objects
        /// </summary>
        private void initialize(string filePath)
        {
            try
            {
                //If a SteamAPIKey is present save it
                if (File.Exists("steamapikey"))
                {
                    steamApiKey = File.ReadAllText("steamapikey");
                    enableSteamInfo.Hide();
                    showSteamInfoCheckBox.Show();
                }
                //Collapse General page and open other options pages.
                foreach (TabPage page in savedTabPages)
                {
                    tabControl.TabPages.Add(page);
                }
                savedTabPages.Clear();
                savedTabPages.Add(tabControl.TabPages[0]);
                tabControl.TabPages.Remove(tabControl.TabPages[0]);

                saveCFGButton.Enabled = true;
                startAgainButton.Enabled = true;

                //Define config file fields
                configFileArray = File.ReadAllLines(filePath);
                configFile = File.ReadAllText(filePath);
                configFilePath = filePath;


                // Tick Box Options
                globalBadgeOptions.Add("enable_staff_access", configFileArray.First(i => i.Contains("enable_staff_access")).Contains("true"));
                globalBadgeOptions.Add("enable_manager_access", configFileArray.First(i => i.Contains("enable_manager_access")).Contains("true"));
                globalBadgeOptions.Add("enable_banteam_access", configFileArray.First(i => i.Contains("enable_banteam_access")).Contains("true"));
                globalBadgeOptions.Add("enable_banteam_reserved_slots", configFileArray.First(i => i.Contains("enable_banteam_reserved_slots")).Contains("true"));
                globalBadgeOptions.Add("enable_banteam_bypass_geoblocking", configFileArray.First(i => i.Contains("enable_banteam_bypass_geoblocking")).Contains("true"));
                enable_staff_access.Checked = globalBadgeOptions["enable_staff_access"];
                enable_manager_access.Checked = globalBadgeOptions["enable_manager_access"];
                enable_banteam_access.Checked = globalBadgeOptions["enable_banteam_access"];
                enable_banteam_reserved_slots.Checked = globalBadgeOptions["enable_banteam_reserved_slots"];
                enable_banteam_bypass_geoblocking.Checked = globalBadgeOptions["enable_banteam_bypass_geoblocking"];

                //Populate Group List
                int rolesPos = Array.FindIndex(configFileArray, i => i.Contains("Roles")) + 1;
                for (; ; )
                {
                    if (configFileArray[rolesPos].Contains("#"))
                    {
                        rolesPos++;
                        continue;
                    }
                    if (!configFileArray[rolesPos].Contains("-")) break;
                    string groupName = configFileArray[rolesPos].Remove(0, 3).TrimEnd();
                    Groups.Items.Add(groupName);
                    assignmentGroupsCombo.Items.Add(groupName);
                    changeGroupCombo.Items.Add(groupName);
                    groupList.Add(new Group
                    {
                        name = groupName,
                        badge = "",
                        badgeColour = "default",
                        badgeCover = false,
                        badgeHidden = false,
                        kickPower = 0,
                        requiredKickPower = 0,
                        KickingAndShortTermBanning = false,
                        BanningUpToDay = false,
                        LongTermBanning = false,
                        ForceclassSelf = false,
                        ForceclassToSpectator = false,
                        ForceclassWithoutRestrictions = false,
                        GivingItems = false,
                        WarheadEvents = false,
                        RespawnEvents = false,
                        RoundEvents = false,
                        SetGroup = false,
                        GameplayData = false,
                        Overwatch = false,
                        FacilityManagement = false,
                        PlayersManagement = false,
                        PermissionsManagement = false,
                        ServerConsoleCommands = false,
                        ViewHiddenBadges = false,
                        ServerConfigs = false,
                        Broadcasting = false,
                        PlayerSensitiveDataAccess = false,
                        Noclip = false,
                        AFKImmunity = false
                    });
                    rolesPos++;
                }

                //Define Group Paramaters
                foreach (Group group in groupList)
                {
                    groupNameList.Add(group.name);
                    group.badge = configFileArray.First(i => i.Contains(group.name + "_badge")).Split(':')[1].Remove(0, 1);
                    group.badgeColour = configFileArray.First(i => i.Contains(group.name + "_color")).Split(':')[1].Remove(0, 1);
                    group.kickPower = int.TryParse(configFileArray.FirstOrDefault(i => i.Contains(group.name + "_kick_power")).IfDefaultGiveMe(group.name + "_kick_power: 0").Split(':')[1].Remove(0, 1), out int a) ? a : 0;
                    group.requiredKickPower = int.TryParse(configFileArray.FirstOrDefault(i => i.Contains(group.name + "_required_kick_power")).IfDefaultGiveMe(group.name + "_required_kick_power: 0").Split(':')[1].Remove(0, 1), out int b) ? b : 0;
                    group.badgeCover = bool.TryParse(configFileArray.First(i => i.Contains(group.name + "_cover")).Split(':')[1].Remove(0, 1), out bool x) ? x : true;
                    group.badgeHidden = bool.TryParse(configFileArray.First(i => i.Contains(group.name + "_hidden")).Split(':')[1].Remove(0, 1), out bool y) ? y : false;
                }

                //Define Permissions
                int permissionsPos = Array.FindIndex(configFileArray, i => i.Contains("Permissions:")) + 1;

                for (; ; )
                {
                    if (configFileArray[permissionsPos].Contains("#"))
                    {
                        permissionsPos++;
                        continue;
                    }
                    if (configFileArray.Count() == permissionsPos) break;
                    if (!configFileArray[permissionsPos].Contains("-")) break;
                    string line = configFileArray[permissionsPos].Replace(" ", "").Replace("-", "").Replace("[", "").Replace("]", "");
                    string permName = line.Split(':')[0];
                    List<string> permMembers = new List<string>();
                    foreach (string x in line.Split(':')[1].Split(','))
                    {
                        if (string.IsNullOrEmpty(x)) continue;
                        permMembers.Add(x);
                    }
                    permissionsDict.Add(permName, permMembers);
                    permissionsPos++;
                }

                //Get Users
                int membersPos = Array.FindIndex(configFileArray, i => i.Contains("Members:")) + 1;
                for (; ; )
                {
                    if (configFileArray[membersPos].Contains("#"))
                    {
                        membersPos++;
                        continue;
                    }
                    if (configFileArray.Count() == membersPos) break;
                    if (!configFileArray[membersPos].Contains("-")) break;
                    string line = configFileArray[membersPos].Replace(" ", "").Replace("-", "").Replace("[", "").Replace("]", "");
                    string userID = line.Split(':')[0];
                    string userGroup = line.Split(':')[1];
                    usersList.Add(userID, userGroup);
                    membersPos++;
                }
            }
            catch (Exception e) 
            {   
                File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ErrorLog.txt"), e.ToString());
                MessageBox.Show("An error occured when parsing your config file. Please ensure the file is a working config_remoteadmin. If you believe that the files does work please contact @Takail#6969 on Discord");
            }
        }


        private void globalBadgeOptionChanged(object sender, EventArgs e)
        {
            CheckBox s = sender as CheckBox;
            globalBadgeOptions[s.Name] = s.Checked;
        }

        #region Picture box methods
        private void PictureBox1_Click(object sender, EventArgs e) => System.Diagnostics.Process.Start("https://github.com/takail/omicron");
        private void PictureBox1_MouseEnter(object sender, EventArgs e) => Cursor = Cursors.Hand;
        private void PictureBox1_MouseLeave(object sender, EventArgs e) => Cursor = Cursors.Arrow;
        #endregion

        #region Group box methods
        private void Groups_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Groups.SelectedItem == null) return;
            selectedGroup = groupList.First(i => i.name == Groups.SelectedItem.ToString());
            selectedGroupIndex = groupList.IndexOf(selectedGroup);
            badgeNameTextBox.Text = selectedGroup.badge;
            badgeCoverCheckBox.Checked = selectedGroup.badgeCover;
            kickPowerNumUD.Value = selectedGroup.kickPower;
            reqKickPowerNumUD.Value = selectedGroup.requiredKickPower;
            badgeColourComboBox.SelectedItem = char.ToUpper(selectedGroup.badgeColour[0]) + selectedGroup.badgeColour.Substring(1).Replace(" ", "_");
            StringBuilder strb = new StringBuilder();
            foreach (KeyValuePair<string, List<string>> x in permissionsDict)
            {
                strb.AppendLine(x.Key + x.Value.FirstOrDefault());
            }

            foreach (Control ctrl in tabPage2.Controls.Find("permissionsBox", false)[0].Controls)
            {
                if (ctrl is CheckBox)
                {
                    CheckBox box = ctrl as CheckBox;
                    box.Checked = permissionsDict[box.Name].Contains(selectedGroup.name);
                }                    
            }
        }

        private void Groups_DoubleClick(object sender, EventArgs e)
        {
            ListBox listBox = sender as ListBox;
            int index = listBox.IndexFromPoint(((MouseEventArgs)e).Location);
            if (index != ListBox.NoMatches)
            {
                InputDlg frm = new InputDlg();
                Globals.inputResult = (string)listBox.SelectedItem;
                Globals.inputType = "role";
                frm.ShowDialog();
                if (frm.DialogResult == DialogResult.Cancel) return;
                if (groupList.Any(i => i.name == Globals.inputResult))
                {
                    MessageBox.Show("A rank with that name already exists. Please choose a different name");
                    Groups_DoubleClick(listBox, null);
                    return;
                }
                if (string.IsNullOrWhiteSpace(Globals.inputResult))
                {
                    MessageBox.Show("Please enter a rank name.");
                    Groups_DoubleClick(listBox, null);
                    return;
                }
                foreach (KeyValuePair<string, List<string>> perm in permissionsDict)
                {
                    if (perm.Value.Contains(listBox.SelectedItem))
                    {
                        perm.Value.Remove((string)listBox.SelectedItem);
                        perm.Value.Add(Globals.inputResult);
                    }
                }
                groupNameList.Remove(groupNameList.First(i => i == (string)listBox.SelectedItem));
                groupNameList.Add(Globals.inputResult);
                groupList[groupList.IndexOf(groupList.First(i => i.name == (string)listBox.SelectedItem))].name = Globals.inputResult;
                Groups.Items[listBox.SelectedIndex] = Globals.inputResult;
            }
        }

        private void GroupsRemoveButton_Click(object sender, EventArgs e)
        {
            DialogResult confirmResult = MessageBox.Show($"Are you want to delete the {Groups.SelectedItem} rank?\nThis cannot be undone.", "Confirm Deletion", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                if (Groups.Items.Count == 1)
                {
                    MessageBox.Show("You can't have less than 1 group");
                    return;
                }
                int newIndex = Groups.SelectedIndex <= 0 ? 1 : Groups.SelectedIndex - 1;
                string itemToRemove = (string)Groups.SelectedItem;
                groupNameList.Remove(itemToRemove);
                Groups.SelectedIndex = newIndex;
                groupList.Remove(groupList.First(i => i.name == itemToRemove));
                foreach (var perm in permissionsDict)
                {
                    if (perm.Value.Contains(itemToRemove))
                        perm.Value.Remove(itemToRemove);
                }
                foreach (KeyValuePair<string, string> x in usersList.ToList().FindAll(i => i.Value == itemToRemove))
                {
                    usersList.Remove(x.Key);
                }

                Groups.Items.Remove(itemToRemove);
            }
        }

        private void GroupsAddButton_Click(object sender, EventArgs e)
        {
            Groups.BeginUpdate();
            int newItem = Groups.Items.Add($"newgroup{newGroupNo}");
            Groups.EndUpdate();
            groupList.Add(new Group
            {
                name = $"newgroup{newGroupNo}",
                badge = $"newgroup{newGroupNo}",
                badgeColour = "default",
                badgeCover = false,
                badgeHidden = false,
                kickPower = 0,
                requiredKickPower = 0,
                KickingAndShortTermBanning = false,
                BanningUpToDay = false,
                LongTermBanning = false,
                ForceclassSelf = false,
                ForceclassToSpectator = false,
                ForceclassWithoutRestrictions = false,
                GivingItems = false,
                WarheadEvents = false,
                RespawnEvents = false,
                RoundEvents = false,
                SetGroup = false,
                GameplayData = false,
                Overwatch = false,
                FacilityManagement = false,
                PlayersManagement = false,
                PermissionsManagement = false,
                ServerConsoleCommands = false,
                ViewHiddenBadges = false,
                ServerConfigs = false,
                Broadcasting = false,
                PlayerSensitiveDataAccess = false,
                Noclip = false,
                AFKImmunity = false
            });
            groupNameList.Add($"newgroup{newGroupNo}");
            newGroupNo++;
            Groups.SelectedItem = Groups.Items[newItem];
        }
        #endregion

        #region Save and restart button methods

        private void startAgainButton_Click(object sender, EventArgs e)
        {
            DialogResult confirmResult = MessageBox.Show($"Are you want to start again?\nThis cannot be undone.", "Please Confirm.", MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.Yes)
            {
                groupList.Clear();
                usersList.Clear();
                groupNameList.Clear();
                permissionsDict.Clear();
                globalBadgeOptions.Clear();
                Groups.Items.Clear();
                restartProgram(tabControl);
            }
        }

        private static void restartProgram(TabControl tabControl)
        {            
            foreach (TabPage tab in tabControl.TabPages)
            {
                savedTabPages.Add(tab);
                tabControl.TabPages.Remove(tab);
            }
            tabControl.TabPages.Add(savedTabPages[0]);
            savedTabPages.Remove(savedTabPages[0]);
        }

        private void saveCFGButton_Click(object sender, EventArgs e)
        {
            StringBuilder strb = new StringBuilder();
            string groupName = groupList[selectedGroupIndex].name;
            strb.AppendLine("Members:");
            foreach (KeyValuePair<string, string> member in usersList)
            {
                strb.AppendLine($" - {member.Key}: {member.Value}");
            }
            strb.AppendLine();
            foreach (KeyValuePair<string, bool> option in globalBadgeOptions)
            {
                strb.AppendLine($"{option.Key}: {option.Value.ToString().ToLower()}");
            }
            strb.AppendLine();
            foreach(Group group in groupList)
            {
                strb.AppendLine($"{group.name}_badge: {group.badge}");
                strb.AppendLine($"{group.name}_color: {group.badgeColour}");
                strb.AppendLine($"{group.name}_cover: {group.badgeCover.ToString().ToLower()}");
                strb.AppendLine($"{group.name}_hidden: {group.badgeHidden.ToString().ToLower()}");
                strb.AppendLine($"{group.name}_kick_power: {group.kickPower}");
                strb.AppendLine($"{group.name}_required_kick_power: {group.requiredKickPower}");
                strb.AppendLine();
            }
            strb.AppendLine();
            strb.AppendLine("Roles:");
            foreach (Group group in groupList)
            {
                strb.AppendLine($" - {group.name}");
            }
            strb.AppendLine();
            strb.AppendLine("Permissions:");
            foreach (KeyValuePair<string, List<string>> perm in permissionsDict)
            {
                strb.AppendLine($" - {perm.Key}: [{string.Join(", ", perm.Value)}]");
            }
            strb.AppendLine();

            if(configFilePath == @".\TemplateConfig\DO NOT EDIT THIS FILE.txt")
            {
                finalConfig = strb.ToString();
                saveFileDialog1.ShowDialog();                
                return;
            }
            File.WriteAllText(configFilePath + ".backup", configFile);
            File.WriteAllText(configFilePath, strb.ToString());
            MessageBox.Show("Your new config has been saved to file location you chose the existing config and a backup of the original config has been created. Thank you for using Omicron Pi.");
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            File.WriteAllText(saveFileDialog1.FileName, finalConfig);
            MessageBox.Show("Your new config has been saved. Thank you for using Omicron Pi.");
        }
        #endregion

        #region Role settings methods
        private void badgeNameTextBox_TextChanged(object sender, EventArgs e)
        {
            groupList[selectedGroupIndex].badge = badgeNameTextBox.Text;
        }
        private void kickPowerNumUD_ValueChanged(object sender, EventArgs e)
        {
            groupList[selectedGroupIndex].kickPower = (int)kickPowerNumUD.Value;
        }
        private void reqKickPowerNumUD_ValueChanged(object sender, EventArgs e)
        {
            groupList[selectedGroupIndex].requiredKickPower = (int)reqKickPowerNumUD.Value;
        }

        private void badgeHiddenCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            groupList[selectedGroupIndex].badgeHidden = badgeHiddenCheckBox.Checked;
        }
        private void badgeCoverCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            groupList[selectedGroupIndex].badgeCover = badgeCoverCheckBox.Checked;
        }

        private void badgeColourComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupList[selectedGroupIndex].badgeColour = badgeColourComboBox.SelectedItem.ToString().ToLower().Replace(" ", "_");
        }
        #endregion

        #region Permissions methods
        private void permUpdate(object sender, EventArgs e)
        {
            CheckBox s = sender as CheckBox;
            if (s.Checked)
            {
                if(!permissionsDict[s.Name].Contains(selectedGroup.name))
                    permissionsDict[s.Name].Add(selectedGroup.name);
            }
            else
            {
                if (permissionsDict[s.Name].Contains(selectedGroup.name))
                    permissionsDict[s.Name].Remove(selectedGroup.name);
            }
        }        
        private void selectAllButton_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in tabPage2.Controls.Find("permissionsBox", false)[0].Controls)
            {
                if (ctrl is CheckBox)
                    ((CheckBox)ctrl).Checked = true;
            }
        }

        private void deselectAllButton_Click(object sender, EventArgs e)
        {
            foreach (Control ctrl in tabPage2.Controls.Find("permissionsBox", false)[0].Controls)
            {
                if (ctrl is CheckBox)
                    ((CheckBox)ctrl).Checked = false;
            }
        }
        #endregion

        #region Tab control methods
        private void updateComboBox(ComboBox cbox, List<string> data)
        {
            cbox.Items.Clear();
            cbox.Items.AddRange(data.Cast<object>().ToArray());
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            TabControl tc = sender as TabControl;
            if(tc.SelectedIndex == 1)
            {
                Groups.SelectedIndex = 0;
            }
            if (tc.SelectedIndex == 2)
            {
                updateComboBox(assignmentGroupsCombo, groupNameList);
                updateComboBox(changeGroupCombo, groupNameList);
                assignmentGroupsCombo.SelectedIndex = 0;
                changeGroupCombo.SelectedIndex = 0;
                if (Users.Items.Count > 0) Users.SelectedIndex = 0;
            }
        }
        #endregion

        #region Role assignment section methods
        private void assignmentGroupsCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            Users.Items.Clear();
            foreach (KeyValuePair<string, string> user in usersList)
            {                
                if(user.Value == (string)cb.SelectedItem)                
                    Users.Items.Add(user.Key);
            }
            if(Users.Items.Count > 0) Users.SelectedIndex = 0;
        }

        private void addUser_Click(object sender, EventArgs e)
        {
            for (; ; )
            {
                InputDlg frm = new InputDlg();
                Globals.inputType = "userid";
                frm.ShowDialog();
                if (frm.DialogResult == DialogResult.Cancel) break;
                if (usersList.Any(i => i.Key == Globals.inputResult))
                {
                    MessageBox.Show($"This user already has the role {usersList.First(i => i.Key == Globals.inputResult).Value}");
                    break;
                }
                if(!Globals.inputResult.Contains("@discord") && !Globals.inputResult.Contains("@steam") && !Globals.inputResult.Contains("@northwood"))
                {
                    MessageBox.Show($"You didnt include a discriminator. Please add either @steam @discord or @northwood at the end of the ID depending on the ID type. Eg. 76561197960287930@steam");
                    break;
                }
                if(Globals.inputResult.Contains("@steam") && getSteamInfo(Globals.inputResult.Split('@')[0]) == null)
                {
                    MessageBox.Show($"That SteamID doesnt exist. Please try again.");
                    continue;
                }
                usersList.Add(Globals.inputResult, (string)assignmentGroupsCombo.SelectedItem);
                Users.Items.Add(Globals.inputResult);
                Users.SelectedItem = Globals.inputResult;
                break;
            }            
        }

        private void removeUser_Click(object sender, EventArgs e)
        {
            if (Users.Items.Count == 0) return;
            int newIndex = Users.SelectedIndex <= 0 ? Users.SelectedIndex : Users.SelectedIndex - 1;
            usersList.Remove((string)Users.SelectedItem);
            Users.Items.Remove(Users.SelectedItem);
            if(Users.Items.Count > 0) Users.SelectedIndex = newIndex;
        }

        private void changeUserGroup_Click(object sender, EventArgs e)
        {
            if (Users.SelectedItem == null) return;
            usersList[(string)Users.SelectedItem] = (string)changeGroupCombo.SelectedItem;
            Users.Items.Remove(Users.SelectedItem);
        }
        private void Users_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(steamApiKey) && showSteamInfoCheckBox.Checked)
            {
                ListBox lb = sender as ListBox;
                if (((string)lb.SelectedItem).Length != 23)
                {
                    steamAvatar.Image = OmicronMain.Properties.Resources.omipilogo;
                    steamUrlLabel.Text = "";
                    steamNameLabel.Text = "";
                    return;
                }
                Player player = getSteamInfo(((string)lb.SelectedItem).Split('@')[0]);
                steamAvatar.ImageLocation = player.avatarfull;
                steamNameLabel.Text = player.personaname;
                steamUrlLabel.Text = player.profileurl;
            }
        }
        #endregion

        #region First page methods
        private void openExistingConfigButton_Click(object sender, EventArgs e) => openFileDialog1.ShowDialog();
        private void createNewConfigButton_Click(object sender, EventArgs e) => initialize(@".\TemplateConfig\DO NOT EDIT THIS FILE.txt");
        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            if (openFileDialog1.FileName.Contains("config_remoteadmin.txt"))
            {
                initialize(openFileDialog1.FileName);
                return;
            }
            MessageBox.Show("Please selecet an existing config_remoteadmin.txt");
        }
        #endregion

        #region Steam info methods
        private Player getSteamInfo(string steamid, string key = "")
        {
            if (string.IsNullOrEmpty(key)) key = steamApiKey;
            string ApiEndPoint = $"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={key}&steamids={steamid}";
            WebRequest request = WebRequest.Create(ApiEndPoint);
            request.Method = "GET";
            WebResponse response = null;
            try
            {
                response = request.GetResponse();
            }
            catch(WebException e)
            {
                if(((HttpWebResponse)e.Response).StatusCode.ToString() == "Forbidden")
                    return null;
            }
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string content = reader.ReadToEnd();
            reader.Close();
            response.Close();

            var x = JsonConvert.DeserializeObject<RootObject>(content);            
            return x.response.players.Count > 0 ? x.response.players.First() : null;
        }
        private void enableSteamInfo_Click(object sender, EventArgs e)
        {
            InputDlg frm = new InputDlg();
            Globals.inputType = "steamAPIKey";
            frm.ShowDialog();
            if (frm.DialogResult == DialogResult.Cancel) return;
            if (getSteamInfo("76561197960287930", Globals.inputResult) == null)
            {
                MessageBox.Show($"The SteamAPI key you entered is invalid. Please try again.");
                enableSteamInfo_Click(frm, null);
                return;
            }
            File.WriteAllText(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "steamapikey")), Globals.inputResult);
            steamApiKey = Globals.inputResult;
            enableSteamInfo.Hide();
            showSteamInfoCheckBox.Show();
            showSteamInfoCheckBox.Checked = true;
        }

        private void showSteamInfoCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if(!((CheckBox)sender).Checked)
            {
                steamAvatar.Image = OmicronMain.Properties.Resources.omipilogo;
                steamUrlLabel.Text = "";
                steamNameLabel.Text = "";
                return;
            }
            if (Users.Items.Count > 0 && ((string)Users.SelectedItem).Length == 23)
            {
                Player player = getSteamInfo(((string)Users.SelectedItem).Split('@')[0]);
                steamAvatar.ImageLocation = player.avatarfull;
                steamNameLabel.Text = player.personaname;
                steamUrlLabel.Text = player.profileurl;
            }
        }

        private void steamUrlLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(Uri.IsWellFormedUriString(((LinkLabel)sender).Text, UriKind.Absolute))
                System.Diagnostics.Process.Start(((LinkLabel)sender).Text);
        }
        #endregion

        private void OmicronPiMainWindow_FormClosed(object sender, FormClosedEventArgs e) => Application.Exit();
    }
}