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
    /// Changes a vector's direction randomly, but by no more than a specified angle.
    /// </summary>
    public static Vector3 RotateRandomly(Vector3 original, float maxAngleDegrees) {
        float rotAngle = (float)(new System.Random().NextDouble()) * maxAngleDegrees;
        
        // We start with the original vector
        Vector3 result = new Vector3(original.x, original.y, original.z);

        // Rotate it the desired rotation angle around an orthogonal axis
        Vector3 orthogonal = GetAnyOrthogonalUnitVector(result);
        result = Quaternion.AngleAxis(rotAngle, orthogonal) * result;

        // Then rotate it a completely random angle around its original direction
        float randomAngle = (float)(new System.Random().NextDouble()) * 360;
        result = Quaternion.AngleAxis(randomAngle, original) * result;

        return result;
    }

    /// <summary>
    /// Returns any orthogonal unit vector to the given vector.
    /// </summary>
    public static Vector3 GetAnyOrthogonalUnitVector(Vector3 v) {
        if (v == Vector3.zero) {
            return Vector3.up;
        }

        Vector3 copyV = new Vector3(v.x, v.y, v.z);

        Vector3 orthogonal = Vector3.zero;
        Vector3.OrthoNormalize(ref copyV, ref orthogonal);
        return orthogonal;
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
