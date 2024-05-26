using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MonsterController : MonoBehaviour
{
    public MonsterAmount[] amountOfMonsters;
    public float speed = 5.0f;
    public Transform monsterParent;
    public GameObject wanderArea;
    public GameObject player;
    public string monsterTag = "Monster";
    private List<Agent> monsters;
    public float spawnRange = 10.0f;
    public Color activeColor, inacitve;
    public SpriteRenderer spriteRenderer;
    public bool active_attack = false;
    public bool mode_updatede = false;
    public bool targetFromPlayer = false;
    void Start()
    {
        monsters = new List<Agent>();
        for (int i = 0; i < amountOfMonsters.Length; i++) {
            int amount = amountOfMonsters[i].amount;
            for (int j = 0; j < amount; j++)
                {
                    Vector3 randomOffset = new Vector3(
                        Random.Range(-spawnRange, spawnRange),
                        0.0f,
                        Random.Range(-spawnRange, spawnRange)
                    );

                    Vector3 spawnPosition = player.transform.position + randomOffset;
                    GameObject instance = Instantiate(amountOfMonsters[i].prefab, spawnPosition, Quaternion.identity);
                    Agent agent = new Agent(instance, monsterTag, monsterParent, player);
                    if (targetFromPlayer)
                    {
                    agent.be.SetBehaviorParam("searchBody", player);
                    }
                    monsters.Add(agent);
                }
        }



    }

    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        transform.Translate(movement * speed * Time.deltaTime);
        bool btn_pressed = (Input.GetAxis("Jump") != 0);
        if (!mode_updatede) {
            mode_updatede = (btn_pressed != active_attack);
            if (btn_pressed)
            {
                spriteRenderer.color = activeColor;
            }
            else {
                spriteRenderer.color = inacitve;
            }

        }

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

[Serializable]
public struct MonsterAmount {
    public GameObject prefab;
    public int amount;
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
