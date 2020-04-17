using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelfManager : MonoBehaviour {


	bool upState = false;

	private float upPos = 436f;

	RectTransform rectTransform;

	public float openSpeed = 10,
		scrollSpeed = 10;

	public GameObject displayPrefab;

	public ShelfDisplayRack displayRack;
	private int size;
	private DisplayWorker[] displayWorkers;

	public RectTransform shelfBody, displayGroup;

	float topLimit, botLimit, totalLength;

	public int displaysFit = 4; //how many displays can fit onto the shelf at one time

	void Start() {
		rectTransform = GetComponent<RectTransform>();
	}

	void Awake() {
		size = displayRack.displays.Length;
		GameManager.shelfManager = this;
#if UNITY_EDITOR
		Invoke("InitDisplay", 0.8f);
#else
		InitDisplay();
#endif
	}

	void Update() {
		Move();
		Scrolling();
	}

	bool openOrClose;
	void Move() {
		openOrClose = true;
		float x = rectTransform.position.x;
		if(upState && x > -upPos) {
			float diff = upPos + x + (openSpeed * 4);
			rectTransform.position -= new Vector3((((diff * (diff / 2)) / (upPos / 2))) * Time.deltaTime * openSpeed, 0, 0);
		}
		else if(upState && x < -upPos) {
			rectTransform.position -= new Vector3(x + upPos, 0, 0);
		}
		else if(!upState && x < 0) {
			float diff = -x + (openSpeed * 4);
			rectTransform.position += new Vector3((((diff * (diff / 2)) / (upPos / 2))) * Time.deltaTime * openSpeed, 0, 0);
		}
		else if(!upState && x > 0) {
			rectTransform.position -= new Vector3(x, 0, 0);
		} else {
			openOrClose = false;
		}
	}

	public float drag = 1;

	bool isScrolling = false;
	float velocity = 0;
	Vector2 accTouchDiff; //accumulated touch difference
	public float shelfLeadingEdge = 0;

	[HideInInspector]
	public bool canHover = true;

	void Scrolling() {
		Vector3 changedPos = displayGroup.position;

		shelfLeadingEdge = shelfBody.position.x - shelfBody.sizeDelta.y;

		if(!isScrolling && velocity != 0) {
			changedPos += Vector3.up * (velocity * Time.deltaTime);
			velocity -= (velocity * Time.deltaTime) * 20;
			if(velocity < 10 && velocity > -10)
				velocity = 0;
			//print(velocity);
		}
		if(Input.touchCount != 0 && !openOrClose) {
			Touch touch = Input.touches[0];
			//print(touch.position.x + "|" + shelfBody.position + "|" + shelfBody.localPosition);
			//print(accTouchDiff.y);
			if((touch.position.x > shelfLeadingEdge && upState) || isScrolling) {
				if(touch.phase == TouchPhase.Began) {
					isScrolling = true;
					accTouchDiff = Vector2.zero;
				}
				else if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) {
					if(accTouchDiff.y > 40 && canHover) {
						canHover = false;
						foreach(DisplayWorker d in displayWorkers) {
							d.StopHover();
						}
					}
					else if(canHover)
						accTouchDiff.y += Mathf.Abs(touch.deltaPosition.y);
					changedPos += Vector3.up * touch.deltaPosition.y;
				}
				else {
					if(accTouchDiff.y > 40) {
						velocity = touch.deltaPosition.y * 10;
					}
					else {

					}
					isScrolling = false;
					canHover = true;
				}
			}
		}
		if(changedPos.y > botLimit) {
			velocity = 0;
			changedPos.y = botLimit;
		}
		else if(changedPos.y < topLimit) {
			velocity = 0;
			changedPos.y = topLimit;
		}

		displayGroup.position = changedPos;
	}
	public void Selected(int num) {
		if(num == -1) {
			GameManager.boardManager.Deselect();
			return;
		}
		GameManager.boardManager.ShelfSelect(displayRack.displays[num].prefab);

		for(int i = 0; i < size; i++) {
			if(i == num)
				continue;
			displayWorkers[i].Selected(false);
		}
	}
	public void Deselect() {
		for(int i = 0; i < size; i++)
			displayWorkers[i].Selected(false);
	}


	void InitDisplay() {
		displayWorkers = new DisplayWorker[size];
		float padding = (shelfBody.sizeDelta.x - (displayPrefab.GetComponent<RectTransform>().sizeDelta.y * displaysFit)) / (displaysFit);
		//print(padding);
		float placePos = 0;

		for(int i = 0; i < size; i++) {
			GameObject tmp = Instantiate(displayPrefab, displayGroup, false);

			displayWorkers[i] = tmp.GetComponent<DisplayWorker>();
			displayWorkers[i].sprite.texture = displayRack.displays[i].sprite;
			displayWorkers[i].text.text = displayRack.displays[i].name;
			displayWorkers[i].num = i;
			displayWorkers[i].boss = this;

			RectTransform rect = displayWorkers[i].rectTransform;
			placePos += (padding / 2) + (rect.sizeDelta.y / 2);
			rect.localPosition += Vector3.right * placePos;
			placePos += (padding / 2) + (rect.sizeDelta.y / 2);
		}
		totalLength = placePos;

		topLimit = -totalLength + shelfBody.sizeDelta.x;
		botLimit = 0;
	}

	public void Pressed() {
		upState = !upState;
	}
}
