using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour {
    public static ObjectPooler Instance {get; private set;}

    public class PoolObj{
        public PoolableObject objToPool;
        public GameObject poolParent;
        public List<GameObject> pooledObj;
    }

    public List<PoolableObject> ObjectToPool;
    private Dictionary<string, PoolObj> pool = new Dictionary<string, PoolObj>();
    public bool allowExpandPool = true;
    public int totalPool = 10;

    private void Awake() {
        Instance = this;

        foreach (var obj in ObjectToPool) 
        {
            for (int i = 0; i < totalPool; i++)
            {
                PoolableObject iobj = Instantiate(obj);
                iobj.gameObject.SetActive(false);

                if(!pool.ContainsKey(obj.name)){
                    pool.Add(obj.name, null);
                    pool[obj.name] = new PoolObj(){
                        objToPool = obj,
                        poolParent = new GameObject(obj.name + "'s Pool"),
                        pooledObj = new List<GameObject>(),
                    };
                }

                pool[obj.name].poolParent.transform.SetParent(transform);
                iobj.transform.SetParent(pool[obj.name].poolParent.transform);
                iobj.OriginalParent = iobj.transform.parent;
                pool[obj.name].pooledObj.Add(iobj.gameObject);
            }
        }

    }

    public GameObject GetPooledObject(string dictKey) {
        if(!pool.ContainsKey(dictKey)) return null;

        for (int i = 0; i < pool[dictKey].pooledObj.Count; i++) {
            if (!pool[dictKey].pooledObj[i].activeInHierarchy) {
                return pool[dictKey].pooledObj[i];
            }
        }

        if(allowExpandPool) {
            GameObject iobj = Instantiate(pool[dictKey].objToPool).gameObject;
            iobj.transform.SetParent(pool[dictKey].poolParent.transform);
            pool[dictKey].pooledObj.Add(iobj);
            return iobj;
        } else {
            return null;
        }
    }

}