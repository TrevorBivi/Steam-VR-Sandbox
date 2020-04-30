using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class BoltGrip : Grip
{
	
	public float minDistanceToGrip;
	public float openPosition;//=-0.0359;
	public float closedPosition;//=-0.0924;
	public byte state;
	public float unlockRot;
	public Transform bolt;

	void Awake(){
		state=Constants.BREECH_CLOSED;
	}

	void Start(){
		
	}

	protected virtual void HandAttachedUpdate(Hand hand)
	{
		if (hand.IsGrabEnding(this.gameObject))
		{
			print("GRIP > HAND ATTACHED UPDATE");
			if(hand != null){
				print("HAND NOT NULL");
			}else{
				print("HAND NULL");
			}
			//attachedHand = null;
			hand.DetachObject(this.gameObject, true);
			if(onDetachFromHand != null){
				onDetachFromHand.Invoke(hand);
			}
		} else {
			
			///if(this.state == Constants.BREECH_UNLOCKED_ROTATING){
				Vector3 rot = this.weapon.GetComponent<Transform>().rotation.eulerAngles + new Vector3(0,0,180);
				Vector3 delta = Quaternion.Euler(rot) * (this.GetComponent<Transform>().position - bolt.position);
				print("DX DZ  DZ" + " " + delta.x + " " + delta.y +" "+ delta.z);
				float boltRotation = -Mathf.Atan2(delta.z,delta.x)*Mathf.Rad2Deg;  //Quaternion.LookRotation().eulerAngles.y;
				boltRotation = -90 - Mathf.Clamp(boltRotation,-90,0);
				print("ROTATION"+boltRotation);
				bolt.transform.localRotation = Quaternion.Euler(0,boltRotation,180);
				/*if (boltRotation == -90 && this.transform.localPosition.y >= bolt.localPosition.y){
					this.state = Constants.BREECH_UNLOCKED_SLIDING;
				}*/
				print("ROTATED");
			/*}

			if(this.state == Constants.BREECH_UNLOCKED_SLIDING){
				float slidePosition = this.transform.localPosition.y;
				slidePosition = Mathf.Clamp(slidePosition,closedPosition,openPosition);
				this.transform.localPosition = new Vector3(0,slidePosition,0);
				if(slidePosition == closedPosition){
					float boltRotation = Quaternion.LookRotation(this.GetComponent<Transform>().localPosition - bolt.localPosition).eulerAngles.y;
					if (boltRotation > -90){
						this.state = Constants.BREECH_UNLOCKED_ROTATING;
					}
				}
				print("SLID");
			}*/

		}

		if (onHeldUpdate != null)
			onHeldUpdate.Invoke(hand);
	}

	
	protected virtual void OnAttachedToHand( Hand hand )
	{
		
		if(this.state == Constants.BREECH_CLOSED){
			this.state = Constants.BREECH_UNLOCKED_ROTATING;
		}
		print("SETTING TO ROTATE");
		/*print("GRIP > ON ATTACHED TO HAND");
		if(hand != null){
			print("HAND NOT NULL");
		}else{
			print("HAND NULL");
		}
		weapon.attachHand(this,hand);*/
	}

	protected virtual void OnDetachedFromHand(Hand hand)
	{
		/*print("GRIP > ON DETATCHED FROM HAND");
		if(hand != null){
			print("HAND NOT NULL");
		}else{
			print("HAND NULL");
		}
		weapon.detachHand(this,hand);*/
	}
}
