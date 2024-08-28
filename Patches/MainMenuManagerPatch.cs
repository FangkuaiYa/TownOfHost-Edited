using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using TOHE;
using TOHE.Templates;
using UnityEngine;

namespace TOHE;

[HarmonyPatch]
public static class MainMenuManagerPatch
{
    public static GameObject GitHubButton;

    public static GameObject QQButton;
    public static MainMenuManager Instance { get; private set; }

    public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> collection)
    {
        return collection.SelectMany(x => x);
    }

    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.OpenGameModeMenu))]
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.OpenAccountMenu))]
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.OpenCredits))]
    [HarmonyPrefix, HarmonyPriority(Priority.Last)]
    public static void ShowRightPanel() => ShowingPanel = true;

    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start))]
    [HarmonyPatch(typeof(OptionsMenuBehaviour), nameof(OptionsMenuBehaviour.Open))]
    [HarmonyPatch(typeof(AnnouncementPopUp), nameof(AnnouncementPopUp.Show))]
    [HarmonyPrefix, HarmonyPriority(Priority.Last)]
    public static void HideRightPanel()
    {
        ShowingPanel = false;
        AccountManager.Instance?.transform?.FindChild("AccountTab/AccountWindow")?.gameObject?.SetActive(false);
    }

    public static void ShowRightPanelImmediately()
    {
        ShowingPanel = true;
        TitleLogoPatch.RightPanel.transform.localPosition = TitleLogoPatch.RightPanelOp;
        Instance.OpenGameModeMenu();
    }

    public static bool ShowedBak = false;
    private static bool ShowingPanel = false;
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.LateUpdate)), HarmonyPostfix]
    public static void MainMenuManager_LateUpdate()
    {
        if (GameObject.Find("MainUI") == null) ShowingPanel = false;

        if (TitleLogoPatch.RightPanel != null)
        {
            var pos1 = TitleLogoPatch.RightPanel.transform.localPosition;
            Vector3 lerp1 = Vector3.Lerp(pos1, TitleLogoPatch.RightPanelOp + new Vector3((ShowingPanel ? 0f : 10f), 0f, 0f), Time.deltaTime * (ShowingPanel ? 3f : 2f));
            if (ShowingPanel
                ? TitleLogoPatch.RightPanel.transform.localPosition.x > TitleLogoPatch.RightPanelOp.x + 0.03f
                : TitleLogoPatch.RightPanel.transform.localPosition.x < TitleLogoPatch.RightPanelOp.x + 9f
                ) TitleLogoPatch.RightPanel.transform.localPosition = lerp1;
        }
    }
    [HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start)), HarmonyPostfix]
    public static void Start_Postfix(MainMenuManager __instance)
    {
        Instance = __instance;

        SimpleButton.SetBase(__instance.quitButton);

        int row = 1; int col = 0;
        GameObject CreatButton(string text, Action action)
        {
            col++; if (col > 2) { col = 1; row++; }
            var template = col == 1 ? __instance.creditsButton.gameObject : __instance.quitButton.gameObject;
            var button = UnityEngine.Object.Instantiate(template, template.transform.parent);
            button.transform.transform.FindChild("FontPlacer").GetChild(0).gameObject.DestroyTranslator();
            var buttonText = button.transform.FindChild("FontPlacer").GetChild(0).GetComponent<TextMeshPro>();
            buttonText.text = text;
            PassiveButton passiveButton = button.GetComponent<PassiveButton>();
            passiveButton.OnClick = new();
            passiveButton.OnClick.AddListener(action);
            AspectPosition aspectPosition = button.GetComponent<AspectPosition>();
            aspectPosition.anchorPoint = new Vector2(col == 1 ? 0.415f : 0.583f, 0.5f - 0.08f * row);
            return button;
        }

        if (QQButton == null) QQButton = CreatButton("QQ群", () => { Application.OpenURL(Main.QQInviteUrl); });
        QQButton.gameObject.SetActive(Main.ShowQQButton);
        QQButton.name = "TOHE QQ Button";

        if (GitHubButton == null) GitHubButton = CreatButton("GitHub", () => Application.OpenURL(Main.GithubRepoUrl));
        GitHubButton.gameObject.SetActive(Main.ShowGithubUrl);
        GitHubButton.name = "TOHE GitHub Button";

        Application.targetFrameRate = Main.UnlockFPS.Value ? 165 : 60;
    }
}