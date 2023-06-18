﻿using HarmonyLib;
using TMPro;
using UnityEngine;

using static TOHE.Translator;

namespace TOHE;

[HarmonyPatch(typeof(AccountTab), nameof(AccountTab.Awake))]
public static class UpdateFriendCodeUIPatch
{
    private static GameObject VersionShower;
    public static void Prefix(AccountTab __instance)
    {

        string credentialsText = string.Format(GetString("MainMenuCredential"), $"<color={Main.ModColor}>KARPED1EM</color>");
        credentialsText += "\t\t\t";
        string versionText = $"<color={Main.ModColor}>{Main.ModName}</color> - {Main.PluginVersion}";

#if CANARY
        versionText = $"<color=#fffe1e>{Main.ModName}</color> - {ThisAssembly.Git.Commit}";
#endif

#if DEBUG
        versionText = $"<color=#00a4ff>{ThisAssembly.Git.Branch}</color> - {ThisAssembly.Git.Commit}";
#endif

        credentialsText += versionText;

        var friendCode = GameObject.Find("FriendCode");
        if (friendCode != null && VersionShower == null)
        {
            VersionShower = Object.Instantiate(friendCode, friendCode.transform.parent);
            VersionShower.name = "TOHE Version Shower";
            VersionShower.transform.localPosition = friendCode.transform.localPosition + new Vector3(3.2f, 0f, 0f);
            VersionShower.transform.localScale *= 1.7f;
            var TMP = VersionShower.GetComponent<TextMeshPro>();
            TMP.alignment = TextAlignmentOptions.Right;
            TMP.fontSize = 30f;
            TMP.SetText(credentialsText);
        }

        var newRequest = GameObject.Find("NewRequest");
        if (newRequest != null)
        {
            newRequest.transform.localPosition -= new Vector3(0f, 0f, 10f);
            newRequest.transform.localScale = new Vector3(0.8f, 1f, 1f);
        }
    }
}