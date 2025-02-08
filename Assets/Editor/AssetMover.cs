using UnityEngine;
using UnityEditor;
using System.IO;

public class AssetMover : EditorWindow
{
    private Material[] selectedMaterials;
    private Vector2 scrollPosition;
    private bool moveShaders = true;
    private bool moveTextures = true;

    [MenuItem("Tools/Asset Mover")]
    public static void ShowWindow()
    {
        GetWindow<AssetMover>("Asset Mover");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Material Asset Mover", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // Options
        moveShaders = EditorGUILayout.Toggle("Move Shaders", moveShaders);
        moveTextures = EditorGUILayout.Toggle("Move Textures", moveTextures);

        EditorGUILayout.Space();

        // Material selection
        if (GUILayout.Button("Get Selected Materials"))
        {
            GetSelectedMaterials();
        }

        // Display selected materials
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Selected Materials:", EditorStyles.boldLabel);
        
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        if (selectedMaterials != null)
        {
            foreach (var material in selectedMaterials)
            {
                EditorGUILayout.ObjectField(material, typeof(Material), false);
            }
        }
        EditorGUILayout.EndScrollView();

        // Move assets button
        EditorGUI.BeginDisabledGroup(selectedMaterials == null || selectedMaterials.Length == 0);
        if (GUILayout.Button("Move Assets"))
        {
            MoveAssets();
        }
        EditorGUI.EndDisabledGroup();
    }

    private void GetSelectedMaterials()
    {
        selectedMaterials = Selection.GetFiltered<Material>(SelectionMode.Assets);
        if (selectedMaterials.Length == 0)
        {
            Debug.LogWarning("No materials selected!");
        }
    }

    private void MoveAssets()
    {
        foreach (var material in selectedMaterials)
        {
            string materialPath = AssetDatabase.GetAssetPath(material);
            string materialDirectory = Path.GetDirectoryName(materialPath);

            if (moveShaders && material.shader != null)
            {
                string shaderPath = AssetDatabase.GetAssetPath(material.shader);
                if (!string.IsNullOrEmpty(shaderPath))
                {
                    string newPath = Path.Combine(materialDirectory, Path.GetFileName(shaderPath));
                    if (shaderPath != newPath)
                    {
                        AssetDatabase.MoveAsset(shaderPath, newPath);
                        Debug.Log($"Moved shader to: {newPath}");
                    }
                }
            }

            if (moveTextures)
            {
                string[] texturePropertyNames = material.GetTexturePropertyNames();
                foreach (string propertyName in texturePropertyNames)
                {
                    Texture texture = material.GetTexture(propertyName);
                    if (texture != null)
                    {
                        string texturePath = AssetDatabase.GetAssetPath(texture);
                        if (!string.IsNullOrEmpty(texturePath))
                        {
                            string newPath = Path.Combine(materialDirectory, Path.GetFileName(texturePath));
                            if (texturePath != newPath)
                            {
                                AssetDatabase.MoveAsset(texturePath, newPath);
                                Debug.Log($"Moved texture to: {newPath}");
                            }
                        }
                    }
                }
            }
        }

        AssetDatabase.Refresh();
        Debug.Log("Assets moved successfully!");
    }
}
