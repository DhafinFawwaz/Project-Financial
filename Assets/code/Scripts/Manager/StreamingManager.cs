using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StreamingManager : MonoBehaviour
{
    [SerializeField] CommentSection _commentSection;
    [SerializeField] TextMeshProUGUI _views;

    [Header("Face")]
    [SerializeField] Sprite[] _faceSprites;
    [SerializeField] Image _face;
    void Start()
    {
        _commentSection.Play();
        _views.text = "1000";
    }
}
