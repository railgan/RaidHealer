using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : CharacterBase
{
    public GameObject projectilePrefab;
    public Transform shootingPoint;

    public float autoAttackInterval = 0.5f;
    private float autoAttackCooldown;

    public GameObject abilityShapePrefab;

    private float damageCircleCooldown = 5f;
    private float damageCircleTimer;

    private bool isAttacking = false;
    private float attackTime = 0.5f;

    protected override void Update()
    {
        base.Update();

        HandleDamageCircle();
        HandleAutoAttack();
        
    }

    private void HandleAutoAttack()
    {
        autoAttackCooldown += Time.deltaTime;
        if (autoAttackCooldown >= autoAttackInterval && !isAttacking)
        {
            AutoAttack();
            autoAttackCooldown = 0f;
        }
    }

    private void HandleDamageCircle()
    {
        damageCircleTimer += Time.deltaTime;
        if (damageCircleTimer >= damageCircleCooldown && !isAttacking)
        {
            SpawnDamageCircle();
            damageCircleTimer = 0f;
        }
    }

    public void SpawnDamageCircle()
    {
        if (isAttacking) return; // Boss is already attacking
        isAttacking = true;

        PlayerController[] potentialTargets = FindObjectsOfType<PlayerController>();
        if (potentialTargets.Length == 0)
        {
            isAttacking = false;
            return; // No targets found
        }

        PlayerController target = potentialTargets[Random.Range(0, potentialTargets.Length)];
        GameObject shape = Instantiate(abilityShapePrefab, target.transform.position, Quaternion.identity);
        shape.GetComponent<AbilityShape>().targetTag = "Friendly";

        // Add attack rotation logic here (e.g., using a coroutine)

        Invoke("EndAttack", attackTime);
    }

    void AutoAttack()
    {
        if (isAttacking) return; // Boss is already attacking
        isAttacking = true;

        // Find all potential targets tagged as "Friendly"
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag("Friendly");
        if (potentialTargets.Length == 0)
        {
            isAttacking = false;
            return; // No targets found
        }

        // Select a random target
        GameObject target = potentialTargets[Random.Range(0, potentialTargets.Length)];
        Vector2 directionTowardsTarget = target.transform.position - shootingPoint.position;

        // Instantiate and initialize the projectile
        GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript.Initialize(directionTowardsTarget);

        // Set Projectile Stats
        projectileScript.targetTag = "Friendly"; // Enemy projectiles should target friendly units
        projectileScript.damage = 5; // Set Damage

        // Set the projectile's rotation to face the target
        Vector2 directionRotatedClockwise = new Vector2(directionTowardsTarget.y, -directionTowardsTarget.x);
        projectile.transform.right = directionRotatedClockwise;

        // Add attack rotation logic here (e.g., using a coroutine)

        Invoke("EndAttack", attackTime);
    }

    private void EndAttack()
    {
        isAttacking = false;
    }
}