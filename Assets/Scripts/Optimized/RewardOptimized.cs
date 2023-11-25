using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardOptimized : MonoBehaviour
{
    //! We get the GameController instance to update the score
    private GameControllerOptimized GC => GameControllerOptimized.Instance;

    void OnTriggerEnter(Collider hit)
    {
        if(hit.tag == "Player")
        {
            GC.UpdateScore();
            GC.Rewards.Remove(gameObject); //! Remove the reward from the list
            //! We check on the AI if there's any other reward available on the field
            if(hit.TryGetComponent<AICharacterOptimized>(out AICharacterOptimized character)){
                character.CheckCube();
            }

            Destroy(gameObject);
        }
    }
}
