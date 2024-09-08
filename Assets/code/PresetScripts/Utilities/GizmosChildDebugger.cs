using UnityEditor;
using UnityEngine;

public class GizmosChildDebugger : MonoBehaviour
{
    [SerializeField] Color _color = Color.red;
    [SerializeField] bool _drawGizmos = true;
    void OnDrawGizmos()
    {
        if(!_drawGizmos) return;
        Gizmos.color = _color;
        foreach(Transform child in transform)
        {
            Gizmos.DrawLine(transform.position, child.position);
        }
    }
}
