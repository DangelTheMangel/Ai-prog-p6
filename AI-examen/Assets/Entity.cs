using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
    public int healt = 3;
    public ParticleSystem hurtPs;
    public BehaviorExecutor behaviorExecutor;
    public NavMeshAgent navAgent;
    // Start is called before the first frame update
    void Start()
    {
        behaviorExecutor = GetComponent<BehaviorExecutor>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int amount) {
        healt -= amount;
        if (hurtPs != null)
            hurtPs.Play();
        if (healt <= 0) {
            death();        
        }
    }

    public virtual void death()
    {
        Destroy(gameObject);
    }
}
