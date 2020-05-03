using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
	public GameObject localPlayer;
	public GameObject otherPlayer;
	public GameObject cubeObject;

	private const int steamVrObjectsIndex = 0;
	private const int leftHandIndex = 1;
	private const int rightHandIndex = 2;
	private const int headIndex = 3;
	private Transform leftHand;
	private Transform rightHand;
	private Transform head;
	private CharacterController characterController;

	public override void OnStartClient() {
		base.OnStartClient();
	}

	void Awake(){
		characterController = GetComponentInParent<CharacterController>();
	}

    void Start() { 
		if(base.hasAuthority){
			this.transform.position = new Vector3(Random.Range(-4f,4f),0f,0f);
			
			localPlayer.SetActive(true);
			leftHand = localPlayer.transform.GetChild(steamVrObjectsIndex).GetChild(leftHandIndex);
			rightHand = localPlayer.transform.GetChild(steamVrObjectsIndex).GetChild(rightHandIndex);
			head = localPlayer.transform.GetChild(steamVrObjectsIndex).GetChild(headIndex);
			
			localPlayer.transform.localPosition = new Vector3(-head.localPosition.x, localPlayer.transform.localPosition.y, -head.localPosition.z);
			InvokeRepeating("ReportTrackedTransforms",0.09f,0.09f);
			
		}else{
			//Instantiate(cubeObject, new Vector3(0,0,0), Quaternion.Euler(0,0,0));
			otherPlayer.SetActive(true);

			leftHand = otherPlayer.transform.GetChild(leftHandIndex);
			rightHand = otherPlayer.transform.GetChild(rightHandIndex);
			head = otherPlayer.transform.GetChild(headIndex);

		}
	}

	void ReportTrackedTransforms(){
		print("RUNNING");
		float movedX = head.localPosition.x + localPlayer.transform.localPosition.x;
		float movedZ = head.localPosition.z + localPlayer.transform.localPosition.z;
		localPlayer.transform.localPosition = new Vector3(-head.localPosition.x, localPlayer.transform.localPosition.y, -head.localPosition.z);
		characterController.Move(new Vector3(movedX,0,movedZ));
		
		CmdSetTrackedTransforms(
			head.position.y,
			head.rotation,
			leftHand.position,
			leftHand.rotation,
			rightHand.position,
			rightHand.rotation
		);
	}

	[Command]
	void CmdSetTrackedTransforms(float headHeight, Quaternion headRotation, Vector3 leftHandPosition, Quaternion leftHandRotation, Vector3 rightHandPosition, Quaternion rightHandRotation ){
		RpcSetTrackedTransforms(headHeight, headRotation, leftHandPosition, leftHandRotation, rightHandPosition, rightHandRotation);
	}

	[ClientRpc]
	void RpcSetTrackedTransforms(float headHeight, Quaternion headRotation, Vector3 leftHandPosition, Quaternion leftHandRotation, Vector3 rightHandPosition, Quaternion rightHandRotation){
		if(!base.hasAuthority){
			SetTrackedTransforms(headHeight, headRotation, leftHandPosition, leftHandRotation, rightHandPosition, rightHandRotation);
		}
	}

	void SetTrackedTransforms(float headHeight, Quaternion headRotation, Vector3 leftHandPosition, Quaternion leftHandRotation, Vector3 rightHandPosition, Quaternion rightHandRotation){
		head.position = new Vector3(head.position.x, headHeight, head.position.z);
		head.rotation = headRotation;
		leftHand.position = leftHandPosition;
		leftHand.rotation = leftHandRotation;
		rightHand.position = rightHandPosition;
		rightHand.rotation = rightHandRotation;
	}
}
