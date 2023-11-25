using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerOptimized : MonoBehaviour
{
    private UIControllerOptimized UI => UIControllerOptimized.Instance;

    //! We use singleton on GameController
    public static GameControllerOptimized Instance { get; private set; }
    public int Score {get; set;}
    //! List of rewards to check if there are any rewards in the game
    public List<GameObject> Rewards {get; set;}

    void Awake()
    {
        if(Instance != null && Instance != this){
            Destroy(this);
        } else {
            Instance = this;
        }

        Rewards = new List<GameObject>();
    }
    
    //! Too many redudant FindObjectOfType, instead we can make local class and a list of rewards
    /* void Start()
    {
        GF.InitGameField(64, 64);

        int blockCount = 128;

        while(blockCount > 0)
        {
            int rdX = Random.Range(0, 64);
            int rdY = Random.Range(0, 64);
            if (GF.IsCellBlocked(rdX, rdY))
                continue;

            GF.BlockCell(rdX, rdY);
            blockCount--;
        }

        int rewardCount = 16;

        while(rewardCount> 0)
        {
            int rdX = Random.Range(0, 64);
            int rdY = Random.Range(0, 64);

            if (GF.IsCellBlocked(rdX, rdY))
                continue;

            GF.CreateReward(rdX, rdY);
            rewardCount--;
        }

        GF.InitAICharacter(0, 0);


        score = 0;

        GameObject firstReward = GF.CreateReward(6, 9);

        Vector3 rewardPosition = firstReward.transform.position;

        AICharacterOptimized aiCharacter = FindObjectOfType<AICharacterOptimized>();

        Queue<Vector3> queuePath = FindObjectOfType<AstarPathFinding>().GetPath(aiCharacter.transform.position, rewardPosition);

        aiCharacter.SetPath(queuePath);
    } */

    void Start()
    {
        GameField GF = FindObjectOfType<GameField>();

        GF.InitGameField(64, 64);

        int blockCount = 128;

        while(blockCount > 0)
        {
            int rdX = Random.Range(0, 64);
            int rdY = Random.Range(0, 64);
            if (GF.IsCellBlocked(rdX, rdY))
                continue;

            GF.BlockCell(rdX, rdY);
            blockCount--;
        }

        int rewardCount = 16;

        while(rewardCount> 0)
        {
            int rdX = Random.Range(0, 64);
            int rdY = Random.Range(0, 64);

            if (GF.IsCellBlocked(rdX, rdY))
                continue;

            Rewards.Add(GF.CreateReward(rdX, rdY)); //! Add to the list
            rewardCount--;
        }

        GF.InitAICharacter(0, 0);

        Score = 0;

        GameObject firstReward = GF.CreateReward(6, 9);
        Rewards.Add(firstReward); //! Add to the list

        Vector3 rewardPosition = firstReward.transform.position;

        AICharacterOptimized aiCharacter = FindObjectOfType<AICharacterOptimized>();

        Queue<Vector3> queuePath = FindObjectOfType<AstarPathFinding>().GetPath(aiCharacter.transform.position, rewardPosition);

        aiCharacter.SetPath(queuePath);
    }

    //! FindObjectOfType on update cost fps, instead we removed this and make a new function called UpdateScore
    /* void Update()
    {
        FindObjectOfType<UIController>().SetScore(score);
    } */
    
    public void UpdateScore(){
        Score++;
        UI.SetScore(Score);
    }
}
