using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FiscalGuardianLoader : MonoBehaviour
{
    [SerializeField] FiscalGuardianData[] _data;
    [SerializeField] StreamingManager _streamingManager;

    [Header("Dialog")]
    [SerializeField] bool _isStartWithDialog = false;
    [SerializeField] RectTransformAnimation _textRTA;
    [SerializeField] TopText _topText;
    [TextArea]
    [SerializeField] string _message;
    [SerializeField] GameObject _dialogCloser;
    void Awake()
    {
        StreamingManager.CurrentFiscalGuardianData = _data[Save.Data.StreamingCounter[Save.Data.CurrentDay] % _data.Length];
    }

    public void Play()
    {
        StartCoroutine(DelayAwake());
    }

    IEnumerator DelayAwake()
    {
        yield return new WaitForSeconds(0.1f);
        // if(!_isStartWithDialog) _streamingManager.StartGame();
        // else
        // {
            _dialogCloser.SetActive(false);
            _textRTA.SetEnd(Vector3.one).TweenLocalScale();
            _topText.SetText(_message).Show().Play().SetOnceComplete(() => {
                _dialogCloser.SetActive(true);
            });
        // }
    }

    public void CloseStartDialog()
    {
        _textRTA.SetEnd(Vector3.zero).TweenLocalScale();
        _streamingManager.StartGame();
        _dialogCloser.SetActive(false);
    }

}
