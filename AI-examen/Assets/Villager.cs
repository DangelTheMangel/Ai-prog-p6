using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : Entity
{
    public LayerMask monsterLayer;
    public LayerMask obstacleLayer;
    public float visionField = 10f;
    public string panicBool = "escaping";
    public ParticleSystem panicPS;
    public bool panicing = false;
    private void FixedUpdate()
    {
        if (!panicing) {
            // Find all colliders within the vision field radius
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, visionField, monsterLayer);

            foreach (var hitCollider in hitColliders)
            {
                Vector3 directionToEnemy = (hitCollider.transform.position - transform.position).normalized;
                float distanceToEnemy = Vector3.Distance(transform.position, hitCollider.transform.position);

                // Perform a raycast to check if there are any obstacles between the villager and the enemy
                if (!Physics.Raycast(transform.position, directionToEnemy, distanceToEnemy, obstacleLayer))
                {
                    // If no obstacles, call panic
                    panic();
                    break; // Stop checking further enemies once panic is triggered
                }
            }
        }
    }

    public void panic()
    {
        navAgent.isStopped = true;
        panicing = true;
        if (!panicPS.isPlaying)
            panicPS.Play();
        //behaviorExecutor.SetBehaviorParam(panicBool, true);
    }
}
