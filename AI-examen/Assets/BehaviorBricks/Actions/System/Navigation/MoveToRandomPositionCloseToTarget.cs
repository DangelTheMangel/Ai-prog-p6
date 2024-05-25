using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{
    /// <summary>
    /// It is an action to move the GameObject to a random position in an area using a NavMeshAgent.
    /// </summary>
    [Action("Navigation/MoveToRandomPositionInArea")]
    [Help("Gets a random position from a close to a targetand moves the game object to that point by using a NavMeshAgent")]
    public class MoveToRandomPositionCloseToTarget : GOAction
    {
        private UnityEngine.AI.NavMeshAgent navAgent;

        ///<value>Input game object Parameter that must have a BoxCollider or SphereColider, which will determine the area from which the position is extracted.</value>
        [InParam("area")]
        [Help("game object that must have a BoxCollider or SphereColider, which will determine the area from which the position is extracted")]
        public GameObject area;

        [InParam("areaSize")]
        public float areaSize;

        /// <summary>Initialization Method of MoveToRandomPosition.</summary>
        /// <remarks>Check if there is a NavMeshAgent to assign it one by default and assign it
        /// to the NavMeshAgent the destination a random position calculated with <see cref="getRandomPosition()"/> </remarks>
        public override void OnStart()
        {
            navAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (navAgent == null)
            {
                Debug.LogWarning("The " + gameObject.name + " game object does not have a Nav Mesh Agent component to navigate. One with default values has been added", gameObject);
                navAgent = gameObject.AddComponent<UnityEngine.AI.NavMeshAgent>();
            }
            navAgent.SetDestination(getRandomPosition());

            #if UNITY_5_6_OR_NEWER
                navAgent.isStopped = false;
            #else
                navAgent.Resume();
            #endif
        }
        /// <summary>Method of Update of MoveToRandomPosition </summary>
        /// <remarks>Check the status of the task, if it has traveled the road or is close to the goal it is completed
        /// and otherwise it will remain in operation.</remarks>
        public override TaskStatus OnUpdate()
        {
            if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance || navAgent.isStopped)
                return TaskStatus.COMPLETED;
            return TaskStatus.RUNNING;
        }

        private Vector3 getRandomPosition()
        {
            return area.transform.position + new Vector3(UnityEngine.Random.Range(-areaSize, areaSize), 0, UnityEngine.Random.Range(-areaSize, areaSize));
        }
        /// <summary>Abort method of MoveToRandomPosition </summary>
        /// <remarks>When the task is aborted, it stops the navAgentMesh.</remarks>
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
