using System;
using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace BBUnity.Actions
{

    [Action("Entity/ShoutForHelp")]
    public class ShoutForHelp : GOAction
    {

        [InParam("Layer")]
        public LayerMask layer;

        [InParam("ShoutingDistance")]
        public float shoutingDistance;

        [InParam("target")]
        public GameObject targetGameobject;
        [InParam("body")]
        public GameObject body;
        public override void OnStart()
        {
            // Create a spherecast within shoutingDistance and get all the colliders that are Villager
            Collider[] villagerColliders = Physics.OverlapSphere(body.transform.position, shoutingDistance, layer);

            foreach (var villagerCollider in villagerColliders)
            {
                Villager otherVillager = villagerCollider.GetComponent<Villager>();
                if (otherVillager != null && !otherVillager.panicing)
                {
                    if (targetGameobject != null)
                        otherVillager.panic(targetGameobject);
                }
            }
        }
        /// <summary>Method of Update of AddComponent.</summary>
        /// <remarks>Complete the task.</remarks>
        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}
