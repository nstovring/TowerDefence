using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(TerrainGenerator))]
public class TerrainGeneratorEditor : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TerrainGenerator gen = target as TerrainGenerator;

        if (GUILayout.Button("Generate Terrain"))
        {
            gen.GenerateTerrain();
        }
    }
}
