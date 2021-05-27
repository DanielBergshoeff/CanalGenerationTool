using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HouseEditor))]
public class HouseEditorEditor : Editor
{
    private ChoiceSelection currentChoiceSelection;
    private WindowChoiceSelection currentWindowChoice;

    private List<KeyCode> keyCodes = new List<KeyCode> { KeyCode.Keypad0, KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9 };

    private void OnSceneGUI() {
        HouseEditor h = target as HouseEditor;

        if (h == null || h.gameObject == null)
            return;

        SetSelection(h);

        Event e = Event.current;
        switch (e.type) {
            case EventType.KeyDown:
                if (e.keyCode == KeyCode.U)
                    Add(h.CurrentSelection, h);
                else if (e.keyCode == KeyCode.I)
                    Reduce(h.CurrentSelection, h);
                else if (e.keyCode == KeyCode.C)
                    SetChoiceSelection(ChoiceSelection.Color);
                else if (e.keyCode == KeyCode.O) {
                    SetChoiceSelection(ChoiceSelection.Windows);
                }
                else if (e.keyCode == KeyCode.P) {
                    SetChoiceSelection(ChoiceSelection.Ivy);
                }
                else if(e.keyCode == KeyCode.LeftBracket) {
                    h.RandomizeWindows();
                }

                if (keyCodes.Contains(e.keyCode)) {
                    switch (currentChoiceSelection) {
                        case ChoiceSelection.None:
                            Set(h.CurrentSelection, h, keyCodes.IndexOf(e.keyCode));
                            break;
                        case ChoiceSelection.Color:
                            SetColor(h, keyCodes.IndexOf(e.keyCode));
                            break;
                        case ChoiceSelection.Windows:
                            if (currentWindowChoice == WindowChoiceSelection.None) {
                                if (keyCodes.IndexOf(e.keyCode) < System.Enum.GetValues(typeof(WindowChoiceSelection)).Length)
                                    currentWindowChoice = (WindowChoiceSelection)keyCodes.IndexOf(e.keyCode);
                            }
                            else {
                                if (keyCodes.IndexOf(e.keyCode) == 0)
                                    currentWindowChoice = WindowChoiceSelection.None;
                                else {
                                    if (currentWindowChoice == WindowChoiceSelection.Balcony) {
                                        h.SetBalcony(keyCodes.IndexOf(e.keyCode));
                                    }
                                    else if (currentWindowChoice == WindowChoiceSelection.InsideFloor) {
                                        h.SetInsideFloor(keyCodes.IndexOf(e.keyCode));
                                    }
                                    else if(currentWindowChoice == WindowChoiceSelection.Obstruction) {
                                        h.SetObstruction(keyCodes.IndexOf(e.keyCode));
                                    }
                                }
                            }
                            break;
                        case ChoiceSelection.Ivy:
                            SetIvy(h, keyCodes.IndexOf(e.keyCode));
                            break;
                        default:
                            break;
                    }
                }

                break;
        }

        
        Handles.BeginGUI();

        Rect[] nrButtons = new Rect[0];
        Rect[] nrButtons2 = new Rect[0];

        switch (currentChoiceSelection) {
            case ChoiceSelection.None:
                break;
            case ChoiceSelection.Color:
                nrButtons = DrawButtons(Screen.width / 2f, Screen.height - 200f, 100f, 50f, 50f, "1 - WHITE", "2 - BEIGE", "3 - PURPLE", "4 - ORANGE");
                break;
            case ChoiceSelection.Windows:
                nrButtons = DrawButtons(Screen.width / 2f, Screen.height - 200f, 100f, 50f, 50f, "1 - BALCONY", "2 - INSIDE FLOOR", "3 - OBSTRUCTION");
                switch (currentWindowChoice) {
                    case WindowChoiceSelection.None:
                        break;
                    case WindowChoiceSelection.Balcony:
                        nrButtons2 = DrawButtons(Screen.width / 2f, Screen.height - 300f, 100f, 50f, 50f, "1 - STONE", "2 - PILLARS", "3 - IRON", "0 - RETURN");
                        break;
                    case WindowChoiceSelection.InsideFloor:
                        nrButtons2 = DrawButtons(Screen.width / 2f, Screen.height - 300f, 100f, 50f, 50f, "1 - FIRST FLOOR", "2 - SECOND FLOOR", "3 - THIRD FLOOR", "4 - FOURTH FLOOR", "5 - FIFTH FLOOR", "6 - SIXTH FLOOR", "7 - SEVENTH FLOOR", "8 - EIGTH FLOOR", "9 - OFF", "0 - RETURN");
                        break;
                    case WindowChoiceSelection.Obstruction:
                        nrButtons2 = DrawButtons(Screen.width / 2f, Screen.height - 300f, 100f, 50f, 50f, "1 - LEFT", "2 - RIGHT", "3 - BACK", "0 - RETURN");
                        break;
                    default:
                        break;
                }
                break;
            case ChoiceSelection.Ivy:
                nrButtons = DrawButtons(Screen.width / 2f, Screen.height - 200f, 100f, 50f, 50f, "1 - OFF", "2 - ON", "3 - ON ALL");
                break;
            default:
                break;
        }

        

        Rect[] buttons = DrawButtons(Screen.width / 2f, Screen.height - 100f, 100f, 50f, 50f, "U - ADD", "I - REDUCE", "NUM - SET", "C - COLOR", "O - WINDOWS", "P - IVY", "{ - RANDOMIZE WINDOWS");
        if (Event.current.type == EventType.MouseDown) {
            bool buttonPressed = false;

            for (int i = 0; i < nrButtons.Length; i++) {
                if (!nrButtons[i].Contains(Event.current.mousePosition))
                    continue;

                switch (currentChoiceSelection) {
                    case ChoiceSelection.None:
                        Set(h.CurrentSelection, h, i + 1);
                        break;
                    case ChoiceSelection.Color:
                        SetColor(h, i + 1);
                        break;
                    case ChoiceSelection.Windows:
                        if(i + 1 < System.Enum.GetValues(typeof(WindowChoiceSelection)).Length)
                            currentWindowChoice = (WindowChoiceSelection)i+1;
                        break;
                    case ChoiceSelection.Ivy:
                        SetIvy(h, i + 1);
                        break;
                    default:
                        break;
                }

                buttonPressed = true;
                h.ReSelect = true;
            }

            for (int i = 0; i < nrButtons2.Length; i++) {
                if (!nrButtons2[i].Contains(Event.current.mousePosition))
                    continue;

                if (i < nrButtons2.Length - 1) {
                    switch (currentWindowChoice) {
                        case WindowChoiceSelection.None:
                            break;
                        case WindowChoiceSelection.Balcony:
                            h.SetBalcony(i + 1);
                            break;
                        case WindowChoiceSelection.InsideFloor:
                            h.SetInsideFloor(i + 1);
                            break;
                        case WindowChoiceSelection.Obstruction:
                            h.SetObstruction(i + 1);
                            break;
                        default:
                            break;
                    }
                }
                else
                    currentWindowChoice = WindowChoiceSelection.None;

                buttonPressed = true;
                h.ReSelect = true;
            }

            for (int i = 0; i < buttons.Length; i++) {
                if (!buttons[i].Contains(Event.current.mousePosition))
                    continue;

                switch (i) {
                    case 0:
                        Add(h.CurrentSelection, h);
                        break;
                    case 1:
                        Reduce(h.CurrentSelection, h);
                        break;
                    case 3:
                        SetChoiceSelection(ChoiceSelection.Color);
                        break;
                    case 4:
                        SetChoiceSelection(ChoiceSelection.Windows);
                        break;
                    case 5:
                        SetChoiceSelection(ChoiceSelection.Ivy);
                        break;
                    default:
                        break;
                }

                buttonPressed = true;
                h.ReSelect = true;
            }

            if (!buttonPressed)
                h.ReSelect = false;
        }

        if (h.ReSelect) {
            Selection.activeGameObject = h.gameObject;
        }

        Handles.EndGUI();
    }

