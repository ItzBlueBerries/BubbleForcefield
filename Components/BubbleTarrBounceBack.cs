using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BubbleForcefield.Components
{
    internal class BubbleTarrBounceBack : SRBehaviour
    {
        public float bounceForce = 7;

        void OnCollisionEnter(Collision collision)
        {
            if (!transform.Find("Appearance/fashionBubbleForcefieldAttach(Clone)").gameObject.activeSelf && 
                transform.Find("Appearance/fashionBubbleForcefieldAttach(Clone)").localScale.x < 2)
                return;

            // Object
            GameObject obj = collision.gameObject;
            if (obj == null)
                return;

            // Identifiable
            Identifiable identifiable = obj.GetComponent<Identifiable>();
            if (identifiable == null) 
                return;

            if (!Identifiable.IsTarr(identifiable.id))
                return;
            
            Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
            rigidbody.AddForce((obj.transform.position - transform.position).normalized * bounceForce, ForceMode.Impulse);
            // Debug.Log("Collision with tarr happened");
        }
    }
}
