using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    // [SerializeField] long _price = 10;
    // public void Interact(){
    //     PlayerCore playerCore = PlayerCore.Instance;
    //     if(playerCore.Stats.Money >= _price){
    //         playerCore.Stats.Money -= _price;
    //     }
    // }
    // public bool CanInteract(){
    //     return true;
    // }


    [SerializeField] Transform _feedBackTrans;
    [SerializeField] SpriteRenderer _spriteFeedBack;
    [SerializeField] float _showFeedBackDuration = 0.5f;
    public void ShowPlayerEnter()
    {
        StartCoroutine(TaskLocalScale(_feedBackTrans, _feedBackTrans.localScale, Vector3.one, _showFeedBackDuration, Ease.OutQuart));
        Color currentColor = _spriteFeedBack.color;
        Color targetColor = currentColor; targetColor.a = 1;
        StartCoroutine(TaskColor(_spriteFeedBack, currentColor, targetColor, _showFeedBackDuration, Ease.OutQuart));
    }

    public void ShowPlayerExit()
    {
        StartCoroutine(TaskLocalScale(_feedBackTrans, _feedBackTrans.localScale, Vector3.zero, _showFeedBackDuration, Ease.OutQuart));
        Color currentColor = _spriteFeedBack.color;
        Color targetColor = currentColor; targetColor.a = 0;
        StartCoroutine(TaskColor(_spriteFeedBack, currentColor, targetColor, _showFeedBackDuration, Ease.OutQuart));
    }
    

    byte _localPositionKey;
    IEnumerator TaskLocalPosition(Transform trans, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_localPositionKey;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && requirement == _localPositionKey)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            trans.localPosition = Vector3.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(requirement == _localPositionKey)
            trans.localPosition = end;
    }
    byte _localScaleKey;
    IEnumerator TaskLocalScale(Transform trans, Vector3 start, Vector3 end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_localScaleKey;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && requirement == _localScaleKey)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            trans.localScale = Vector3.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(requirement == _localScaleKey)
            trans.localScale = end;
    }

    byte _colorKey;
    IEnumerator TaskColor(SpriteRenderer sprite, Color start, Color end, float duration, Ease.Function easeFunction)
    {
        byte requirement = ++_colorKey;
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1 && requirement == _colorKey)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            sprite.color = Color.LerpUnclamped(start, end, easeFunction(t));
            yield return null;
        }
        if(requirement == _colorKey)
            sprite.color = end;
    }
}
