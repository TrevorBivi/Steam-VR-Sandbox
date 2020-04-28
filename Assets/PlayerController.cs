using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerController : MonoBehaviour
{
	public SteamVR_Action_Vector2 move;
	public SteamVR_Action_Boolean grabPinch;

	public float speed = 3;
	public Hand leftHand;
	public Hand rightHand;

	public GameObject revolverBullet;

	//public PrefabType revolverPrefabType;

	private Hand[] hands;

	private CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
		hands = new Hand[2];
		hands[0] = leftHand;
		hands[1] = rightHand;
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(move.axis.x, 0, move.axis.y));
		characterController.Move(speed*Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up)-new Vector3(0,9.81f,0)*Time.deltaTime);
		
		foreach(Hand hand in hands){
			if(grabPinch[hand.handType].stateDown){
				print("SPAWN AMMO CHECK");
				GameObject otherHandAttachedObject = hand.otherHand.currentAttachedObject;
				if (otherHandAttachedObject != null){
					if(otherHandAttachedObject.GetComponent<Revolver>()){
						print("SHOULD SPAWN AMMO");
						GameObject newBullet = Instantiate(revolverBullet, hand.transform.position, hand.transform.rotation);
						hand.AttachObject(newBullet, GrabTypes.Pinch, Hand.AttachmentFlags.SnapOnAttach | Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.TurnOnKinematic, null);
					}
				}
			}
		}

	}
}
