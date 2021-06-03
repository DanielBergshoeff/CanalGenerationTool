using HoudiniEngineUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildingGeneratorObject : MonoBehaviour
{
    public void BuildingBaked(HEU_HoudiniAsset h, bool b, List<GameObject> l) {
        Debug.Log("Baked");
        foreach(GameObject g in l) {
            Debug.Log(g.name);
            if (g.GetComponent<MeshFilter>() == null)
                continue;

            Debug.Log(g.name + ": secondary uvs");
            Unwrapping.GenerateSecondaryUVSet(g.GetComponent<MeshFilter>().sharedMesh);
        }
    }
}
