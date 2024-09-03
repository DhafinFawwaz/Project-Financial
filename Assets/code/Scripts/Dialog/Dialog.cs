using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class Dialog : MonoBehaviour
{
    [SerializeField] DialogData _data;
    public static Action<DialogData> s_OnDialogFinished;
    public static Action<DialogData> s_OnDialogStart;
    [SerializeField] Image _actorImage;
    [SerializeField] TextMeshProUGUI _actorName;
    [SerializeField] TextMeshProUGUI _dialogText;
    [SerializeField] float _delayEachCharacter = 0.05f;
    [SerializeField] bool _playOnStart = false;
    [SerializeField] AnimationUI _animationUI;

    [Header("Dialog boxes")]
    [SerializeField] GameObject _dialogAngkasa;
    [SerializeField] GameObject _dialogBaik;
    [SerializeField] GameObject _dialogSus;

    [Header("Actor")]
    [SerializeField] GameObject Angkasa;
    [SerializeField] GameObject Nao;
    [SerializeField] GameObject PakRudi;
    [SerializeField] GameObject PakHendra;
    [SerializeField] GameObject Nadine;
    [SerializeField] GameObject Miau;
    [SerializeField] GameObject MasDanang;
    [SerializeField] GameObject Fajar;
    [SerializeField] GameObject BuYulianti;
    [SerializeField] GameObject BuRatna;
    [SerializeField] GameObject Bima;
    [SerializeField] GameObject BerandalMainJudi;
    [SerializeField] GameObject Anaya;
    [SerializeField] GameObject Riki;
    [SerializeField] GameObject Kunti;

    [SerializeField] GameObject RikiNervous;
    [SerializeField] GameObject RikiPanik;
    [SerializeField] GameObject RikiSenyumLebar;
    [SerializeField] GameObject RikiSenyumTipis;

    [SerializeField] GameObject _miauActually;
    [SerializeField] GameObject _miauNgejek;
    [SerializeField] GameObject _miauTsundere;
    [SerializeField] GameObject _miauNormal;

    [SerializeField] GameObject _naoBlush;
    [SerializeField] GameObject _naoKepo;
    [SerializeField] GameObject _naoMakeupDefault;
    [SerializeField] GameObject _naoMakeupKibas;
    [SerializeField] GameObject _naoMakeupNervous;
    [SerializeField] GameObject _naoMakeupSmirk;
    [SerializeField] GameObject _naoSakit;
    [SerializeField] GameObject _naoSakitSenyum;
    [SerializeField] GameObject _naoSmirk;

    [SerializeField] GameObject _shadow;

    public void SetActor(DialogActor actor)
    {
        Angkasa.SetActive(false);
        Nao.SetActive(false);
        PakRudi.SetActive(false);
        PakHendra.SetActive(false);
        Nadine.SetActive(false);
        Miau.SetActive(false);
        MasDanang.SetActive(false);
        Fajar.SetActive(false);
        BuYulianti.SetActive(false);
        BuRatna.SetActive(false);
        Bima.SetActive(false);
        BerandalMainJudi.SetActive(false);
        Anaya.SetActive(false);
        Riki.SetActive(false);
        Kunti.SetActive(false);
        
        RikiNervous.SetActive(false);
        RikiPanik.SetActive(false);
        RikiSenyumLebar.SetActive(false);
        RikiSenyumTipis.SetActive(false);

        _miauActually.SetActive(false);
        _miauNgejek.SetActive(false);
        _miauTsundere.SetActive(false);
        _miauNormal.SetActive(false);

        _naoBlush.SetActive(false);
        _naoKepo.SetActive(false);
        _naoMakeupDefault.SetActive(false);
        _naoMakeupKibas.SetActive(false);
        _naoMakeupNervous.SetActive(false);
        _naoMakeupSmirk.SetActive(false);
        _naoSakit.SetActive(false);
        _naoSakitSenyum.SetActive(false);
        _naoSmirk.SetActive(false);

        _shadow.SetActive(false);




        switch(actor)
        {
            case DialogActor.Angkasa: Angkasa.SetActive(true); break;
            case DialogActor.Nao: Nao.SetActive(true); break;
            case DialogActor.PakRudi: PakRudi.SetActive(true); break;
            case DialogActor.PakHendra: PakHendra.SetActive(true); break;
            case DialogActor.Nadine: Nadine.SetActive(true); break;
            case DialogActor.Miau: Miau.SetActive(true); break;
            case DialogActor.MasDanang: MasDanang.SetActive(true); break;
            case DialogActor.Fajar: Fajar.SetActive(true); break;
            case DialogActor.BuYulianti: BuYulianti.SetActive(true); break;
            case DialogActor.BuRatna: BuRatna.SetActive(true); break;
            case DialogActor.Bima: Bima.SetActive(true); break;
            case DialogActor.BerandalMainJudi: BerandalMainJudi.SetActive(true); break;
            case DialogActor.Anaya: Anaya.SetActive(true); break;
            case DialogActor.Riki: Riki.SetActive(true); break;
            case DialogActor.Kunti: Kunti.SetActive(true); break;

            case DialogActor.RikiNervous: RikiNervous.SetActive(true); break;
            case DialogActor.RikiPanik: RikiPanik.SetActive(true); break;
            case DialogActor.RikiSenyumLebar: RikiSenyumLebar.SetActive(true); break;
            case DialogActor.RikiSenyumTipis: RikiSenyumTipis.SetActive(true); break;

            case DialogActor.MiauActually: _miauActually.SetActive(true); break;
            case DialogActor.MiauNgejek: _miauNgejek.SetActive(true); break;
            case DialogActor.MiauTsundere: _miauTsundere.SetActive(true); break;
            case DialogActor.MiauNormal: _miauNormal.SetActive(true); break;

            case DialogActor.NaoBlush: _naoBlush.SetActive(true); break;
            case DialogActor.NaoKepo: _naoKepo.SetActive(true); break;
            case DialogActor.NaoMakeupDefault: _naoMakeupDefault.SetActive(true); break;
            case DialogActor.NaoMakeupKibas: _naoMakeupKibas.SetActive(true); break;
            case DialogActor.NaoMakeupNervous: _naoMakeupNervous.SetActive(true); break;
            case DialogActor.NaoMakeupSmirk: _naoMakeupSmirk.SetActive(true); break;
            case DialogActor.NaoSakit: _naoSakit.SetActive(true); break;
            case DialogActor.NaoSakitSenyum: _naoSakitSenyum.SetActive(true); break;
            case DialogActor.NaoSmirk: _naoSmirk.SetActive(true); break;

            case DialogActor.Shadow: _shadow.SetActive(true); break;
            
        }
    }

    void Start()
    {
        if(_playOnStart) Play();    
    }
    int _currentContentIndex = 0;

    bool _isPlaying = false;
    public void SetDataAndPlay(DialogData data)
    {
        _data = data;
        _currentContentIndex = 0;
        _dialogText.text = "";
        _dialogText.maxVisibleCharacters = 0;
        Play();
        Next();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if(_isPlaying)
            {
                Next();
            }
        }
    }

    public void Next()
    {
        if(_currentContentIndex < _data.Content.Length)
        {
            _actorName.text = _data.Content[_currentContentIndex].ActorRight;
            _dialogText.text = _data.Content[_currentContentIndex].Text;
            SetActor(_data.Content[_currentContentIndex].DialogActor);
            
            StartCoroutine(TextAnimation());

            if(_data.Content[_currentContentIndex].SusLevel == SusLevel.Angkasa) {
                _dialogAngkasa.SetActive(true);
                _dialogBaik.SetActive(false);
                _dialogSus.SetActive(false);
                _actorName.gameObject.SetActive(false);
            } else if(_data.Content[_currentContentIndex].SusLevel == SusLevel.Baik) {
                _dialogAngkasa.SetActive(false);
                _dialogBaik.SetActive(true);
                _dialogSus.SetActive(false);
                _actorName.gameObject.SetActive(true);
            } else if(_data.Content[_currentContentIndex].SusLevel == SusLevel.Sus) {
                _dialogAngkasa.SetActive(false);
                _dialogBaik.SetActive(false);
                _dialogSus.SetActive(true);
                _actorName.gameObject.SetActive(true);
            }
            _currentContentIndex++;
        } else {
            Stop();
        }
    }

    public void Skip()
    {
        _currentContentIndex = _data.Content.Length;
        _dialogText.maxVisibleCharacters = _dialogText.text.Length;
        Stop();
    }

    public void Play()
    {
        s_OnDialogStart?.Invoke(_data);
        _isPlaying = true;
        _animationUI.Play();
    }   

    public void Stop()
    {
        _isPlaying = false;
        _animationUI.PlayReversed();
        s_OnDialogFinished?.Invoke(_data);
    }

    byte _key = 0;
    IEnumerator TextAnimation()
    {
        byte requirement = ++_key;
        for(int i = 0; i < _dialogText.text.Length; i++)
        {
            _dialogText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(_delayEachCharacter);
            if(_key != requirement) break;
        }
        if(_key == requirement)
            _dialogText.maxVisibleCharacters = _dialogText.text.Length;
    }
}
