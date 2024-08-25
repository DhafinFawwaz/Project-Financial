using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class CommentSection : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _commentText;
    [SerializeField] CommentList _commentList;
    [SerializeField] float _delayEachComment = 0.3f;
    [SerializeField] bool _playOnAwake = true;
    int _maxComments = 13;

    void Awake()
    {
        if(_playOnAwake) Play();
    }

    Queue<string> _commentQueue = new Queue<string>(10);
    void AddComment(string author, string comment)
    {
        _commentQueue.Enqueue($"<color=\"yellow\"><b>{author}</b><color=\"white\">\n{comment}<line-height=120%>\n<line-height=100%>");
        if(_commentQueue.Count > _maxComments)
        {
            _commentQueue.Dequeue();
        }
        _commentText.text = string.Join("", _commentQueue);
    }

    public void Play()
    {
        StartCoroutine(Animation());
    }
    IEnumerator Animation()
    {
        // foreach (var comment in _commentList.Comments)
        // {
        //     AddComment(comment.Author, comment.Comment);
        //     yield return new WaitForSeconds(_delayEachComment);
        // }

        while(true)
        {
            int idx = Random.Range(0, _commentList.Comments.Count);
            AddComment(_commentList.Comments[idx].Author, _commentList.Comments[idx].Comment);
            yield return new WaitForSeconds(_delayEachComment);
        }
    }
}
