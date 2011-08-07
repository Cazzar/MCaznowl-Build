using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;

namespace MCForge.Commands
{
    public class CmdEconomy : Command
    {
        public override string name { get { return "economy"; } }
        public override string shortcut { get { return "eco"; } }
        public override string type { get { return "other"; } }
        public override bool museumUsable { get { return false; } }
        public override LevelPermission defaultRank { get { return LevelPermission.Banned; } }
        public override void Use(Player p, string message)
        {
            string[] command = message.ToLower().Trim().Split(' ');
            string par0 = String.Empty;
            string par1 = String.Empty;
            string par2 = String.Empty;
            string par3 = String.Empty;
            string par4 = String.Empty;
            string par5 = String.Empty;
            string par6 = String.Empty;
            string par7 = String.Empty;
            string par8 = String.Empty;
            try
            {
                par0 = command[0];
                par1 = command[1];
                par2 = command[2];
                par3 = command[3];
                par4 = command[4];
                par5 = command[5];
                par6 = command[6];
                par7 = command[7];
                par8 = command[8];
            }
            catch { }

            switch (par0)
            {
                case "setup":
                    if (p.group.Permission >= LevelPermission.Operator)
                    {
                        switch (par1)
                        {
                            case "maps":
                            case "levels":
                            case "map":
                            case "level":
                                Economy.Settings.Level lvl = Economy.FindLevel(par3);
                                switch (par2)
                                {
                                    case "new":
                                    case "create":
                                    case "add":
                                        if (Economy.FindLevel(par3) != null) { Player.SendMessage(p, "That preset level already exists"); break; }
                                        else
                                        {
                                            Economy.Settings.Level level = new Economy.Settings.Level();
                                            level.name = par3;
                                            if (isGood(par4)) { level.x = par4; }
                                            if (isGood(par5)) { level.y = par5; }
                                            if (isGood(par6)) { level.z = par6; }
                                            else { Player.SendMessage(p, "A Dimension was wrong, it must a power of 2"); break; }
                                            switch (par7.ToLower())
                                            {
                                                case "flat":
                                                case "pixel":
                                                case "island":
                                                case "mountains":
                                                case "ocean":
                                                case "forest":
                                                case "desert":
                                                case "space":
                                                    level.type = par7.ToLower();
                                                    break;

                                                default:
                                                    Player.SendMessage(p, "Valid types: island, mountains, forest, ocean, flat, pixel, desert, space");
                                                    break;
                                            }
                                            level.price = int.Parse(par8);
                                            Economy.Settings.LevelsList.Add(level);
                                            Player.SendMessage(p, "Added map to presets");
                                            break;
                                        }

                                    case "delete":
                                    case "remove":
                                        if (lvl == null) { Player.SendMessage(p, "That preset level doesn't exist"); break; }
                                        else { Economy.Settings.LevelsList.Remove(lvl); Player.SendMessage(p, "Removed preset"); break; }

                                    case "edit":
                                    case "change":
                                        if (lvl == null) { Player.SendMessage(p, "That preset level doesn't exist"); break; }
                                        else 
                                        {
                                            switch (par4)
                                            {
                                                case "name":
                                                case "title":
                                                    Economy.Settings.LevelsList.Remove(lvl);
                                                    lvl.name = par5;
                                                    Economy.Settings.LevelsList.Add(lvl);
                                                    Player.SendMessage(p, "Changed preset name");
                                                    break;

                                                case "x":
                                                    if (isGood(par5))
                                                    {
                                                        Economy.Settings.LevelsList.Remove(lvl);
                                                        lvl.x = par5;
                                                        Economy.Settings.LevelsList.Add(lvl);
                                                        Player.SendMessage(p, "Changed preset x size");
                                                    }
                                                    else { Player.SendMessage(p, "Dimension was wrong, it must a power of 2"); break; }
                                                    break;

                                                case "y":
                                                    if (isGood(par5))
                                                    {
                                                        Economy.Settings.LevelsList.Remove(lvl);
                                                        lvl.y = par5;
                                                        Economy.Settings.LevelsList.Add(lvl);
                                                        Player.SendMessage(p, "Changed preset y size");
                                                    }
                                                    else { Player.SendMessage(p, "Dimension was wrong, it must a power of 2"); break; }
                                                    break;

                                                case"z":
                                                    if (isGood(par5))
                                                    {
                                                        Economy.Settings.LevelsList.Remove(lvl);
                                                        lvl.z = par5;
                                                        Economy.Settings.LevelsList.Add(lvl);
                                                        Player.SendMessage(p, "Changed preset z size");
                                                    }
                                                    else { Player.SendMessage(p, "Dimension was wrong, it must a power of 2"); break; }
                                                    break;

                                                case "type":
                                                    Economy.Settings.LevelsList.Remove(lvl);
                                                    switch (par5.ToLower())
                                                    {
                                                        case "flat":
                                                        case "pixel":
                                                        case "island":
                                                        case "mountains":
                                                        case "ocean":
                                                        case "forest":
                                                        case "desert":
                                                        case "space":
                                                            lvl.type = par5.ToLower();
                                                            break;

                                                        default:
                                                            Player.SendMessage(p, "Valid types: island, mountains, forest, ocean, flat, pixel, desert, space");
                                                            Economy.Settings.LevelsList.Add(lvl);
                                                            break;
                                                    }
                                                    Economy.Settings.LevelsList.Add(lvl);
                                                    Player.SendMessage(p, "Changed preset type");
                                                    break;

                                                case "dimensions":
                                                case "sizes":
                                                case "dimension":
                                                case "size":
                                                    Economy.Settings.LevelsList.Remove(lvl);
                                                    if (isGood(par4)) { lvl.x = par4; }
                                                    if (isGood(par5)) { lvl.y = par5; }
                                                    if (isGood(par6)) { lvl.z = par6; }
                                                    else { Player.SendMessage(p, "A Dimension was wrong, it must a power of 2"); Economy.Settings.LevelsList.Add(lvl); break; }
                                                    Economy.Settings.LevelsList.Add(lvl);
                                                    Player.SendMessage(p, "Changed preset name");
                                                    break;

                                                case "price":
                                                    Economy.Settings.LevelsList.Remove(lvl);
                                                    lvl.price = int.Parse(par5);
                                                    Economy.Settings.LevelsList.Add(lvl);
                                                    Player.SendMessage(p, "Changed preset price");
                                                    break;

                                                default:
                                                    Player.SendMessage(p, "That wasn't a valid command addition");
                                                    break;
                                            }
                                        }
                                        break;

                                    case "enable":
                                        if (Economy.Settings.Levels == true) { Player.SendMessage(p, "Maps are already enabled for the economy system"); break; }
                                        else { Economy.Settings.Levels = true; Player.SendMessage(p, "Maps are now enabled for the economy system"); break; }

                                    case "disable":
                                        if (Economy.Settings.Levels == false) { Player.SendMessage(p, "Maps are already disabled for the economy system"); break; }
                                        else { Economy.Settings.Levels = false; Player.SendMessage(p, "Maps are now disabled for the economy system"); break; }

                                    default:
                                        Player.SendMessage(p, "That wasn't a valid command addition");
                                        break;
                                }
                                break;

                            case "titles":
                            case "title":
                                switch (par2)
                                {
                                    case "enable":
                                        if (Economy.Settings.Titles == true) { Player.SendMessage(p, "Titles are already enabled for the economy system"); break; }
                                        else { Economy.Settings.Titles = true; Player.SendMessage(p, "Titles are now enabled for the economy system"); break; }

                                    case "disable":
                                        if (Economy.Settings.Titles == false) { Player.SendMessage(p, "Titles are already disabled for the economy system"); break; }
                                        else { Economy.Settings.Titles = false; Player.SendMessage(p, "Titles are now disabled for the economy system"); break; }

                                    case "price":
                                        Economy.Settings.TitlePrice = int.Parse(par3);
                                        Player.SendMessage(p, "Changed title price");
                                        break;

                                    default:
                                        Player.SendMessage(p, "That wasn't a valid command addition");
                                        break;
                                }
                                break;

                            case "colors":
                            case "colours":
                            case "color":
                            case "colour":
                                switch (par2)
                                {
                                    case "enable":
                                        if (Economy.Settings.Colors == true) { Player.SendMessage(p, "Colors are already enabled for the economy system"); break; }
                                        else { Economy.Settings.Colors = true; Player.SendMessage(p, "Colors are now enabled for the economy system"); break; }

                                    case "disable":
                                        if (Economy.Settings.Colors == false) { Player.SendMessage(p, "Colors are already disabled for the economy system"); break; }
                                        else { Economy.Settings.Colors = false; Player.SendMessage(p, "Colors are now disabled for the economy system"); break; }

                                    case "price":
                                        Economy.Settings.ColorPrice = int.Parse(par3);
                                        Player.SendMessage(p, "Changed color price");
                                        break;

                                    default:
                                        Player.SendMessage(p, "That wasn't a valid command addition");
                                        break;
                                }
                                break;

                            case "ranks":
                            case "rank":
                                switch (par2)
                                {
                                    case "enable":
                                        if (Economy.Settings.Ranks == true) { Player.SendMessage(p, "Ranks are already enabled for the economy system"); break; }
                                        else { Economy.Settings.Ranks = true; Player.SendMessage(p, "Ranks are now enabled for the economy system"); break; }

                                    case "disable":
                                        if (Economy.Settings.Ranks == false) { Player.SendMessage(p, "Ranks are already disabled for the economy system"); break; }
                                        else { Economy.Settings.Ranks = false; Player.SendMessage(p, "Ranks are now disabled for the economy system"); break; }

                                    case "price":
                                        Economy.Settings.RankPrice = int.Parse(par3);
                                        Player.SendMessage(p, "Changed rank price");
                                        break;

                                    case "maxrank":
                                    case "max":
                                    case "maximum":
                                    case "maximumrank":
                                        Group grp = Group.Find(par3);
                                        if (grp == null) { Player.SendMessage(p, "That isn't a rank!!"); break; }
                                        else { Economy.Settings.MaxRank = grp.Permission; Player.SendMessage(p, "Set max rank"); break; }

                                    default:
                                        Player.SendMessage(p, "That wasn't a valid command addition");
                                        break;
                                }
                                break;

                            case "enable":
                                if (Economy.Settings.Enabled == true) { Player.SendMessage(p, "The economy system is already enabled"); return; }
                                else { Economy.Settings.Enabled = true; Player.SendMessage(p, "The economy system is now enabled"); return; }

                            case "disable":
                                if (Economy.Settings.Enabled == false) { Player.SendMessage(p, "The economy system is already disabled"); return; }
                                else { Economy.Settings.Enabled = false; Player.SendMessage(p, "The economy system is now disabled"); return; }

                            default:
                                Player.SendMessage(p, "That wasn't a valid command addition");
                                return;
                        }
                        Economy.Save();
                        return;
                    }
                    else { Player.SendMessage(p, "You aren't a high enough rank for that"); return; }

                

                case "buy":
                    switch (par1)
                    {
                        case "map":
                        case "level":
                        case "maps":
                        case "levels":
                            Economy.Settings.Level lvl = Economy.FindLevel(par2);
                            if (lvl == null) { Player.SendMessage(p, "That isn't a level preset"); return; }
                            else 
                            {
                                if (p.EnoughMoney(lvl.price) == false) { Player.SendMessage(p, "You don't have enough " + Server.moneys + " to buy that map"); return; }
                                else
                                {
                                    if (par3 == null) {Player.SendMessage(p, "You didn't specify a name for your level"); return;}
                                    else
                                    {
                                        int old = p.money;
                                        try
                                        {
                                            Command.all.Find("newlvl").Use(null, p.name + "_" + par3 + " " + lvl.x + " " + lvl.y + " " + lvl.z + " " + lvl.type);
                                            Player.SendMessage(p, "Created level '" + p.name + "_" + par3 + "'");
                                            p.money = p.money - lvl.price;
                                            Player.SendMessage(p, "Your balance is now " + p.money.ToString() + " " + Server.moneys);
                                            Command.all.Find("load").Use(null, p.name + "_" + par3);
                                            Thread.Sleep(250);
                                            Level level = Level.Find(p.name + "_" + par3);
                                            if (level.permissionbuild > p.group.Permission) { level.permissionbuild = p.group.Permission; }
                                            if (level.permissionvisit > p.group.Permission) { level.permissionvisit = p.group.Permission; }
                                            Command.all.Find("goto").Use(p, p.name + "_" + par3);
                                            while (p.Loading) { Thread.Sleep(250); }
                                            try
                                            {
                                                //DB
                                                MySQL.executeQuery("INSERT INTO `Zone" + level.name + "` (SmallX, SmallY, SmallZ, BigX, BigY, BigZ, Owner) VALUES (0,0,0," + (level.width - 1) + "," + (level.depth - 1) + "," + (level.height - 1) + ",'" + p.name + "')"); //CHECK!!!!
                                                //DB
                                                Player.SendMessage(p, "Zoning Succesful");
                                                return;
                                            }
                                            catch { Player.SendMessage(p, "Zoning Failed"); return; }
                                        }
                                        catch { Player.SendMessage(p, "Something went wrong, Money restored"); if (old != p.money) { p.money = old; } return; }
                                    }
                                }
                            }

                        case "colors":
                        case "color":
                        case "colours":
                        case "colour":
                            if (!par2.StartsWith("&") || !par2.StartsWith("%"))
                            {
                                switch (par2)
                                {
                                    case "black":
                                        par2 = "&0";
                                        break;
                                    case "navy":
                                        par2 = "&1";
                                        break;
                                    case "green":
                                        par2 = "&2";
                                        break;
                                    case "teal":
                                        par2 = "&3";
                                        break;
                                    case "maroon":
                                        par2 = "&4";
                                        break;
                                    case "purple":
                                        par2 = "&5";
                                        break;
                                    case "gold":
                                        par2 = "&6";
                                        break;
                                    case "silver":
                                        par2 = "&7";
                                        break;
                                    case "gray":
                                        par2 = "&8";
                                        break;
                                    case "blue":
                                        par2 = "&9";
                                        break;
                                    case "lime":
                                        par2 = "&a";
                                        break;
                                    case "aqua":
                                        par2 = "&b";
                                        break;
                                    case "red":
                                        par2 = "&c";
                                        break;
                                    case "pink":
                                        par2 = "&d";
                                        break;
                                    case "yellow":
                                        par2 = "&e";
                                        break;
                                    case "white":
                                        par2 = "&f";
                                        break;
                                    default:
                                        Player.SendMessage(p, "That wasn't a color");
                                        return;
                                }
                            }
                            if (par2 == p.color) { Player.SendMessage(p, "You already have that color"); return; }
                            if (p.EnoughMoney(Economy.Settings.ColorPrice) == false) { Player.SendMessage(p, "You don't have enough " + Server.moneys + " to buy a color"); return; }
                            else { Command.all.Find("color").Use(null, p.name + " " + c.Name(par2)); p.money = p.money - Economy.Settings.ColorPrice; Player.SendMessage(p, "Changed color"); Player.SendMessage(p, "Your balance is now " + p.money.ToString() + " " + Server.moneys); return; }

                        case "titles":
                        case "title":
                            if (par2 == p.title) { Player.SendMessage(p, "You already have that title"); return; }
                            if (p.EnoughMoney(Economy.Settings.TitlePrice) == false) { Player.SendMessage(p, "You don't have enough " + Server.moneys + " to buy a title"); return; }
                            else { Command.all.Find("title").Use(null, p.name + " " + par2); p.money = p.money - Economy.Settings.TitlePrice; Player.SendMessage(p, "Changed title"); Player.SendMessage(p, "Your balance is now " + p.money.ToString() + " " + Server.moneys); return; }

                        case "ranks":
                        case "rank":
                            if (p.group.Permission == Economy.Settings.MaxRank || p.group.Permission >= Economy.Settings.MaxRank) { Player.SendMessage(p, "You are past the max buyable rank"); return; }
                            if (p.EnoughMoney(Economy.Settings.RankPrice) == false) { Player.SendMessage(p, "You don't have enough " + Server.moneys + " to buy the next rank"); return; }
                            else { Command.all.Find("promote").Use(null, p.name); p.money = p.money - Economy.Settings.RankPrice; Player.SendMessage(p, "You bought the rank " + p.group.name); Player.SendMessage(p, "Your balance is now " + p.money.ToString() + " " + Server.moneys); return; }

                        default:
                            Player.SendMessage(p, "That wasn't a valid command addition");
                            return;
                    }

                case "stats":
                case "balance":
                case "amount":
                    if (par1 != null)
                    {
                        Player who = Player.Find(par1);
                        if (who == null) { Player.SendMessage(p, "That player doesn't exist"); return; }
                        else
                        {
                            Player.SendMessage(p, "Stats for: " + who.color + who.name);
                            Player.SendMessage(p, "============================================================");
                            Player.SendMessage(p, Server.moneys + ": &b$" + who.money);
                            return;
                        }
                    }
                    else
                    {
                        Player.SendMessage(p, "Stats for: " + p.color + p.name);
                        Player.SendMessage(p, "============================================================");
                        Player.SendMessage(p, Server.moneys + ": &b$" + p.money);
                        return;
                    }
                
                case "info":
                case "about":
                   if (Economy.Settings.Enabled == true)
                   {
                       switch (par1)
                       {
                           case "map":
                           case "level":
                           case "maps":
                           case "levels":
                               if (Economy.Settings.Levels == false) { Player.SendMessage(p, "Maps are not enabled for the economy system"); return; }
                               Player.SendMessage(p, "Maps avaliable:");
                               foreach (Economy.Settings.Level lvl in Economy.Settings.LevelsList)
                               {
                                   Player.SendMessage(p, lvl.name + " (" + lvl.x + "," + lvl.y + "," + lvl.z + ") " + lvl.type + ":" + lvl.price);
                               }
                               return;

                           case "title":
                           case "titles":
                               if (Economy.Settings.Titles == false) { Player.SendMessage(p, "Titles are not enabled for the economy system"); return; }
                               Player.SendMessage(p, "Titles cost " + Economy.Settings.TitlePrice.ToString() + " each");
                               return;

                           case "colors":
                           case "color":
                           case "colours":
                           case "colour":
                               if (Economy.Settings.Colors == false) { Player.SendMessage(p, "Colors are not enabled for the economy system"); return; }
                               Player.SendMessage(p, "Colors cost " + Economy.Settings.ColorPrice.ToString() + " each");
                               return;

                           case "ranks":
                           case "rank":
                               if (Economy.Settings.Ranks == false) { Player.SendMessage(p, "Ranks are not enabled for the economy system"); return; }
                               Player.SendMessage(p, "Ranks cost " + Economy.Settings.RankPrice.ToString() + " each");
                               Player.SendMessage(p, "The maximum buyable rank is " + Economy.Settings.MaxRank.ToString());
                               return;

                           default:
                               Player.SendMessage(p, "That wasn't a valid command addition");
                               return;
                       }
                   }
                   else { Player.SendMessage(p, "The economy system is currently disabled"); return; }
                default:
                    Player.SendMessage(p, "That wasn't a valid command addition, Sending you to help");
                    Help(p);
                    return;
            }
        }
        public override void Help(Player p)
        {

        }

        public bool isGood(string value)
        {
            ushort uvalue = ushort.Parse(value);
            switch (uvalue)
            {
                case 2:
                case 4:
                case 8:
                case 16:
                case 32:
                case 64:
                case 128:
                case 256:
                case 512:
                case 1024:
                case 2048:
                case 4096:
                case 8192:
                    return true;
            }

            return false;
        }
    }
}
