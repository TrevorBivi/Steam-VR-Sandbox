﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Valve.VR.InteractionSystem
{
	[RequireComponent( typeof( Interactable ) )]
	public class PrimaryGrip : Grip
	{


	/*
		protected virtual void OnAttachedToHand( Hand hand )
		{
			print("PRIMARY GRIP > ON ATTACHED TO HAND");
			if(hand != null){
				print("HAND NOT NULL");
			}else{
				print("HAND NULL");
			}
			weapon.setPrimaryGripHand(hand);
			//RigidBody 
			
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

			weapon.setPrimaryGripHand(hand);
		}

		protected virtual void OnDetachedFromHand(Hand hand)
		{
			print("PRIMARY GRIP > ON DETATCHED FROM HAND");
			if(hand != null){
				print("HAND NOT NULL");
			}else{
				print("HAND NULL");
			}
			weapon.setPrimaryGripHand(null);
			attached = false;
			onDetachFromHand.Invoke(hand);
			hand.HoverUnlock(null);
			parentRigidbody.interpolation = hadInterpolation;
			Vector3 velocity;
			Vector3 angularVelocity;

			//GetReleaseVelocities(hand, out velocity, out angularVelocity);

			parentRigidbody.velocity = new Vector3(0,0,0);//velocity;
			parentRigidbody.angularVelocity = new Vector3(0,0,0);//angularVelocity;

			weapon.setPrimaryGripHand(null);
		}*/


	}
}