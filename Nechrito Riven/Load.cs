﻿namespace NechritoRiven
{
    #region

    using System;

    using Core;

    using Draw;

    using Event;
    using Event.OrbwalkingModes;

    using LeagueSharp;
    using LeagueSharp.Common;

    using Menus;

    #endregion

    internal class Load
    {
        #region Public Methods and Operators

        public static void LoadAssembly()
        {
            MenuConfig.LoadMenu();
            Spells.Load();

            Obj_AI_Base.OnDoCast += AfterAuto.OnDoCast;
            Obj_AI_Base.OnProcessSpellCast += ProcessSpell.OnProcessSpell;
            Obj_AI_Base.OnProcessSpellCast += Core.Core.OnCast;
            Obj_AI_Base.OnPlayAnimation += Animation.OnPlay;

            Drawing.OnEndScene += DrawDmg.DmgDraw;
            Drawing.OnDraw += DrawRange.RangeDraw;
            Drawing.OnDraw += DrawWallSpot.WallDraw;

            Game.OnUpdate += KillSteal.Update;
            Game.OnUpdate += PermaActive.Update;
            Game.OnUpdate += Skinchanger.Update;

            Interrupter2.OnInterruptableTarget += Interrupt2.OnInterruptableTarget;
            AntiGapcloser.OnEnemyGapcloser += Gapclose.Gapcloser;

            Game.PrintChat("<b><font color=\"#FFFFFF\">[</font></b><b><font color=\"#00e5e5\">Nechrito Riven</font></b><b><font color=\"#FFFFFF\">]</font></b><b><font color=\"#FFFFFF\"> Loaded!</font></b>");
            Console.WriteLine("Nechrito Riven: Loaded");
        }

        #endregion
    }
}