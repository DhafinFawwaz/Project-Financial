using System;
using UnityEngine;

public class PlayerCore : Core<PlayerCore, PlayerStates>
{
    public PlayerLocomotion Locomotion;
    public PlayerStats Stats;

    // public IInteractable CurrentInteractableObject;

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
        Debug.Log(item.name);
    }

}