    /// <summary>
    /// Set current selection of house based on mouse position
    /// </summary>
    /// <param name="h"></param>
    private void SetSelection(HouseEditor h) {
        Vector3 mousePosition = Event.current.mousePosition;
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
        //Debug.Log(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000f)) {
            //Debug.Log(hit.collider.name);
            if (hit.collider.gameObject.name == "Roof") {
                h.CurrentSelection = SelectionType.Roof;
            }
            else if (hit.collider.gameObject.name == "RightWall") {
                h.CurrentSelection = SelectionType.RightWall;
            }
            else if (hit.collider.gameObject.name == "BackWall") {
                h.CurrentSelection = SelectionType.BackWall;
            }
            else if (hit.collider.gameObject.name == "FrontWall") {
                h.CurrentSelection = SelectionType.Front;
            }
            else if (hit.collider.gameObject.name == "FrontWallBottom") {
                h.CurrentSelection = SelectionType.FrontBottom;
            }
        }
    }

    private Rect[] DrawButtons(float xPos, float yPos, float width, float height, float distance, params string[] buttons) {
        Rect[] r = new Rect[buttons.Length];

        for (int i = 0; i < buttons.Length; i++) {
            float pos = xPos - (width + distance) * buttons.Length / 2f + (width + distance) * i;
            Rect rect = new Rect(pos, yPos, width, height);
            GUI.color = Color.white;
            GUI.Box(rect, buttons[i]);
            r[i] = rect;
        }

        return r;
    }

    private void SetChoiceSelection(ChoiceSelection selection) {
        if (currentChoiceSelection != selection)
            currentChoiceSelection = selection;
        else
            currentChoiceSelection = ChoiceSelection.None;
    }

    private void SetIvy(HouseEditor h, int index) {
        h.SetIvy(index);
    }

    private void SetColor(HouseEditor h, int index) {
        if (index == 1)
            h.SetColor(0f + 0.01f);
        else if (index == 2)
            h.SetColor(1f / 4 + 0.01f);
        else if (index == 3)
            h.SetColor(2f / 4 + 0.01f);
        else if (index == 4)
            h.SetColor(3f / 4 + 0.01f);
    }

    private void Add(SelectionType s, HouseEditor h) {
        switch (s) {
            case SelectionType.None:
                break;
            case SelectionType.Roof:
                h.AddHeight();
                break;
            case SelectionType.RightWall:
                h.AddWidth();
                break;
            case SelectionType.BackWall:
                h.AddDepth();
                break;
            case SelectionType.Front:
                h.AddWindows();
                break;
            case SelectionType.FrontBottom:
                h.AddWindowsBottom();
                break;
            default:
                break;
        }
    }

    private void Reduce(SelectionType s, HouseEditor h) {
        switch (s) {
            case SelectionType.None:
                break;
            case SelectionType.Roof:
                h.ReduceHeight();
                break;
            case SelectionType.RightWall:
                h.ReduceWidth();
                break;
            case SelectionType.BackWall:
                h.ReduceDepth();
                break;
            case SelectionType.Front:
                h.ReduceWindows();
                break;
            case SelectionType.FrontBottom:
                h.ReduceWindowsBottom();
                break;
            default:
                break;
        }
    }

    private void Set(SelectionType s, HouseEditor h, int length) {
        switch (s) {
            case SelectionType.None:
                break;
            case SelectionType.Roof:
                h.SetHeight(length);
                break;
            case SelectionType.RightWall:
                h.SetWidth(length);
                break;
            case SelectionType.BackWall:
                h.SetDepth(length);
                break;
            case SelectionType.Front:
                h.SetWindows(length);
                break;
            case SelectionType.FrontBottom:
                h.SetWindowsBottom(length);
                break;
            default:
                break;
        }
    }
}

public enum ChoiceSelection
{
    None,
    Color,
    Windows,
    Ivy
}

public enum WindowChoiceSelection
{
    None,
    Balcony,
    InsideFloor,
    Obstruction
}
