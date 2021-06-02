using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class TempUVChildren : MonoBehaviour
{
    [MenuItem("Set UV children/First Command _p")]
    public static void SetUVChildren() {
        Debug.Log("Set UV");


        Unwrapping.GenerateSecondaryUVSet(FindObjectOfType<TempUVChildren>().GetComponent<MeshFilter>().sharedMesh);
    }
}
