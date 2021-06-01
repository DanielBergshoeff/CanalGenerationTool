using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoudiniEngineUnity;

public class TableCube : MonoBehaviour
{
    protected static TableCube[] currentlyCooking;
    protected static HEU_HoudiniAsset MyHoudiniAsset;
    protected static bool cookNow = false;

    public void ConvertThis() {
        currentlyCooking = new TableCube[1];
        currentlyCooking[0] = this;
        ConvertCubes(currentlyCooking);
    }

    public void ConvertAllCubes() {
        currentlyCooking = GameObject.FindObjectsOfType<TableCube>();
        ConvertCubes(currentlyCooking);
    }

    public void ConvertCubes(TableCube[] cubes) {
        CheckHoudiniAsset();
        SetNewInput(cubes);
    }

    protected static void SetNewInput(TableCube[] cubes) {
        MyHoudiniAsset.GetInputNodeByIndex(0).RemoveAllInputEntries();
        foreach (TableCube gc in cubes) {
            MyHoudiniAsset.GetInputNodeByIndex(0).AddInputEntryAtEnd(gc.transform.GetChild(0).gameObject);
        }

        cookNow = true;
        MyHoudiniAsset.RequestCook(true, false, false, true);
    }


    protected void CheckHoudiniAsset() {
        CheckHoudiniAssetTable();
    }

    protected static void CheckHoudiniAssetTable() {
        MyHoudiniAsset = FindObjectOfType<TableGeneratorObject>().GetComponent<HEU_HoudiniAssetRoot>()._houdiniAsset;
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
