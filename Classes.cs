using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omicron_Pi
{
    static class Extensions
    {
        public static T IfDefaultGiveMe<T>(this T value, T alternate)
        {
            if (value == null) return alternate;
            if (value.Equals(default(T))) return alternate;
            return value;
        }
    }

    public class Group
    {
		public string name;
        public string badge;
        public string badgeColour;
        public   bool badgeCover;
        public   bool badgeHidden;
        public   bool KickingAndShortTermBanning;
        public   bool BanningUpToDay;
        public   bool LongTermBanning;
        public   bool ForceclassSelf;
        public   bool ForceclassToSpectator;
        public   bool ForceclassWithoutRestrictions;
        public   bool GivingItems;
        public   bool WarheadEvents;
        public   bool RespawnEvents;
        public   bool RoundEvents;
        public   bool SetGroup;
        public   bool GameplayData;
        public   bool Overwatch;
        public   bool FacilityManagement;
        public   bool PlayersManagement;
        public   bool PermissionsManagement;
        public   bool ServerConsoleCommands;
        public   bool ViewHiddenBadges;
        public   bool ServerConfigs;
        public   bool Broadcasting;
        public   bool PlayerSensitiveDataAccess;
        public   bool Noclip;
        public   bool AFKImmunity;
        public    int kickPower;
        public    int requiredKickPower;
    }

    public class Globals
    {
        public static string[] configFileArray;
        public static string configFile;
        public static string configFilePath;
        public static string inputType;
        public static string inputResult;
    }

    public class Player
    {
        public string steamid { get; set; }
        public int communityvisibilitystate { get; set; }
        public int profilestate { get; set; }
        public string personaname { get; set; }
        public int commentpermission { get; set; }
        public string profileurl { get; set; }
        public string avatar { get; set; }
        public string avatarmedium { get; set; }
        public string avatarfull { get; set; }
        public int lastlogoff { get; set; }
        public int personastate { get; set; }
        public string primaryclanid { get; set; }
        public int timecreated { get; set; }
        public int personastateflags { get; set; }
        public string loccountrycode { get; set; }
    }

    public class Response
    {
        public List<Player> players { get; set; }
    }

    public class RootObject
    {
        public Response response { get; set; }
    }
}
