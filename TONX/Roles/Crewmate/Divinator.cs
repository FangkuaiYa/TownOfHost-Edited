﻿using AmongUs.GameOptions;

using TONX.Roles.Core;
using static TONX.Translator;

namespace TONX.Roles.Crewmate;
public sealed class Divinator : RoleBase
{
    public static readonly SimpleRoleInfo RoleInfo =
        SimpleRoleInfo.Create(
            typeof(Divinator),
            player => new Divinator(player),
            CustomRoles.Divinator,
            () => RoleTypes.Crewmate,
            CustomRoleTypes.Crewmate,
            22200,
            SetupOptionItem,
            "ft|占卜師|占卜",
            "#882c83"
        );
    public Divinator(PlayerControl player)
    : base(
        RoleInfo,
        player
    )
    {
        DidVote = false;
    }

    static OptionItem OptionCheckNums;
    static OptionItem OptionAccurateCheck;
    enum OptionName
    {
        DivinatorSkillLimit,
        AccurateCheckMode
    }

    private int CheckLimit;
    private bool DidVote;
    private static void SetupOptionItem()
    {
        OptionCheckNums = IntegerOptionItem.Create(RoleInfo, 10, OptionName.DivinatorSkillLimit, new(1, 15, 1), 5, false)
            .SetValueFormat(OptionFormat.Times);
        OptionAccurateCheck = BooleanOptionItem.Create(RoleInfo, 11, OptionName.AccurateCheckMode, false, false);
        Options.OverrideTasksData.Create(RoleInfo, 20);
    }
    public override void Add() => CheckLimit = OptionCheckNums.GetInt();
    public override void OnStartMeeting() => DidVote = CheckLimit < 1;
    public override bool CheckVoteAsVoter(PlayerControl votedFor)
    {
        if (votedFor == null || !Player.IsAlive() || DidVote || CheckLimit < 1) return true;

        DidVote = true;
        CheckLimit--;

        if (Is(votedFor))
        {
            string notice1 = GetString("DivinatorCheckSelfMsg") + "\n\n" + string.Format(GetString("DivinatorCheckLimit"), CheckLimit) + GetString("SkillDoneAndYouCanVoteNormallyNow");
            Player.ShowPopUp(notice1);
            Utils.SendMessage(notice1, Player.PlayerId, Utils.ColorString(Utils.GetRoleColor(CustomRoles.Divinator), GetString("DivinatorCheckMsgTitle")));
            return false;
        }

        string msg;

        if (Player.AllTasksCompleted() || OptionAccurateCheck.GetBool())
        {
            msg = string.Format(GetString("DivinatorCheck.TaskDone"), votedFor.GetRealName(), GetString(votedFor.GetCustomRole().ToString()));
        }
        else
        {
            string text = votedFor.GetCustomRole() switch
            {
                CustomRoles.TimeThief or
                CustomRoles.AntiAdminer or
                CustomRoles.SuperStar or
                CustomRoles.Mayor or
                CustomRoles.Snitch or
                CustomRoles.Counterfeiter or
                CustomRoles.God or
                CustomRoles.Judge or
                CustomRoles.Observer or
                CustomRoles.DoveOfPeace or
                CustomRoles.NekoKabocha or
                CustomRoles.Messenger or
                CustomRoles.Insider
                => "HideMsg",

                CustomRoles.Miner or
                CustomRoles.Scavenger or
                CustomRoles.Luckey or
                CustomRoles.Needy or
                CustomRoles.SabotageMaster or
                CustomRoles.Jackal or
                CustomRoles.Mario or
                CustomRoles.Cleaner or
                CustomRoles.Crewpostor or
                CustomRoles.Penguin
                => "Honest",

                CustomRoles.SerialKiller or
                CustomRoles.BountyHunter or
                CustomRoles.Minimalism or
                CustomRoles.Sans or
                CustomRoles.SpeedBooster or
                CustomRoles.Sheriff or
                CustomRoles.Arsonist or
                CustomRoles.Innocent or
                CustomRoles.FFF or
                CustomRoles.Greedier
                => "Impulse",

                CustomRoles.Vampire or
                CustomRoles.Assassin or
                CustomRoles.Escapee or
                CustomRoles.Sniper or
                CustomRoles.SwordsMan or
                CustomRoles.Bodyguard or
                CustomRoles.Opportunist or
                CustomRoles.Pelican or
                CustomRoles.ImperiusCurse or
                CustomRoles.Stealth
                => "Weirdo",

                CustomRoles.EvilGuesser or
                CustomRoles.Bomber or
                CustomRoles.Capitalism or
                CustomRoles.NiceGuesser or
                CustomRoles.Grenadier or
                CustomRoles.Terrorist or
                CustomRoles.Revolutionist or
                CustomRoles.Gamer or
                CustomRoles.Eraser or
                CustomRoles.PlagueDoctor
                => "Blockbuster",

                CustomRoles.Warlock or
                CustomRoles.Hacker or
                CustomRoles.Mafia or
                CustomRoles.Doctor or
                CustomRoles.Transporter or
                CustomRoles.Veteran or
                CustomRoles.Divinator or
                CustomRoles.QuickShooter or
                CustomRoles.Mediumshiper or
                CustomRoles.Judge or
                CustomRoles.BloodKnight
                => "Strong",

                CustomRoles.Witch or
                CustomRoles.Puppeteer or
                CustomRoles.ShapeMaster or
                CustomRoles.Paranoia or
                CustomRoles.Psychic or
                CustomRoles.Executioner or
                CustomRoles.BallLightning or
                CustomRoles.Workaholic or
                CustomRoles.Provocateur or
                CustomRoles.SchrodingerCat
                => "Incomprehensible",

                CustomRoles.FireWorks or
                CustomRoles.EvilTracker or
                CustomRoles.Gangster or
                CustomRoles.Dictator or
                CustomRoles.CyberStar or
                CustomRoles.Collector or
                CustomRoles.Sunnyboy or
                CustomRoles.Bard or
                CustomRoles.Totocalcio
                => "Enthusiasm",

                CustomRoles.BoobyTrap or
                CustomRoles.Zombie or
                CustomRoles.Mare or
                CustomRoles.Detective or
                CustomRoles.TimeManager or
                CustomRoles.Jester or
                CustomRoles.Medicaler or
                CustomRoles.DarkHide or
                CustomRoles.CursedWolf or
                CustomRoles.OverKiller or
                CustomRoles.Hangman or
                CustomRoles.Mortician
                => "Disturbed",

                CustomRoles.Glitch or
                CustomRoles.Concealer or
                CustomRoles.Swooper
                => "Glitch",

                CustomRoles.Succubus
                => "Love",

                _ => "None",
            };
            msg = string.Format(GetString("DivinatorCheck." + text), votedFor.GetRealName());
        }

        string notice2 = GetString("DivinatorCheck") + "\n" + msg + "\n\n" + string.Format(GetString("DivinatorCheckLimit"), CheckLimit) + GetString("SkillDoneAndYouCanVoteNormallyNow");
        Player.ShowPopUp(notice2);
        Utils.SendMessage(notice2, Player.PlayerId, Utils.ColorString(Utils.GetRoleColor(CustomRoles.Divinator), GetString("DivinatorCheckMsgTitle")));

        return false;
    }
}