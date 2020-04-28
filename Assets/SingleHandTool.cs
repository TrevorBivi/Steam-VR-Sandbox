using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Valve.VR.InteractionSystem
{
	[RequireComponent( typeof( Interactable ) )]
	[RequireComponent( typeof( Rigidbody ) )]
	public class SingleHandTool : MonoBehaviour
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
		// Start is called before the first frame update


		void Start()
		{
        
		}

		// Update is called once per frame
		void Update()
		{
        
		}

		public virtual void GetReleaseVelocities(Hand hand, out Vector3 velocity, out Vector3 angularVelocity)
        {
            /*if (hand.noSteamVRFallbackCamera && releaseVelocityStyle != ReleaseStyle.NoChange)
                releaseVelocityStyle = ReleaseStyle.ShortEstimation; // only type that works with fallback hand is short estimation.

            switch (releaseVelocityStyle)
            {
                case ReleaseStyle.ShortEstimation:
                    if (velocityEstimator != null)
                    {
                        velocityEstimator.FinishEstimatingVelocity();
                        velocity = velocityEstimator.GetVelocityEstimate();
                        angularVelocity = velocityEstimator.GetAngularVelocityEstimate();
                    }
                    else
                    {
                        Debug.LogWarning("[SteamVR Interaction System] Throwable: No Velocity Estimator component on object but release style set to short estimation. Please add one or change the release style.");

                        velocity = rigidbody.velocity;
                        angularVelocity = rigidbody.angularVelocity;
                    }
                    break;
                case ReleaseStyle.AdvancedEstimation:
                    hand.GetEstimatedPeakVelocities(out velocity, out angularVelocity);
                    break;
                case ReleaseStyle.GetFromHand:*/
                    velocity = hand.GetTrackedObjectVelocity(releaseVelocityTimeOffset);
                    angularVelocity = hand.GetTrackedObjectAngularVelocity(releaseVelocityTimeOffset);
                   /* break;
                default:
                case ReleaseStyle.NoChange:
                    velocity = rigidbody.velocity;
                    angularVelocity = rigidbody.angularVelocity;
                    break;
            }*/

            /*if (releaseVelocityStyle != ReleaseStyle.NoChange)
            {
                    float scaleFactor = 1.0f;
                    if (scaleReleaseVelocityThreshold > 0)
                    {
                        scaleFactor = Mathf.Clamp01(scaleReleaseVelocityCurve.Evaluate(velocity.magnitude / scaleReleaseVelocityThreshold));
                    }

                    velocity *= (scaleFactor * scaleReleaseVelocity);
            }*/
        }

		protected virtual void HandHoverUpdate( Hand hand )
		{
			GrabTypes startingGrabType = hand.GetGrabStarting();
			if (startingGrabType == GrabTypes.Grip)
			{
				if(hand.otherHand.currentAttachedObject != gameObject){
					hand.AttachObject( gameObject, GrabTypes.Grip, Hand.AttachmentFlags.ParentToHand |  Hand.AttachmentFlags.SnapOnAttach |  Hand.AttachmentFlags.TurnOffGravity, null);
				}else{
					//hand.AttachObject( gameObject, GrabTypes.Grip, 0 , null);
					//SteamVR_Skeleton_PoseSnapshot pose = attachedObject.interactable.skeletonPoser.GetBlendedPose(skeleton);
				}
			}
		}

		protected virtual void OnAttachedToHand( Hand hand )
		{
			//Debug.Log("<b>[SteamVR Interaction]</b> Pickup: " + hand.GetGrabStarting().ToString());

			hadInterpolation = this.GetComponent<Rigidbody>().interpolation;

			attached = true;
			print("singleHandTool invoke on pickup");
			onPickUp.Invoke(hand);

			hand.HoverLock( null );

			GetComponent<Rigidbody>().interpolation = RigidbodyInterpolation.None;

			//if (velocityEstimator != null)
			//    velocityEstimator.BeginEstimatingVelocity();

			//attachTime = Time.time;
			attachPosition = transform.position;
			attachRotation = transform.rotation;

		}

		protected virtual void OnDetachedFromHand(Hand hand)
		{
			attached = false;

			onDetachFromHand.Invoke(hand);

			hand.HoverUnlock(null);

			GetComponent<Rigidbody>().interpolation = hadInterpolation;

			Vector3 velocity;
			Vector3 angularVelocity;

			GetReleaseVelocities(hand, out velocity, out angularVelocity);

			GetComponent<Rigidbody>().velocity = velocity;
			GetComponent<Rigidbody>().angularVelocity = angularVelocity;
		}

		protected virtual void HandAttachedUpdate(Hand hand)
		{


			if (hand.IsGrabEnding(this.gameObject))
			{
				hand.DetachObject(gameObject, restoreOriginalParent);

				// Uncomment to detach ourselves late in the frame.
				// This is so that any vehicles the player is attached to
				// have a chance to finish updating themselves.
				// If we detach now, our position could be behind what it
				// will be at the end of the frame, and the object may appear
				// to teleport behind the hand when the player releases it.
				//StartCoroutine( LateDetach( hand ) );
			}

			if (onHeldUpdate != null)
				onHeldUpdate.Invoke(hand);
		}
	}
}