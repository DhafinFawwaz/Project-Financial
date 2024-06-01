using System.Collections.Generic;
using UnityEngine;

public static class PhysicsExtension
{
    public static RaycastHit[] ConeCastAll(this Physics physics, Vector3 origin, float coneRadius, Vector3 direction, float coneDistance, LayerMask layer)
    {
        List<RaycastHit> hits = new List<RaycastHit>();
        RaycastHit[] raycastHits = Physics.SphereCastAll(origin, coneRadius, direction, coneDistance, layer);
        foreach (RaycastHit hit in raycastHits)
        {
            float angle = Vector3.Angle(direction, hit.point - origin);
            float allowedAngle = Mathf.Atan2(coneRadius, coneDistance) * Mathf.Rad2Deg;
            if (angle < allowedAngle)
            {
                hits.Add(hit);
            }
        }
        return hits.ToArray();
    }
}