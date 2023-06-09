using System.IO;
using MelonLoader;
using MuseDashMirror.CommonPatches;
using Tomlet;
using UnityEngine;
using static MuseDashMirror.BattleComponent;
using static MuseDashMirror.PlayerData;
using static ChartReview.Save;

namespace ChartReview;

internal class Main : MelonMod
{
    private static int LastOffset { get; set; }

    private static int LastCharacter { get; set; } = -1;

    private static int LastElfin { get; set; } = -1;

    public override void OnInitializeMelon()
    {
        Load();
        LastCharacter = Save.Data.LastCharacter;
        LastElfin = Save.Data.LastElfin;
        LastOffset = Save.Data.LastOffset;
        PatchEvents.PnlMenuEvent += Patch.PnlMenuPostfix;
        PatchEvents.SwitchLanguagesEvent += Patch.SwitchLanguagesPostfix;
        PatchEvents.MenuSelectEvent += DisableToggle;
        GameStartEvent += DisableUI;
        MelonLogger.Msg("Chart Review is loaded!");
    }

    public override void OnDeinitializeMelon()
    {
        Save.Data.LastCharacter = SelectedCharacterIndex == 2 ? LastCharacter : SelectedCharacterIndex;
        Save.Data.LastElfin = SelectedElfinIndex == -1 ? SelectedElfinIndex : LastElfin;
        Save.Data.LastOffset = Offset == 0 ? LastOffset : Offset;
        File.WriteAllText(Path.Combine("UserData", "ChartReview.cfg"), TomletMain.TomlStringFrom(Save.Data));
    }

    internal static void ChangeSettings()
    {
        if (Save.Data.ChartReviewEnabled)
        {
            LastCharacter = SelectedCharacterIndex;
            LastElfin = SelectedElfinIndex;
            LastOffset = Offset;

            SetCharacter(2);
            SetElfin(-1);
            SetOffset(0);
            SetAutoFever(false);
        }
        else
        {
            SetCharacter(LastCharacter);
            SetElfin(LastElfin);
            SetOffset(LastOffset);
            SetAutoFever(true);
        }
    }

    private static void DisableUI()
    {
        // if is in game scene and objects are not disabled
        if (Save.Data.ChartReviewEnabled)
        {
            GameObject.Find("Below").SetActive(false);
            GameObject.Find("Score").SetActive(false);
            GameObject.Find("HitPointRoad").SetActive(false);
            GameObject.Find("HitPointAir").SetActive(false);
        }
    }

    private static void DisableToggle(int listIndex, int index, bool isOn)
    {
        if (listIndex == 0 && index == 0 && isOn)
            Patch.ChartReviewToggle.SetActive(true);
        else
            Patch.ChartReviewToggle.SetActive(false);
    }
}