using System.Collections.Generic;
using UnityEngine;
public static class ComponentExtension
{
    public static T FindComponentRecursive<T>(this Component parent) where T : Component
    {
        if (parent == null) return null;

        T component = parent.GetComponent<T>();
        if (component != null)return component;
        foreach (Transform child in parent.transform)
        {
            T result = FindComponentRecursive<T>(child);
            if (result != null) return result;
        }
        return null;
    }

    public static bool TryFindComponentRecursive<T>(this Component parent, ref T component) where T : Component
    {
        if (parent == null) return false;

        component = parent.GetComponent<T>();
        if (component != null) return true;
        foreach (Transform child in parent.transform)
        {
            if (TryFindComponentRecursive<T>(child, ref component)) return true;
        }
        return false;
    }

    public static void FindAllComponentRecursive<T>(this Component parent, List<T> components, int maxDepth) where T : Component
    {
        if (parent == null) return;

        T component = parent.GetComponent<T>();
        if (component != null) components.Add(component);
        if (maxDepth == 0) return;
        foreach (Transform child in parent.transform)
        {
            FindAllComponentRecursive<T>(child, components, maxDepth - 1);
        }

    }
}