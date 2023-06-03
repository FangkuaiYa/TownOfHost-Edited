using AmongUs.GameOptions;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;

using TOHE.Roles.Core;

namespace TOHE.Roles.Crewmate;
public sealed class Psychic : RoleBase
{
    public static readonly SimpleRoleInfo RoleInfo =
        new(
            typeof(Psychic),
            player => new Psychic(player),
            CustomRoles.Psychic,
            () => RoleTypes.Crewmate,
            CustomRoleTypes.Crewmate,
            8020450,
            SetupOptionItem,
            "psy",
            "#6F698C"
        );
    public Psychic(PlayerControl player)
    : base(
        RoleInfo,
        player
    )
    {
        RedNames = new();
    }

    static OptionItem OptionRedNameNum;
    static OptionItem OptionFreshEachMeeting;
    static OptionItem OptionCkshowEvil;
    static OptionItem OptionNBshowEvil;
    static OptionItem OptionNEshowEvil;
    enum OptionName
    {
        PsychicCanSeeNum,
        PsychicFresh,
        CrewKillingRed,
        NBareRed,
        NEareRed,
    }

    private List<byte> RedNames;
    private static void SetupOptionItem()
    {
        OptionRedNameNum = IntegerOptionItem.Create(RoleInfo, 10, OptionName.PsychicCanSeeNum, new(1, 15, 1), 3, false)
            .SetValueFormat(OptionFormat.Pieces);
        OptionFreshEachMeeting = BooleanOptionItem.Create(RoleInfo, 11, OptionName.PsychicFresh, false, false);
        OptionCkshowEvil = BooleanOptionItem.Create(RoleInfo, 12, OptionName.CrewKillingRed, true, false);
        OptionNBshowEvil = BooleanOptionItem.Create(RoleInfo, 13, OptionName.NBareRed, false, false);
        OptionNEshowEvil = BooleanOptionItem.Create(RoleInfo, 14, OptionName.NEareRed, true, false);
    }
    public override void OnStartMeeting()
    {
        if (OptionFreshEachMeeting.GetBool()) GetRedNames();
    }
    public override void OverrideNameAsSeer(PlayerControl seen, ref string nameText, bool isForMeeting = false)
    {
        if (seen == null || Is(seen) || !isForMeeting || !Player.IsAlive()) return;
        if (RedNames == null || RedNames.Count < 1) GetRedNames();
        if (RedNames.Contains(seen.PlayerId))
            Utils.ColorString(Utils.GetRoleColor(CustomRoles.Impostor), nameText);
    }
    private void GetRedNames()
    {
        List<PlayerControl> BadListPc = Main.AllAlivePlayerControls.Where(x =>
        x.Is(CustomRoleTypes.Impostor) || x.Is(CustomRoles.Madmate) ||
        (x.IsCrewKiller() && OptionCkshowEvil.GetBool()) ||
        (x.IsNeutralKiller() && OptionNEshowEvil.GetBool()) ||
        (x.IsNeutralBenign() && OptionNBshowEvil.GetBool())
        ).ToList();

        List<byte> BadList = new();
        BadListPc.Do(x => BadList.Add(x.PlayerId));
        List<byte> AllList = new();
        Main.AllAlivePlayerControls.Where(x => !BadList.Contains(x.PlayerId) && !x.Is(CustomRoles.Psychic)).Do(x => AllList.Add(x.PlayerId));

        int ENum = 1;
        for (int i = 1; i < OptionRedNameNum.GetInt(); i++)
            if (IRandom.Instance.Next(0, 100) < 18) ENum++;
        int BNum = OptionRedNameNum.GetInt() - ENum;
        ENum = Math.Min(ENum, BadList.Count);
        BNum = Math.Min(BNum, AllList.Count);

        if (ENum < 1) goto EndOfSelect;

        RedNames = new();
        for (int i = 0; i < ENum && BadList.Count >= 1; i++)
        {
            RedNames.Add(BadList[IRandom.Instance.Next(0, BadList.Count)]);
            BadList.RemoveAll(RedNames.Contains);
        }

        AllList.RemoveAll(RedNames.Contains);
        for (int i = 0; i < BNum && AllList.Count >= 1; i++)
        {
            RedNames.Add(AllList[IRandom.Instance.Next(0, AllList.Count)]);
            AllList.RemoveAll(RedNames.Contains);
        }

    EndOfSelect:

        Logger.Info($"需要{OptionRedNameNum.GetInt()}个红名，其中需要{ENum}个邪恶。计算后显示红名{RedNames.Count}个", "Psychic");
        RedNames.Do(x => Logger.Info($"红名：{x}: {Main.AllPlayerNames[x]}", "Psychic"));
    }
}