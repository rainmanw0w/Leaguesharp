﻿namespace ReformedAIO.Champions.Lucian.OrbwalkingMode.Combo
{
    using System;
    using System.Linq;

    using LeagueSharp;
    using LeagueSharp.Common;

    using ReformedAIO.Champions.Lucian.Core.Spells;
   
    using RethoughtLib.FeatureSystem.Abstract_Classes;
    using RethoughtLib.FeatureSystem.Implementations;

    internal sealed class QCombo : OrbwalkingChild
    {
        public override string Name { get; set; } = "Q";

        private readonly QSpell qSpell;

        private readonly Q2Spell q2Spell;

        public QCombo(QSpell qSpell, Q2Spell q2Spell)
        {
            this.qSpell = qSpell;
            this.q2Spell = q2Spell;
        }

        private void OnUpdate(EventArgs args)
        {
            if (!CheckGuardians())
            {
                return;
            }

            var target = TargetSelector.GetTarget(q2Spell.Spell.Range, TargetSelector.DamageType.Physical);

            if (target == null)
            {
                return;
            }

            if (Menu.Item("ExtendedQ").GetValue<bool>() && target.Distance(ObjectManager.Player) > qSpell.Spell.Range && q2Spell.QMinionExtend())
            {
                var m = MinionManager.GetMinions(qSpell.Spell.Range).FirstOrDefault();

                qSpell.Spell.CastOnUnit(m);
            }
        }

        private void OnDoCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe 
                || ObjectManager.Player.HasBuff("LucianPassiveBuff")
                || !Orbwalking.IsAutoAttack(args.SData.Name) 
                || !qSpell.Spell.IsReady() 
                || Menu.Item("QMana").GetValue<Slider>().Value > ObjectManager.Player.ManaPercent
                || !CheckGuardians())
            {
                return;
            }

            var heroes = HeroManager.Enemies.Where(x => x.IsValidTarget(q2Spell.Spell.Range));

            foreach (var target in heroes as Obj_AI_Hero[] ?? heroes.ToArray())
            {
                qSpell.Spell.CastOnUnit(target);
            }
        }

        protected override void OnLoad(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            base.OnLoad(sender, featureBaseEventArgs);

            Menu.AddItem(new MenuItem("ExtendedQ", "Extended Q").SetValue(true));
            Menu.AddItem(new MenuItem("QMana", "Min Mana %").SetValue(new Slider(5, 0, 100)));
        }

        protected override void OnDisable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate -= OnUpdate;
            Obj_AI_Base.OnDoCast -= OnDoCast;
        }

        protected override void OnEnable(object sender, FeatureBaseEventArgs featureBaseEventArgs)
        {
            Game.OnUpdate += OnUpdate;
            Obj_AI_Base.OnDoCast += OnDoCast;
        }
    }
}
