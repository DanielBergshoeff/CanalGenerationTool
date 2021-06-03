using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoudiniEngineUnity;

public class TableCube : GeneratorCube<TableGeneratorObject>
{
    public override void ConvertAllCubes() {
        TableCube[] tcs = GameObject.FindObjectsOfType<TableCube>();
        Transform[] gos = new Transform[tcs.Length];
        for (int i = 0; i < tcs.Length; i++) {
            gos[i] = tcs[i].transform;
        }
        currentlyCooking = gos;
        ConvertCubes(currentlyCooking);
    }
}
