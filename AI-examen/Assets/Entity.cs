using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int healt = 3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void takeDamage(int amount) {
        healt -= amount;
        if (healt <= 0) {
            death();        
        }
    }

    public virtual void death()
    {
        Destroy(gameObject);
    }
}
