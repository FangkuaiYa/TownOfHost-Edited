﻿using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TOHE.Roles.Core;
using static TOHE.Translator;

namespace TOHE;

public static class MeetingStartNotify
{
    public static void OnMeetingStart()
    {
        if (!AmongUsClient.Instance.AmHost) return;

        List<(string, byte, string)> msgToSend = new();

        void AddMsg(string text, byte sendTo = 255, string title = "")
            => msgToSend.Add((text, sendTo, title));

        //首次会议技能提示
        if (Options.SendRoleDescriptionFirstMeeting.GetBool() && MeetingStates.FirstMeeting)
            foreach (var pc in Main.AllAlivePlayerControls.Where(x => !x.IsModClient()))
            {
                var role = pc.GetCustomRole();
                var sb = new StringBuilder();
                sb.Append(GetString(role.ToString()) + Utils.GetRoleDisplaySpawnMode(role) + pc.GetRoleInfo(true));
                if (Options.CustomRoleSpawnChances.TryGetValue(role, out var opt))
                    Utils.ShowChildrenSettings(opt, ref sb, command: true);
                var txt = sb.ToString();
                sb.Clear().Append(txt.RemoveHtmlTags());
                foreach (var subRole in PlayerState.AllPlayerStates[pc.PlayerId].SubRoles)
                    sb.Append($"\n\n" + GetString($"{subRole}") + Utils.GetRoleDisplaySpawnMode(subRole) + GetString($"{subRole}InfoLong"));
                if (CustomRoles.Ntr.Exist() && (role is not CustomRoles.GM and not CustomRoles.Ntr))
                    sb.Append($"\n\n" + GetString($"Lovers") + Utils.GetRoleDisplaySpawnMode(CustomRoles.Lovers) + GetString($"LoversInfoLong"));
                AddMsg(sb.ToString(), pc.PlayerId);
            }
        if (msgToSend.Count >= 1)
        {
            var msgTemp = msgToSend.ToList();
            new LateTask(() => { msgTemp.Do(x => Utils.SendMessage(x.Item1, x.Item2, x.Item3 ?? "")); }, 3f, "NotifyOnMeetingStart");
        }

        //Mimic Msg Combine
        var mimicSb = new StringBuilder();
        foreach (var vic in Main.AllPlayerControls.Where(p => !p.IsAlive()))
        {
            var killer = PlayerState.GetByPlayerId(vic.PlayerId)?.GetRealKiller();
            if (killer != null && Main.AllPlayerControls.Any(p => !p.IsAlive() && p.Is(CustomRoles.Mimic) && p.PlayerId == killer))
                mimicSb.Append($"\n{vic.GetNameWithRole(true)}");
        }
        if (mimicSb.Length > 1)
        {
            string mimicMsg = GetString("MimicDeadMsg") + "\n" + mimicSb.ToString();
            foreach (var ipc in Main.AllPlayerControls.Where(x => x.Is(CustomRoleTypes.Impostor)))
                AddMsg(mimicMsg, ipc.PlayerId, Utils.ColorString(Utils.GetRoleColor(CustomRoles.Mimic), GetString("MimicMsgTitle")));
        }

        msgToSend = new();
        CustomRoleManager.AllActiveRoles.Values.Do(x => x.NotifyOnMeetingStart(ref msgToSend));
        msgToSend.Do(x => Logger.Info($"To:{x.Item2} {x.Item3 ?? ""} => {x.Item1}", "NotifyOnMeetingStart"));
        new LateTask(() => { msgToSend.DoIf(x => x.Item1 != null, x => Utils.SendMessage(x.Item1, x.Item2, x.Item3 ?? "")); }, 3f, "NotifyOnMeetingStart");
    }
}
