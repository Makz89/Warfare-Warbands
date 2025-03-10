﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using WarfareAndWarbands.Warband;

namespace WAWLeadership.UI
{
    public class Window_Leadership:Window
    {
        private readonly CompLeadership leader;
        private Warband warband;
        private Vector2 scrollPosition;

        public override Vector2 InitialSize => new Vector2(630f, 630f);

        public Window_Leadership(CompLeadership leader, Warband warband)
        {
            this.leader = leader;   
            this.warband = warband; 
        }

        public override void DoWindowContents(Rect inRect)
        {
            // Some stupid checks
            if (leader == null ||
                leader.Leadership == null ||
                leader.Leadership.AttributeSet == null ||
                leader.Leadership.AttributeSet.Attributes == null)
            {
                Log.Message($"Status: " +
                    $"{leader.Leadership == null}" +
                    $"{leader.Leadership.AttributeSet == null}" +
                    $"{leader.Leadership.AttributeSet.Attributes == null}");
                this.Close();
            }


            // Draw Pawn Portrait
            GUI.color = Color.white;
            Rect portraitRect = new Rect(Vector2.zero, new Vector2(InitialSize.x / 2, 400));
            Widgets.ThingIcon(portraitRect, leader.Pawn, 1f, null, false);
            Widgets.Label(portraitRect, "WAW.LeadershipTitle".Translate());
            // Draw Attribute Panel
            Rect otherHalf = new Rect(new Vector2(portraitRect.xMax + 100, 100), new Vector2(InitialSize.x / 2, InitialSize.y));
            Rect levelLabelRect = new Rect(portraitRect.xMax + 50, 400, 200, 30);
            Rect pointsRect = new Rect(levelLabelRect.x, levelLabelRect.yMax, 200, 50);
            Rect barRect = new Rect(levelLabelRect.x, pointsRect.yMax, 200, 30);
            Rect toggleRect = new Rect(levelLabelRect.x, barRect.yMax + 30, 200, 60);
            Rect buffsRect = new Rect(portraitRect.x, 400, portraitRect.width, 600 - 400);

            var attributeSet = leader.Leadership.AttributeSet;
            var exp = leader.Leadership.Exp;
            LeadershipUI.DrawHexagon(otherHalf, 100, out List<Vector2> points, out Vector2 center);
            LeadershipUI.DrawCurrentAttributes(points, center, leader);
            LeadershipUI.DrawLeadershipAttributes(points, attributeSet, leader);
            LeadershipUI.DrawPoints(pointsRect, attributeSet);
            LeadershipUI.DrawLevel(levelLabelRect, exp);
            LeadershipUI.DrawExpBar(barRect, exp);
            LeadershipUI.DrawBuffs(buffsRect, leader, ref scrollPosition);
            LeadershipUI.DrawToggleSpawnLeader(toggleRect, ref warband);

        }
    }
}
