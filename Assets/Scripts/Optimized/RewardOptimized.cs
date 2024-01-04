using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RewardOptimized : MonoBehaviour
{
    //! We get the GameController instance to update the score
    private GameControllerOptimized GC => GameControllerOptimized.Instance;
    private ObjectPooler Pooler => ObjectPooler.Instance;
    public UnityEvent OnHit;


    void OnTriggerEnter(Collider hit)
    {
        if(hit.gameObject.CompareTag("Player")) //! Changed to CompareTag
        {
            OnHit?.Invoke();
            GC.Rewards.Remove(gameObject); //! Remove the reward from the list
            //! We check on the AI if there's any other reward available on the field
            if(hit.TryGetComponent<AICharacterOptimized>(out AICharacterOptimized character)){
                character.CheckCube();
            }

            GameObject explosion = Pooler.GetPooledObject("Explosion");
            explosion.SetActive(true);
            explosion.transform.position = transform.position;

            Destroy(gameObject);
        }
    }

    private void OnEnable() {
        OnHit.AddListener(GC.UpdateScore);
    }

    private void OnDisable() {
        OnHit.RemoveListener(GC.UpdateScore);
    }
}
