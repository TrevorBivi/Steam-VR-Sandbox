using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Grip : MonoBehaviour
{
	/*[Tooltip("The local point which acts as a positional and rotational offset to use while held")]
	public Transform attachmentOffset;
	public float releaseVelocityTimeOffset = -0.011f;


	

	protected Vector3 attachPosition;
	protected Quaternion attachRotation;
	protected RigidbodyInterpolation hadInterpolation = RigidbodyInterpolation.None;
	protected bool attached = false;
	*/
	public Hand attachedHand;

	public HandEvent onPickUp; //Changed from UnityEvent
	public HandEvent onDetachFromHand; //Changed from UnityEvent
	public HandEvent onHeldUpdate;

	public Vector3 gripOffset;
	public bool restoreOriginalParent = false;
	public Weapon weapon;
	public bool allowPinchGrab = false;

	public void Awake(){
		//attachedHand = null;
		gripOffset = this.GetComponent<Transform>().localPosition;

	}

	protected virtual void HandHoverUpdate( Hand hand )
	{
		GrabTypes startingGrabType = hand.GetGrabStarting();
		if (startingGrabType == GrabTypes.Grip || (startingGrabType == GrabTypes.Pinch && allowPinchGrab))
		{
			print("GRIP > HAND HOVER UPDATE ");
			if(hand != null){
				print("HAND NOT NULL");
			}else{
				print("HAND NULL");
			}
			//attachedHand = hand;
			hand.AttachObject(this.gameObject, GrabTypes.Grip, Hand.AttachmentFlags.ParentToHand |  Hand.AttachmentFlags.SnapOnAttach, null);
			if( onPickUp != null ){
				onPickUp.Invoke(hand);
			}
		}
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
		}

		weapon.heldUpdate();

		if (onHeldUpdate != null)
			onHeldUpdate.Invoke(hand);
	}

	
	protected virtual void OnAttachedToHand( Hand hand )
	{
		print("GRIP > ON ATTACHED TO HAND");
		if(hand != null){
			print("HAND NOT NULL");
		}else{
			print("HAND NULL");
		}
		weapon.attachHand(this,hand);
		//RigidBody 
		/*
		hadInterpolation = parentRigidbody.interpolation;

		attached = true;
		onPickUp.Invoke(hand);

		hand.HoverLock( null );

		parentRigidbody.interpolation = RigidbodyInterpolation.None;

		//if (velocityEstimator != null)
		//    velocityEstimator.BeginEstimatingVelocity();
		//attachTime = Time.time;
		attachPosition = transform.position;
		attachRotation = transform.rotation;

		weapon.setPrimaryGripHand(hand);*/
	}

	protected virtual void OnDetachedFromHand(Hand hand)
	{
		print("GRIP > ON DETATCHED FROM HAND");
		if(hand != null){
			print("HAND NOT NULL");
		}else{
			print("HAND NULL");
		}
		weapon.detachHand(this,hand);
		/*attached = false;
		onDetachFromHand.Invoke(hand);
		hand.HoverUnlock(null);
		parentRigidbody.interpolation = hadInterpolation;
		Vector3 velocity;
		Vector3 angularVelocity;

		//GetReleaseVelocities(hand, out velocity, out angularVelocity);

		parentRigidbody.velocity = new Vector3(0,0,0);//velocity;
		parentRigidbody.angularVelocity = new Vector3(0,0,0);//angularVelocity;

		weapon.setPrimaryGripHand(null);*/
	}

}
