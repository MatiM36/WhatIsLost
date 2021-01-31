using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(HideInClassAttribute))]
public class HideInClassDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        HideInClassAttribute hideInClassAttrib = (HideInClassAttribute)attribute;
        string objType = property.serializedObject.targetObject.GetType().ToString();

        if (objType != hideInClassAttrib.classHidden.ToString())
            return base.GetPropertyHeight(property, label);
        else
            return 0;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        HideInClassAttribute hideInClassAttrib = (HideInClassAttribute)attribute;

        string objType = property.serializedObject.targetObject.GetType().ToString();

        if (objType != hideInClassAttrib.classHidden.ToString())
            EditorGUI.PropertyField(position, property, label);
    }
}