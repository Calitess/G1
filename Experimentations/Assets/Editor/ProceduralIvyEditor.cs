using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ProceduralIvy))]
public class ProceduralIvyEditor : Editor
{
    private SerializedProperty meshManagerProperty, camProperty, recycleIntervalProperty, branchesProperty, maxPointsForBranchProperty, segmentLengthProperty, branchRadiusProperty, branchMaterialProperty, leafMaterialProperty, flowerMaterialProperty, leafPrefabProperty, flowerPrefabProperty, wantBlossomsProperty, drawIvyProperty;
    private ProceduralIvy proceduralIvy;

    private void OnEnable()
    {
        proceduralIvy = target as ProceduralIvy;
;

        if (proceduralIvy == null)
        {
            return;
        }


        // Get the serialized bool field from the target object
        meshManagerProperty = serializedObject.FindProperty("meshManager");
        camProperty = serializedObject.FindProperty("cam");
        recycleIntervalProperty = serializedObject.FindProperty("recycleInterval");
        branchesProperty = serializedObject.FindProperty("branches");
        maxPointsForBranchProperty = serializedObject.FindProperty("maxPointsForBranch");
        segmentLengthProperty = serializedObject.FindProperty("segmentLength");
        branchRadiusProperty = serializedObject.FindProperty("branchRadius");
        branchMaterialProperty = serializedObject.FindProperty("branchMaterial");
        leafMaterialProperty = serializedObject.FindProperty("leafMaterial");
        flowerMaterialProperty = serializedObject.FindProperty("flowerMaterial");
        leafPrefabProperty = serializedObject.FindProperty("leafPrefab");
        flowerPrefabProperty = serializedObject.FindProperty("flowerPrefab");
        wantBlossomsProperty = serializedObject.FindProperty("wantBlossoms");
        drawIvyProperty = serializedObject.FindProperty("drawIvy");

    }

    public override void OnInspectorGUI()
    {
        // Update the serialized object with any changes made in the inspector
        serializedObject.Update();

        // Draw the fields for all the property
        EditorGUILayout.PropertyField(meshManagerProperty);
        EditorGUILayout.PropertyField(camProperty);
        EditorGUILayout.PropertyField(recycleIntervalProperty);
        EditorGUILayout.PropertyField(branchesProperty);
        EditorGUILayout.PropertyField(maxPointsForBranchProperty);
        EditorGUILayout.PropertyField(segmentLengthProperty);
        EditorGUILayout.PropertyField(branchRadiusProperty);
        EditorGUILayout.PropertyField(branchMaterialProperty);
        EditorGUILayout.PropertyField(leafMaterialProperty);
        EditorGUILayout.PropertyField(flowerMaterialProperty);
        EditorGUILayout.PropertyField(leafPrefabProperty);
        EditorGUILayout.PropertyField(flowerPrefabProperty);
        EditorGUILayout.PropertyField(wantBlossomsProperty);
        EditorGUILayout.PropertyField(drawIvyProperty);

        // Apply any changes made to the serialized object
        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        serializedObject.Update();

        if (drawIvyProperty.boolValue == true)
        {

            //Make everything else in scene not selectable
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

            // Handle mouse events
            HandleMouseEvents();

            // Repaint the scene view
            SceneView.RepaintAll();
        }
        else { }




        serializedObject.ApplyModifiedProperties();

    }

    private void HandleMouseEvents()
    {


        if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {

            Debug.Log("clicking down");

            Event.current.Use();

        }
        else if (Event.current.type == EventType.MouseDrag && Event.current.button == 0)
        {

            proceduralIvy.DrawIvy();
            Event.current.Use();
        }
        else if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
        {
            Debug.Log("no longer clicking down");

            proceduralIvy.meshManager.combineAll();
            proceduralIvy.combineAndClear();


            Event.current.Use();
        }

    }
}