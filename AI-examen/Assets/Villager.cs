using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Villager : Entity
{
    public LayerMask monsterLayer, villagerLayer;
    public LayerMask obstacleLayer;
    public float visionField = 10f;
    public string panicBool = "escaping", panicAnimName = "panicVillager";
    public ParticleSystem panicPS;
    public bool panicing = false;
    public bool canCalloutToOthers = true;
    public float shoutingDistance = 6f; // Corrected typo from shoutingDistances to shoutingDistance
    public Animator animator;
   
    private UnityEngine.AI.NavMeshAgent navAgent;
    private BehaviorExecutor behaviorExecutor;
    public GameObject target;
    private void Start()
    {
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        behaviorExecutor = GetComponent<BehaviorExecutor>();
        behaviorExecutor.SetBehaviorParam("body", gameObject);
    }

    private void FixedUpdate()
    {
        if (!panicing)
        {
            // Find all colliders within the vision field radius
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, visionField, monsterLayer);

            foreach (var hitCollider in hitColliders)
            {
                Vector3 directionToEnemy = (hitCollider.transform.position - transform.position).normalized;
                float distanceToEnemy = Vector3.Distance(transform.position, hitCollider.transform.position);

                // Perform a raycast to check if there are any obstacles between the villager and the enemy
                if (!Physics.Raycast(transform.position, directionToEnemy, distanceToEnemy, obstacleLayer))
                {
                    panic(hitCollider.gameObject);
                    break; // Stop checking further enemies once panic is triggered
                }
            }
        }
       else if (canCalloutToOthers)
        {
            // Create a spherecast within shoutingDistance and get all the colliders that are Villager
            Collider[] villagerColliders = Physics.OverlapSphere(transform.position, shoutingDistance, villagerLayer);

            foreach (var villagerCollider in villagerColliders)
            {
                Villager otherVillager = villagerCollider.GetComponent<Villager>();
                if (otherVillager != null && !otherVillager.panicing)
                {
                    if(target != null)
                        otherVillager.panic(target);
                }
            }
        }
    }

    public void panic(GameObject target)
    {
        animator.Play(panicAnimName);
        navAgent.isStopped = true;
        panicing = true;
        this.target = target;
        behaviorExecutor.SetBehaviorParam("target", target);
        behaviorExecutor.SetBehaviorParam(panicBool, true);
        if (!panicPS.isPlaying)
            panicPS.Play();
    }
}
