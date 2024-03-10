using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class FishingBobble : MonoBehaviour
{
    public List<NPCBehavior> CaughtNCPS = new List<NPCBehavior>();
    public NPCBehavior NPCControl = null;
    public FishPoint FishingSpot = null;

    public void Update()
    {
        
    }

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
                NPCControl.Captured();
                FixedJoint joint = gameObject.AddComponent<FixedJoint>();
                joint.connectedBody = other.GetComponent<Rigidbody>();
            }
        }
        else if (other.CompareTag("FishPoint"))
        {
            FishingSpot = other.GetComponent<FishPoint>();
            if (FishingSpot != null)
            {
                Debug.Log("Catching Fish");
                StartCoroutine(FishingSpot.FishBite(this.gameObject));
            }
        }
    }
}
