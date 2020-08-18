using System;
using UnityEngine;
using UnityEditor;

namespace Chankiyu22.DialogueSystem
{

public static class Utils
{
    public static void ExecuteSetter(UnityEngine.Object obj, string propertyPath, Action<SerializedProperty> setter)
    {
        SerializedObject serializedObject = new SerializedObject(obj);
        SerializedProperty prop = serializedObject.FindProperty(propertyPath);
        setter.Invoke(prop);
        serializedObject.ApplyModifiedPropertiesWithoutUndo();
    }

    public static void ExecuteSetter(SerializedProperty serializedProperty, string propertyPath, Action<SerializedProperty> setter)
    {
        SerializedProperty prop = serializedProperty.FindPropertyRelative(propertyPath);
        setter.Invoke(prop);
        serializedProperty.serializedObject.ApplyModifiedPropertiesWithoutUndo();
    }

    public static void ExecuteSetterArray(SerializedProperty serializedArray, int index, string propertyPath, Action<SerializedProperty> setter)
    {
        serializedArray.arraySize = Mathf.Max(serializedArray.arraySize, index + 1);
        SerializedProperty serializedItem = serializedArray.GetArrayElementAtIndex(index);
        if (propertyPath == null || propertyPath.Length == 0)
        {
            setter.Invoke(serializedItem);
        }
        else
        {
            SerializedProperty prop = serializedItem.FindPropertyRelative(propertyPath);
            setter.Invoke(prop);
        }
        serializedArray.serializedObject.ApplyModifiedPropertiesWithoutUndo();
    }

    public static void ExecuteSetterArrayAutoIncrementReset(SerializedProperty serializedArray, string propertyPath, Action<SerializedProperty> setter)
    {
        serializedArray.arraySize = 1;
        SerializedProperty serializedItem = serializedArray.GetArrayElementAtIndex(serializedArray.arraySize - 1);
        if (propertyPath == null || propertyPath.Length == 0)
        {
            setter.Invoke(serializedItem);
        }
        else
        {
            SerializedProperty prop = serializedItem.FindPropertyRelative(propertyPath);
            setter.Invoke(prop);
        }
        serializedArray.serializedObject.ApplyModifiedPropertiesWithoutUndo();
    }

    public static void ExecuteSetterArrayAutoIncrement(SerializedProperty serializedArray, string propertyPath, Action<SerializedProperty> setter)
    {
        serializedArray.arraySize += 1;
        SerializedProperty serializedItem = serializedArray.GetArrayElementAtIndex(serializedArray.arraySize - 1);
        if (propertyPath == null || propertyPath.Length == 0)
        {
            setter.Invoke(serializedItem);
        }
        else
        {
            SerializedProperty prop = serializedItem.FindPropertyRelative(propertyPath);
            setter.Invoke(prop);
        }
        serializedArray.serializedObject.ApplyModifiedPropertiesWithoutUndo();
    }
}

}