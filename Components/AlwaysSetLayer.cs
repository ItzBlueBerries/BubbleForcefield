using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleForcefield.Components
{
    internal class AlwaysSetLayer : SRBehaviour
    {
        public int layer;

        void Update()
        {
            if (gameObject.layer != layer)
            {
                gameObject.layer = layer;
                // Debug.Log("Changed layer back to default");
            }
        }
    }
}
