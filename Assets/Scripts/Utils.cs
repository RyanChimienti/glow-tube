using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    public static float RAD_TO_DEG = 57.29577951f;

    /// <summary>
    /// Find some projected angle measure off some forward around some axis.
    /// Credit to lordofduct from the Unity forums:
    /// https://forum.unity.com/threads/is-vector3-signedangle-working-as-intended.694105/
    /// </summary>
    public static float AngleOffAroundAxis(Vector3 v, Vector3 forward, Vector3 axis, bool clockwise = false) {
        Vector3 right;
        if (clockwise) {
            right = Vector3.Cross(forward, axis);
            forward = Vector3.Cross(axis, right);
        }
        else {
            right = Vector3.Cross(axis, forward);
            forward = Vector3.Cross(right, axis);
        }
        return Mathf.Atan2(Vector3.Dot(v, right), Vector3.Dot(v, forward)) * RAD_TO_DEG;
    }

    /// <summary>
    /// Returns the bounding box of an object's colliders, relative to the standard axes.
    /// Takes into account the object itself and all its descendants. Assumes the position of
    /// the object's transform is within the bounding box. 
    /// </summary>
    public static Bounds GetBoundingBox(GameObject obj) {
        Bounds boundingBox = new Bounds(obj.transform.position, Vector3.zero);
        Collider[] colliders = obj.GetComponentsInChildren<Collider>(true);
        foreach (Collider col in colliders) {
            boundingBox.Encapsulate(col.bounds);
        }
        return boundingBox;
    }

    /// <summary>
    /// Debug.Log, but your string is prefaced with a more precise timestamp.
    /// </summary>
    public static void DebugLog(string str) {
        Debug.Log($"[{GetSystemTimeString()}] {str}");
    }

    public static string GetSystemTimeString() {
        return TimeToPreciseString(System.DateTime.Now);
    }

    public static string TimeToPreciseString(DateTime time) {
        return time.ToString("H:mm:ss.fff");
    }
}
