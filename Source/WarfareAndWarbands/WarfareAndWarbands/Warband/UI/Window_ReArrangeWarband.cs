﻿using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Noise;
using Verse.Sound;
using WarfareAndWarbands.Warband.UI;
using WarfareAndWarbands.Warband.UI.WarbandConfigureSteps;

namespace WarfareAndWarbands.Warband
{
    public class Window_ReArrangeWarband : Window
    {
        private Vector2 scrollPosition;
        private Warband warband;
        private readonly float descriptionHeight = 70f;
        private readonly float descriptionWidth = 120f;
        private readonly float entryHeight = 20f;
        private readonly float entryWidth = 20f;
        private readonly int pawnKindsEachRow = 6;
        private int step = 0;

        public Window_ReArrangeWarband(Warband warband)
        {
            WarbandUtil.RefreshSoldierPawnKinds();
            this.warband = warband;
            for (int i = 0; i < GameComponent_WAW.playerWarband.bandMembers.Count; i++)
            {
                var ele = GameComponent_WAW.playerWarband.bandMembers.ElementAt(i);
                GameComponent_WAW.playerWarband.bandMembers[ele.Key] = 0;
                if (warband.bandMembers.ContainsKey(ele.Key))
                {
                    GameComponent_WAW.playerWarband.bandMembers[ele.Key] = warband.bandMembers[ele.Key];
                }
            }
            GameComponent_WAW.playerWarband.pawnFactionType = warband.PawnKindFactionType;
            GameComponent_WAW.playerWarband.colorOverride = warband.playerWarbandManager.colorOverride.GetColorOverride();
            StepTwo.currentIndex = 0;
            if (warband.PawnKindFactionType != null)
            {
                var faction= Find.FactionManager.AllFactions.FirstOrDefault(x =>
                x.def.humanlikeFaction
                && !x.Hidden 
                && x.def.defName == warband.PawnKindFactionType.defName
                && !x.def.factionIconPath.NullOrEmpty()
                && !x.Hidden);
                if(faction != null)
                {
                    StepTwo.SetFaction(faction.def);
                }
            }


        }
        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(800f, 500f);
            }
        }
        protected override void SetInitialSizeAndPosition()
        {
            base.SetInitialSizeAndPosition();

        }
        public override void DoWindowContents(Rect inRect)
        {
            //Basics
            if (this.warband == null)
            {
                this.Close();
            }
            WarbandUI.DrawExitButton(this, inRect);
            DoArrangeWindow(inRect);

        }

        void DoArrangeWindow(Rect inRect)
        {

            WarbandUI.DrawExitButton(this, inRect);
            if (step <= 0)
            {
                StepOne.Draw(inRect);
            }
            else if (step == 1)
            {
                StepTwo.Draw(inRect);
            }
            else
            {
                StepThree.Draw(inRect, ref scrollPosition, pawnKindsEachRow, descriptionHeight, descriptionWidth, entryWidth, entryHeight, warband);
                DrawExtraCost(inRect);
                DrawRecruitButton(inRect);
            }
            WarbandUI.DrawNextStepButton(inRect, ref step);
        }

        void DrawExtraCost(Rect inRect)
        {
            Rect costRect = new Rect(30, inRect.y, 200, 50);
            string costLabel = "WAW.Cost".Translate(GameComponent_WAW.playerWarband.GetCostExtra(warband.bandMembers, warband.playerWarbandManager.NewRecruitCostMultiplier).ToString());
            Widgets.Label(costRect, costLabel + $"(-{(1 - warband.playerWarbandManager.NewRecruitCostMultiplier) * 100}%)");

        }

        void DrawRecruitButton(Rect inRect)
        {
            bool doRecruit = Widgets.ButtonText(new Rect(inRect.x + inRect.width / 2 - 100, 350, 200, 50), "WAW.ConfigWarband".Translate());
            if (doRecruit)
            {
                this.Close();
                GameComponent_WAW.playerWarband.SetNewWarBandMembers(warband);

            }
        }




     
    }
}
