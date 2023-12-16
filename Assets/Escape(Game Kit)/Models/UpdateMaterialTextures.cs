using UnityEngine;
using UnityEditor;
using System.IO;

public class UpdateMaterialTextures : MonoBehaviour
{
    [MenuItem("Tools/Update Material Textures to JPG")]
    static void UpdateTextures()
    {
        string[] allMaterials = AssetDatabase.FindAssets("t:Material", new[] { "Assets/Escape(Game Kit)/Models" });

        foreach (string mat in allMaterials)
        {
            string path = AssetDatabase.GUIDToAssetPath(mat);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (material != null)
            {
                var texture = material.mainTexture as Texture2D;
                if (texture != null)
                {
                    string texturePath = AssetDatabase.GetAssetPath(texture);
                    string newTexturePath = Path.ChangeExtension(texturePath, ".jpg");

                    if (AssetDatabase.LoadAssetAtPath<Texture2D>(newTexturePath) != null)
                    {
                        Texture2D newTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(newTexturePath);
                        material.mainTexture = newTexture;
                        EditorUtility.SetDirty(material);
                    }
                }
            }
        }

        AssetDatabase.SaveAssets();
    }
}
