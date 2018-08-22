using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{

    [SerializeField]
    private GameObject[] clouds;

    private float distanceBetweenClouds = 3f;

    private float minX, maxX;

    private float lastCloudPositionY;

    private float controlX;

    [SerializeField]
    private GameObject[] collectables;

    private GameObject player;

    private void Awake()
    {
        controlX = 0;
        SetMinAndMAxX();
        CreateClouds();
        player = GameObject.Find("Player");
    }

    // Use this for initialization
    void Start()
    {
        PositionThePlayer();
    }

    void SetMinAndMAxX()
    {
        Vector3 bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        maxX = bounds.x - 0.5f;
        minX = -bounds.x + 0.5f;
    }

    void Shuffle(GameObject[] arrayToShuffle)
    {
        // random change the clouds position

        for (int i = 0; i < arrayToShuffle.Length; ++i)
        {
            GameObject temp = arrayToShuffle[i];
            int random = Random.Range(i, arrayToShuffle.Length);
            arrayToShuffle[i] = arrayToShuffle[random];
            arrayToShuffle[random] = temp;
        }
    }

    void CreateClouds()
    {
        Shuffle(clouds);

        float positionY = 0f;
        for (int i = 0; i < clouds.Length; ++i)
        {
            Vector3 temp = clouds[i].transform.position;
            temp.y = positionY;

            if (controlX == 0)
            {
                temp.x = Random.Range(0.0f, maxX);
                controlX = 1;
            }
            else if (controlX == 1)
            {
                temp.x = Random.Range(0.0f, minX);
                controlX = 2;
            }
            else if (controlX == 2)
            {
                temp.x = Random.Range(1.0f, maxX);
                controlX = 3;
            }
            else if (controlX == 3)
            {
                temp.x = Random.Range(-1.0f, minX);
                controlX = 0;
            }

            lastCloudPositionY = positionY;
            clouds[i].transform.position = temp;
            positionY -= distanceBetweenClouds;
        }
    }

    void PositionThePlayer()
    {
        GameObject[] darkClouds = GameObject.FindGameObjectsWithTag("Deadly");
        GameObject[] cloudsInGame = GameObject.FindGameObjectsWithTag("Cloud");

        for (int i = 0; i < darkClouds.Length; i++)
        {
            if (darkClouds[i].transform.position.y == 0f)
            {
                Vector3 tempCloudPosition = darkClouds[i].transform.position;

                darkClouds[i].transform.position = new Vector3(cloudsInGame[0].transform.position.x,
                                                               cloudsInGame[0].transform.position.y,
                                                               cloudsInGame[0].transform.position.z);

                cloudsInGame[0].transform.position = tempCloudPosition;
            }
        }

        Vector3 temp = cloudsInGame[0].transform.position;
        for (int i = 0; i < cloudsInGame.Length; i++)
        {
            if (temp.y < cloudsInGame[i].transform.position.y)
            {
                temp = cloudsInGame[i].transform.position;
            }
        }

        temp.y += 0.8f;

        player.transform.position = temp;
    }
}
