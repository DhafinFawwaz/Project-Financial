using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StreamCutsceneData", menuName = "StreamCutsceneData", order = 1)]
public class StreamCutsceneData : ScriptableObject
{
    
    [System.Serializable]
    public class Dialog
    {
        public enum Type
        {
            Streamer,
            Toaster,
        }
        public Type type;
        public string Name;
        public string Message;
    }

    public Dialog[] Initial3Dialogs = new Dialog[3];

    public Dialog[] Dialogs;
}

#if UNITY_EDITOR    
[UnityEditor.CustomPropertyDrawer(typeof(StreamCutsceneData.Dialog))]
public class DialogDrawer : UnityEditor.PropertyDrawer
{
    public override void OnGUI(Rect position, UnityEditor.SerializedProperty property, GUIContent label)
    {
        UnityEditor.EditorGUI.BeginProperty(position, label, property);
        position = UnityEditor.EditorGUI.PrefixLabel(position, label);

        int indent = UnityEditor.EditorGUI.indentLevel;
        UnityEditor.EditorGUI.indentLevel = 0;

        Rect typeRect = new Rect(position.x, position.y, 80, position.height);
        
        UnityEditor.EditorGUI.PropertyField(typeRect, property.FindPropertyRelative("type"), GUIContent.none);
        if((StreamCutsceneData.Dialog.Type)property.FindPropertyRelative("type").enumValueIndex == StreamCutsceneData.Dialog.Type.Toaster)
        {
            Rect nameRect = new Rect(position.x + 80, position.y, 80, position.height);
            Rect messageRect = new Rect(position.x + 160, position.y, position.width - 160, position.height);
            UnityEditor.EditorGUI.PropertyField(nameRect, property.FindPropertyRelative("Name"), GUIContent.none);
            UnityEditor.EditorGUI.PropertyField(messageRect, property.FindPropertyRelative("Message"), GUIContent.none);
        }
        else
        {
            Rect messageRect = new Rect(position.x + 80, position.y, position.width - 80, position.height);
            UnityEditor.EditorGUI.PropertyField(messageRect, property.FindPropertyRelative("Message"), GUIContent.none);
        }

        UnityEditor.EditorGUI.indentLevel = indent;
    }
}
#endif