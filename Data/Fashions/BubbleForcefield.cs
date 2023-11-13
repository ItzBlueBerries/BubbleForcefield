using SRML.SR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SRML.Utils;
using UnityEngine.UI;
using BubbleForcefield.Components;
using SRML.SR.Translation;

namespace BubbleForcefield.Data.Fashions
{
    internal class BubbleForcefield
    {
        internal static GameObject bubbleForcefieldFashion;
        internal static GameObject bubbleForcefieldAttach;
        internal static GameObject bubbleForcefieldPod;

        public static void Load()
        {
            #region BUBBLE_FORCEFIELD_FASHION
            // ATTACH PREFAB
            GameObject quicksilverSlowField = Identifiable.Id.VALLEY_AMMO_3.GetPrefab().GetComponent<DestroyAndCreateOnTouching>().prefab;
            bubbleForcefieldAttach = new GameObject("fashionBubbleForcefieldAttach");
            bubbleForcefieldAttach.Prefabitize();

            bubbleForcefieldAttach.AddComponent<MeshFilter>().sharedMesh = GameObject.CreatePrimitive(PrimitiveType.Sphere).GetComponent<MeshFilter>().mesh;
            bubbleForcefieldAttach.AddComponent<MeshRenderer>().material = UnityEngine.Object.Instantiate(quicksilverSlowField.GetComponent<MeshRenderer>().material);
            bubbleForcefieldAttach.AddComponent<SphereCollider>();
            bubbleForcefieldAttach.AddComponent<AlwaysSetLayer>().layer = LayerMask.NameToLayer("Default");
            bubbleForcefieldAttach.AddComponent<BubbleForcefieldController>();

            SECTR_PointSource pointSource = bubbleForcefieldAttach.AddComponent<SECTR_PointSource>();
            pointSource.pitch = 1.2f;
            pointSource.Loop = false;
            pointSource.RestartLoopsOnEnabled = false;

            bubbleForcefieldAttach.layer = LayerMask.NameToLayer("Default");
            bubbleForcefieldAttach.transform.localScale = Vector3.zero;

            // FASHION PREFAB
            Sprite iconFashionBubbleForcefield = LoadTextureFromAssembly("Files.Icons.iconFashionBubbleForcefield").ConvertToSprite();
            bubbleForcefieldFashion = PrefabUtils.CopyPrefab(Identifiable.Id.CLIP_ON_FASHION.GetPrefab());
            bubbleForcefieldFashion.name = "fashionBubbleForcefield";

            bubbleForcefieldFashion.GetComponent<Identifiable>().id = Enums.BUBBLE_FORCEFIELD_FASHION;
            bubbleForcefieldFashion.GetComponent<Fashion>().slot = Fashion.Slot.FRONT;
            bubbleForcefieldFashion.GetComponent<Fashion>().attachPrefab = bubbleForcefieldAttach;
            bubbleForcefieldFashion.GetComponentsInChildren<Image>().ForEach(x => x.sprite = iconFashionBubbleForcefield);

            // THE REST
            Identifiable.FASHION_CLASS.Add(Enums.BUBBLE_FORCEFIELD_FASHION);
            // TranslationPatcher.AddActorTranslation("l.bubble_forcefield_fashion", Enums.BUBBLE_FORCEFIELD_FASHION);
            AmmoRegistry.RegisterAmmoPrefab(PlayerState.AmmoMode.DEFAULT, bubbleForcefieldFashion);
            LookupRegistry.RegisterVacEntry(Enums.BUBBLE_FORCEFIELD_FASHION, LoadHex("f3f3f3"), iconFashionBubbleForcefield);
            LookupRegistry.RegisterIdentifiablePrefab(bubbleForcefieldFashion);
            #endregion

            #region BUBBLE_FORCEFIELD_POD
            // PREFAB
            bubbleForcefieldPod = PrefabUtils.CopyPrefab(Gadget.Id.FASHION_POD_CLIP_ON.GetGadgetDefinition().prefab);
            bubbleForcefieldPod.name = "gadgetFashionPodBubbleForcefield";

            bubbleForcefieldPod.GetComponent<Gadget>().id = Enums.FASHION_POD_BUBBLE_FORCEFIELD;
            bubbleForcefieldPod.GetComponent<FashionPod>().fashionId = Enums.BUBBLE_FORCEFIELD_FASHION;

            // DEFINITION
            GadgetDefinition.CraftCost[] craftCosts = new GadgetDefinition.CraftCost[]
            {
                new GadgetDefinition.CraftCost()
                {
                    id = Identifiable.Id.MOSAIC_PLORT,
                    amount = 35
                },
                new GadgetDefinition.CraftCost()
                {
                    id = Identifiable.Id.PUDDLE_PLORT,
                    amount = 35
                },
                new GadgetDefinition.CraftCost()
                {
                    id = Identifiable.Id.ROCK_PLORT,
                    amount = 35
                },
                new GadgetDefinition.CraftCost()
                {
                    id = Identifiable.Id.DEEP_BRINE_CRAFT,
                    amount = 35
                },
                new GadgetDefinition.CraftCost()
                {
                    id = Identifiable.Id.GLASS_SHARD_CRAFT,
                    amount = 17,
                },
                new GadgetDefinition.CraftCost()
                {
                    id = Identifiable.Id.STRANGE_DIAMOND_CRAFT,
                    amount = 4
                }
            };

            GadgetDefinition fashionPodBubbleForcefield = ScriptableObject.CreateInstance<GadgetDefinition>();
            fashionPodBubbleForcefield.name = "FashionPodBubbleForcefield";
            fashionPodBubbleForcefield.icon = iconFashionBubbleForcefield;

            fashionPodBubbleForcefield.id = Enums.FASHION_POD_BUBBLE_FORCEFIELD;
            fashionPodBubbleForcefield.prefab = bubbleForcefieldPod;
            fashionPodBubbleForcefield.pediaLink = PediaDirector.Id.CURIOS;
            fashionPodBubbleForcefield.craftCosts = craftCosts;
            fashionPodBubbleForcefield.blueprintCost = 11000;

            // THE REST
            Gadget.ALL_FASHIONS.Add(Enums.FASHION_POD_BUBBLE_FORCEFIELD);
            Gadget.FASHION_POD_CLASS.Add(Enums.FASHION_POD_BUBBLE_FORCEFIELD);
            Identifiable.GADGET_NAME_DICT.Add(Enums.BUBBLE_FORCEFIELD_FASHION, Enums.FASHION_POD_BUBBLE_FORCEFIELD);
            LookupRegistry.RegisterGadget(fashionPodBubbleForcefield);
            GadgetTranslationExtensions.GetTranslation(Enums.FASHION_POD_BUBBLE_FORCEFIELD)
                .SetNameTranslation("Bubble Forcefield Fashion Pod")
                .SetDescriptionTranslation("Fashion pods allow you to vac up fashionable accessories for your slimes. Shoot them on to slimes to totally up their game.");
            #endregion
        }
    }
}
