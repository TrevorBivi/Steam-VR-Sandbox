using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Weapon : MonoBehaviour
{
    private PrimaryGrip primaryGrip;
	private PrimaryGrip secondaryGrip;

	void awake(){
		PrimaryGrip primaryGrip = this.GetComponentInChildren<PrimaryGrip>();
		SecondaryGrip secondaryGrip = this.GetComponentInChildren<SecondaryGrip>();
	}

	private void setPositioning() {
		if(this.primaryGrip.attachedHand != null) {

			if(this.secondaryGrip.attachedHand != null) {
				Vector3 secondaryHandOffset = secondaryGrip.attachedHand.transform.position - primaryGrip.attachedHand.transform.position;
				this.transform.rotation = Quaternion.LookRotation(secondaryHandOffset, Vector3.up);
			} else {
				this.transform.rotation = primaryGrip.transform.rotation;
			}

			this.transform.position = this.primaryGrip.attachedHand.transform.position - this.transform.rotation * this.primaryGrip.transform.localPosition;

		} else if (this.secondaryGrip.attachedHand != null) {

			this.transform.rotation = secondaryGrip.transform.rotation;
			this.transform.position = secondaryGrip.attachedHand.transform.position - this.transform.rotation * secondaryGrip.transform.localPosition;

		}
	}

	public void setPrimaryGripHand(Hand hand){
		this.dropCheck();
	}

	public void setSecondaryGripHand(Hand hand){
		this.dropCheck();
	}

	void dropCheck(){
		if(this.secondaryGrip.attachedHand == null && this.primaryGrip.attachedHand == null){
			
		}
	}
}
