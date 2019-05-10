using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    private Transform[] wayPoints;
    private Rigidbody rigid;
    private int currentWayPoint;

    private float waypointSize = 1;

    [SerializeField] [Range(0, 10)] private float maxVelocity;


    public static Enemy Create(GameObject prefab, Transform[] wayPoints)
    {
        var enemy = Instantiate(prefab).GetComponent<Enemy>();
        enemy.transform.position = wayPoints[0].position;
        enemy.wayPoints = wayPoints;
        return enemy;
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var toNextWaypoint = GetNextWaypointDelta();
        if (toNextWaypoint.magnitude < waypointSize)
        {
            currentWayPoint++;
            if (currentWayPoint == wayPoints.Length)
            {
                LastWaypointReached();
                return;
            }

            toNextWaypoint = GetNextWaypointDelta();
        }

        rigid.velocity = maxVelocity * toNextWaypoint.normalized;
    }

    private void LastWaypointReached()
    {
        Destroy(gameObject);
    }

    private Vector3 GetNextWaypointDelta()
    {
        return wayPoints[currentWayPoint].position - transform.position;
    }
}