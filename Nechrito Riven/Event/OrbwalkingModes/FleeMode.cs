﻿namespace NechritoRiven.Event.OrbwalkingModes
{
    #region

    using System.Linq;

    using Core;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Menus;

    #endregion

    internal class FleeMode : Core
    {
        #region Public Methods and Operators

        public static void Flee()
        {
            if (MenuConfig.WallFlee)
            {
                var end = Player.ServerPosition.Extend(Game.CursorPos, 350);
                var isWallDash = FleeLogic.IsWallDash(end, 350);

                var eend = Player.ServerPosition.Extend(Game.CursorPos, Spells.E.Range);
                var wallE = FleeLogic.GetFirstWallPoint(Player.ServerPosition, eend);
                var wallPoint = FleeLogic.GetFirstWallPoint(Player.ServerPosition, end);

                Player.GetPath(wallPoint);

                if (Spells.Q.IsReady() && Qstack < 3)
                {
                    Spells.Q.Cast(Game.CursorPos);
                }

                if (isWallDash && Qstack == 3 && wallPoint.Distance(Player.ServerPosition) <= 800 && wallPoint.Distance(Player.Position) > 150)
                {
                    ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, wallPoint);
                }

                if (Spells.E.IsReady() && wallPoint.Distance(Player.ServerPosition) <= 150)
                {
                    Spells.E.Cast(wallE);
                }

                if (!(wallPoint.Distance(Player.ServerPosition) <= 55)) return;

                if (Qstack != 3 || !wallPoint.IsValid()) return;

                Player.IssueOrder(GameObjectOrder.MoveTo, wallPoint);

                Spells.Q.Cast(wallE);
            }
            else
            {
                var enemy =
                    HeroManager.Enemies.Where(
                        target =>
                        target.IsValidTarget(
                            Player.HasBuff("RivenFengShuiEngine")
                                ? 70 + 195 + Player.BoundingRadius
                                : 70 + 120 + Player.BoundingRadius) && Spells.W.IsReady());

                var x = Player.Position.Extend(Game.CursorPos, 300);

                var targets = enemy as Obj_AI_Hero[] ?? enemy.ToArray();

                if (Spells.W.IsReady() && targets.Any()) foreach (var target in targets) if (InWRange(target)) Spells.W.Cast();

                if (Spells.Q.IsReady() && !Player.IsDashing()) Spells.Q.Cast(Game.CursorPos);

                if (MenuConfig.FleeYomuu)
                {
                    Usables.CastYoumoo();
                }

                if (Spells.E.IsReady() && !Player.IsDashing()) Spells.E.Cast(x);
            }
        }

        #endregion
    }
}
