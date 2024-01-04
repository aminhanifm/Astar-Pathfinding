using UnityEngine;

public class PoolableObject : MonoBehaviour {
    public Transform OriginalParent {get; set;}

    public void ReturnToPool() {
        transform.SetParent(OriginalParent);
        gameObject.SetActive(false);
    }

    private void OnDisable() {
        ReturnToPool();
    }

}