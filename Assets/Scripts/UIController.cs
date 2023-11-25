using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject obj_winText;
    public Text text_score;

    public void DisplayWinText()
    {
        obj_winText.SetActive(true);
    }

    public void SetScore(int score)
    {
        text_score.text = "Score: " + score.ToString();
    }
}
