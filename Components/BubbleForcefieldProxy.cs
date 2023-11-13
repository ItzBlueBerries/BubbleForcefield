using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleForcefield.Components
{
    internal class BubbleForcefieldProxy : SRBehaviour
    {
        internal BubbleForcefieldController bubbleForcefieldController;
        internal BubbleTarrBounceBack bubbleTarrBounceBack;

        void Start()
        {
            if (!bubbleForcefieldController)
                bubbleForcefieldController = GetComponentInChildren<BubbleForcefieldController>();
            if (!bubbleTarrBounceBack)
                bubbleTarrBounceBack = GetComponent<BubbleTarrBounceBack>();
        }

        void Update()
        {
            if (bubbleForcefieldController)
            {
                bubbleForcefieldController.Update();
                if (bubbleTarrBounceBack)
                {
                    if (bubbleForcefieldController.IsVisible() && !bubbleTarrBounceBack.enabled)
                        bubbleTarrBounceBack.enabled = true;
                    else if (!bubbleForcefieldController.IsVisible() && bubbleTarrBounceBack.enabled)
                        bubbleTarrBounceBack.enabled = false;
                }
            }
        }
    }
}
