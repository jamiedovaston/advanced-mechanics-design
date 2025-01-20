using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TankBASESO), true)]
public class TankSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SerializedProperty bannerImageProperty = serializedObject.FindProperty("BannerImage");
        if (bannerImageProperty != null)
        {
            if (bannerImageProperty.objectReferenceValue != null)
            {
                Texture2D bannerImage = (Texture2D)bannerImageProperty.objectReferenceValue;

                if (bannerImage)
                {
                    float aspectRatio = (float)bannerImage.height / bannerImage.width;

                    float inspectorWidth = EditorGUIUtility.currentViewWidth;

                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(bannerImage, GUILayout.Width(inspectorWidth - 40), GUILayout.Height((inspectorWidth - 40) * aspectRatio));
                    GUILayout.FlexibleSpace();
                    GUILayout.EndHorizontal();
                }
            }
        }

        EditorGUILayout.Space();
        serializedObject.ApplyModifiedProperties();

        base.OnInspectorGUI();
    }
}
