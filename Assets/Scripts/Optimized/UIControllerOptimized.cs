using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerOptimized : MonoBehaviour
{
    //! Singleton pattern
    public static UIControllerOptimized Instance {get; private set;}
    public GameObject obj_winText;
    //! We use TextMeshPro instead of legacy text
    public TMP_Text text_score;

    void Awake()
    {
        if(Instance != null && Instance != this){
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    public void DisplayWinText()
    {
        obj_winText.SetActive(true);
    }

    public void SetScore(int score)
    {
        //! We use SetText function for more readable line code
        text_score.SetText($"Score: {score}");
    }
}
