using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public float Point;
    [HideInInspector] public float multiplefactor = 1f;
    [HideInInspector] public float HighScore;

    public GameObject thornPrefab;
    public GameObject blockPrefab;

    public GameObject LevelObstacle;

    PlayerController player;
    UIManager uIManager;

    bool isThornCoroutineStart = true;
    bool isBlockCoroutineStart = true;

    float random1;
    float random2;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        uIManager = FindObjectOfType<UIManager>();

        HighScore = PlayerPrefs.GetFloat("HighScore");

        PlayerController.PlayerChange += PlayerChanged;  // Registering Action in PlayerController
    }

    void Update()
    {
        if (!uIManager.isGameStart)
        {
            return;  //Stoping GameManager update while user dont click to play in game
        } 

        if (!Enumerable.Range((int)LevelObstacle.transform.position.y - 8, 14).ToArray().Contains<int>((int)player.transform.position.y - 15))
        {
            if (isThornCoroutineStart)
            {
                StartCoroutine(ThornSpawner());
            }
            if (isBlockCoroutineStart)
            {
                StartCoroutine(BlockSpawner());
            }
        }

        GetPoint();
    }

    IEnumerator ThornSpawner()
    {
        random1 = Random.Range(-9f, 9f);
        if (Mathf.Abs(random2 - random1) < 1f)
        {
            if (random2 < 0)
            {
                random1 += 2f;
            }
            else
            {
                random1 -= 2f;
            }
        }

        Instantiate(thornPrefab, new Vector3(random1, player.transform.position.y - 15, 0), Quaternion.Euler(0,0,Random.Range(0,360)));
        isThornCoroutineStart = !isThornCoroutineStart;
        yield return new WaitForSeconds(2f);
        isThornCoroutineStart = !isThornCoroutineStart;
    }

    IEnumerator BlockSpawner()
    {
        random2 = Random.Range(-9f, 9f);
        if (Mathf.Abs(random2 - random1) < 1f)
        {
            if(random1 < 0)
            {
                random2 += 2f;
            }
            else
            {
                random2 -= 2f;
            }
        }
        Instantiate(blockPrefab, new Vector3(random2, player.transform.position.y - 15, 0), Quaternion.Euler(0, 0, Random.Range(0, 360)));
        isBlockCoroutineStart= !isBlockCoroutineStart;
        yield return new WaitForSeconds(3f);
        isBlockCoroutineStart = !isBlockCoroutineStart;
    }

    void GetPoint()
    {
        multiplefactor = (player.GetComponent<PlayerController>().ChildAmount * 0.2f) + 1;
        Point += Time.deltaTime * 3f * multiplefactor;
    }

    void PlayerChanged(PlayerController playerObject)
    {
        player = playerObject;
        Point *= 0.9f;
    }

    private void OnEnable()
    {
        Point = 0;
        multiplefactor = 1f;
    }
}