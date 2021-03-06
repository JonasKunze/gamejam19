using UnityEngine;

    public class WayPoint : MonoBehaviour
    {
        public enum WaypointType
        {
            START, NORMAL, WATER_BORDER, GOAL
            
        }

        public WaypointType type = WaypointType.NORMAL;
        
        public Vector3 GetRandomTargetPosition()
        {
            return transform.TransformPoint(Vector3.right * Random.Range(-50, 50)/100f);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.DrawRay(transform.position-transform.right*transform.localScale.x/2, transform.right*transform.localScale.x);
        }
    }