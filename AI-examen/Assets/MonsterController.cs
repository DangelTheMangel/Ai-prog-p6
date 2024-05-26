using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Tilemaps;
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
    public Color activeColor, inactiveColor;
    public SpriteRenderer spriteRenderer;
    public bool activeAttack = false;
    public bool modeUpdated = false;
    public bool targetFromPlayer = false;
    public CityController cityController;
    public void createMonster(List<Vector3Int> possiableSpawnPos) {
        monsters = new List<Agent>();
        Vector3Int playerCellPos = cityController.tilemap.WorldToCell(transform.position);
        possiableSpawnPos.Sort((a, b) =>
        {
            float distA = Vector3.Distance((Vector3)playerCellPos, (Vector3)a);
            float distB = Vector3.Distance((Vector3)playerCellPos, (Vector3)b);
            return distB.CompareTo(distA);
        });
        

        for (int i = 0; i < amountOfMonsters.Length; i++)
        {
            int amount = amountOfMonsters[i].amount;
            for (int j = 0; j < amount; j++)
            {
                Vector3Int spawnCellPos = possiableSpawnPos[0];
                possiableSpawnPos.RemoveAt(0);
                Vector3 realPos = cityController.tilemap.CellToWorld(spawnCellPos);
                if (i == 0 && j == 0) {
                    transform.position = new Vector3(realPos.x, 0, realPos.y);
                }
                GameObject instance = Instantiate(amountOfMonsters[i].prefab, new Vector3(realPos.x,0, realPos.y), Quaternion.identity);
                Agent agent = new Agent(instance, monsterTag, monsterParent, player);
                if (targetFromPlayer)
                {
                    agent.be.SetBehaviorParam("searchBody", player);
                }
                monsters.Add(agent);
            }
        }
    }

    string axisHorizontal = "Horizontal";
    string axisVertical = "Vertical";
    string pressButton = "Jump";

    void Update()
    {

        float moveHorizontal = Input.GetAxis(axisHorizontal);
        float moveVertical = Input.GetAxis(axisVertical);
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        transform.Translate(movement * speed * Time.deltaTime);

        bool jumpButtonPressed = (Input.GetAxis(pressButton) != 0);

        if (!modeUpdated)
        {
            modeUpdated = (jumpButtonPressed != activeAttack);
            if (jumpButtonPressed)
            {
                spriteRenderer.color = activeColor;
            }
            else
            {
                spriteRenderer.color = inactiveColor;
            }
        }

        activeAttack = jumpButtonPressed;
    }

    void FixedUpdate()
    {
        foreach (Agent agent in monsters)
        {
            if (agent == null|| agent.be == null) {
                monsters.Remove(agent);
                continue;
            }
            agent.be.SetBehaviorParam("shouldAttack",activeAttack);
            if (modeUpdated) {
                agent.navAgent.isStopped = true;
            }
        }
        modeUpdated = false;
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
