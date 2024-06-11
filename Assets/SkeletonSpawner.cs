using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSpawner : MonoBehaviour
{
    public GameObject skeletonPrefab;
    private float timer = 0;
    private float spawnRate = 10f;
    // Start is called before the first frame update
    void Start()
    {
        spawnSkeleton();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            spawnSkeleton();
            timer = 0;
        }
    }

    void spawnSkeleton()
    {
        Instantiate(skeletonPrefab, transform.position, transform.rotation);
    }
}
