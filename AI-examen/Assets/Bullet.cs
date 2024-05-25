using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Bullet : MonoBehaviour
{
    public float aliveTime = 10;
    public int damage = 1;
    public LayerMask obsticaleLayer, targetLayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        aliveTime -= Time.deltaTime;
        if (aliveTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enterede: " + other.gameObject.name + " Layer: " + other.gameObject.layer);
        Debug.Log("Layers looking at: " + obsticaleLayer.value + " " + targetLayer.value);
        if ((targetLayer & (1 << other.gameObject.layer)) != 0)
        {
            Debug.Log("Hit enemy");
            Entity entity = other.gameObject.GetComponent<Entity>();
            if (entity != null) {
                entity.takeDamage(damage);
            }
        }
        else if ((obsticaleLayer & (1 << other.gameObject.layer)) != 0)
        {
            Debug.Log("Bullet kaboom");
            Destroy(gameObject);
        }
    }

}
