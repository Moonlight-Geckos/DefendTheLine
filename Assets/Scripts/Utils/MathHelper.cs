using UnityEngine;
using UnityEngine.AI;

public static class MathHelper
{
    public static float VesicaPiscisArea(float r)
    {
        return 1.28884f * (r * r);
    }
    public static Vector3 GetPointAtHeight(Ray ray, float height)
    {
        return ray.origin + (((ray.origin.y - height) / -ray.direction.y) * ray.direction);
    }
    public static float CalculatePathDistance(NavMeshPath path, Vector3 startingPos)
    {
        float dis = 0;
        foreach (var u in path.corners)
        {
            dis += Vector3.Distance(u, startingPos);
            startingPos = u;
        }
        return dis;
    }
}
