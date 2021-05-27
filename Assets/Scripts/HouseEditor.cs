using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using HoudiniEngineUnity;

[ExecuteInEditMode]
public class HouseEditor : MonoBehaviour
{
    public SelectionType CurrentSelection;
    public HEU_HoudiniAsset MyHoudiniAsset;
    public HEU_Parameters MyParameters;
    public Mesh CubeMesh;

    public Transform Roof;
    public Transform RightWall;
    public Transform LeftWall;
    public Transform BackWall;
    public Transform FrontWall;
    public Transform FrontWallBottom;

    public Transform ColliderParent;

    public bool ReSelect = false;

    [SerializeField] private int height;
    [SerializeField] private int width;
    [SerializeField] private int depth;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnDrawGizmosSelected() {
        if (MyParameters == null)
            return;

        if (CurrentSelection == SelectionType.Roof) {
            Gizmos.DrawWireMesh(CubeMesh, Roof.position, transform.rotation, Roof.localScale);
        }
        else if (CurrentSelection == SelectionType.RightWall) {
            Gizmos.DrawWireMesh(CubeMesh, RightWall.position - RightWall.forward * 0.5f, RightWall.rotation, RightWall.localScale);
        }
        else if (CurrentSelection == SelectionType.BackWall) {
            Gizmos.DrawWireMesh(CubeMesh, BackWall.position - BackWall.forward * 0.5f, BackWall.rotation, BackWall.localScale);
        }
        else if (CurrentSelection == SelectionType.Front) {
            Gizmos.DrawWireMesh(CubeMesh, FrontWall.position - FrontWall.forward * 0.25f, FrontWall.rotation, FrontWall.localScale);
        }
        else if (CurrentSelection == SelectionType.FrontBottom) {
            Gizmos.DrawWireMesh(CubeMesh, FrontWallBottom.position - FrontWallBottom.forward * 0.25f, FrontWallBottom.rotation, FrontWallBottom.localScale);
        }
    }

    private void SetParameters() {
        MyHoudiniAsset = GetComponent<HEU_HoudiniAssetRoot>()._houdiniAsset;
        MyParameters = MyHoudiniAsset.Parameters;
    }

    public void Cooked(HEU_HoudiniAsset h, bool b, List<GameObject> l) {
        SetParameters();
        height = MyParameters.GetParameter("height")._intValues[0];
        width = MyParameters.GetParameter("width")._intValues[0];
        depth = MyParameters.GetParameter("depth")._intValues[0];

        //Roof.transform.localPosition = Vector3.up * (height * 2f + 1f) - Vector3.right * depth / 2f - Vector3.forward * width / 2f;
        Roof.parent = transform;
        Roof.localPosition = Vector3.up * (height * 2f + 1f) - Vector3.right * width / 2f - Vector3.forward * depth / 2f;
        Roof.localScale = new Vector3(width, 2f, depth);
        Roof.localRotation = Quaternion.identity;
        Roof.parent = ColliderParent;

        RightWall.parent = transform;
        RightWall.localPosition = Vector3.up * (height * 2f) / 2f - Vector3.right * width - Vector3.forward * depth / 2f;
        RightWall.localScale = new Vector3(depth, height * 2f, 1f);
        RightWall.localRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
        RightWall.parent = ColliderParent;

        LeftWall.parent = transform;
        LeftWall.localPosition = Vector3.up * (height * 2f) / 2f - Vector3.forward * depth / 2f;
        LeftWall.localScale = new Vector3(depth, height * 2f, 1f);
        LeftWall.localRotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
        LeftWall.parent = ColliderParent;

        BackWall.parent = transform;
        BackWall.localPosition = Vector3.up * (height * 2f) / 2f - Vector3.right * width / 2f - Vector3.forward * depth;
        BackWall.localScale = new Vector3(width, height * 2f, 1f);
        BackWall.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        BackWall.parent = ColliderParent;

        FrontWall.parent = transform;
        FrontWall.localPosition = Vector3.up * (1f + height * 2f / 2f) - Vector3.right * width / 2f;
        FrontWall.localScale = new Vector3(width, height * 2f - 2f, 0.5f);
        FrontWall.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        FrontWall.parent = ColliderParent;

        FrontWallBottom.parent = transform;
        FrontWallBottom.localPosition = Vector3.up * 1f - Vector3.right * width / 2f;
        FrontWallBottom.localScale = new Vector3(width, 2f, 0.5f);
        FrontWallBottom.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        FrontWallBottom.parent = ColliderParent;

        MeshRenderer mr = transform.GetChild(1).GetComponent<MeshRenderer>();
        int lastEmpty = mr.sharedMaterials.Length;
        if (mr != null) {
            for (int i = mr.sharedMaterials.Length - 1; i >= 0; i--) {
                if (mr.sharedMaterials[i] == null) {
                    lastEmpty = i;
                    break;
                }
            }

            if (lastEmpty != mr.sharedMaterials.Length) {
                List<Material> tempMats = new List<Material>(mr.sharedMaterials);
                for (int i = tempMats.Count - 1; i >= 0; i--) {
                    if (tempMats[i] == null)
                        tempMats.RemoveAt(i);
                }
                mr.sharedMaterials = tempMats.ToArray();
            }
        }
    }

    public void AddHeight() {
        MyParameters.GetParameter("height")._intValues = new int[] { height + 1 };
        MyHoudiniAsset.RequestCook(true, false, false, true);
    }

    public void ReduceHeight() {
        if (height - 1 < 1)
            return;
        MyParameters.GetParameter("height")._intValues = new int[] { height - 1 };
        MyHoudiniAsset.RequestCook(true, false, false, true);
    }

