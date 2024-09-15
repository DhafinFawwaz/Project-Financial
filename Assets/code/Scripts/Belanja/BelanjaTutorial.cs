using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BelanjaTutorial : MonoBehaviour
{
    [SerializeField] GameObject _shadow;
    [SerializeField] AnimationUI _startPopUpAnimation;
    [SerializeField] MissionSetter _missionSetter;
    [SerializeField] Flashlight _flashLight;
    [SerializeField] BelanjaList belanjaList;
    [SerializeField] ItemData _saosTomat;
    [SerializeField] ItemData _eskrim;
    [SerializeField] Rak _rak;

    void Awake()
    {
        if(Save.Data.CurrentDay <= 1)
            this.Invoke(StartTutorial, 0.1f);
        else if(Save.Data.CurrentDay == 2)
            this.Invoke(StartTutorial2, 0.1f);
    }

    [SerializeField] SphereCollider[] _colliders;
    void StartTutorial()
    {
        foreach(var col in _colliders)
        {
            col.enabled = false;
        }

        _shadow.SetActive(false);
        _startPopUpAnimation.Play();
        _missionSetter.Set();
        belanjaList.AddItem(_saosTomat);
        // belanjaList.AddItem(_eskrim); 


        _textBoxAnimation.TweenLocalScale();
        SetTextAndPlay("Ada saus tomat di list belanjamu! Dekati Rak Saus Tomat lalu tekan E untuk membeli!");
    }

    public void SetTextAndPlay(string text)
    {
        _textAnimation.SetTextAndPlay(text);
    }

    bool _isTomatBought = false;
    bool _tutorial2Finished = false;
    bool _isTutorial2Started = false;
    void Update()
    {
        if(Save.Data.CurrentDay <= 1)
        {
            if(!_isTomatBought && belanjaList.ListCart.Find(x => x.Item.Name == _saosTomat.Name) != null)
            {
                _isTomatBought = true;
                // DoneBuy();
                this.Invoke(DoneBuy, 0.05f);
            }
        } else {
            if(_isTutorial2Started && !_tutorial2Finished && _rakToDarken.All(x => !x.IsLocked))
            {
                _tutorial2Finished = true;
                DoneTutorial2();
            }
        }
    }

    [SerializeField] Vector3 _kasirPos;
    void DoneBuy()
    {
        _rak.DisableDetection();
        _missionSetter.ArrowPosition = _kasirPos;
        _missionSetter.Set();
        SetTextAndPlay("Sekarang pergi ke kasir untuk membayar! Tekan E di dekat kasir!");
    }
    [SerializeField] SceneTransition _sceneTransition;
    public void DoneTutorial()
    {
        Save.Data.NeedsMoney = Save.Data.TempNeedsMoney;
        _sceneTransition.StartSceneTransition("Belanja");
    }



    [Header("Animation")]
    [SerializeField] TransformAnimation _textBoxAnimation;
    [SerializeField] TextPlayer _textAnimation;

    [Header("Darken Rak")]
    [SerializeField] Rak[] _rakToDarken;
    
    void StartTutorial2()
    {
        _shadow.SetActive(true);
        _startPopUpAnimation.Play();
        foreach(var rak in _rakToDarken)
        {
            rak.SetDarken(true);
        }
        _textBoxAnimation.TweenLocalScale();
        SetTextAndPlay("Jika kamu terlambat saat memilih rak, rak bisa menjadi gelap! Gunakan senter untuk membuka rak yang menjadi gelap!");
        _isTutorial2Started = true;
    }
    public void DoneTutorial2()
    {
        SetTextAndPlay("Mantap! Sekarang kamu bisa mengakses rak tersebut kembali!");
        this.Invoke(() => {
            Save.Data.NeedsMoney = Save.Data.TempNeedsMoney;
            _sceneTransition.StartSceneTransition("Belanja");
        }, 2f);
    }
}
