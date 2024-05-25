using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    public string monsterTag = "Monster";
    private List<Agent> monsters;

    public float spawnRange = 10.0f; // Range around the player where monsters will spawn

    public bool active_attack = false;
    public bool mode_updatede = false;
    void Start()
    {
        monsters = new List<Agent>();

        for (int i = 0; i < amountOfMonsters; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-spawnRange, spawnRange),
                0.0f,
                Random.Range(-spawnRange, spawnRange)
            );

            Vector3 spawnPosition = player.transform.position + randomOffset;
            GameObject instance = Instantiate(prefab, spawnPosition, Quaternion.identity);
            Agent agent = new Agent(instance, monsterTag, monsterParent, player);
            monsters.Add(agent);
        }
        



    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        transform.Translate(movement * speed * Time.deltaTime);
        bool btn_pressed = (Input.GetAxis("Jump") != 0);
        if(!mode_updatede)
            mode_updatede = btn_pressed != active_attack;
        active_attack = btn_pressed;

    }

    void FixedUpdate()
    {
        foreach (Agent agent in monsters)
        {
            if (agent == null|| agent.be == null) {
                monsters.Remove(agent);
                continue;
            }
            agent.be.SetBehaviorParam("shouldAttack",active_attack);
            if (mode_updatede) {
                agent.navAgent.isStopped = true;
            }
        }
        mode_updatede = false;
    }
}

public class Agent {
    public BehaviorExecutor be;
    public NavMeshAgent navAgent;
    public GameObject obj;

    public Agent(GameObject obj,string tag,Transform parent, GameObject player) {
        be = obj.GetComponent<BehaviorExecutor>();
        navAgent = obj.GetComponent<NavMeshAgent>();
        obj.transform.parent = parent;
        obj.tag = tag;
        if (be != null)
        {
            be.SetBehaviorParam("moveToArea", player);
        }
    }

}
