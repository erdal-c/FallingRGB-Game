using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    PlayerController player;
    bool isPlayerChange;
    float playerPositionY;
    float accelerator;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        PlayerController.PlayerChange += PlayerChanged;
    }

    void Update()
    {
        playerPositionY = player.transform.position.y - 2f;

        accelerator -= Time.deltaTime / 250f;        
        CameraLerpToNewPlayer();
    }

    private void FixedUpdate()
    {
        if (isPlayerChange == false)
        {
            transform.position = new Vector3(0, playerPositionY, player.transform.position.z - 20);
        }
    }

    void PlayerChanged(PlayerController playerObject)
    {
        isPlayerChange = true;
        player = playerObject;
    }

    void CameraLerpToNewPlayer()
    {
        if (isPlayerChange) 
        {
            transform.position += new Vector3(0,accelerator,0);

            if (transform.position.y <= playerPositionY)
            {
                isPlayerChange= false;
            }
        }
    }
}
