using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreamCutscene : MonoBehaviour
{
    [SerializeField] Toaster _toaster;
    [SerializeField] Vector3[] _toasterPositions;
    [SerializeField] Vector3 _toasterThrowPosition;
    [SerializeField] Transform _toasterParent;

    List<Toaster> _toasters = new List<Toaster>();

    StreamCutsceneData _currentCutscene;
    [SerializeField] TopText _topText;
    void Start()
    {
        Play();
    }
    [SerializeField] GameObject _blocker;



    [ContextMenu("Test Toast")]
    public void TestToast()
    {
        SpawnToaster("Test", "Test test test");
    }

    [SerializeField] float _tweenDuration = 0.3f;
    public void SpawnToaster(string title, string message)
    {
        Toaster toaster = Instantiate(_toaster, _toasterParent);
        toaster.SetText(title, message);
        RectTransform rt = toaster.GetComponent<RectTransform>();
        Vector2 pos = _toasterPositions[0];
        rt.anchoredPosition = new Vector2(-pos.x, pos.y);
        _toasters.Insert(0, toaster);
        MoveOtherToasters();
    }
    public void DespawnAllToasters()
    {
        foreach(var toaster in _toasters)
        {
            RectTransform rt = toaster.gameObject.GetComponent<RectTransform>();
            toaster.Move(rt, rt.anchoredPosition, _toasterThrowPosition, _tweenDuration, Ease.OutQuart, () => Destroy(rt.gameObject));
        }
        _toasters.Clear();
    }
    void MoveOtherToasters()
    {
        for(int i = 0; i < Mathf.Min(_toasterPositions.Length, _toasters.Count); i++)
        {
            RectTransform rt = _toasters[i].gameObject.GetComponent<RectTransform>();
            _toasters[i].Move(rt, rt.anchoredPosition, _toasterPositions[i], _tweenDuration, Ease.OutQuart);
        }
        if(_toasters.Count > _toasterPositions.Length) {
            var toaster = _toasters[_toasterPositions.Length];
            RectTransform rt = toaster.gameObject.GetComponent<RectTransform>();
            _toasters.RemoveAt(_toasters.Count-1);
            toaster.Move(rt, rt.anchoredPosition, _toasterThrowPosition, _tweenDuration, Ease.OutQuart, () => Destroy(rt.gameObject));
        }

    }



    [SerializeField] StreamCutsceneData[] _cutscenes;
    int _cutsceneIndex = -1;
    
    bool _isPlaying = false;
    public void Play()
    {
        _currentCutscene = _cutscenes[Save.Data.CurrentDay];
        _blocker.SetActive(true);
        _cutsceneIndex = -1;

        // spawn 3 initial toasters
        this.Invoke(() => {
            for(int i = 0; i < 3; i++)
            {
                var dialog = _currentCutscene.Initial3Dialogs[i];
                this.Invoke(() => SpawnToaster(GetRandomUsername(), dialog.Message), i * _tweenDuration*0.65f);
            }
            this.Invoke(() => _isPlaying = true, 3 * _tweenDuration);
        }, 0.5f);
    }

    void Update()
    {
        if(!_isPlaying) return;

        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Space))
        {
            Next();
        }
    }

    void Next()
    {
        _cutsceneIndex++;
        if(_cutsceneIndex >= _currentCutscene.Dialogs.Length)
        {
            _topText.Hide();
            DespawnAllToasters();
            _isPlaying = false;
            this.Invoke(() => _blocker.SetActive(false), 0.5f);
            return;
        }
        var dialog = _currentCutscene.Dialogs[_cutsceneIndex];
        if(dialog.type == StreamCutsceneData.Dialog.Type.Streamer)
        {
            _topText.SetText(dialog.Message).Show().Play();
        }
        else
        {
            SpawnToaster(GetRandomUsername(), dialog.Message);
            _topText.Hide();
        }
    }


    [Header("Toaster Names")]
    [TextArea]
    [SerializeField]
    string _toasterNames;
    public string GetRandomUsername()
    {
        var names = _toasterNames.Split('\n');
        return names[UnityEngine.Random.Range(0, names.Length)];
    } 
}
