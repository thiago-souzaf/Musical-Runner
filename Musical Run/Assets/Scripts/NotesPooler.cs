using System.Collections.Generic;
using UnityEngine;

public class NotesPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static NotesPooler instance;

    private void Awake()
    {
        instance = this;
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    void Start()
    {
        poolDictionary = new();

        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector2 position, Quaternion rotation, NoteInfo noteInfo)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError("Key not found in dictionary");
            return null;
        }
        GameObject objToSpawn = poolDictionary[tag].Dequeue();
        objToSpawn.SetActive(true);
        objToSpawn.transform.SetPositionAndRotation(position, rotation);

        if (objToSpawn.TryGetComponent(out NoteHandle noteHandle))
        {
            noteHandle.note = noteInfo;
        }

        if (objToSpawn.TryGetComponent(out IPooledObject pooledObj))
        {
            pooledObj.OnObjectSpawn();
        }

        poolDictionary[tag].Enqueue(objToSpawn);

        return objToSpawn;
    }
  
}
