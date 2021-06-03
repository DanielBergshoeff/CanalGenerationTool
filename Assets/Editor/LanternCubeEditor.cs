using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LanternCube))]
public class LanternCubeEditor : GeneratorCubeEditor
{
    private void OnSceneGUI() {
        LanternCube lc = target as LanternCube;

        if (lc == null || lc.gameObject == null)
            return;

        Handles.BeginGUI();

        DrawStandardButtons(lc);

        Rect rect = new Rect(Screen.width / 2f, Screen.height - 100f, 100, 100);
        GUI.color = Color.white;

        Handles.EndGUI();
    }

    private void DrawStandardButtons(LanternCube lc) {
        bool[] buttons = DrawButtons(Screen.width / 2f, Screen.height - 125f, 200f, 50f, 50f, "CONVERT TO LANTERN", "CONVERT ALL TO LANTERNS");
        for (int i = 0; i < buttons.Length; i++) {
            if (buttons[i] == false)
                continue;
            switch (i) {
                case 0:
                    lc.ConvertThis();
                    break;
                case 1:
                    lc.ConvertAllCubes();
                    break;
                default:
                    break;
            }
        }
    }
}
