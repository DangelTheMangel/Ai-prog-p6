using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : Entity
{
    private void Start()
    {
        behaviorExecutor.SetBehaviorParam("searcgBody",gameObject);
    }
}
