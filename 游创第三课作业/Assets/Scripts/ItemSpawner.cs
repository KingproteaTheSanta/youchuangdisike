using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject item1Prefab;
    public GameObject item2Prefab;
    public GameObject item3Prefab;

    public float spawnInterval = 1f;
    public Vector2 xRange = new Vector2(-5f, 5f);
    public float spawnHeight = 5f;

    void Start()
    {
        InvokeRepeating(nameof(Spawn), 1f, spawnInterval);
    }

    void Spawn()
    {
        int type = Random.Range(0, 3);

        GameObject prefab = type == 0 ? item1Prefab :
                            type == 1 ? item2Prefab :
                                        item3Prefab;

        Vector2 pos = new Vector2(
            Random.Range(xRange.x, xRange.y),
            spawnHeight
        );

        Instantiate(prefab, pos, Quaternion.identity);
    }
}
