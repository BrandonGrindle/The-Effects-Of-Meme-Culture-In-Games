using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingBobble : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("literally anything");
        if (other.CompareTag("NPC"))
        {
            NPCBehavior NPCControl = other.GetComponentInParent<NPCBehavior>();
            if (NPCControl != null)
            {
                NPCControl.Captured(this.gameObject);
                FixedJoint joint = gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = other.GetComponent<Rigidbody>();
            }
        }
    }
}
