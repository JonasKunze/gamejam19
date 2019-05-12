using System;
using System.Collections;
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

    private bool isLastWaypointReached = false;

    public float damageCadance = 1;

    [SerializeField] [Range(0.1f, 10)] protected float maxVelocity;

    public static Enemy Create(GameObject prefab, WayPoint[] wayPoints)
    {
        Enemy enemy = PhotonNetwork.Instantiate(prefab.name,
            wayPoints[0].transform.position,
            Quaternion.identity, 0).GetComponent<Enemy>();
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
        if (!isLastWaypointReached)
        {
            var toNextWaypoint = GetNextWaypointDelta();
            if (toNextWaypoint.magnitude < waypointSize)
            {
                currentWayPoint++;
                if (currentWayPoint >= wayPoints.Length)
                {
                    LastWaypointReached();
                    isLastWaypointReached = true;
                    return;
                }

                currentTarget = wayPoints[currentWayPoint].GetRandomTargetPosition();
                toNextWaypoint = GetNextWaypointDelta();
            }

            rigid.velocity = maxVelocity * toNextWaypoint.normalized;
            transform.forward = Vector3.ProjectOnPlane(toNextWaypoint, Vector3.up).normalized;
        }
    }

    private void OnDestroy()
    {
        DungeonMaster.Instance.RegisterEnemyKilled();
    }

    private void LastWaypointReached()
    {
        StartCoroutine(AttackCoro());
    }

    private IEnumerator AttackCoro()
    {
        while (this)
        {
            yield return new WaitForSeconds(damageCadance);
            DungeonMaster.Instance.MakeDamage(1);
        }
    }

    private Vector3 GetNextWaypointDelta()
    {
        return currentTarget - transform.position;
    }

    public virtual void OnDamageTaken()
    {
    }
}