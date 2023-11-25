using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AICharacterOptimized : MonoBehaviour
{
    //! GameController & UIController instance
    private UIControllerOptimized UI => UIControllerOptimized.Instance;
    private GameControllerOptimized GC => GameControllerOptimized.Instance;

    private Queue<Vector3> path = new Queue<Vector3>();
    private Vector3 targetPosition;
    //! We use cinemachine for target follow
    private CinemachineVirtualCamera vCam;
    //! We store Astar class on awake
    private AstarPathFinding astar;
    private float closeDistance = 0.05f;
    public float speed = 5;
    public bool isMoving;

    void Awake()
    {
        targetPosition = transform.position;
        vCam = FindObjectOfType<CinemachineVirtualCamera>();
        astar = FindObjectOfType<AstarPathFinding>();
    }

    //! We set follow and lookat on cinemachine to the AI 
    void Start()
    {
        vCam.Follow = transform;
        vCam.LookAt = transform;
    }

    public void SetPath(Queue<Vector3> path)
    {
        this.path = path;        
    }

    //! Function to check if there any cube available
    public void CheckCube(){
        if(GC.Rewards.Count == 0){
            UI.DisplayWinText();
        } else {
            //! We look for the nearest reward
            Transform nearestCube = GC.Rewards.OrderBy(rw => (transform.position - rw.transform.position).sqrMagnitude).First().transform;

            Vector3 rewardPosition = nearestCube.position;

            Queue<Vector3> queuePath = astar.GetPath(transform.position, rewardPosition);
            SetPath(queuePath);
        }
    }    
    
    void Update()
    {
        //! We use sqrMagnitude instead of distance because of slowest calculation

        Vector3 offset = targetPosition - transform.position;
        float sqrLen = offset.sqrMagnitude;

        if (sqrLen < closeDistance * closeDistance)
        {
            if(path.Count <= 0)
            {
                isMoving = false;
                return;
            }
            targetPosition = path.Dequeue();
        }

        // if(Vector3.Distance(transform.position,targetPosition) < 0.05f)
        // {
        //     if(path.Count <= 0)
        //     {
        //         isMoving = false;
        //         return;
        //     }
        //     targetPosition = path.Dequeue();
        // }
        isMoving = true;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
}
