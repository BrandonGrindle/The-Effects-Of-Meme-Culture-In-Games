using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;


public class WeaponController : MonoBehaviour
{
    public static WeaponController instance;

    [Header("Weapon Stats")]
    [SerializeField] private float AttackCooldown = 1f;
    [SerializeField] private float AttackRange = 100f;
    [SerializeField] private GameObject WeaponOrigin;
    [SerializeField] private int weaponDmg = 5;

    public bool canAttack = true;
    private int layerMask;
    private GameObject _mainCamera;
    private void Awake()
    {
        instance = this;
        layerMask = ~LayerMask.GetMask("Player");
    }
    public void Attack() {
        //Debug.Log("Attack attempt");
        canAttack = false;
        Ray raycast = new Ray(WeaponOrigin.transform.position, WeaponOrigin.transform.forward);
        if (Physics.Raycast(raycast, out RaycastHit hit, AttackRange, layerMask))
        {
            Debug.DrawLine(WeaponOrigin.transform.position, hit.point, Color.red, 10.0f);
            Debug.Log(hit.collider.tag);
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
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(AttackCooldown);
        canAttack = true;
    }
}
