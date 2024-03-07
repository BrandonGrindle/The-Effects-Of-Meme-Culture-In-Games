using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class NPCBehavior : MonoBehaviour
{
    public Items item;
    public Collider MainCol;
    public Animator animator;


    public void pickupItem()
    {
        InventoryManager.Instance.AddItem(item);
        Destroy(gameObject);
    }

    public void Captured(GameObject BOBBLE)
    {
        //MainCol.enabled = false;
        animator.enabled = false;       
        
    }
}
