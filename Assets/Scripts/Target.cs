using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;
    public GameManager gameManager;
    public GameObject particleSystemPrefab;

    public float forceUpMin = 12;
    public float forceUpMax = 16;

    public float torqueMin = -10;
    public float torqueMax = 10;

    public float minXRange = -4;
    public float maxXRange = 4;
    public float startYPos = -2;

    public int cost = 5;

    private Vector3 savedVelocity;
    private Vector3 savedRotation;
    // Start is called before the first frame update
    void Start()
    {
        gameManager=GameObject.Find("GameManager").GetComponent<GameManager>();
        targetRb = GetComponent<Rigidbody>();

        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);

        transform.position = RandomSpawnPosition();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Pause( bool pause)
    {
        if (pause)
        {
            savedVelocity = targetRb.velocity;
            savedRotation = targetRb.angularVelocity;

            targetRb.velocity *= 0;
            targetRb.angularVelocity *= 0;
        }
        else
        {
            targetRb.velocity = savedVelocity;
            targetRb.angularVelocity= savedRotation ;
        }
        targetRb.useGravity = !targetRb.useGravity;
    }

    private void OnTriggerEnter(Collider other)
    {
        gameManager.SendMessage("RemoveFromList", gameObject);
        
        if (!gameObject.CompareTag("Bad") && other.gameObject.CompareTag("Sensor"))
        {
            gameManager.LostOne();
        }
        else if(!other.gameObject.CompareTag("Sensor"))
        {
            gameManager.UpdateScore(cost);
            Instantiate(particleSystemPrefab, transform.position, particleSystemPrefab.transform.rotation);
        }

        Destroy(gameObject);

    }


    private void OnMouseDown()
    {
        if (gameManager.isGameActive)
        {
            gameManager.UpdateScore(cost);
            Instantiate(particleSystemPrefab, transform.position, particleSystemPrefab.transform.rotation);
            gameManager.SendMessage("RemoveFromList", gameObject);
            Destroy(gameObject);
            
        }
    }
    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(forceUpMin, forceUpMax);
    }
    float RandomTorque()
    {
        return Random.Range(torqueMin, torqueMax);
    }
    Vector3 RandomSpawnPosition()
    {
        return new Vector3(Random.Range(minXRange, maxXRange), startYPos);
    }
}
