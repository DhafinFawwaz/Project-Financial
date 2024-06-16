using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StreamingManager : MonoBehaviour
{
    [SerializeField] CommentSection _commentSection;
    [SerializeField] TextMeshProUGUI _views;
    void Start()
    {
        _commentSection.Play();
        _views.text = "1000";
    }
}
