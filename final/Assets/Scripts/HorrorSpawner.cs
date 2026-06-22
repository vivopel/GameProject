using UnityEngine;

public class HorrorSpawner : MonoBehaviour
{
    public GameObject steakPrefab;

    void Start()
    {
        InvokeRepeating(nameof(SpawnSteak), 2f, 3f);
    }

    void SpawnSteak()
    {
        float x = Random.Range(-8f, 8f);
        Instantiate(steakPrefab, new Vector3(x, 6f, 0), Quaternion.identity);
    }
}