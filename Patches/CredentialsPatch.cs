using HarmonyLib;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using static TOHE.Translator;

namespace TOHE;

[HarmonyPatch(typeof(PingTracker), nameof(PingTracker.Update))]
internal class PingTrackerUpdatePatch
{
    private static readonly StringBuilder sb = new();

    private static void Postfix(PingTracker __instance)
    {
        __instance.text.alignment = TextAlignmentOptions.TopRight;

        sb.Clear();

        sb.Append(Main.credentialsText);

        var ping = AmongUsClient.Instance.Ping;
        string color = "#ff4500";
        if (ping < 30) color = "#44dfcc";
        else if (ping < 100) color = "#7bc690";
        else if (ping < 200) color = "#f3920e";
        else if (ping < 400) color = "#ff146e";
        sb.Append($"\r\n").Append($"<color={color}>Ping: {ping} ms</color>");

        if (Options.NoGameEnd.GetBool()) sb.Append($"\r\n").Append(Utils.ColorString(Color.red, GetString("NoGameEnd")));
        if (Options.AllowConsole.GetBool()) sb.Append($"\r\n").Append(Utils.ColorString(Color.red, GetString("AllowConsole")));
        if (!GameStates.IsModHost) sb.Append($"\r\n").Append(Utils.ColorString(Color.red, GetString("Warning.NoModHost")));
        if (DebugModeManager.IsDebugMode) sb.Append("\r\n").Append(Utils.ColorString(Color.green, GetString("DebugMode")));
        if (Options.LowLoadMode.GetBool()) sb.Append("\r\n").Append(Utils.ColorString(Color.green, GetString("LowLoadMode")));

        var offset_x = 1.2f; //右端からのオフセット
        if (HudManager.InstanceExists && HudManager._instance.Chat.chatButton.active) offset_x += 0.8f; //チャットボタンがある場合の追加オフセット
        if (FriendsListManager.InstanceExists && FriendsListManager._instance.FriendsListButton.Button.active) offset_x += 0.8f; //フレンドリストボタンがある場合の追加オフセット
        __instance.GetComponent<AspectPosition>().DistanceFromEdge = new Vector3(offset_x, 0f, 0f);

        __instance.text.text = sb.ToString();
    }
}
[HarmonyPatch(typeof(VersionShower), nameof(VersionShower.Start))]
internal class VersionShowerStartPatch
{
    public static GameObject OVersionShower;
    private static TextMeshPro VisitText;
    private static void Postfix(VersionShower __instance)
    {
        string credentialsText = string.Format(GetString("MainMenuCredential"), $"<color={Main.ModColor}>两个萌新开发组</color>");
        credentialsText += "\t\t\t";
        string versionText = $"<color={Main.ModColor}>{Main.ModName}</color> - {Main.PluginVersion}";

        credentialsText += versionText;

        var friendCode = GameObject.Find("FriendCode");
        if (friendCode != null && OVersionShower == null)
        {
            OVersionShower = Object.Instantiate(friendCode, friendCode.transform.parent);
            OVersionShower.name = "TONX Version Shower";
            OVersionShower.transform.localPosition = friendCode.transform.localPosition + new Vector3(3.2f, 0f, 0f);
            OVersionShower.transform.localScale *= 1.7f;
            var TMP = OVersionShower.GetComponent<TextMeshPro>();
            TMP.alignment = TextAlignmentOptions.Right;
            TMP.fontSize = 30f;
            TMP.SetText(credentialsText);
        }


        Main.credentialsText = $"\r\n<color={Main.ModColor}>{Main.ModName}</color> v{Main.PluginVersion}";
        if (Main.IsAprilFools) Main.credentialsText = $"\r\n<color=#00bfff>Town Of Host</color> v11.45.14";

#if CANARY
        Main.credentialsText += $"\r\n<color=#fffe1e>Canary({ThisAssembly.Git.Commit})</color>";
#endif

#if DEBUG
        Main.credentialsText += $"\r\n<color={Main.ModColor}>{ThisAssembly.Git.Branch}({ThisAssembly.Git.Commit})</color>";
#endif

#if RELEASE || CANARY
        string additionalCredentials = GetString("TextBelowVersionText");
        if (additionalCredentials != null && additionalCredentials != "*TextBelowVersionText")
        {
            Main.credentialsText += $"\n{additionalCredentials}";
        }
#endif

        ErrorText.Create(__instance.text);
        if (Main.hasArgumentException && ErrorText.Instance != null)
            ErrorText.Instance.AddError(ErrorCode.Main_DictionaryError);
        if ((OVersionShower = GameObject.Find("VersionShower")) != null && VisitText == null)
        {
            VisitText = Object.Instantiate(__instance.text);
            VisitText.name = "TOHE User Counter";
            VisitText.alignment = TextAlignmentOptions.Left;
            VisitText.text = /*ModUpdater.visit > 0
                ? string.Format(GetString("TOHEVisitorCount"), Main.ModColor, ModUpdater.visit):*/
            $"欢迎使用<color={Main.ModColor}>TownOfHost-Edited</color>";
            VisitText.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            VisitText.transform.localPosition = new Vector3(-3.92f, -2.9f, 0f);
            VisitText.enabled = GameObject.Find("TOHE Background") != null;

            __instance.text.alignment = TextAlignmentOptions.Left;
            OVersionShower.transform.localPosition = new Vector3(-4.92f, -3.3f, 0f);

            var ap1 = OVersionShower.GetComponent<AspectPosition>();
            if (ap1 != null) Object.Destroy(ap1);
            var ap2 = VisitText.GetComponent<AspectPosition>();
            if (ap2 != null) Object.Destroy(ap2);
        };
    }
}

