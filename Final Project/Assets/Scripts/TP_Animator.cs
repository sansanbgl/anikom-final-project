using UnityEngine;
using System.Collections;

public class TP_Animator : MonoBehaviour
{
	private float faceDirection=0f;
	
	public enum Direction
	{
		Stationary, Forward, Backward, Left, Right,
		LeftForward, RightForward, LeftBackward, RightBackward
	}

    public enum CharacterState
    {
        Idle, Running, WalkingBackwards, StrafingLeft, StrafingRight, Jumping,
        Falling, Landing, Climbing, Sliding, Using, Dead, ActionLocked
    }
	public static TP_Animator Instance;
	
	private CharacterState lastState;
	
	public Direction MoveDirection{get; set;}
    public CharacterState State { get; set; }
	public bool IsDead{get; set;}

	void Awake()
	{
		Instance=this;
	}
	
	void Update()
	{
        DetermineCurrentState();
        ProcessCurrentState();
        
        //Debug.Log("Current Character State: "+State.ToString());
	}
	
	public void DetermineCurrentMoveDirection()
	{
		var forward = false;
		var backward = false;
		var left = false;
		var right = false;
		
		if(TP_Motor.Instance.MoveVector.z > 0)
			forward = true;
		if(TP_Motor.Instance.MoveVector.z < 0)
			backward = true;
		if(TP_Motor.Instance.MoveVector.x > 0)
			right = true;
		if(TP_Motor.Instance.MoveVector.x < 0)
			left = true;
		
		if(forward)
		{
			if(left)
				MoveDirection = Direction.LeftForward;
			else if(right)
				MoveDirection = Direction.RightForward;
			else
				MoveDirection = Direction.Forward;
		}
		else if(backward)
		{
			if(left)
				MoveDirection = Direction.LeftBackward;
			else if(right)
				MoveDirection = Direction.RightBackward;
			else
				MoveDirection = Direction.Backward;
		}
		else if(left)
			MoveDirection = Direction.Left;
		else if(right)
			MoveDirection = Direction.Right;
		else
			MoveDirection = Direction.Stationary;
	}

    void DetermineCurrentState()
    {
        if (State == CharacterState.Dead)
            return;

        if (!TP_Controller.characterCtrller.isGrounded)
        {
            if (State != CharacterState.Falling &&
                State != CharacterState.Jumping &&
                State != CharacterState.Landing)
            {
                // We should be falling
            }
        }

        if (State != CharacterState.Falling &&
           State != CharacterState.Jumping &&
           State != CharacterState.Landing &&
           State != CharacterState.Using &&
           State != CharacterState.Climbing &&
           State != CharacterState.Sliding)
        {
            switch (MoveDirection)
            {
                case Direction.Stationary:
                    State = CharacterState.Idle;
                    break;
                case Direction.Forward:
                    State = CharacterState.Running;
					faceDirection=0;
                    break;
                case Direction.Backward:
                    State = CharacterState.WalkingBackwards;
					faceDirection =180;
                    break;
                case Direction.Left:
                    State = CharacterState.StrafingLeft;
					faceDirection = 90;
                    break;
                case Direction.Right:
                    State = CharacterState.StrafingRight;
                    faceDirection = 90;
					break;
                case Direction.LeftForward:
                    State = CharacterState.Running;
					faceDirection= -45;
                    break;
                case Direction.RightForward:
                    State = CharacterState.Running;
                    faceDirection=45;
					break;
                case Direction.LeftBackward:
                    State = CharacterState.WalkingBackwards;
                    faceDirection=225;
					break;
                case Direction.RightBackward:
                    State = CharacterState.WalkingBackwards;
					faceDirection=135;
                    break;
            }
        }
    }

    void ProcessCurrentState()
    {
        switch (State)
        {
            case CharacterState.Idle:
				Idle();
                break;
            case CharacterState.Running:
                Walking();
				animation.transform.Rotate(0, faceDirection,0);
				break;
            case CharacterState.WalkingBackwards:
				Walking();
				animation.transform.Rotate(0,faceDirection,0);
                break;
            case CharacterState.StrafingLeft:
				Walking();
				animation.transform.Rotate(0,-faceDirection,0);
                break;
            case CharacterState.StrafingRight:
				Walking();
				animation.transform.Rotate(0,faceDirection,0);
                break;
            case CharacterState.Jumping:
				Jumping();
                break;
            case CharacterState.Falling:
                break;
            case CharacterState.Landing:
                break;
            case CharacterState.Climbing:
                break;
            case CharacterState.Sliding:
                break;
            case CharacterState.Using:
                break;
            case CharacterState.Dead:
                break;
            case CharacterState.ActionLocked:
                break;
        }
    }
	
	#region Character State Methods
	
	void Idle()
	{
		animation.CrossFade("idle");
	}
	
	void Running()
	{
		animation.CrossFade("lari");
	}
	
	void Jumping()
	{
		if((!animation.isPlaying && TP_Controller.characterCtrller.isGrounded) ||
			TP_Controller.characterCtrller.isGrounded)
		{
			State= CharacterState.Idle;
			/*if(lastState == CharacterState.Running)
				animation.CrossFade("jalan");*/
		}
		else if(!animation.IsPlaying("lompat"))
		{
			State=CharacterState.Idle;
			
		}
		else
		{
			State=CharacterState.Jumping;
			//Help determine if we fell too far
		}
	}
	
	void Walking()
	{
		if(Input.GetKey("left shift"))
			Running ();
		else
			animation.CrossFade("jalan");
	}
	#endregion
	
	#region Start Action Method
	
	public void Jump()
	{
		if(!TP_Controller.characterCtrller.isGrounded || IsDead || State == CharacterState.Jumping)
			return;
		
		lastState = State;
		State = CharacterState.Jumping;
		animation.CrossFade("lompat");
	}
	
	public void Attack()
	{
		animation.CrossFade("tangkap");
		
	}
	#endregion
}


