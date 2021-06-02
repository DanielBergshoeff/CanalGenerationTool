using HoudiniEngineUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternCube : GeneratorCube<LanternGeneratorObject>
{
    public override void ConvertAllCubes() {
        LanternCube[] tcs = GameObject.FindObjectsOfType<LanternCube>();
        Transform[] gos = new Transform[tcs.Length];
        for (int i = 0; i < tcs.Length; i++) {
            gos[i] = tcs[i].transform;
        }
        currentlyCooking = gos;
        ConvertCubes(currentlyCooking);
    }

    public override void ConvertCubes(Transform[] cubes) {
        CheckHoudiniAsset();
        SetNewInput(cubes);
        currentlyCooking = cubes;
        CheckHoudiniAssetLanternLight();
        SetNewInput(cubes);
    }

    protected static void CheckHoudiniAssetLanternLight() {
        MyHoudiniAsset = FindObjectOfType<LanternLightGeneratorObject>().GetComponent<HEU_HoudiniAssetRoot>()._houdiniAsset;
    }
}