[HarmonyPatch(typeof(MainMenuManager), nameof(MainMenuManager.Start)), HarmonyPriority(Priority.First)]
internal class TitleLogoPatch
{
    public static GameObject ModStamp;
    public static GameObject TOHE_Background;
    public static GameObject Ambience;
    public static GameObject Starfield;
    public static GameObject LeftPanel;
    public static GameObject RightPanel;
    public static GameObject CloseRightButton;
    public static GameObject Tint;
    public static GameObject Sizer;
    public static GameObject AULogo;
    public static GameObject BottomButtonBounds;

    public static Vector3 RightPanelOp;

    private static void Postfix(MainMenuManager __instance)
    {
        GameObject.Find("BackgroundTexture")?.SetActive(MainMenuManagerPatch.ShowedBak);

        if (!(ModStamp = GameObject.Find("ModStamp"))) return;
        ModStamp.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);

        TOHE_Background = new GameObject("TOHE Background");
        TOHE_Background.transform.position = new Vector3(0, 0, 520f);
        var bgRenderer = TOHE_Background.AddComponent<SpriteRenderer>();
        bgRenderer.sprite = Utils.LoadSprite("TOHE.Resources.Images.TOHE-BG.jpg", 179f);

        if (!(Ambience = GameObject.Find("Ambience"))) return;
        if (!(Starfield = Ambience.transform.FindChild("starfield").gameObject)) return;
        StarGen starGen = Starfield.GetComponent<StarGen>();
        starGen.SetDirection(new Vector2(0, -2));
        Starfield.transform.SetParent(TOHE_Background.transform);
        Object.Destroy(Ambience);

        if (!(LeftPanel = GameObject.Find("LeftPanel"))) return;
        LeftPanel.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        static void ResetParent(GameObject obj) => obj.transform.SetParent(LeftPanel.transform.parent);
        LeftPanel.ForEachChild((Il2CppSystem.Action<GameObject>)ResetParent);
        LeftPanel.SetActive(false);

        Color shade = new(0f, 0f, 0f, 0f);
        var standardActiveSprite = __instance.newsButton.activeSprites.GetComponent<SpriteRenderer>().sprite;
        var minorActiveSprite = __instance.quitButton.activeSprites.GetComponent<SpriteRenderer>().sprite;

        Dictionary<List<PassiveButton>, (Sprite, Color, Color, Color, Color)> mainButtons = new()
        {
            {new List<PassiveButton>() {__instance.playButton, __instance.inventoryButton, __instance.shopButton},
                (standardActiveSprite, new(1f, 0.524f, 0.549f, 0.8f), shade, Color.white, Color.white) },
            {new List<PassiveButton>() {__instance.newsButton, __instance.myAccountButton, __instance.settingsButton},
                (minorActiveSprite, new(1f, 0.825f, 0.686f, 0.8f), shade, Color.white, Color.white) },
            {new List<PassiveButton>() {__instance.creditsButton, __instance.quitButton},
                (minorActiveSprite, new(0.526f, 1f, 0.792f, 0.8f), shade, Color.white, Color.white) },
        };

        void FormatButtonColor(PassiveButton button, Sprite borderType, Color inActiveColor, Color activeColor, Color inActiveTextColor, Color activeTextColor)
        {
            button.activeSprites.transform.FindChild("Shine")?.gameObject?.SetActive(false);
            button.inactiveSprites.transform.FindChild("Shine")?.gameObject?.SetActive(false);
            var activeRenderer = button.activeSprites.GetComponent<SpriteRenderer>();
            var inActiveRenderer = button.inactiveSprites.GetComponent<SpriteRenderer>();
            activeRenderer.sprite = minorActiveSprite;
            inActiveRenderer.sprite = minorActiveSprite;
            activeRenderer.color = activeColor.a == 0f ? new Color(inActiveColor.r, inActiveColor.g, inActiveColor.b, 1f) : activeColor;
            inActiveRenderer.color = inActiveColor;
            button.activeTextColor = activeTextColor;
            button.inactiveTextColor = inActiveTextColor;
        }

        foreach (var kvp in mainButtons)
            kvp.Key.Do(button => FormatButtonColor(button, kvp.Value.Item1, kvp.Value.Item2, kvp.Value.Item3, kvp.Value.Item4, kvp.Value.Item5));

