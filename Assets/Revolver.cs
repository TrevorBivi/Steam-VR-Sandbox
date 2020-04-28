using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Revolver : Gun
{
	public GameObject trigger;
	public GameObject hammer;
	public GameObject cylinder;
	public GameObject cylinderRelease;
	public GameObject loadedBullets;

	public float maxTriggerRot;
	public float maxHammerRot;
	public float maxCylinderReleaseRot;
	public float cylinderReleaseSpeed;
	public byte maxRounds;
	public float minDumpAngle;
	public float minTriggerRelease;
	public GameObject bulletPrefab;
	public GameObject casingPrefab;
	private byte[] chamberStates;

	public SteamVR_Action_Single m_SqueezeTrigger;
	public SteamVR_Action_Boolean m_fireAction;
	public SteamVR_Action_Boolean m_ammoRelease;

	private Interactable interactable;
	private byte cylinderIndex;
	private bool attached;
	private bool needTriggerRelease;
	private bool cocked;
	private bool cylinderReleased;
	private float cylinderReleaseRotationTarget;

	private Hand primaryHand;

	void awake(){
		//m_Vector1Action = SteamVR_Actions._default.Squeeze;
		//m_Vector1Action[SteamVR_Input_Sources.Any].onAxis += AxisTest;
	}

	//private void AxisTest(SteamVR_Action_Vector1 action, SteamVR_Input_Sources source, Vector1 axis, Vector1 delta){
	//    print("AxisTest" + axis);
	//}

    // Start is called before the first frame update
    void Start()
    {
		chamberStates = new byte[maxRounds];
		for(int i = 0; i < maxRounds; i++){
			chamberStates[i] = Constants.CHAMBERED_BULLET;
		}
		cylinderIndex = 0;
		cocked = false;
		attached = false;
		interactable = GetComponent<Interactable>();
		needTriggerRelease = false;
		cylinderReleased = false;
		cylinderReleaseRotationTarget = 0;
    }

    // Update is called once per frame
    void Update()
    {
		
        //print("triggerAxis" + m_SqueezeTrigger.axis);
    }

	public void HandlePickup(Hand hand){
		//print("PICKED A REVOLVER");
		if(primaryHand == null){
			primaryHand = hand;
		}
	}

	public override void attemptReload(GameObject reloadObject){
		int testChamber = cylinderIndex+1;
		for(int i = 0; i < maxRounds; i++){
			if (testChamber == maxRounds){
				testChamber = 0;
			}
			if (chamberStates[testChamber] == Constants.CHAMBER_EMPTY){
				chamberStates[testChamber] = Constants.CHAMBERED_BULLET;
				loadedBullets.transform.GetChild(testChamber).gameObject.SetActive(true);
				reloadObject.GetComponent<Interactable>().attachedToHand.DetachObject(reloadObject);
				Object.Destroy(reloadObject);
				break;
			}
			testChamber++;
		}
	}

	public void HandleDetachFromHand(Hand hand){
		print("DROPPED A REVOLVER");
		if(hand == primaryHand){
			trigger.transform.localRotation = Quaternion.Euler( 0,0,0  );		
			cylinder.transform.localRotation = Quaternion.Euler( 0,cylinderIndex * 360/maxRounds,0 );
			if(!cocked){
				hammer.transform.localRotation = Quaternion.Euler( 0,0,0 );
			}
			if(hand.otherHand.currentAttachedObject == this.gameObject){
				hand.otherHand.DetachObject(this.gameObject);
			}
		}else{
			//interactable.attachedTohand = primaryHand;

			//primaryHand.AttachObject( this.gameObject, GrabTypes.Grip, Hand.AttachmentFlags.ParentToHand |  Hand.AttachmentFlags.SnapOnAttach |  Hand.AttachmentFlags.TurnOffGravity, null);
		}
	}

	public void attemptFire(){
		needTriggerRelease=true;
		hammer.transform.localRotation = Quaternion.Euler( 0,0,0 );
		cylinder.transform.localRotation = Quaternion.Euler( 0,cylinderIndex * 360/maxRounds,0 );

		cylinderIndex++;
		if(cylinderIndex == maxRounds){
			cylinderIndex = 0;
		}
		if(chamberStates[cylinderIndex] == Constants.CHAMBERED_BULLET){
			print("FIRE");
			chamberStates[cylinderIndex] = Constants.CHAMBERED_CASING;
		}
	}

	public void dumpChamberContents(){
		for (int i = 0; i < maxRounds; i++){
			float cylinderAngle = 360/maxRounds * (cylinderIndex + i);
			if (chamberStates[i] != Constants.CHAMBER_EMPTY){
				
				Transform child = loadedBullets.transform.GetChild(i);
				child.gameObject.SetActive(false);

				if (chamberStates[i] == Constants.CHAMBERED_CASING){
					Instantiate(casingPrefab, child.position, child.rotation);
				} else if (chamberStates[i] == Constants.CHAMBERED_BULLET){
					Instantiate(bulletPrefab, child.position, child.rotation);
				}

				chamberStates[i] = Constants.CHAMBER_EMPTY;
			} 
		}
		
	}


	public void HandleHeldUpdate(Hand hand){
		print("HANDLE HELD UPDATE");
		if(interactable.attachedToHand != null){//Ensure that object is attached to a hand
			print("INTERACTABLE");
			SteamVR_Input_Sources source = primaryHand.handType;

			float squeezeWeight = m_SqueezeTrigger[source].axis;//Get how squeezed the trigger is
			trigger.transform.localRotation = Quaternion.Euler( maxTriggerRot*squeezeWeight,0,0);//Apply squeeze to trigger object

			if(m_fireAction[source].stateDown && !needTriggerRelease && !cylinderReleased){//If gun is fired this frame
				attemptFire();
			}else if(m_fireAction[source].state == false){//If trigger isn't fully squeezed
				if(needTriggerRelease){
					if(squeezeWeight <= minTriggerRelease){
					    print("REVOLVER TRIGGER RELEASED");
						needTriggerRelease=false;
					}
				}else{
					hammer.transform.localRotation = Quaternion.Euler( maxHammerRot*squeezeWeight,0,0);
					cylinder.transform.localRotation = Quaternion.Euler( 0, 360/maxRounds*(squeezeWeight + cylinderIndex),0);
				}
			}

			if(m_ammoRelease[source].stateDown){//If ammo release toggled this frame
				if(cylinderReleaseRotationTarget == 0){
					cylinderReleaseRotationTarget=maxCylinderReleaseRot;
					cylinderReleased = true;
				}else{
					cylinderReleaseRotationTarget=0;
				}
			}

			if( cylinderReleased ){//If cylinder release needs to be handled
				if ( Mathf.Abs(Quaternion.Angle( cylinderRelease.transform.localRotation, Quaternion.Euler(0,maxCylinderReleaseRot,0))) > 0.01 ){ // Moving cylinder release
					print("Animating");
					Quaternion targetRotation = Quaternion.Euler(0,cylinderReleaseRotationTarget,0);
					Quaternion currentRotation = cylinderRelease.transform.localRotation;
					cylinderRelease.transform.localRotation = Quaternion.RotateTowards(
						currentRotation,
						targetRotation,
						cylinderReleaseSpeed*Time.deltaTime);
				} else if (cylinderReleaseRotationTarget==0) { // Finished cylinder release
					print("Done release" + Quaternion.Angle( cylinderRelease.transform.localRotation, Quaternion.Euler(0,maxCylinderReleaseRot,0)));
					cylinderRelease.transform.localRotation = Quaternion.Euler(0,0,0);
					cylinderReleased=false;
				} else if (minDumpAngle < Vector3.Dot(this.transform.rotation*(new Vector3(0,-1,0)), new Vector3(0,1,0)) ) { // Fully released, wait to angle for dump
					dumpChamberContents();
				}
			} 
		}
	}
}
