using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleForcefield
{
    internal class HarmonyPatches
    {
        internal static string bubbleForcefieldPath = "Appearance/fashionBubbleForcefieldAttach(Clone)";

        [HarmonyPatch(typeof(Vacuumable), "UpdateLayer")]
        internal class VacuumableUpdateLayerPatch
        {
            public static bool Prefix(Vacuumable __instance)
            {
                var identifiable = __instance.gameObject.GetComponent<Identifiable>();
                if (!identifiable || !Identifiable.IsSlime(identifiable.id))
                    return true;

                var bubbleForcefield = __instance.gameObject.transform.Find(bubbleForcefieldPath) ?? null;
                if (bubbleForcefield && bubbleForcefield.gameObject.activeSelf)
                    return false;

                return true;
            }
        }

        [HarmonyPatch(typeof(TentacleGrapple), "Action")]
        internal class TentacleGrappleActionPatch
        {
            public static bool Prefix(TentacleGrapple __instance)
            {
                var identifiable = __instance.gameObject.GetComponent<Identifiable>();
                if (!identifiable || !Identifiable.IsSlime(identifiable.id))
                    return true;

                if (__instance.target && __instance.IsGrappling(__instance.target))
                {
                    var bubbleForcefield = __instance.target.transform.Find(bubbleForcefieldPath) ?? null;
                    if (bubbleForcefield && bubbleForcefield.gameObject.activeSelf && bubbleForcefield.localScale.x >= 2)
                    {
                        __instance.Release();
                        return false;
                    } 
                }

                return true;
            }
        }

        [HarmonyPatch(typeof(SlimeEat), "EatAndProduce")]
        internal class SlimeEatEatAndProducePatch
        {
            public static bool Prefix(SlimeEat __instance, ref GameObject target)
            {
                var slimeDefinition = __instance.slimeDefinition;
                var targetIdentifiable = target.GetComponent<Identifiable>();

                if (slimeDefinition && targetIdentifiable)
                {
                    if (!Identifiable.IsTarr(slimeDefinition.IdentifiableId))
                        return true;

                    if (!Identifiable.IsSlime(targetIdentifiable.id))
                        return true;

                    var bubbleForcefield = target.transform.Find(bubbleForcefieldPath) ?? null;
                    if (bubbleForcefield && bubbleForcefield.gameObject.activeSelf && bubbleForcefield.localScale.x >= 2)
                        return false;
                }

                return true;
            }
        }
    }
}
