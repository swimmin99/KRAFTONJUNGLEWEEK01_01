using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float EnemyPatrolDistanceMin;
    [SerializeField]
    private float EnemyPatrolDistanceMax;
    [SerializeField]
    private float EnemyPatrolTime;
    [SerializeField]
    private float speed;

    private Vector2 originPosition;
    private Vector2 targetPosition;

    private void Start()
    {
        originPosition = transform.position;
        getNewPosition();
        StartCoroutine(EnemyPatrol());
    }

    IEnumerator EnemyPatrol()
    {
        while (true)
        {
            yield return new WaitForSeconds(EnemyPatrolTime);
            getNewPosition();

        }
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, targetPosition) > 0.1f)
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed/100);
    }


    void getNewPosition()
    {
        print("newPos");

        targetPosition = 
            new Vector2(originPosition.x + Random.Range(EnemyPatrolDistanceMin, EnemyPatrolDistanceMax), 
            originPosition.y);
    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(originPosition.x + EnemyPatrolDistanceMin, originPosition.y), Vector3.forward);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(new Vector3(originPosition.x + EnemyPatrolDistanceMax, originPosition.y), Vector3.forward);

    }

}
