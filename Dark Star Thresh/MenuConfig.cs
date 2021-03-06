﻿namespace Dark_Star_Thresh
{
    using LeagueSharp.Common;

    internal class MenuConfig : Core.Core
    {
        public static Menu Config;
        public static Menu TargetSelectorMenu;
        public static string MenuName = "Dark Star Thresh";

        public static void LoadMenu()
        {
            Config = new Menu(MenuName, MenuName, true);
            
            TargetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(TargetSelectorMenu);
            Config.AddSubMenu(TargetSelectorMenu);

            var orbwalker = new Menu("Orbwalker", "rorb");
            Orbwalker = new Orbwalking.Orbwalker(orbwalker);
            Config.AddSubMenu(orbwalker);
            

            var combo = new Menu("Combo", "Combo");
            combo.AddItem(new MenuItem("ComboFlash", "Flash Combo").SetValue(new KeyBind('T', KeyBindType.Press))).SetTooltip("Does Flash Combo");
            combo.AddItem(new MenuItem("ComboR", "Min Enemies For R").SetValue(new Slider(3, 0, 5)));
            combo.AddItem(new MenuItem("ComboQ", "Max Q Range").SetValue(new Slider(110, 0, 110)));
            combo.AddItem(new MenuItem("ComboTaxi", "Taxi Mode (Beta!)").SetValue(true).SetTooltip("Will Cast Q To Minions, Logic implented ofc."));
            Config.AddSubMenu(combo);

            var harass = new Menu("Harass", "Harass");
            harass.AddItem(new MenuItem("HarassAA", "Disable AA In Harass").SetValue(false).SetTooltip("Wont Use AA"));
            harass.AddItem(new MenuItem("HarassQ", "Use Q").SetValue(true).SetTooltip("Wont cast Q2"));
            harass.AddItem(new MenuItem("HarassE", "Use E").SetValue(true).SetTooltip("Throws the target away from you"));
            Config.AddSubMenu(harass);

            var misc = new Menu("Misc", "Misc");
            misc.AddItem(new MenuItem("Interrupt", "Interrupter").SetValue(true));
            misc.AddItem(new MenuItem("Gapcloser", "Gapcloser").SetValue(true));
            misc.AddItem(new MenuItem("UseSkin", "Use Skinchanger").SetValue(false));
            misc.AddItem(new MenuItem("Skin", "Skin").SetValue(new StringList(new[] { "Default", "Deep Terror Thresh", "Championship Thresh", "Blood Moon Thresh", "SSW Thresh", "Dark Star Thresh" })));
            misc.AddItem(new MenuItem("Flee", "Flee").SetValue(new KeyBind('A', KeyBindType.Press))).SetTooltip("Flee To Minion / Mobs");
            Config.AddSubMenu(misc);


            var draw = new Menu("Draw", "Draw");
            draw.AddItem(new MenuItem("DrawDmg", "Draw Damage").SetValue(true).SetTooltip("Somewhat Fps Heavy, Be Careful"));
            draw.AddItem(new MenuItem("DrawPred", "Draw Q Prediction").SetValue(true));
            draw.AddItem(new MenuItem("DrawQ", "Draw Q Range").SetValue(true));
            draw.AddItem(new MenuItem("DrawW", "Draw W Range").SetValue(true));
            draw.AddItem(new MenuItem("DrawE", "Draw E Range").SetValue(true));
            draw.AddItem(new MenuItem("DrawR", "Draw R Range").SetValue(true));
            Config.AddSubMenu(draw);

            Config.AddItem(new MenuItem("Debug", "Debug Mode").SetValue(false).SetTooltip("Prints In Chat What's Going On"));

            Config.AddToMainMenu();
        }



        // Keybind
        public static bool ComboFlash => Config.Item("ComboFlash").GetValue<KeyBind>().Active;
        public static bool Flee => Config.Item("Flee").GetValue<KeyBind>().Active;

        // Slider
        public static int ComboR => Config.Item("ComboR").GetValue<Slider>().Value;
        public static int ComboQ => Config.Item("ComboQ").GetValue<Slider>().Value;

        // Bool
        public static bool ComboTaxi => Config.Item("ComboTaxi").GetValue<bool>();

        public static bool HarassAa => Config.Item("HarassAA").GetValue<bool>();
        public static bool HarassQ => Config.Item("HarassQ").GetValue<bool>();
        public static bool HarassE => Config.Item("HarassE").GetValue<bool>();

        public static bool Interrupt => Config.Item("Interrupt").GetValue<bool>();
        public static bool Gapcloser => Config.Item("Gapcloser").GetValue<bool>();

        public static bool UseSkin => Config.Item("UseSkin").GetValue<bool>();

        public static bool DrawDmg => Config.Item("DrawDmg").GetValue<bool>();
        public static bool DrawPred => Config.Item("DrawPred").GetValue<bool>();
        public static bool DrawQ => Config.Item("DrawQ").GetValue<bool>();
        public static bool DrawW => Config.Item("DrawW").GetValue<bool>();
        public static bool DrawE => Config.Item("DrawE").GetValue<bool>();
        public static bool DrawR => Config.Item("DrawR").GetValue<bool>();

        public static bool Debug => Config.Item("Debug").GetValue<bool>();
    }
}
