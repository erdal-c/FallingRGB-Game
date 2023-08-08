using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float PlayerSpeed = 300f;
    public float fallFactor = 100f;
    public Transform ParentAll;

    [HideInInspector] public int ChildAmount;
    [HideInInspector] public bool isDead;

    public static event Action<PlayerController> PlayerChange;

    [SerializeField] float rotationFactor;
    MeshFilter meshFilter;
    Rigidbody playerRB;
    BoxCollider boxCollider;
    MeshRenderer meshRenderer;

    [HideInInspector]public bool isDash;
    public float dashPower = 8f;
    float dashTime;
    float dashCoolDown = 1f;

    float colorValue;

    UIManager uIManager;
    GameManager gameManager;

    void Start()
    {
        meshFilter = GetComponent<MeshFilter>();
        playerRB = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();

        meshRenderer = GetComponent<MeshRenderer>();
       
        colorValue = UnityEngine.Random.Range(0f, 1f);

        PlayerChange?.Invoke(this);
        ChildCounter();    

        uIManager = FindObjectOfType<UIManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        ColorEdit();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(dashTime < Time.timeSinceLevelLoad)
            {
                isDash = true;
                dashTime = 0.2f + Time.timeSinceLevelLoad;
            }
        }
    }


    private void FixedUpdate()
    {
        if(uIManager.isGameStart) 
        {
            PlayerMove();
            PlayerRotate();
            PlayerDash();
        }
    }

    void PlayerMove()
    {
        playerRB.velocity = Vector3.ClampMagnitude(new Vector3(playerRB.velocity.x,0,0), 8f);

        playerRB.AddForce(PlayerSpeed * Input.GetAxis("Horizontal") * Time.deltaTime, 0, 0);
        if(Mathf.Abs(Input.GetAxis("Horizontal")) <= 0.2f)
        {
            playerRB.drag = 2f;
        }
        else
        {
            playerRB.drag = 0f;
        }

        /*
         * Speed cease when change movement direction. 
         * If the Input axis direction and the velocity direction are opposite, 
         * changing the direction is facilitated by changing the velocity.
        */
        if (Input.GetAxis("Horizontal") > 0.5 && playerRB.velocity.x < 0)
        {
            playerRB.velocity = new Vector3(playerRB.velocity.x + 0.2f, playerRB.velocity.y, 0);
        }
        else if(Input.GetAxis("Horizontal") < -0.5 && playerRB.velocity.x > 0)
        {
            playerRB.velocity = new Vector3(playerRB.velocity.x - 0.2f, playerRB.velocity.y, 0);
        }
        
        playerRB.velocity = new Vector3(playerRB.velocity.x, -fallFactor * Time.deltaTime ,0);  //Fall movement
        fallFactor += 0.1f;
    }

    void PlayerRotate()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            rotationFactor += 2f;
            transform.rotation = Quaternion.Euler(0, 0, rotationFactor);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            rotationFactor -= 2f;
            transform.rotation = Quaternion.Euler(0, 0, rotationFactor);
        }

        if (Mathf.Abs(rotationFactor) % 360 == 0)
        {
            rotationFactor = (int)transform.rotation.eulerAngles.z;
        }

        if (Mathf.Abs(playerRB.angularVelocity.z) > 0.05f)
        {
            playerRB.angularVelocity = Vector3.zero;
        }
    }
    void PlayerDash()
    {
        if(isDash && dashTime > Time.timeSinceLevelLoad)
        {
            playerRB.AddForce(Input.GetAxisRaw("Horizontal") * dashPower, 0, 0, ForceMode.Impulse);
        }
        else if(isDash && dashTime <= Time.timeSinceLevelLoad)
        {
            isDash = false;
            dashTime += dashCoolDown;
        }  
    }

    void PlayerDeath()
    {
        isDead = true;
        colorValue = 4;
        
        if(gameManager.Point > gameManager.HighScore)
        {
            PlayerPrefs.SetFloat("HighScore", gameManager.Point);
        }
        Time.timeScale = 0;
    }

    void ColorEdit()
    {
        if (!isDead)
        {
            colorValue += Time.deltaTime * 0.1f;
            meshRenderer.material.SetColor("_EmissionColor", Color.HSVToRGB(colorValue % 1, 0.8f, 0.6f) * 4);
        }
        else
        {
            if(colorValue > 1)
            {
                colorValue -= 0.01f;
            }
            else
            {
                uIManager.DeathMenu.SetActive(true);
                if (gameManager.Point > gameManager.HighScore)
                {
                    uIManager.RecordText.gameObject.SetActive(true);
                }
                else
                {
                    uIManager.RecordText.gameObject.SetActive(false);
                }

            }

            meshRenderer.material.SetColor("_EmissionColor", Color.HSVToRGB(1f, 0f, 0.3f) * colorValue);
        }   
    }

    public void ChangePlayableObject()
    {
        if (transform.childCount == 0)
        {
            PlayerDeath();
        }
        else if (transform.childCount > 0)
        {
            int randomvalue = UnityEngine.Random.Range(0, transform.childCount);

            Transform newChild = transform.GetChild(randomvalue);

            while (transform.childCount > 0)
            {
                transform.GetChild(0).parent = transform.parent;
            }

            newChild.parent = transform.parent;

            newChild.gameObject.AddComponent<PlayerController>();
            newChild.gameObject.AddComponent<Rigidbody>();

            newChild.GetComponent<Rigidbody>().useGravity = false;
            newChild.GetComponent<Rigidbody>().drag = 0;
            newChild.GetComponent<Rigidbody>().angularDrag = 0.2f;
            newChild.GetComponent<Rigidbody>().constraints = (RigidbodyConstraints)8 + 16 + 32;

            Destroy(newChild.GetComponent<ObjectSticking>());
        }
    }

    public void ChildCounter()
    {
        ChildAmount = transform.GetComponentsInChildren<Transform>().Length - 1;
    }

    private void OnDisable()
    {
        PlayerSpeed = 300f;
        fallFactor = 100f;
        transform.position += Vector3.down * 20f;
        if (transform.childCount > 0) 
        {
            foreach(Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        if (isDead)
        {
            isDead = false;
        }
    }

}
