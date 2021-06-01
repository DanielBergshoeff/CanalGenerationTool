using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(GeneratorCube))]
public abstract class GeneratorCubeEditor : Editor
{
    protected void OnSceneGUI() {
        GeneratorCube gc = target as GeneratorCube;

        if (gc == null || gc.gameObject == null)
            return;

        bool[] buttons = DrawButtons(Screen.width / 2f, Screen.height - 100f, 100f, 50f, 50f, "Convert to cube", "Convert all to cubes");
    }

    protected bool[] DrawButtons(float xPos, float yPos, float width, float height, float distance, params string[] buttons) {
        bool[] b = new bool[buttons.Length];

        for (int i = 0; i < buttons.Length; i++) {
            float pos = xPos - (width + distance) * buttons.Length / 2f + (width + distance) * i;
            Rect rect = new Rect(pos, yPos, width, height);
            GUI.color = Color.white;
            b[i] = GUI.Button(rect, buttons[i]);
        }

        return b;
    }
}
