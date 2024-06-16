using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "CommentList", menuName = "CommentList", order = 1)]
[System.Serializable]
public class CommentList: ScriptableObject
{
    public List<CommentData> Comments;
}

[System.Serializable]
public class CommentData
{
    public string Author;
    public string Comment;
}

// draw Author and Comment fields in the inspector side by side without label
#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(CommentData))]
public class CommentDataDrawer : PropertyDrawer
{
    float ratio = 0.3f;
    float space = 2;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = EditorGUIUtility.singleLineHeight;
        var authorRect = new Rect(position.x, position.y, position.width * ratio, position.height);
        var commentRect = new Rect(position.x + position.width * ratio + space, position.y, position.width * (1 - ratio), position.height);
        EditorGUI.PropertyField(authorRect, property.FindPropertyRelative("Author"), GUIContent.none);
        EditorGUI.PropertyField(commentRect, property.FindPropertyRelative("Comment"), GUIContent.none);
    }
}

#endif