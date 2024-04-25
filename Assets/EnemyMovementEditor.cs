using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using Unity.VisualScripting;
using System.Linq;
using System;
using System.Runtime.InteropServices;

[CustomEditor(typeof(EnemyMoveScript))]
public class EnemyMovementEditor : Editor
{
    SerializedProperty positions;
    ReorderableList list; 

    int selectedIndex = 0;
    void OnEnable(){
        positions = serializedObject.FindProperty("positions");
        list = new ReorderableList(serializedObject, positions,true,true,true,true);
        list.drawElementCallback = DrawListItems;
        list.drawHeaderCallback = DrawHeader;
        list.elementHeight = EditorGUIUtility.singleLineHeight*2.5f;
        list.onSelectCallback = (a) =>{
            selectedIndex = a.index;
        };
        list.onRemoveCallback = (a) =>{
            positions.arraySize--;
            selectedIndex = positions.arraySize-1;
            a.ClearSelection();
            a.Select(selectedIndex);
        };
        list.onAddCallback = (a)=>{
            positions.arraySize++;
            var element = list.serializedProperty.GetArrayElementAtIndex(positions.arraySize-1);
            var prevElement = list.serializedProperty.GetArrayElementAtIndex(selectedIndex);
            var lastPositionName = prevElement.FindPropertyRelative("positionName").stringValue;
            var lastChar = lastPositionName[lastPositionName.Length-1];
            int num;
            string newPositionName;
            if (!int.TryParse(lastChar.ToString(),out num)){
                num = 2;
                newPositionName = lastPositionName + $" {num}";
            }else{
                newPositionName = lastPositionName.Remove(lastPositionName.Length-1)+$"{num+1}";
            }
            element.FindPropertyRelative("positionName").stringValue = newPositionName;
            element.FindPropertyRelative("parentId").intValue = selectedIndex;
        };
    }

    // Draws the elements on the list
    void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);
        EditorGUI.LabelField(new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), "Name");
        EditorGUI.PropertyField(
        new Rect(rect.x+40, rect.y, 100, EditorGUIUtility.singleLineHeight),
        element.FindPropertyRelative("positionName"),
        GUIContent.none
        ); 
        EditorGUI.LabelField(new Rect(rect.x+150, rect.y, 100, EditorGUIUtility.singleLineHeight), "From");
        // EditorGUI.PropertyField(
        // new Rect(rect.x+190, rect.y, 100, EditorGUIUtility.singleLineHeight),
        // element.FindPropertyRelative("parentName"),
        // GUIContent.none
        // ); 

        List<string> options = new List<string>();

        for(int i = 0; i<positions.arraySize; i++){
            var el = list.serializedProperty.GetArrayElementAtIndex(i);
            options.Add(el.FindPropertyRelative("positionName").stringValue);
        }

        element.FindPropertyRelative("parentId").intValue = EditorGUI.Popup(new Rect(rect.x+190, rect.y, 100, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("parentId").intValue,
            options.ToArray()
            
        );
        
        EditorGUI.PropertyField(
        new Rect(rect.x, rect.y+EditorGUIUtility.singleLineHeight+5, 250, EditorGUIUtility.singleLineHeight), 
            element.FindPropertyRelative("position"),
            GUIContent.none
        ); 
    }

    //Draws the header
    void DrawHeader(Rect rect)
    {
         EditorGUI.LabelField(rect, "Positions");
    }

    public override void OnInspectorGUI()
    {
        // base.OnInspectorGUI();
        serializedObject.Update();
        list.DoLayoutList(); 
        serializedObject.ApplyModifiedProperties();
        
        EnemyMoveScript myScript = (EnemyMoveScript)target;
        if(GUILayout.Button("Update Position")){
            myScript.SetSelectedPosition(selectedIndex);
        }
        if(GUILayout.Button("Revert Position")){
            myScript.RevertSelectedPosition(selectedIndex);
        }
    }
}
