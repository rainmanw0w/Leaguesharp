﻿using System.Linq;

namespace ReformedAIO.Champions.Gnar.Core
{
    internal class GnarState
    {
        public bool Mini => Vars.Player.CharData.BaseSkinName == "Gnar";
        
        public bool Mega => Vars.Player.CharData.BaseSkinName == "GnarBig";

        public bool TransForming => Vars.Player.Buffs.Any(x => x.DisplayName.Contains("gnartransformsoon"))
            || (Mini && Vars.Player.ManaPercent >= 95)
            || (Mega && Vars.Player.ManaPercent <= 10);   
    }
}