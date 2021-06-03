using HoudiniEngineUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneratorCube<T> : MonoBehaviour where T: Component
{
    protected static Transform[] currentlyCooking;
    protected static bool cookNow = false;


    protected static HEU_HoudiniAsset MyHoudiniAsset;
    protected static HEU_Parameters MyParameters;

    protected void CheckHoudiniAsset() {
        MyHoudiniAsset = FindObjectOfType<T>().GetComponent<HEU_HoudiniAssetRoot>()._houdiniAsset;
    }
    public abstract void ConvertAllCubes();

    public void ConvertThis() {
        currentlyCooking = new Transform[1];
        currentlyCooking[0] = transform;
        ConvertCubes(currentlyCooking);
    }

    public virtual void ConvertCubes(Transform[] cubes) {
        CheckHoudiniAsset();
        SetNewInput(cubes);
    }

    protected static void SetNewInput(Transform[] cubes) {
        MyHoudiniAsset.GetInputNodeByIndex(0).RemoveAllInputEntries();
        foreach (Transform t in cubes) {
            MyHoudiniAsset.GetInputNodeByIndex(0).AddInputEntryAtEnd(t.GetChild(0).gameObject);
        }

        cookNow = true;
        MyHoudiniAsset.RequestCook(true, false, false, true);
    }

    public static GameObject CookingComplete(bool deleteOriginal = true) {
        if (currentlyCooking == null)
            return null;

        if (currentlyCooking.Length < 1 || !cookNow)
            return null;

        cookNow = false;

        GameObject g = MyHoudiniAsset.BakeToNewStandalone();

        HEU_SessionBase session = MyHoudiniAsset.GetAssetSession(true);
        MyHoudiniAsset.GetInputNodeByIndex(0).ResetInputNode(session);
        MyHoudiniAsset.RequestCook(true, false, false, true);

        if (deleteOriginal) {
            for (int i = currentlyCooking.Length - 1; i >= 0; i--) {
                DestroyImmediate(currentlyCooking[i].gameObject);
            }
        }

        currentlyCooking = null;

        return g;
    }
}
