using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsticleGenerator : MonoBehaviour
{
    public GameObject[] obsticles;
    public List<GameObject> generatedObsticles;
    public GameObject obsticleParent;
    public int obsticleCount;

    void Awake()
    {
        generatedObsticles = new List<GameObject>();
    }

    void Start()
    {
        GenerateObsticle();
    }

    void Update()
    {
        
    }

    public void GenerateObsticle()
    {
        
        for (int i = 0; i < obsticleCount; i++)
        {
            int x = UnityEngine.Random.Range(-5,5);
            int y = UnityEngine.Random.Range(-3, 4);
            bool isGenerated = false;
            for (int j = 0; j < generatedObsticles.Count; j++)
            {
                if (generatedObsticles[j].transform.position.x == x && generatedObsticles[j].transform.position.y == y)
                {
                    isGenerated = true;
                }
            }
            if (!isGenerated)
            {
                GameObject newObsticle = Instantiate(obsticles[UnityEngine.Random.Range(0, obsticles.Length)], new Vector3(x+0.5f, y, 0), Quaternion.identity, obsticleParent.transform);
                generatedObsticles.Add(newObsticle);
            }
        }
    }
}
