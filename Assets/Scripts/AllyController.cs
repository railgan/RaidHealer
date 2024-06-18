using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyController : CharacterBase
{
    public GameObject projectilePrefab;
    public Transform shootingPoint;

    public float attackSpeed = 1f;
    private float attackCooldown;
    private float calculatedAttackSpeed;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        calculatedAttackSpeed = attackSpeed * haste/100;
        Debug.Log(calculatedAttackSpeed);
        attackCooldown += Time.deltaTime;
        if (attackCooldown >= calculatedAttackSpeed)
        {
            AutoAttack();
            haste += 1;
            attackCooldown = 0f;
        }
    }

    void AutoAttack()
    {
        // Find all potential targets tagged as "Enemy"
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag("Enemy");
        if (potentialTargets.Length == 0) return; // No targets found

        // Select a random target
        GameObject target = potentialTargets[Random.Range(0, potentialTargets.Length)];
        Vector2 directionTowardsTarget = target.transform.position - shootingPoint.position;

        // Instantiate and initialize the projectile
        GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.Initialize(directionTowardsTarget);

        // Set Projectile Stats
        projectileScript.targetTag = "Enemy"; // Enemy projectiles should target friendly units
        projectileScript.damage = 1; // Set Damage

        // Set the projectile's rotation to face the target
        Vector2 directionRotatedClockwise = new Vector2(directionTowardsTarget.y, -directionTowardsTarget.x);
        projectile.transform.right = directionRotatedClockwise;
    }
}
