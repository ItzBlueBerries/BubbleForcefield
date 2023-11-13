using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleForcefield.Components
{
    internal class BubbleForcefieldController : SRBehaviour
    {
        private SECTR_PointSource pointSource;
        private SlimeAppearanceApplicator slimeAppearanceApplicator;
        private SlimeEmotions slimeEmotions;
        private Identifiable identifiable;
        private Vacuumable vacuumable;
        private bool isVisible;
        private bool isSlime;

        public SECTR_AudioCue activationCue;
        public SECTR_AudioCue deactivationCue;

        void Start()
        {
            pointSource = GetComponent<SECTR_PointSource>();
            if (!activationCue)
                activationCue = GetResource<SECTR_AudioCue>("FashionApply");
            if (!deactivationCue)
                deactivationCue = GetResource<SECTR_AudioCue>("FashionDespawn");

            vacuumable = GetComponentInParent<Vacuumable>() ?? null;
            identifiable = GetComponentInParent<Identifiable>() ?? null;
            slimeEmotions = GetComponentInParent<SlimeEmotions>() ?? null;
            slimeAppearanceApplicator = GetComponentInParent<SlimeAppearanceApplicator>() ?? null;

            if (identifiable != null)
                isSlime = Identifiable.IsSlime(identifiable.id);
            if (identifiable == null || !isSlime)
            {
                Destroy(gameObject);
                return;
            }

            identifiable.gameObject.AddComponent<BubbleForcefieldProxy>().bubbleForcefieldController = this;
            identifiable.gameObject.GetComponent<BubbleForcefieldProxy>().bubbleTarrBounceBack = identifiable.gameObject.AddComponent<BubbleTarrBounceBack>();

            transform.parent = identifiable.gameObject.transform.Find("Appearance").transform;
            transform.position = identifiable.gameObject.transform.position;
            AdjustBubbleColor(identifiable.gameObject);

            if (slimeAppearanceApplicator)
                slimeAppearanceApplicator.OnAppearanceChanged += AdjustBubbleColor;

            gameObject.SetActive(false);
        }

        public void Update()
        {
            if (!isSlime)
                Destroy(gameObject);

            if (vacuumable)
                if (vacuumable.isHeld())
                    return;

            if (slimeEmotions)
            {
                if (slimeEmotions.GetCurr(SlimeEmotions.Emotion.FEAR) >= 0.6f && !IsVisible())
                    ActivateBubble();
                else if (slimeEmotions.GetCurr(SlimeEmotions.Emotion.FEAR) < 0.6f && IsVisible())
                    DeactiveBubble();
            }
        }

        void OnDestroy()
        {
            if (identifiable && identifiable.gameObject.GetComponent<BubbleForcefieldProxy>())
                Destroy(identifiable.gameObject.GetComponent<BubbleForcefieldProxy>());

            if (identifiable && identifiable.gameObject.GetComponent<BubbleTarrBounceBack>())
                Destroy(identifiable.gameObject.GetComponent<BubbleTarrBounceBack>());

            if (slimeAppearanceApplicator)
                slimeAppearanceApplicator.OnAppearanceChanged -= AdjustBubbleColor;
        }

        public bool IsVisible() => isVisible;

        void AdjustBubbleColor(SlimeAppearance slimeAppearance) => AdjustBubbleColor(identifiable.gameObject);

        public void DeactiveBubble()
        {
            isVisible = false;
            ShortcutExtensions.DOScale(transform, Vector3.zero, 2).OnComplete(() => PlayDeactivation());
        }

        public void ActivateBubble()
        {
            isVisible = true;
            PlayActivation();
            ShortcutExtensions.DOScale(transform, new Vector3(5, 5, 5), 2);
        }

        IEnumerator SetInactiveOnlyAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            if (!isVisible)
                gameObject.SetActive(false);
            yield break;
        }

        public void PlayActivation(bool setActive = true)
        {
            if (isVisible)
            {
                if (setActive && !gameObject.activeSelf)
                    gameObject.SetActive(true);
                if (pointSource.Cue != activationCue)
                    pointSource.Cue = activationCue;
                pointSource.Play();
            }
        }

        public void PlayDeactivation(bool setActive = true)
        {
            if (!isVisible)
            {
                if (pointSource.Cue != deactivationCue)
                    pointSource.Cue = deactivationCue;
                pointSource.Play();
                if (setActive && gameObject.activeSelf)
                    StartCoroutine(SetInactiveOnlyAfterDelay(1));
            }
        }

        public void AdjustBubbleColor(GameObject gameObject)
        {
            try
            {
                Material bubbleMaterial = Instantiate(GetComponent<MeshRenderer>().material);

                if (isSlime)
                    try
                    {
                        bubbleMaterial.SetColor("_Color", gameObject.GetComponent<SlimeAppearanceApplicator>().Appearance.Structures[0].DefaultMaterials[0].GetColor("_TopColor"));
                    }
                    catch
                    {
                        bubbleMaterial.SetColor("_Color", gameObject.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial.GetColor("_TopColor"));
                    }
                else
                    bubbleMaterial.SetColor("_Color", gameObject.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterial.GetColor("_TopColor"));

                GetComponent<MeshRenderer>().material = bubbleMaterial;
            }
            catch
            {

            }
        }
    }
}
