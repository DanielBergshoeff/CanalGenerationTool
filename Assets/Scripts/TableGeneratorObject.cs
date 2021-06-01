using HoudiniEngineUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TableGeneratorObject : MonoBehaviour
{
    public Transform TableParent;

    private float timeToRecook = float.PositiveInfinity;

    private void Update() {
        if (Time.realtimeSinceStartup > timeToRecook) {
            ReCook();
            timeToRecook = float.PositiveInfinity;
        }
    }

    public void TablesCooked(HEU_HoudiniAsset h, bool b, List<GameObject> l) {
        GameObject g = TableCube.CookingComplete();
        if (g == null)
            return;

        g.transform.parent = TableParent;
    }

    public void TablesBaked(HEU_HoudiniAsset h, bool b, List<GameObject> l) {
        timeToRecook = Time.realtimeSinceStartup + 0.1f;
    }

    public void ReCook() {
        GetComponent<HEU_HoudiniAssetRoot>()._houdiniAsset.RequestCook(true, false, false, true, true);
    }
}
