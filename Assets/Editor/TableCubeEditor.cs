using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(TableCube))]
public class TableCubeEditor : GeneratorCubeEditor
{
    private void OnSceneGUI() {
        TableCube tc = target as TableCube;

        if (tc == null || tc.gameObject == null)
            return;

        Handles.BeginGUI();

        DrawStandardButtons(tc);

        Handles.EndGUI();
    }

    private void DrawStandardButtons(TableCube tc) {
        bool[] buttons = DrawButtons(Screen.width / 2f, Screen.height - 125f, 200f, 50f, 50f, "CONVERT TO TABLE", "CONVERT ALL TO TABLES");
        for (int i = 0; i < buttons.Length; i++) {
            if (buttons[i] == false)
                continue;
            switch (i) {
                case 0:
                    tc.ConvertThis();
                    break;
                case 1:
                    tc.ConvertAllCubes();
                    break;
                default:
                    break;
            }
        }
    }
}
