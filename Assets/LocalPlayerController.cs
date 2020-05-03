using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR;
using Valve.VR.InteractionSystem;

public class LocalPlayerController : MonoBehaviour
{
	public float speed = 3;
	public SteamVR_Action_Vector2 move;
	private CharacterController characterController;
	//
	void Awake(){
		characterController = GetComponentInParent<CharacterController>();
	}

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(move.axis.x, 0, move.axis.y));
		characterController.Move(speed*Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up)-new Vector3(0,9.81f,0)*Time.deltaTime);
	}
}
