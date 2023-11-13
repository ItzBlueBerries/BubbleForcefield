using SRML.SR;
using SRML.SR.SaveSystem;
using SRML.Utils;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleForcefield.Data.Treasures
{
    internal class BubbleForcefieldTreasure
    {
        internal static GameObject bubbleForcefieldTreasure;

        public static void Load()
        {
            SRCallbacks.PreSaveGameLoad += delegate (SceneContext sceneContext)
            {
                if (!bubbleForcefieldTreasure)
                {
                    GameObject treasurePodRank3 = GameObject.Find("zoneDESERT/cellDesert_TempleReceiver/Sector/Loot/treasurePod Rank3").gameObject;
                    bubbleForcefieldTreasure = GameObjectUtils.InstantiateInactive(
                        treasurePodRank3,
                        new Vector3(39.3f, 1003.96f, -185.3f),
                        Quaternion.Euler(0, 180, 0),
                        GameObject.Find("zoneDESERT/cellDesert_HobsonEndTemple/Sector/Loot").transform, true);
                    bubbleForcefieldTreasure.transform.RotateAround(bubbleForcefieldTreasure.transform.position, bubbleForcefieldTreasure.transform.up, 160);

                    TreasurePod treasurePod = bubbleForcefieldTreasure.GetComponent<TreasurePod>();
                    treasurePod.blueprint = Enums.FASHION_POD_BUBBLE_FORCEFIELD;
                    treasurePod.director = bubbleForcefieldTreasure.GetComponentInParent<IdDirector>();
                    treasurePod.director.persistenceDict.Add(bubbleForcefieldTreasure.GetComponent<TreasurePod>(), ModdedStringRegistry.ClaimID("pod", "treasureBubbleForcefield"));

                    bubbleForcefieldTreasure.SetActive(true);
                }
            };
        }
    }
}
