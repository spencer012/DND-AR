using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour {

	public List<GameObject> pieces;

	private float selectCooldown = 0;

	public LayerMask raycastHit;

	private GameObject selected;
	private Renderer selectedRenderer;
	public Material selectedMaterial;
	private Material prevMaterial;

	private bool shelfPlace = false;

	private static float selectDelay = 0.2f;

	public const float scale = 0.05f;

	public float moveHeight = 1;

	[HideInInspector]
	public bool uiAction = false;

	public BoardManager boardManager;

	void Awake() {
		GameManager.selectionManager = this;
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
				if(selectCooldown > 0) { //if it was touched before the cooldown was ended
					selectCooldown = Mathf.Min(selectCooldown * 1.5f, selectDelay);
				}
				if(selectCooldown <= 0 && Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hit, raycastHit)) {
					GameObject gHit = hit.collider.gameObject;
					if(gHit.tag.Equals("Piece") || gHit.tag.Equals("Moving")) { //if it is a piece on the board
						if(selected == null) { //if nothing is selected
							selectedRenderer = gHit.GetComponentInChildren<Renderer>();
							prevMaterial = selectedRenderer.material;
							selectedRenderer.material = selectedMaterial;
							selected = gHit;
							selectCooldown = selectDelay;
						}
						else if(!gHit.Equals(selected)) { //if something else is selected
							if(Deselect()) {
								selectedRenderer = gHit.GetComponentInChildren<Renderer>();
								prevMaterial = selectedRenderer.material;
								selectedRenderer.material = selectedMaterial;
								selected = gHit;
								selectCooldown = selectDelay;
							}
						}
						else if(gHit.Equals(selected)) { //if the selected piece is selected
							Deselect();
						}
					}
					else if(gHit.tag.Equals("Tile") && selected != null) { //after it has selected a piece, if it is selecting a tile
						if(shelfPlace) {
							GameObject tmp = Instantiate(selected, transform);
							tmp.transform.localPosition = gHit.transform.parent.localPosition;
							boardManager.AddPiece(tmp);
							Deselect();
						}
						else {
							PieceMover tmp;
							if(selected.tag.Equals("Moving")) { //if it is already moving
								tmp = selected.GetComponent<PieceMover>();
							}
							else {
								tmp = selected.AddComponent<PieceMover>();
							}
							tmp.MoveTo(gHit.transform.parent.localPosition);

							Deselect();
						}
					}
					else if(selectCooldown <= 0) { //if something not part of the board was hit
						Deselect();
					}
				}
				else if(selectCooldown <= 0) { //if nothing was hit
					Deselect();
				}
			} //touchphase began
			break;
		}
	}

	public void ShelfSelect(GameObject g) {
		if(selected != null && !shelfPlace) {
			selectedRenderer.material = prevMaterial;
			selected = null;
			selectCooldown = selectDelay;
		}
		selectCooldown = selectDelay;
		selected = g;
		shelfPlace = true;
	}

	public bool Deselect() {
		if(uiAction) {
			uiAction = false;
			return false;
		}
		else {
			if(shelfPlace) {
				GameManager.shelfManager.Deselect();
				selected = null;
				selectCooldown = selectDelay;
				shelfPlace = false;
			}
			else if(selected != null) {
				selectedRenderer.material = prevMaterial;
				selected = null;
				selectCooldown = selectDelay;
			}
		}
		return true;
	}

	public void DeleteSelected() {
		//print("Got here");
		if(selected != null && !shelfPlace) {
			//print("Deleted");
			Destroy(selected);
			selectedRenderer = null;
			selected = null;
		}
	}

	public void UIAction() {
		uiAction = true;
	}
	public void ShelfUIAction() {
		if(shelfPlace) {
			uiAction = true;
			Deselect();
		}
	}
}
