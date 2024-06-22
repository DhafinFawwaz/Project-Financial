using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MouseCursor : MonoBehaviour
{
    public static MouseCursor Main { get; private set; }
    void Awake()
    {
        Cursor.visible = false;
        Main = this;
    }
    
    [SerializeField] float normalScale = 0.3f;
    [SerializeField] float downScale = 0.1f;
    [SerializeField] Image _cursorImg;
    [SerializeField] Sprite _unclickedCursor;
    [SerializeField] Sprite _clickedCursor;
    [SerializeField] Sprite _gosokCursor;
    public Vector2 MousePosition { 
        get { 
            return ScreenToRectPos(_rt.parent as RectTransform, Input.mousePosition, _canvas);
        }
    }

    public Vector2 ScreenToRectPos(RectTransform rectTransform, Vector2 screen_pos, Canvas canvas)
    { 
        Vector2 anchorPos = screen_pos - new Vector2(rectTransform.position.x, rectTransform.position.y);
        anchorPos= new Vector2(anchorPos.x / rectTransform.lossyScale.x, anchorPos.y / rectTransform.lossyScale.y);
        return anchorPos;
    }

    [SerializeField] Canvas _canvas;
    [SerializeField] RectTransform _rt;

    void Update()
    {
        _rt.anchoredPosition = MousePosition;
        if(Input.GetMouseButtonDown(0))
        {
            StartCoroutine(LocalScaleAnimation(transform, transform.localScale, Vector3.one * downScale, 0.2f));
            if(_cursorImg.sprite == _gosokCursor) return;
            _cursorImg.sprite = _clickedCursor;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            StartCoroutine(LocalScaleAnimation(transform, transform.localScale, Vector3.one * normalScale, 0.2f));
            if(_cursorImg.sprite == _gosokCursor) return;
            _cursorImg.sprite = _unclickedCursor;
        }
    }

    byte _key;
    IEnumerator LocalScaleAnimation(Transform trans, Vector3 start, Vector3 end, float duration)
    {
        byte requirement = ++_key;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && requirement == _key)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            trans.localScale = Vector3.LerpUnclamped(start, end, Ease.OutQuart(t));
            yield return null;
        }
        if(requirement == _key)
            trans.localScale = end;
    }


    public void SetToGosok()
    {
        if(_cursorImg.sprite == _gosokCursor) return;
        _cursorImg.sprite = _gosokCursor;
    }
    public void SetToCursor()
    {
        if(_cursorImg.sprite == _unclickedCursor || _cursorImg.sprite == _clickedCursor) return;
        _cursorImg.sprite = _unclickedCursor;
    }
}
