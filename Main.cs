using System.IO;
using MelonLoader;
using MuseDashMirror.CommonPatches;
using MuseDashMirror.UICreate;
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
        LastCharacter = data.LastCharacter;
        LastElfin = data.LastElfin;
        LastOffset = data.LastOffset;
        PatchEvents.PnlMenuEvent += Patch.PnlMenuPostfix;
        PatchEvents.SwitchLanguagesEvent += Patch.SwitchLanguagesPostfix;
        PatchEvents.MenuSelectEvent += DisableToggle;
        GameStartEvent += DisableUI;
        ToggleCreate.OnToggleClick += ChangeSettings;
        MelonLogger.Msg("Chart Review is loaded!");
    }

    public override void OnDeinitializeMelon()
    {
        data.LastCharacter = LastCharacter;
        data.LastElfin = LastElfin;
        data.LastOffset = LastOffset;
        File.WriteAllText(Path.Combine("UserData", "ChartReview.cfg"), TomletMain.TomlStringFrom(data));
    }

    private void ChangeSettings()
    {
        if (data.ChartReviewEnabled)
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

    private void DisableUI()
    {
        // if is in game scene and objects are not disabled
        if (data.ChartReviewEnabled)
        {
            GameObject.Find("Below").SetActive(false);
            GameObject.Find("Score").SetActive(false);
            GameObject.Find("HitPointRoad").SetActive(false);
            GameObject.Find("HitPointAir").SetActive(false);
        }
    }

    private void DisableToggle(int listIndex, int index, bool isOn)
    {
        if (listIndex == 0 && index == 0 && isOn)
            Patch.ChartReviewToggle.SetActive(true);
        else
            Patch.ChartReviewToggle.SetActive(false);
    }
}