using Assets.Scripts.UI.Panels;
using MelonLoader;
using MuseDashMirror.CommonPatches;
using MuseDashMirror.UICreate;
using System;
using System.IO;
using Tomlet;
using UnityEngine;
using static MuseDashMirror.BattleComponent;
using static MuseDashMirror.PlayerData;

namespace ChartReview
{
    internal class Main : MelonMod
    {
        private static int LastOffset { get; set; }

        private static int LastCharacter { get; set; } = -1;

        private static int LastElfin { get; set; } = -1;

        public override void OnInitializeMelon()
        {
            Save.Load();
            PatchEvents.PnlMenuEvent += new Action<PnlMenu>(Patch.PnlMenuPostfix);
            PatchEvents.SwitchLanguagesEvent += new Action(Patch.SwitchLanguagesPostfix);
            PatchEvents.MenuSelectEvent += new Action<int, int, bool>(DisableToggle);
            GameStartEvent += new Action(DisableUI);
            ToggleCreate.OnToggleClick += new Action(ChangeSettings);
            MelonLogger.Msg("Chart Review is loaded!");
        }

        public override void OnDeinitializeMelon()
        {
            File.WriteAllText(Path.Combine("UserData", "ChartReview.cfg"), TomletMain.TomlStringFrom(Save.data));
        }

        private void ChangeSettings()
        {
            if (Save.data.ChartReviewEnabled)
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
            if (Save.data.ChartReviewEnabled)
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
            {
                Patch.ChartReviewToggle.SetActive(true);
            }
            else
            {
                Patch.ChartReviewToggle.SetActive(false);
            }
        }
    }
}