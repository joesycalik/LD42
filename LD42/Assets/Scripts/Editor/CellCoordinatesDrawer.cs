using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Assets.Scripts.Level
{
    [CustomPropertyDrawer(typeof(CellCoordinates))]
    public class CellCoordinatesDrawer : PropertyDrawer
    {

        public override void OnGUI(
            Rect position, SerializedProperty property, GUIContent label
        )
        {
            CellCoordinates coordinates = new CellCoordinates(
                property.FindPropertyRelative("x").intValue,
                property.FindPropertyRelative("z").intValue
            );

            position = EditorGUI.PrefixLabel(position, label);
            GUI.Label(position, coordinates.ToString());
        }
    }
}

