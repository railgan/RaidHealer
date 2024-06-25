using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : CharacterBase
{
    public GameObject projectilePrefab;
    public Transform shootingPoint;

    public float shootingInterval = 2f;
    private float shootingTimer;

    protected override void Update()
    {
        base.Update();
        shootingTimer += Time.deltaTime;
        if (shootingTimer >= shootingInterval)
        {
            ShootAtRandomTarget();
            shootingTimer = 0f;
        }
    }

    void ShootAtRandomTarget()
    {
        // Find all potential targets tagged as "Friendly"
        GameObject[] potentialTargets = GameObject.FindGameObjectsWithTag("Friendly");
        if (potentialTargets.Length == 0) return; // No targets found

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
    }
}