        GameObject.Find("Divider")?.SetActive(false);

        if (!(RightPanel = GameObject.Find("RightPanel"))) return;
        var rpap = RightPanel.GetComponent<AspectPosition>();
        if (rpap) Object.Destroy(rpap);
        RightPanelOp = RightPanel.transform.localPosition;
        RightPanel.transform.localPosition = RightPanelOp + new Vector3(10f, 0f, 0f);
        RightPanel.GetComponent<SpriteRenderer>().color = new(1f, 0.78f, 0.9f, 1f);

        CloseRightButton = new GameObject("CloseRightPanelButton");
        CloseRightButton.transform.SetParent(RightPanel.transform);
        CloseRightButton.transform.localPosition = new Vector3(-4.78f, 1.3f, 1f);
        CloseRightButton.transform.localScale = new(1f, 1f, 1f);
        CloseRightButton.AddComponent<BoxCollider2D>().size = new(0.6f, 1.5f);
        var closeRightSpriteRenderer = CloseRightButton.AddComponent<SpriteRenderer>();
        closeRightSpriteRenderer.sprite = Utils.LoadSprite("TOHE.Resources.Images.RightPanelCloseButton.png", 100f);
        closeRightSpriteRenderer.color = new(1f, 0.78f, 0.9f, 1f);
        var closeRightPassiveButton = CloseRightButton.AddComponent<PassiveButton>();
        closeRightPassiveButton.OnClick = new();
        closeRightPassiveButton.OnClick.AddListener((System.Action)MainMenuManagerPatch.HideRightPanel);
        closeRightPassiveButton.OnMouseOut = new();
        closeRightPassiveButton.OnMouseOut.AddListener((System.Action)(() => closeRightSpriteRenderer.color = new(1f, 0.78f, 0.9f, 1f)));
        closeRightPassiveButton.OnMouseOver = new();
        closeRightPassiveButton.OnMouseOver.AddListener((System.Action)(() => closeRightSpriteRenderer.color = new(1f, 0.68f, 0.99f, 1f)));

        Tint = __instance.screenTint.gameObject;
        var ttap = Tint.GetComponent<AspectPosition>();
        if (ttap) Object.Destroy(ttap);
        Tint.transform.SetParent(RightPanel.transform);
        Tint.transform.localPosition = new Vector3(-0.0824f, 0.0513f, Tint.transform.localPosition.z);
        Tint.transform.localScale = new Vector3(1f, 1f, 1f);

        if (!DebugModeManager.AmDebugger)
        {
            __instance.howToPlayButton.gameObject.SetActive(false);
            __instance.howToPlayButton.transform.parent.Find("FreePlayButton").gameObject.SetActive(false);
        }

        var creditsScreen = __instance.creditsScreen;
        if (creditsScreen)
        {
            var csto = creditsScreen.GetComponent<TransitionOpen>();
            if (csto) Object.Destroy(csto);
            var closeButton = creditsScreen.transform.FindChild("CloseButton");
            closeButton?.gameObject.SetActive(false);
        }

        if (!(Sizer = GameObject.Find("Sizer"))) return;
        if (!(AULogo = GameObject.Find("LOGO-AU"))) return;
        Sizer.transform.localPosition += new Vector3(0f, 0.12f, 0f);
        AULogo.transform.localScale = new Vector3(0.66f, 0.67f, 1f);
        AULogo.transform.position += new Vector3(0f, 0.1f, 0f);
        var logoRenderer = AULogo.GetComponent<SpriteRenderer>();
        logoRenderer.sprite = Utils.LoadSprite("TOHE.Resources.Images.TOHE-Logo.png");

        if (!(BottomButtonBounds = GameObject.Find("BottomButtonBounds"))) return;
        BottomButtonBounds.transform.localPosition -= new Vector3(0f, 0.1f, 0f);
    }
}
[HarmonyPatch(typeof(ModManager), nameof(ModManager.LateUpdate))]
internal class ModManagerLateUpdatePatch
{
    public static void Prefix(ModManager __instance)
    {
        __instance.ShowModStamp();

        LateTask.Update(Time.deltaTime);
        CheckMurderPatch.Update();
    }
    public static void Postfix(ModManager __instance)
    {
        var offset_y = HudManager.InstanceExists ? 1.6f : 0.9f;
        __instance.ModStamp.transform.position = AspectPosition.ComputeWorldPosition(
            __instance.localCamera, AspectPosition.EdgeAlignments.RightTop,
            new Vector3(0.4f, offset_y, __instance.localCamera.nearClipPlane + 0.1f));
    }
}
[HarmonyPatch(typeof(CreditsScreenPopUp))]
internal class CreditsScreenPopUpPatch
{
    [HarmonyPatch(nameof(CreditsScreenPopUp.OnEnable))]
    public static void Postfix(CreditsScreenPopUp __instance)
    {
        __instance.BackButton.transform.parent.FindChild("Background").gameObject.SetActive(false);
    }
}
