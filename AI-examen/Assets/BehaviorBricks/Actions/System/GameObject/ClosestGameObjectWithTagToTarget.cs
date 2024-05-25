﻿using Pada1.BBCore.Tasks;
using Pada1.BBCore;
using UnityEngine;

namespace BBUnity.Actions
{
    /// <summary>
    /// It is an action to find the closest game object with a given label.
    /// </summary>
    [Action("GameObject/ClosestGameObjectWithTagToTarget")]
    [Help("Finds the closest game object with a given tag")]
    public class ClosestGameObjectWithTagToTarget : GOAction
    {
        ///<value>Input Tag of the target game object.</value>
        [InParam("tag")]
        [Help("Tag of the target game object")]
        public string tag;

        [InParam("target")]
        public GameObject target;

        ///<value>OutPut The closest game object with the given tag Parameter.</value>
        [OutParam("foundGameObject")]
        [Help("The closest game object with the given tag")]
        public GameObject foundGameObject;

        private float elapsedTime;

        /// <summary>Initialization Method of ClosestGameObjectWithTag.</summary>
        /// <remarks>Get all the GamesObject with that tag and check which is the closest one.</remarks>
        public override void OnStart()
        {
            float dist = float.MaxValue;
            foreach(GameObject go in GameObject.FindGameObjectsWithTag(tag))
            {
                float newdist = Vector3.Distance(go.transform.position, target.transform.position);
                if(newdist < dist)
                {
                    dist = newdist;
                    foundGameObject = go;
                }
            }
        }
        /// <summary>Method of Update of ClosestGameObjectWithTag.</summary>
        /// <remarks>Complete the task.</remarks>
        public override TaskStatus OnUpdate()
        {
            return TaskStatus.COMPLETED;
        }
    }
}
