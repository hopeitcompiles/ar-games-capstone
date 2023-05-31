using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class bauScript : MonoBehaviour {


	public string player = "Player";
	private float velocity = 30.0f;

	public GameObject target;
	public List<GameObject> Contents;

	bool abrir = true;
	GameObject alvo;
	bool cheio = true;

	//rotatação max  -60 -900 -900
	//0, -720, -720

	// Use this for initialization
	void Start () {
		velocity = 40.0f;
	}
	
	// Update is called once per frame
	void Update () {

		if (abrir) {
			if (this.transform.localRotation.x > -0.9) {
				this.transform.Rotate (new Vector3 (-velocity * Time.deltaTime * 2, 0.0f, 0.0f));
				if(this.transform.localRotation.x < -0.45 && cheio == true){
					liberar ();
					cheio = false;
				}
			} 
		} else {
			if (this.transform.localRotation.x < 0) {
				this.transform.Rotate (new Vector3 (velocity * Time.deltaTime * 2 , 0.0f, 0.0f));

			} 
		}
	
	}


	void OnTriggerStay(Collider other) { 

		if (alvo == null && abrir == false) {
			Vector3 pos = this.transform.localPosition;
			pos.y += 0.5f;
			pos.z += 0.25f;
			alvo = Instantiate (target, pos, Quaternion.identity) as GameObject;
		}

		abrir = true;

	}

	void OnTriggerExit(Collider other) {		
		abrir = false;
	}


	public void liberar(){

		Vector3 pos = this.transform.position;
		//pos.x = 0;
		pos.y += 0.4f;
		pos.z += 0.4f;

		for (int i = 0; i < Contents.Count; i++) {
			GameObject premio = Instantiate (Contents[i], pos, Quaternion.identity) as GameObject;

			Rigidbody rb = premio.GetComponent<Rigidbody>();
			if (rb == null) {
				rb = premio.AddComponent<Rigidbody> ();
			}
			rb.AddForce (new Vector3(0,1,1));
		}



	}
}
