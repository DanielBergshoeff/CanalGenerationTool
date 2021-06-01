using HoudiniEngineUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GeneratorCube : MonoBehaviour
{
    protected static GeneratorCube[] currentlyCooking;
    protected static bool cookNow = false;


    protected static HEU_HoudiniAsset MyHoudiniAsset;
    protected static HEU_Parameters MyParameters;

    protected abstract void CheckHoudiniAsset();
    public abstract void ConvertAllCubes();

    public void ConvertThis() {
        currentlyCooking = new GeneratorCube[1];
        currentlyCooking[0] = this;
        ConvertCubes(currentlyCooking);
    }

    public void ConvertCubes(GeneratorCube[] cubes) {
        CheckHoudiniAsset();
        SetNewInput(cubes);
    }

    protected static void SetNewInput(GeneratorCube[] cubes) {
        MyHoudiniAsset.GetInputNodeByIndex(0).RemoveAllInputEntries();
        foreach (GeneratorCube gc in cubes) {
            MyHoudiniAsset.GetInputNodeByIndex(0).AddInputEntryAtEnd(gc.transform.GetChild(0).gameObject);
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
        /*
        if(currentlyCooking.Length == 1) {
            CubeInfo ci = g.AddComponent<CubeInfo>();
            ci.Scale = currentlyCooking[0].transform.localScale;
            ci.Position = currentlyCooking[0].transform.position;
            ci.Rotation = currentlyCooking[0].transform.rotation;
        }
        else {
            for (int i = 0; i < currentlyCooking.Length; i++) {
                CubeInfo ci = g.transform.GetChild(i).gameObject.AddComponent<CubeInfo>();
                ci.Scale = currentlyCooking[i].transform.localScale;
                ci.Position = currentlyCooking[i].transform.position;
                ci.Rotation = currentlyCooking[i].transform.rotation;
            }
        }*/
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
