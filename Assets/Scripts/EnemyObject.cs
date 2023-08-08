using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : MonoBehaviour
{
    GameObject parentAll;

    PlayerController player;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        parentAll = GameObject.Find("Playable");

        PlayerController.PlayerChange += PlayerChanged;   // Registering Action in PlayerController
    }

    void Update()
    {
        if(transform.position.y > player.transform.position.y + 20)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Collider col = collision.collider;
        col.transform.parent = parentAll.transform;

        if (col.gameObject.GetComponent<PlayerController>())
        {
            col.gameObject.GetComponent<PlayerController>().ChangePlayableObject();
        }

        player.GetComponent<PlayerController>().ChildCounter();

        if(player.GetComponent<PlayerController>().isDead == false)
        {
            Destroy(col.gameObject);
        }
    }

    void PlayerChanged(PlayerController playerObject)
    {
        player = playerObject;
    }
}
