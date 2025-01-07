using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TankBASESO), true)]
public class TankSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Check if the target object has a "BannerImage" field
        SerializedProperty bannerImageProperty = serializedObject.FindProperty("BannerImage");
        if (bannerImageProperty != null)
        {
            if (bannerImageProperty.objectReferenceValue != null)
            {
                // Get the banner image
                Texture2D bannerImage = (Texture2D)bannerImageProperty.objectReferenceValue;

                if (bannerImage)
                {
                    float aspectRatio = (float)bannerImage.height / bannerImage.width;

                    // Get the current Inspector width
                    float inspectorWidth = EditorGUIUtility.currentViewWidth;

                    // Draw the banner image
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(bannerImage, GUILayout.Width(inspectorWidth - 40), GUILayout.Height((inspectorWidth - 40) * aspectRatio));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
            }
        }

        // Draw the rest of the default inspector
        EditorGUILayout.Space();
        serializedObject.ApplyModifiedProperties(); // Apply changes to serialized fields
        base.OnInspectorGUI();
    }
}