    public void SetHeight(int newHeight) {
        if (newHeight < 1)
            return;

        MyParameters.GetParameter("height")._intValues = new int[] { newHeight };
        MyHoudiniAsset.RequestCook(true, false, false, true);
    }

    public void AddWidth() {
        MyParameters.GetParameter("width")._intValues = new int[] { width + 1 };
        MyHoudiniAsset.RequestCook(true, false, false, true);
    }

    public void ReduceWidth() {
        if (width - 1 < 1)
            return;

        MyParameters.GetParameter("width")._intValues = new int[] { width - 1 };
        MyHoudiniAsset.RequestCook(true, false, false, true);
    }

    public void SetWidth(int newWidth) {
        if (newWidth < 1)
            return;

        MyParameters.GetParameter("width")._intValues = new int[] { newWidth };
        MyHoudiniAsset.RequestCook(true, false, false, true);
    }

    public void AddDepth() {
        MyParameters.GetParameter("depth")._intValues = new int[] { depth + 1 };
        MyHoudiniAsset.RequestCook(true, false, false, true);
    }

    public void ReduceDepth() {
        if (depth - 1 < 1)
            return;

        MyParameters.GetParameter("depth")._intValues = new int[] { depth - 1 };
        MyHoudiniAsset.RequestCook(true, false, false, true);
    }

    public void SetDepth(int newDepth) {
        if (newDepth < 1)
            return;

        MyParameters.GetParameter("depth")._intValues = new int[] { newDepth };
        MyHoudiniAsset.RequestCook(true, false, false, true);
    }

    public void AddWindows() {
        float f = MyParameters.GetParameter("windows")._floatValues[0] - 0.1f;

        if (f < 0f)
            f = 0.01f;

        MyParameters.GetParameter("windows")._floatValues = new float[] { f };
        MyHoudiniAsset.RequestCook(true, false, false, true);
    }

    public void ReduceWindows() {
        float f = MyParameters.GetParameter("windows")._floatValues[0] + 0.1f;

        if (f > 1f)
            f = 0.99f;

        MyParameters.GetParameter("windows")._floatValues = new float[] { f };
        MyHoudiniAsset.RequestCook(true, false, false, true);
    }

    public void SetWindows(int value) {
        float f = 1f - (value / 9f);

        MyParameters.GetParameter("windows")._floatValues = new float[] { f };
        MyHoudiniAsset.RequestCook(true, false, false, true);
    }

    public void AddWindowsBottom() {
        float f = MyParameters.GetParameter("windowsBottom")._floatValues[0] - 0.1f;

        if (f < 0f)
            f = 0.01f;

        MyParameters.GetParameter("windowsBottom")._floatValues = new float[] { f };
        MyHoudiniAsset.RequestCook(true, false, false, true);
    }

    public void ReduceWindowsBottom() {
        float f = MyParameters.GetParameter("windowsBottom")._floatValues[0] + 0.1f;

        if (f > 1f)
            f = 0.99f;

        MyParameters.GetParameter("windowsBottom")._floatValues = new float[] { f };
        MyHoudiniAsset.RequestCook(true, false, false, true);
    }

    public void SetWindowsBottom(int value) {
        float f = 1f - (value / 9f);

        MyParameters.GetParameter("windowsBottom")._floatValues = new float[] { f };
        MyHoudiniAsset.RequestCook(true, false, false, true);
    }

    public void SetColor(float redValue) {
        Color c = MyParameters.GetParameter("myColor")._color;
        c.r = redValue;
        MyParameters.GetParameter("myColor")._color = c;
        MyHoudiniAsset.RequestCook(true, false, false, true);
    }

    public void SetIvy(int state) {
        if (state == 1) {
            MyParameters.GetParameter("ivyEnabled")._toggle = false;
        }
        else if (state == 2) {
            MyParameters.GetParameter("ivyEnabled")._toggle = true;
            MyParameters.GetParameter("ivyNO")._toggle = false;
        }
        else if (state == 3) {
            MyParameters.GetParameter("ivyEnabled")._toggle = true;
            MyParameters.GetParameter("ivyNO")._toggle = true;
        }

        MyHoudiniAsset.RequestCook(true, false, false, true);
    }

    public void SetBalcony(int state) {
        if (state - 1 >= 0 && state - 1 <= 2) {
            MyParameters.GetParameter("balconyType")._intValues = new int[] { state - 1 };
            MyHoudiniAsset.RequestCook(true, false, false, true);
        }
    }

    public void SetInsideFloor(int floor) {
        if (floor == 9) floor = 0;
        MyParameters.GetParameter("insideFloor")._intValues = new int[] { floor };
        MyHoudiniAsset.RequestCook(true, false, false, true);
    }

    public void SetObstruction(int wall) {
        if(wall == 1)
            MyParameters.GetParameter("rightObstructed")._toggle = !MyParameters.GetParameter("rightObstructed")._toggle;
        else if(wall == 2)
            MyParameters.GetParameter("leftObstructed")._toggle = !MyParameters.GetParameter("leftObstructed")._toggle;
        else if (wall == 3)
            MyParameters.GetParameter("backObstructed")._toggle = !MyParameters.GetParameter("backObstructed")._toggle;
        MyHoudiniAsset.RequestCook(true, false, false, true);
    }

    public void RandomizeWindows() {
        float f = Random.value;
        MyParameters.GetParameter("windowSeed")._floatValues = new float[] { f };
        MyHoudiniAsset.RequestCook(true, false, false, true);
    }
}

public enum SelectionType
{
    None,
    Roof,
    RightWall,
    BackWall,
    Front,
    FrontBottom
}
