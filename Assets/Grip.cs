using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class Grip : MonoBehaviour
{
	[Tooltip("The local point which acts as a positional and rotational offset to use while held")]
	public Transform attachmentOffset;
	public float releaseVelocityTimeOffset = -0.011f;

	public HandEvent onPickUp; //Changed from UnityEvent
	public HandEvent onDetachFromHand; //Changed from UnityEvent
	public HandEvent onHeldUpdate;
	public bool restoreOriginalParent = false;

	protected Vector3 attachPosition;
	protected Quaternion attachRotation;
	protected RigidbodyInterpolation hadInterpolation = RigidbodyInterpolation.None;
	protected bool attached = false;

	public Rigidbody parentRigidbody;
	public Weapon weapon;

	public Hand attachedHand;

	public void awake(){
		attachedHand = null;
		parentRigidbody = this.GetComponentInParent<Rigidbody>();
		weapon = GetComponentInParent<Weapon>();
	}

	protected virtual void HandHoverUpdate( Hand hand )
	{
		GrabTypes startingGrabType = hand.GetGrabStarting();
		if (startingGrabType == GrabTypes.Grip)
		{
			attachedHand = hand;
			hand.AttachObject(this.gameObject, GrabTypes.Grip, Hand.AttachmentFlags.ParentToHand |  Hand.AttachmentFlags.SnapOnAttach |  Hand.AttachmentFlags.TurnOffGravity, null);
		}
	}

	protected virtual void HandAttachedUpdate(Hand hand)
	{
		if (hand.IsGrabEnding(this.gameObject))
		{
			attachedHand = null;
			hand.DetachObject(gameObject, restoreOriginalParent);
		}

		if (onHeldUpdate != null)
			onHeldUpdate.Invoke(hand);
	}

}
