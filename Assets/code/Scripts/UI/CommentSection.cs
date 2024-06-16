using System.Collections;
using TMPro;
using UnityEngine;
public class CommentSection : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _commentText;
    [SerializeField] CommentList _commentList;
    [SerializeField] float _delayEachComment = 0.3f;

    void AddComment(string author, string comment)
    {
        _commentText.text += $"<b>{author}</b>: {comment}\n";
    }

    public void Play()
    {
        StartCoroutine(Animation());
    }
    IEnumerator Animation()
    {
        foreach (var comment in _commentList.Comments)
        {
            AddComment(comment.Author, comment.Comment);
            yield return new WaitForSeconds(_delayEachComment);
        }
    }
}
