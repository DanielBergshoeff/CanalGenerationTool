using HoudiniEngineUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BuildingGeneratorObject : MonoBehaviour
{
    public Transform BuildingParent;

    public void BuildingBaked(HEU_HoudiniAsset h, bool b, List<GameObject> l) {
        foreach(GameObject g in l) {
            Debug.Log(g.name);
            if (g.GetComponent<MeshFilter>() == null)
                continue;

            Unwrapping.GenerateSecondaryUVSet(g.GetComponent<MeshFilter>().sharedMesh);
            g.transform.parent = BuildingParent;
        }
    }
}
