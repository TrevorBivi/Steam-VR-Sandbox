using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ReloadDetecter : MonoBehaviour
{
	public string expectedBulletTag;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider collider){
		if(collider.gameObject.tag == expectedBulletTag && collider.gameObject.GetComponent<Interactable>().attachedToHand != null){
			print("Bullet entered reload area");

			this.GetComponentInParent<Gun>().attemptReload(collider.gameObject);
		}
	}
}
