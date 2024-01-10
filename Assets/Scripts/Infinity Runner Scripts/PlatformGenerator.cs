using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    #region Variables
    //[SerializeField]
    //private GameObject thePlatform;

    [SerializeField]
    private Transform generationPoint;

    private float distanceBetween;

    //private float platformWidth;

    [SerializeField]
    private float distanceBetweenMin;
    [SerializeField]
    private float distanceBetweenMax;

    //public GameObject[] thePlatforms;
    private int platformSelector;
    private float[] platformWidths;

    public ObjectPooler[] theObjectPools;

    private float minHeight;

    public Transform maxHeightPoint;
    private float maxHeight;

    [SerializeField]
    private float maxHeightChange;

    private float heightChange;

    private CoinGenerator coinGenerator;

    public float randomCoinThreshold;

    #endregion

    void Start()
    {
        platformWidths = new float[theObjectPools.Length];

        for (int i = 0; i < theObjectPools.Length; i++)
        {
            platformWidths[i] = theObjectPools[i].pooledObject.GetComponent<BoxCollider2D>().size.x;
        }

        minHeight = transform.position.y;
        maxHeight = maxHeightPoint.position.y;

        coinGenerator = FindObjectOfType<CoinGenerator>();
    }

    void Update()
    {
        if(transform.position.x < generationPoint.position.x)
        {
            distanceBetween = Random.Range(distanceBetweenMin, distanceBetweenMax);

            transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector] / 2) + distanceBetween, heightChange, transform.position.z);

            platformSelector = Random.Range(0, theObjectPools.Length);

            heightChange = transform.position.y + Random.Range(maxHeightChange, -maxHeightChange);

            if(heightChange > maxHeight)
            {
                heightChange = maxHeight;
            }
            else if (heightChange < minHeight)
            {
                heightChange = minHeight;
            }

            //transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector] / 2) + distanceBetween, heightChange, transform.position.z);

            //Instantiate(thePlatforms[platformSelector], transform.position, transform.rotation);

            GameObject newPlatform = theObjectPools[platformSelector].GetPooledObject();

            newPlatform.transform.position = transform.position;
            newPlatform.transform.rotation = transform.rotation;
            newPlatform.SetActive(true);

            if (Random.Range(0f, 100f) < randomCoinThreshold)
            {
                //coinGenerator.SpawnCoins(new Vector3(Random.Range(0f, platformWidths[platformSelector]), transform.position.y + 3f, transform.position.z));
                coinGenerator.SpawnCoins(new Vector3(transform.position.x + Random.Range(platformWidths[platformSelector] * -0.1f, platformWidths[platformSelector] * 0.85f), transform.position.y + Random.Range(1f, 4f), transform.position.z));

            }
            transform.position = new Vector3(transform.position.x + (platformWidths[platformSelector] / 3.5f), transform.position.y, transform.position.z);

        }
    }
}
