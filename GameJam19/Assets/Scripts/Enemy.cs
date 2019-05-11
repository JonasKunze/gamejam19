using System;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    private WayPoint[] wayPoints;
    private Rigidbody rigid;
    private int currentWayPoint;

    private float waypointSize = 0.5f;
    private Vector3 currentTarget;

    [SerializeField] [Range(0.1f, 10)] private float maxVelocity;

    public static Enemy Create(GameObject prefab, WayPoint[] wayPoints)
    {
        var enemy = Instantiate(prefab).GetComponent<Enemy>();
        enemy.transform.position = wayPoints[0].transform.position;
        enemy.wayPoints = wayPoints;
        DungeonMaster.Instance.RegisterEnemyBorn();
        return enemy;
    }

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        currentTarget = wayPoints[0].GetRandomTargetPosition();
    }

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

            currentTarget = wayPoints[currentWayPoint].GetRandomTargetPosition();
            toNextWaypoint = GetNextWaypointDelta();
        }

        rigid.velocity = maxVelocity * toNextWaypoint.normalized;
        transform.forward = Vector3.ProjectOnPlane(toNextWaypoint, Vector3.up).normalized;
    }

    private void LastWaypointReached()
    {
        DungeonMaster.Instance.RegisterEnemyKilled();
        Destroy(gameObject);
    }

    private Vector3 GetNextWaypointDelta()
    {
        return currentTarget - transform.position;
    }
}