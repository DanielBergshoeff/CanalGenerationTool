using HoudiniEngineUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternCube : MonoBehaviour
{
    protected static bool cookNow = false;
    protected static HEU_HoudiniAsset MyHoudiniAsset;

    protected static LanternCube[] currentlyCooking;

    public void ConvertThis() {
        currentlyCooking = new LanternCube[1];
        currentlyCooking[0] = this;
        ConvertCubes(currentlyCooking);
    }
    public void ConvertAllCubes() {
        currentlyCooking = GameObject.FindObjectsOfType<LanternCube>();
        ConvertCubes(currentlyCooking);
    }

    public void ConvertCubes(LanternCube[] cubes) {
        CheckHoudiniAsset();
        SetNewInput(cubes);
        CheckHoudiniAssetLanternLight();
        SetNewInput(cubes);
    }

    protected static void SetNewInput(LanternCube[] cubes) {
        MyHoudiniAsset.GetInputNodeByIndex(0).RemoveAllInputEntries();
        foreach (LanternCube gc in cubes) {
            MyHoudiniAsset.GetInputNodeByIndex(0).AddInputEntryAtEnd(gc.transform.GetChild(0).gameObject);
        }

        cookNow = true;
        MyHoudiniAsset.RequestCook(true, false, false, true);
    }

    protected void CheckHoudiniAsset() {
        CheckHoudiniAssetLantern();
    }

    protected static void CheckHoudiniAssetLantern() {
        MyHoudiniAsset = FindObjectOfType<LanternGeneratorObject>().GetComponent<HEU_HoudiniAssetRoot>()._houdiniAsset;
    }

    protected static void CheckHoudiniAssetLanternLight() {
        MyHoudiniAsset = FindObjectOfType<LanternLightGeneratorObject>().GetComponent<HEU_HoudiniAssetRoot>()._houdiniAsset;
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

            currentlyCooking = null;
        }

        return g;
    }
}
