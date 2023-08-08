using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSticking : MonoBehaviour
{
    public bool stickable = true;
    string currentParent;
    PlayerController player;
    MeshRenderer meshRenderer;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        meshRenderer = GetComponent<MeshRenderer>();
        PlayerController.PlayerChange += PlayerChanged;  // Registering Action in PlayerController
    }

    void Update()
    {
        if(transform.position.y > player.transform.position.y + 10)
        {
            Destroy(gameObject);
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -9.5f, 9.5f), transform.position.y, transform.position.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (stickable && currentParent != collision.gameObject.name) 
        {
            transform.parent = collision.collider.transform;
            currentParent = collision.gameObject.name;
        }
        
        collision.gameObject.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        meshRenderer.material.SetColor("_EmissionColor", player.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor")/2);

        player.GetComponent<PlayerController>().ChildCounter();
    }

    private void OnCollisionStay(Collision collision)
    {

    }

    void PlayerChanged(PlayerController playerObject)
    {
        player = playerObject;
    }
}
