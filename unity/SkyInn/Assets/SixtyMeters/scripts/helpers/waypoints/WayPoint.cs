using UnityEngine;

namespace SixtyMeters.scripts.helpers.waypoints
{
    public class WayPoint : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    
        void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(transform.position, new Vector3(0.2f, 0.2f, 0.2f));
        }

    }
}
