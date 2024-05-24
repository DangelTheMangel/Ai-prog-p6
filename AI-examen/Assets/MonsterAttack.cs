using BBUnity.Actions;
using Pada1.BBCore;           // Code attributes
using Pada1.BBCore.Framework;
using Pada1.BBCore.Tasks;     // TaskStatus
using System;
using UnityEngine;

namespace BBSamples.PQSG // Programmers Quick Start Guide
{
    /// <summary>
    /// DoneShootOnce is a action inherited from DoneShootOnce and Periodically clones a 'bullet' and
    /// shoots it throught the Forward axis with the specified velocity. This action never ends.
    /// </summary>
    [Action("Entity/Attack")]
    [Help("Periodically clones a 'bullet' and shoots it throught the Forward axis " +
          "with the specified velocity. This action never ends.")]
    public class MonsterAttack : BasePrimitiveAction
    {
        ///<value>Input delay Parameter, 30 by default.</value>
        // Define the input parameter delay, with the waited game loops between shoots.
        [InParam("delay", DefaultValue = 30)]
        public int delay;

        [InParam("damage", DefaultValue = 1)]
        public int damage;

        [InParam("minDistance", DefaultValue = 2f)]
        public float minDistance;

        [InParam("target")]
        public GameObject target;

        // Game loops since the last shoot.
        private int elapsed = 0;

        /// <summary>Update method of DoneShoot.</summary>
        /// <returns>Return Running task.</returns>
        // Main class method, invoked by the execution engine.
        public override TaskStatus OnUpdate()
        {
            if (delay > 0)
            {
                ++elapsed;
                elapsed %= delay;
                if (elapsed != 0)
                    return TaskStatus.RUNNING;
            }

            try {
                Entity e = target.GetComponent<Entity>();
                Debug.Log("Target" + e.gameObject.name);
                e.takeDamage(damage);
            }
            catch (Exception e) {
                Debug.LogError(e.ToString());
            }

            

            
            return TaskStatus.COMPLETED;

        } // OnUpdate

    } // class DoneShoot

} // namespace