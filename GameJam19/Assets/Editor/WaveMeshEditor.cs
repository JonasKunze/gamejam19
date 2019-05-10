using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaveMesh))]
public class WaveMeshEditor : Editor
{
    private WaveMesh _waveMesh;
    
    private void OnEnable()
    {
        _waveMesh = (WaveMesh) target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();


        if (GUILayout.Button("Create Mesh"))
        {
           _waveMesh.CreateMesh(); 
        }
    }
}
