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

    [SerializeField] private Animator animator;

    [SerializeField] [Range(0.1f, 10)] protected float maxVelocity;

    [SerializeField] private GameObject WalkAnim;
    [SerializeField] private GameObject JumpAnim;
    [SerializeField] private GameObject SwimAnim;

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

        WalkAnim.SetActive(true);
        SwimAnim.SetActive(false);
    }

    void Update()
    {
        if (!isLastWaypointReached)
        {
            var toNextWaypoint = GetNextWaypointDelta();
            if (toNextWaypoint.magnitude < waypointSize)
            {
                WaypointReached(wayPoints[currentWayPoint]);

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

    private void WaypointReached(WayPoint wp)
    {
        if (wp.type == WayPoint.WaypointType.WATER_BORDER && SwimAnim)
        {
            WalkAnim.SetActive(false);
            if (JumpAnim)
            {
                JumpAnim.SetActive(true);
                StartCoroutine(StartSwimming());
            }
            else
            {
                StartSwimming();
            }
        }
    }

    private IEnumerator StartSwimming()
    {
        yield return new WaitForSeconds(2);
        JumpAnim.SetActive(false);
        SwimAnim.SetActive(true);
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