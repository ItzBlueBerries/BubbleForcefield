global using UnityEngine;
global using static Utility;
global using SRML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleForcefield
{
    public class BubbleEntry : ModEntryPoint
    {
        public override void PreLoad() => HarmonyInstance.PatchAll();

        public override void Load()
        {
            Data.Fashions.BubbleForcefield.Load();
            Data.Treasures.BubbleForcefieldTreasure.Load();
        }
    }
}
