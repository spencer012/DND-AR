using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	public GameObject tilePrefab;
	public GameObject[] tiles;
	public List<GameObject> pieces;

	private int[,] map = { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };
	private float selectCooldown = 0;

	public LayerMask raycastHit;

	private GameObject selected;
	public Material selectedMaterial;
	private Material prevMaterial;

	private static float selectDelay = 0.2f;

	void Start() {

	}

	private RaycastHit hit;
	void Update() {
		if(selectCooldown > 0) {
			selectCooldown -= Time.deltaTime;
		}
		TouchInput();
	}

	void TouchInput() {
		foreach(Touch touch in Input.touches) {
			if(touch.phase == TouchPhase.Began) {
				if(selectCooldown > 0) {
					selectCooldown = selectDelay;
				}
				if(Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hit, raycastHit)) {
					GameObject gHit = hit.collider.gameObject;
					if(gHit.tag.Equals("Piece") && selectCooldown <= 0) {
						if(selected == null) { //if nothing was selected
							Renderer tmp = gHit.GetComponent<Renderer>();
							prevMaterial = tmp.material;
							tmp.material = selectedMaterial;
							selected = gHit;
							selectCooldown = selectDelay;
						}
						else if(!gHit.Equals(selected)) { //if something else was selected
							selected.GetComponent<Renderer>().material = prevMaterial;
							Renderer tmp = gHit.GetComponent<Renderer>();
							prevMaterial = tmp.material;
							tmp.material = selectedMaterial;
							selected = gHit;
							selectCooldown = selectDelay;
						}
					}
					else if(gHit.tag.Equals("Tile") && selectCooldown <= 0 && selected != null) {

					}
				}
				else if(selectCooldown <= 0) {
					selected.GetComponent<Renderer>().material = prevMaterial;
					selected = null;
				}
			} //touchphase began
		}
	}
}
