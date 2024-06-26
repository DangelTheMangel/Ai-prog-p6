﻿using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{
    [Action("Navigation/MoveAwayFromTarget")]
    [Help("Gets a random position from a close to a targetand moves the game object to that point by using a NavMeshAgent")]
    public class MoveAwayFromTarget : GOAction
    {
        private UnityEngine.AI.NavMeshAgent navAgent;

      
        [InParam("target")]
        [Help("game object that must have a BoxCollider or SphereColider, which will determine the area from which the position is extracted")]
        public GameObject target;
        [InParam("uncertainty", DefaultValue = 5f)]
        float uncertainty;
        [InParam("distanceToMoveAway",DefaultValue = 10.0f)]
        float distanceToMoveAway;
        [InParam("maxAttempts", DefaultValue = 10)]
        int maxAttempts = 10; 
        [InParam("radiusIncrement", DefaultValue = 1f)]
        float radiusIncrement;

        public override void OnStart()
        {
            navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (navAgent == null)
            {
                Debug.LogWarning("The " + gameObject.name + " game object does not have a Nav Mesh Agent component to navigate. One with default values has been added", gameObject);
                navAgent = gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
            }
            
            navAgent.SetDestination(getnewpos());

            #if UNITY_5_6_OR_NEWER
                navAgent.isStopped = false;
            #else
                navAgent.Resume();
            #endif
        }

        public override TaskStatus OnUpdate()
        {
            if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance || navAgent.isStopped)
                return TaskStatus.COMPLETED;
            return TaskStatus.RUNNING;
        }

        private Vector3 getnewpos()
        {
            Vector3 directionAwayFromTarget = (gameObject.transform.position - target.transform.position).normalized;
            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                directionAwayFromTarget += new Vector3(Random.Range(-uncertainty, uncertainty), 0, Random.Range(-uncertainty, uncertainty)).normalized;
                Vector3 newPosition = gameObject.transform.position + directionAwayFromTarget * (distanceToMoveAway - attempt * radiusIncrement);
                UnityEngine.AI.NavMeshHit navMeshHit;
                if (UnityEngine.AI.NavMesh.SamplePosition(newPosition, out navMeshHit, distanceToMoveAway, UnityEngine.AI.NavMesh.AllAreas))
                {
                    return navMeshHit.position;
                }
            }
            Vector3 fallbackPosition = gameObject.transform.position + new Vector3(Random.Range(-5.0f, 5.0f), 0, Random.Range(-5.0f, 5.0f));
            UnityEngine.AI.NavMeshHit fallbackHit;
            if (UnityEngine.AI.NavMesh.SamplePosition(fallbackPosition, out fallbackHit, 5.0f, UnityEngine.AI.NavMesh.AllAreas))
            {
                return fallbackHit.position;
            }
            return gameObject.transform.position;
        }

        public override void OnAbort()
        {
            #if UNITY_5_6_OR_NEWER
                navAgent.isStopped = true;
            #else
                navAgent.Stop();
            #endif
        }
    }
}
