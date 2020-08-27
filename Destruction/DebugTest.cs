using UnityEngine;

public class DebugTest : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }
}
