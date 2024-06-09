using System;
using System.Collections;
using UnityEngine;

public class PlayerCore : Core<PlayerCore, PlayerStates>
{
    public PlayerLocomotion Locomotion;
    public PlayerStats Stats;

    // public IInteractable CurrentInteractableObject;
    [SerializeField] ListBelanja _list;

    void Start()
    {
        States = new PlayerStates(this);
        CurrentState = States.Idle();
        CurrentState.StateEnter();
    }

    public override void OnHurt(HitRequest hitRequest, ref HitResult hitResult)
    {

    }


    void Update()
    {
        CurrentState.StateUpdate();
    }
    void FixedUpdate()
    {
        CurrentState.StateFixedUpdate();
    }

    bool CanBuy()
    {
        // if(!CurrentInteractableObject.CanInteract()) return;
        return true;
    }

    public Vector3 GetInputDirection(){
        return new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    }



    public void Collect(Item item){
        StartCoroutine(CollectAnimation(item.transform, item.transform.position, transform, 0.6f, new Vector3(0,0.25f,0), 1));
    }

    IEnumerator CollectAnimation(Transform rt, Vector3 start, Transform endTrans, float duration, Vector3 offset, float height)
    {
        float startTime = Time.time;
        float t = (Time.time-startTime)/duration;
        while (t <= 1)
        {
            t = Mathf.Clamp((Time.time-startTime)/duration, 0, 2);
            rt.position = Vector3.LerpUnclamped(start, endTrans.position, t) + offset + Vector3.up * height * Parabole(t);
            yield return null;
        }
        rt.position = endTrans.position;
        Destroy(rt.gameObject);
    }


    float Parabole(float x) => -4 * x * x + 4 * x;

}
