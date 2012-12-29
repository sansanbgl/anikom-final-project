using UnityEngine;
using System.Collections;

public class TP_Controller : MonoBehaviour {
	
	public static CharacterController characterCtrller;
	public static TP_Controller Instance;
	private float speed = 2.5f;
	
	GameObject weapon1;
	GameObject weapon2;
	
	void Awake() 
	{
		characterCtrller = GetComponent("CharacterController") as  CharacterController;
		Instance = this;
		TP_Camera.UseExistingOrCreateNewMainCamera();
		animation.wrapMode=WrapMode.Loop;
		animation["lompat"].wrapMode =WrapMode.Once;
		animation["tangkap"].wrapMode=WrapMode.Once;
		animation["tangkap"].AddMixingTransform(transform.Find("Armature/master/Bone/Bone_R/Bone_R_001"),true);
		animation["tangkap"].layer=1;
		
		weapon1 = GameObject.Find("SpongeBob's Jellyfish Cashing Net");
		weapon2 = GameObject.Find("spatula");
		weapon2.active=false;
	}
	
	void Update() 
	{
		if(Camera.mainCamera == null)
			return;
		
		GetLocomotionInput();
		HandleActionInput();
		TP_Motor.Instance.UpdateMotor(); 
	}
	
	void GetLocomotionInput()
	{
		var deadZone = 0.01f;
		
		TP_Motor.Instance.VerticalVelocity = TP_Motor.Instance.MoveVector.y;
		TP_Motor.Instance.MoveVector = Vector3.zero;
		
		if(Input.GetAxis("Vertical") > deadZone || Input.GetAxis("Vertical")< -deadZone)
        {
            animation.CrossFade("jalan");
			TP_Motor.Instance.MoveVector += new Vector3(0,0,Input.GetAxis("Vertical"));
		}

        if (Input.GetAxis("Horizontal") > deadZone || Input.GetAxis("Horizontal") < -deadZone)
        {
            animation.CrossFade("jalan");
            TP_Motor.Instance.MoveVector += new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        }
		
		TP_Animator.Instance.DetermineCurrentMoveDirection();
	}
	
	void HandleActionInput()
	{
		if(Input.GetKey("space"))
		{
			Jump();
		}
		if(Input.GetMouseButton(0))
			Attack ();
		else if(Input.GetKey(KeyCode.Q))
			SwapWeapons();
	}
	
	void Jump()
	{
		//animation.CrossFade("lompat");
		TP_Motor.Instance.Jump();
		TP_Animator.Instance.Jump();
	}
	
	void Attack()
	{
		TP_Animator.Instance.Attack();
	}
	
	void SwapWeapons(){
		if (weapon1.active == true) {
			weapon1.SetActiveRecursively(false);
			weapon2.SetActiveRecursively(true);
		} else {
			weapon1.SetActiveRecursively(true);
			weapon2.SetActiveRecursively(false);
		}
	}
}

