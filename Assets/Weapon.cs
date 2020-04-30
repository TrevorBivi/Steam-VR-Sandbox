using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Weapon : MonoBehaviour
{
    private PrimaryGrip primaryGrip;
	private SecondaryGrip secondaryGrip;

	//public HandEvent onPickUp; //Changed from UnityEvent
    //public HandEvent onDetachFromHand; //Changed from UnityEvent
    //public HandEvent onHeldUpdate;

	public float releaseVelocityTimeOffset = -0.011f;
	RigidbodyInterpolation hadInterpolation = RigidbodyInterpolation.None;

	private Hand primaryHand;
	private Hand secondaryHand;

	void Awake(){
		this.primaryGrip = GetComponentInChildren<PrimaryGrip>();
		this.secondaryGrip = GetComponentInChildren<SecondaryGrip>();
	}

	void Start(){
		

		if(primaryGrip == null){
			print("NUL PRIMIARY GIRP !!!!!!!!!!!!!!!!!!!!!!");
		}else{
			print("GOT GRIPS");
		}
	}

	private void setPositioning() {
		Quaternion newRotation;
		if(this.primaryHand != null) {

			if(this.secondaryHand != null) {
				Vector3 secondaryHandOffset = secondaryGrip.transform.position - primaryGrip.transform.position;
				newRotation = Quaternion.LookRotation(secondaryHandOffset, Vector3.up);
			} else {
				newRotation = primaryGrip.transform.rotation;
			}
			newRotation *= Quaternion.Euler(-90,0,0);

			this.transform.position = this.primaryGrip.transform.position - newRotation * this.primaryGrip.gripOffset;

		} else if (this.secondaryHand != null) {

			newRotation = secondaryGrip.transform.rotation;
			newRotation *= Quaternion.Euler(-90,0,0);
			this.transform.position = this.secondaryGrip.transform.position - newRotation * secondaryGrip.gripOffset;

		}else{
			print("GARBAGE POSTION SET ATTEMPT");
			return;
		}

		this.transform.rotation = newRotation;

		if(primaryGrip == null){
			print("NUL PRIMIARY GIRP !!!!!!!!!!!!!!!!!!!!!!");
		}
	}

	public void attachHand(Grip grip, Hand hand){
		checkAndHandleAttach(hand);
		if(grip == primaryGrip){
			primaryHand = hand;
		} else if (grip == secondaryGrip){
			secondaryHand = hand;
		}
	}

	public void detachHand(Grip grip, Hand hand){
		checkAndHandleDetach(hand);
		if(grip == primaryGrip){
			primaryHand = null;
		} else if (grip == secondaryGrip){
			secondaryHand = null;
		}
	}

	public void heldUpdate(){
		print("SETTING POSITION");
		setPositioning();
	}
	/*
	public void setPrimaryGripHand(Hand hand){
		print("WEAPON > SET PRIMARY GRIP HAND");
		if(hand != null){
			print("HAND NOT NULL");
		}else{
			print("HAND NULL");
		}
		checkAndHandleAttach(hand);
		this.primaryHand = hand;
		checkAndHandleDrop(hand);
	}

	public void setSecondaryGripHand(Hand hand){
		print("WEAPON > SET SECONDARY GRIP HAND");
		if(hand != null){
			print("HAND NOT NULL");
		}else{
			print("HAND NULL");
		}
		this.checkAndHandleAttach(hand);
		this.secondaryHand = hand;
		this.checkAndHandleDrop(hand);
		
	}*/

	void checkAndHandleAttach(Hand hand){
		if(this.secondaryHand == null && this.primaryHand == null && hand != null){
			hadInterpolation = this.GetComponent<Rigidbody>().interpolation;

			//attached = true;
			print("singleHandTool invoke on pickup");
			//if (onPickup != null){
			//	onPickUp.Invoke(hand);
			//}

			hand.HoverLock( null );

			this.GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;

			//if (velocityEstimator != null)
			//velocityEstimator.BeginEstimatingVelocity();
			//attachTime = Time.time;
			//attachPosition = transform.position;
			//attachRotation = transform.rotation;
		}
	}

	void checkAndHandleDetach(Hand hand){
		if(this.secondaryHand == null && this.primaryHand == null){
			//attached = false;
			//if (onDetachFromHand != null){
			//	onDetachFromHand.Invoke(hand);
			//}

			hand.HoverUnlock(null);

			this.GetComponent<Rigidbody>().interpolation = hadInterpolation;
			this.GetComponent<Rigidbody>().velocity = hand.GetTrackedObjectVelocity(releaseVelocityTimeOffset);
			this.GetComponent<Rigidbody>().angularVelocity = hand.GetTrackedObjectAngularVelocity(releaseVelocityTimeOffset);
		}
	}
}
