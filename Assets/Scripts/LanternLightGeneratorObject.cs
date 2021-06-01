using HoudiniEngineUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LanternLightGeneratorObject : MonoBehaviour
{
    public Transform LanternParent;
    private float timeToRecook = float.PositiveInfinity;

    private void Update() {
        if (Time.realtimeSinceStartup > timeToRecook) {
            ReCook();
            timeToRecook = float.PositiveInfinity;
        }
    }

    public void LanternsCooked(HEU_HoudiniAsset h, bool b, List<GameObject> l) {
        GameObject g = LanternCube.CookingComplete(true);
        if (g == null)
            return;

        if (g.transform.childCount > 1) {
            int cc = g.transform.childCount;
            for (int i = cc - 1; i >= 0; i--) {
                g.transform.GetChild(i).parent = LanternParent.GetChild(LanternParent.childCount - 1).GetChild(i);
            }
        }
        else {
            g.transform.GetChild(0).parent = LanternParent.GetChild(LanternParent.childCount - 1);
        }

        DestroyImmediate(g.transform.gameObject);
    }

    public void LanternsBaked(HEU_HoudiniAsset h, bool b, List<GameObject> l) {
        LanternGeneratorObject.Instance.ManualBaked();
        timeToRecook = Time.realtimeSinceStartup + 0.2f;
    }
    public void ReCook() {
        GetComponent<HEU_HoudiniAssetRoot>()._houdiniAsset.RequestCook(true, false, false, true, true);
    }
}
