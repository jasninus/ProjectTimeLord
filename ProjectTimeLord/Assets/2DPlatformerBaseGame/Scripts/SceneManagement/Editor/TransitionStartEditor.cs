using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TransitionPoint))]
public class TransitionStartEditor : Editor
{
    SerializedProperty transitioningGameObjectProperty;
    SerializedProperty transitionTypeProperty;
    SerializedProperty newSceneNameProperty;
    SerializedProperty transitionDestinationTagProperty;
    SerializedProperty destinationTransformProperty;
    SerializedProperty transitionWhenProperty;
    SerializedProperty resetInputValuesOnTransitionProperty;
    SerializedProperty requiresInventoryCheckProperty;
    SerializedProperty inventoryControllerProperty;
    SerializedProperty inventoryCheckProperty;
    SerializedProperty inventoryItemsProperty;
    SerializedProperty onHasItemProperty;
    SerializedProperty onDoesNotHaveItemProperty;

    GUIContent[] inventoryControllerItems = new GUIContent[0];

    void OnEnable()
    {
        transitioningGameObjectProperty = serializedObject.FindProperty("transitioningGameObject");
        transitionTypeProperty = serializedObject.FindProperty("transitionType");
        newSceneNameProperty = serializedObject.FindProperty("newSceneName");
        transitionDestinationTagProperty = serializedObject.FindProperty("transitionDestinationTag");
        destinationTransformProperty = serializedObject.FindProperty("destinationTransform");
        transitionWhenProperty = serializedObject.FindProperty("transitionWhen");
        resetInputValuesOnTransitionProperty = serializedObject.FindProperty("resetInputValuesOnTransition");
        requiresInventoryCheckProperty = serializedObject.FindProperty("requiresInventoryCheck");
        inventoryControllerProperty = serializedObject.FindProperty("inventoryController");
        inventoryCheckProperty = serializedObject.FindProperty("inventoryCheck");
        inventoryItemsProperty = inventoryCheckProperty.FindPropertyRelative("inventoryItems");
        onHasItemProperty = inventoryCheckProperty.FindPropertyRelative("OnHasItem");
        onDoesNotHaveItemProperty = inventoryCheckProperty.FindPropertyRelative("OnDoesNotHaveItem");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(transitioningGameObjectProperty);

        EditorGUILayout.PropertyField(transitionTypeProperty);
        EditorGUI.indentLevel++;
        if ((TransitionPoint.TransitionType)transitionTypeProperty.enumValueIndex == TransitionPoint.TransitionType.SameScene)
        {
            EditorGUILayout.PropertyField(destinationTransformProperty);
        }
        else
        {
            EditorGUILayout.PropertyField(newSceneNameProperty);
            EditorGUILayout.PropertyField(transitionDestinationTagProperty);
        }
        EditorGUI.indentLevel--;

        EditorGUILayout.PropertyField(transitionWhenProperty);
        EditorGUILayout.PropertyField(resetInputValuesOnTransitionProperty);

        EditorGUILayout.PropertyField(requiresInventoryCheckProperty);
        if (requiresInventoryCheckProperty.boolValue)
        {
            EditorGUI.indentLevel++;

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(inventoryControllerProperty);
            if (EditorGUI.EndChangeCheck() || (inventoryControllerProperty.objectReferenceValue != null && inventoryControllerItems.Length == 0))
            {
               
            }

           

            EditorGUI.indentLevel--;
        }

        serializedObject.ApplyModifiedProperties();
    }



   
}
