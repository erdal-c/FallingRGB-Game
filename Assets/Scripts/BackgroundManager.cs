using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public Camera camForPos;    // Using cam for referencee to background movement.
    public GameObject Background;
    public GameObject Obstacle;

    Vector3 startPoint;
    float lenght;

    UIManager uIManager;

    void Start()
    {
        startPoint = Background.transform.position;
        lenght = Background.GetComponentInChildren<SpriteRenderer>().bounds.size.y;

        uIManager = FindObjectOfType<UIManager>();
    }

    void Update()
    {
        if(camForPos.transform.position.y < startPoint.y - lenght/2)
        {
            startPoint.y -= lenght;
            Background.transform.position = startPoint;
        }

        if(Obstacle.transform.position.y > camForPos.transform.position.y + 10)
        {
            Obstacle.transform.position = new Vector3(Random.Range(-6f,6f), 
                                                    camForPos.transform.position.y - Random.Range(40,60),
                                                    0);
            Obstacle.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-30, 30));
        }

        if (!uIManager.isGameStart)
        {
            Background.transform.position += Vector3.up * 0.005f;
            startPoint = Background.transform.position;
        }
    }

    private void OnEnable()
    {
        Obstacle.transform.position += Vector3.down * 30f;
    }
}
