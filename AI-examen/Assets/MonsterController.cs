using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public int amountOfMonsters = 10;
    public float speed = 5.0f;
    ///<value>The gameobject that will be spawned</value>
    public GameObject prefab;
    ///<value>The position that the gameobject will be spawned</value>
    public Transform monsterParent;
    ///<value>Area where the GameObjects will move</value>
    public GameObject wanderArea;
    ///<value>Target GameObject to follow</value>
    public GameObject player;

    private List<BehaviorExecutor> monsters;

    public float spawnRange = 10.0f; // Range around the player where monsters will spawn

    public bool active_attack = false;
    void Start()
    {
        monsters = new List<BehaviorExecutor>();

        for (int i = 0; i < amountOfMonsters; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnRange, spawnRange),
                0.0f,
                Random.Range(-spawnRange, spawnRange)
            );

            Vector3 spawnPosition = player.transform.position + randomOffset;
            GameObject instance = Instantiate(prefab, spawnPosition, Quaternion.identity);
            BehaviorExecutor behaviorExecutor = instance.GetComponent<BehaviorExecutor>();
            instance.transform.parent = monsterParent;
            if (behaviorExecutor != null)
            {
                behaviorExecutor.SetBehaviorParam("player", player);
                monsters.Add(behaviorExecutor);
            }
        }
        



    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        transform.Translate(movement * speed * Time.deltaTime);
       // active_attack = (Input.GetAxis("Jump") != 0);
    }

    void FixedUpdate()
    {
        foreach (BehaviorExecutor be in monsters)
        {
            be.SetBehaviorParam("player_pos", transform.position);
            be.SetBehaviorParam("active_attack", active_attack);
        }
    }
}
