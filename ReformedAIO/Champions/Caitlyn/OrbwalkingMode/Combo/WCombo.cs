﻿namespace ReformedAIO.Champions.Caitlyn.OrbwalkingMode.Combo
{
    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Caitlyn.Logic;

    using RethoughtLib.FeatureSystem.Abstract_Classes;

    internal sealed class WCombo : ChildBase
    {
        private readonly Orbwalking.Orbwalker orbwalker;

        public WCombo(Orbwalking.Orbwalker orbwalker)
        {
            this.orbwalker = orbwalker;
        }


        public override string Name { get; set; }

        private Obj_AI_Hero Target => TargetSelector.GetTarget(Spells.Spell[SpellSlot.W].Range, TargetSelector.DamageType.Physical);

        private bool ewq;

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Obj_AI_Base.OnProcessSpellCast -= OnProcessSpellCast;
            AntiGapcloser.OnEnemyGapcloser -= Gapcloser;
            Game.OnUpdate -= OnUpdate;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Obj_AI_Base.OnProcessSpellCast += OnProcessSpellCast;
            AntiGapcloser.OnEnemyGapcloser += Gapcloser;
            Game.OnUpdate += OnUpdate;
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Menu.AddItem(new MenuItem("WMana", "Mana %").SetValue(new Slider(30, 0, 100)));

            Menu.AddItem(new MenuItem("AntiGapcloser", "Anti-Gapcloser").SetValue(true));

            Menu.AddItem(new MenuItem("WTarget", "W Behind Target").SetValue(true));

            Menu.AddItem(new MenuItem("WImmobile", "W On Immobile").SetValue(true));

            Menu.AddItem(new MenuItem("WBush", "Auto W On Bush").SetValue(false));
        }

        private void OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if(!sender.IsMe || !Spells.Spell[SpellSlot.W].IsReady()) return;
          
            this.ewq = args.SData.Name == "CaitlynPiltoverPeacemaker";
        }

        private void Gapcloser(ActiveGapcloser gapcloser)
        {
            if (!Menu.Item("AntiGapcloser").GetValue<bool>()) return;

            var target = gapcloser.Sender;

            if (target == null) return;

            if (!target.IsEnemy || !Spells.Spell[SpellSlot.W].IsReady()) return;

            Spells.Spell[SpellSlot.W].Cast(gapcloser.End);
        }

      
        private void OnUpdate(EventArgs args)
        {
            if (this.orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.None
                && Menu.Item("WBush").GetValue<bool>()
                && Utils.TickCount - Spells.Spell[SpellSlot.W].LastCastAttemptT > 10000
                && !Vars.Player.IsRecalling()
                && !Vars.Player.IsWindingUp)
            {
                // Beta
                if (Vars.Player.Spellbook.GetSpell(SpellSlot.W).Ammo < 2) return;

                var path = Vars.Player.GetWaypoints().LastOrDefault().To3D();

                if (!NavMesh.IsWallOfGrass(path, 0)) return;

                Utility.DelayAction.Add(400, ()=> Spells.Spell[SpellSlot.W].Cast(path));
            }

            if (this.orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo
                || Vars.Player.IsWindingUp
                || !Spells.Spell[SpellSlot.W].IsReady()
                || Target == null
                || Menu.Item("WMana").GetValue<Slider>().Value > Vars.Player.ManaPercent
                || Utils.TickCount - Spells.Spell[SpellSlot.W].LastCastAttemptT < 5000) return;

            var wPrediction = Spells.Spell[SpellSlot.W].GetPrediction(Target);

            if (Menu.Item("WTarget").GetValue<bool>()) 
            {
                if (this.ewq)
                {
                    Utility.DelayAction.Add(170, ()=> Spells.Spell[SpellSlot.W].Cast(Target.Position));
                }

                if (Target.IsInvulnerable || Target.CountEnemiesInRange(1000) < Target.CountAlliesInRange(1000))
                {
                    Spells.Spell[SpellSlot.W].Cast(wPrediction.CastPosition);
                }
            }

            if (wPrediction.Hitchance < HitChance.Immobile || !Menu.Item("WImmobile").GetValue<bool>()) return;

            Spells.Spell[SpellSlot.W].Cast(wPrediction.CastPosition);
        }
    }
}
