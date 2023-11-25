using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacter : MonoBehaviour
{
    private Queue<Vector3> path = new Queue<Vector3>();
    private Vector3 targetPosition;
    public float speed = 5;
    public bool isMoving;

    void Awake()
    {
        targetPosition = transform.position;
    }

    public void SetPath(Queue<Vector3> path)
    {
        this.path = path;        
    }    

    void Update()
    {
        if(Vector3.Distance(transform.position,targetPosition) < 0.05f)
        {
            if(path.Count <= 0)
            {
                isMoving = false;
                return;
            }
            targetPosition = path.Dequeue();
        }
        isMoving = true;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
}
