using HoudiniEngineUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class LanternGeneratorObject : MonoBehaviour
{
    public static LanternGeneratorObject Instance;

    public Transform LanternParent;
    public Material LitLanternMat;

    private float timeToRecook = float.PositiveInfinity;

    private void Update() {
        if(Time.realtimeSinceStartup > timeToRecook) {
            ReCook();
            timeToRecook = float.PositiveInfinity;
        }
    }

    public void LanternsCooked(HEU_HoudiniAsset h, bool b, List<GameObject> l) {
        Instance = this;

        GameObject g = LanternCube.CookingComplete(false);
        if (g == null)
            return;

        g.transform.parent = LanternParent;

        if (g.transform.childCount > 0) {
            for (int i = 0; i < g.transform.childCount; i++) {
                if (g.transform.GetChild(i).GetComponent<MeshRenderer>().sharedMaterial == null) {
                    g.transform.GetChild(i).GetComponent<MeshRenderer>().sharedMaterial = LitLanternMat;
                }
            }
        }
        else {
            if (g.transform.GetComponent<MeshRenderer>().sharedMaterial == null) {
                g.transform.GetComponent<MeshRenderer>().sharedMaterial = LitLanternMat;
            }
        }
    }

    public void LanternsBaked(HEU_HoudiniAsset h, bool b, List<GameObject> l) {
        //timeToRecook = Time.realtimeSinceStartup + 0.1f;
    }

    public void ManualBaked() {
        timeToRecook = Time.realtimeSinceStartup + 0.1f;
    }

    public void ReCook() {
        GetComponent<HEU_HoudiniAssetRoot>()._houdiniAsset.RequestCook(true, false, false, true, true);
    }
}
