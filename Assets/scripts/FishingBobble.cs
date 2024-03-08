using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingBobble : MonoBehaviour
{
    public List<NPCBehavior> CaughtNCPS = null;
    public NPCBehavior NPCControl = null;
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("literally anything");
        if (other.CompareTag("NPC"))
        {
            Debug.Log("Triger Entered");
            NPCControl = other.GetComponentInParent<NPCBehavior>();
            if (NPCControl != null && NPCControl.captured == false)
            {
                CaughtNCPS.Add(NPCControl);
                NPCControl.Captured(this.gameObject);
                FixedJoint joint = gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = other.GetComponent<Rigidbody>();
            }
        }
    }
}
