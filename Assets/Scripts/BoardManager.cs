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

	public readonly static float scale = 0.05f;

	void Start() {

	}

	private RaycastHit hit;
	
	void Update() {
		if (selectCooldown > 0) {
			selectCooldown -= Time.deltaTime;
		}
		TouchInput();
	}

	void TouchInput() {
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) {
				if (selectCooldown > 0) { //if it was touched before the cooldown was ended
					selectCooldown = Mathf.Min(selectCooldown * 1.5f, selectDelay);
				}
				if (selectCooldown <= 0 && Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hit, raycastHit)) {
					GameObject gHit = hit.collider.gameObject;
					if (gHit.tag.Equals("Piece") || gHit.tag.Equals("Moving")) { //if it is a piece on the board
						if (selected == null) { //if nothing is selected
							Renderer tmp = gHit.GetComponent<Renderer>();
							prevMaterial = tmp.material;
							tmp.material = selectedMaterial;
							selected = gHit;
							selectCooldown = selectDelay;
						}
						else if (!gHit.Equals(selected)) { //if something else is selected
							selected.GetComponent<Renderer>().material = prevMaterial;
							Renderer tmp = gHit.GetComponent<Renderer>();
							prevMaterial = tmp.material;
							tmp.material = selectedMaterial;
							selected = gHit;
							selectCooldown = selectDelay;
						}
						else if (!gHit.Equals(selected)) { //if the selected piece is selected
							selected.GetComponent<Renderer>().material = prevMaterial;
							selected = null;
							selectCooldown = selectDelay;
						}
					}
					else if (gHit.tag.Equals("Tile") && selected != null) { //after it has selected a piece, if it is selecting a tile
						PieceMover tmp;
						if (gHit.tag.Equals("Moving")) { //if it is already moving
							tmp = selected.GetComponent<PieceMover>();
						}
						else {
							tmp = selected.AddComponent<PieceMover>();
						}
						tmp.MoveTo(gHit.transform.position + (Vector3.up * scale / 2));

						selected.GetComponent<Renderer>().material = prevMaterial;
						selected = null;
						selectCooldown = selectDelay;
					}
				}
				else if (selectCooldown <= 0) { //if nothing was hit
					selected.GetComponent<Renderer>().material = prevMaterial;
					selected = null;
				}
			} //touchphase began
			break;
		}
	}
}
