using StarterAssets;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;


public class WeaponController : MonoBehaviour
{
    public static WeaponController instance;

    [Header("Weapon Stats")]
    [SerializeField] private float AttackCooldown = 1f;
    [SerializeField] private float AttackRange = .5f;
    [SerializeField] private GameObject WeaponOrigin;
    [SerializeField] private int weaponDmg = 5;

    public bool canAttack = true;
    private int layerMask;
    public Camera _mainCamera;
    private void Awake()
    {
        instance = this;
        layerMask = ~LayerMask.GetMask("Player");
    }
    //public void Attack()
    //{
    //    //Debug.Log("Attack attempt");
    //    canAttack = false;
    //    Ray raycast = new Ray(WeaponOrigin.transform.position, WeaponOrigin.transform.forward);
    //    if (Physics.Raycast(raycast, out RaycastHit hit, AttackRange, layerMask))
    //    {
    //        Debug.DrawLine(WeaponOrigin.transform.position, hit.point, Color.red, 10.0f);
    //        Debug.Log(hit.collider.tag);
    //        if (hit.collider.CompareTag("Enemy"))
    //        {
    //            EnemyAI EnemyScript = hit.collider.gameObject.GetComponentInParent<EnemyAI>();
    //            if (EnemyScript != null)
    //            {
    //                EnemyScript.DamageTaken(weaponDmg);
    //            }
    //            else
    //            {
    //                Debug.Log("no enemy script found");
    //            }
    //        }
    //    }
    //    StartCoroutine(Cooldown());
    //}


    public void Attack(Animator anim, int AnimID)
    {
        //Debug.Log("Attack attempt");
        canAttack = false;

        // Use the camera's forward direction for the attack direction
        Vector3 attackDirection = _mainCamera.transform.forward;
        Vector3 origin = WeaponOrigin.transform.position;

        // Check if the attack direction is not backward through the player by ensuring the dot product is not negative
        if (Vector3.Dot(WeaponOrigin.transform.forward, attackDirection) > 0)
        {
            Ray raycast = new Ray(origin, attackDirection);
            if (Physics.Raycast(raycast, out RaycastHit hit, AttackRange, layerMask))
            {
                //Debug.DrawLine(origin, hit.point, Color.red, 10.0f);
                //Debug.Log(hit.collider.tag);
                anim.SetBool(AnimID, true);
                if (hit.collider.CompareTag("Enemy"))
                {
                    EnemyAI EnemyScript = hit.collider.gameObject.GetComponentInParent<EnemyAI>();
                    if (EnemyScript != null)
                    {
                        EnemyScript.DamageTaken(weaponDmg);
                    }
                    else
                    {
                        Debug.Log("no enemy script found");
                    }
                }
            }
            else
            {
                Debug.Log("Nothing was hit by the attack.");
            }
        }
        else
        {
            Debug.Log("Attack direction is invalid (backwards through player).");
        }

        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(AttackCooldown);
        canAttack = true;
    }
}
