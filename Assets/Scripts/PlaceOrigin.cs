using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARReferencePointManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class PlaceOrigin : MonoBehaviour {

	public static GameObject anchor;
	[HideInInspector]
	public Pose startOriginPose;

	public GameObject confirmUI, markerPrefab, marker;

	private ARPlane plane = null;

	private bool placed, adjust, done;
	public float height, rotation, scale;
	private float defaultScale;

	public static ARRaycastManager raycastManager;
	ARReferencePointManager refManager;
	ARPlaneManager planeManager;

	private float cooldown = 0;
	public float cooldownDelay;

	void Start() {
		raycastManager = GetComponent<ARRaycastManager>();
		refManager = GetComponent<ARReferencePointManager>();
		planeManager = GetComponent<ARPlaneManager>();

		confirmUI.SetActive(false);
	}

	private void Awake() {
		scale = 1;
		CheckMarker();
	}

	private List<ARRaycastHit> hits = new List<ARRaycastHit>();
	void Update() { 
		if(!placed) { //if it is the first phase
			if (cooldown <= 0) {
				foreach(Touch touch in Input.touches) {
					if(touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended) {
						if(raycastManager.Raycast(touch.position, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinBounds)) {
							if(marker.activeSelf == false) {
								marker.SetActive(true);
							}
							//Pose p = new Pose(hits[0].pose.position + (Vector3.up * height / 100), hits[0].pose.rotation); //move marker to new hit
							marker.transform.position = hits[0].pose.position + (Vector3.up * height / 100);
							marker.transform.rotation = hits[0].pose.rotation;
							marker.transform.localScale = new Vector3(scale, scale, scale) * defaultScale;
							if(touch.phase == TouchPhase.Ended) { //if the lifting of the touch is on a plane go to the rotation screen
								plane = planeManager.GetPlane(hits[0].trackableId);
								startOriginPose = new Pose(hits[0].pose.position + (Vector3.up * height / 100), hits[0].pose.rotation);
								confirmUI.SetActive(true);
								placed = true;
								adjust = true;

								foreach(ARPlane pl in planeManager.trackables) {
									if(pl != plane) {
										pl.gameObject.SetActive(false);
									}
								}
								planeManager.detectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.None;
							}
						}
						else {
							marker.SetActive(false);
						}
					}
				}
			}
			else {
				cooldown -= Time.deltaTime;
			}
		}
		else if(adjust) { //if it is the second phase
			marker.transform.position = startOriginPose.position + (Vector3.up * height / 100);
			marker.transform.localEulerAngles = startOriginPose.rotation.eulerAngles + new Vector3(0, rotation, 0);
			marker.transform.localScale = new Vector3(scale, scale, scale) * defaultScale;
			//Debug.Log((Vector3.up * height / 100));
			if(done) {
				Destroy(marker.gameObject);
				startOriginPose.position += (Vector3.up * height / 100);
				anchor = refManager.AttachReferencePoint(plane, startOriginPose).gameObject;
				print("Anchor placed");
				done = adjust = false;
				confirmUI.SetActive(false);

				MySceneManager.sceneManager.ChangeScene(MySceneManager.MAIN);
			}
		}
	}

	public void Confirm() {
		done = true;
	}

	public void Back() { //pressed the back button
		cooldown = cooldownDelay;
		ChangeScene(MySceneManager.CHOOSINGPOS);
	}

	public void HeightSlider(float v) {
		height = v;
	}

	public void RotationSlider(float v) {
		rotation = v;
	}

	public void ScaleSlider(float v) {
		scale = v;
	}

	public void ChangeScene(int scene) {
		if(scene == MySceneManager.CHOOSINGPOS) { //the place you go to after pressing start in the start menu
			placed = adjust = done = false;
			confirmUI.SetActive(false);

			foreach(ARPlane pl in planeManager.trackables) {
				pl.gameObject.SetActive(true);
			}
			planeManager.detectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.Horizontal;

			if(anchor != null) {
				Destroy(anchor);
			}
			if (marker != null) {
				Destroy(marker);
			}
			CheckMarker();
		}
		else if(scene == MySceneManager.CHOOSINGROT) { // after you press change rot/pos in options menu
			placed = done = false;
			adjust = true;
			confirmUI.SetActive(true);

			if(anchor != null) {
				Destroy(anchor);
			}

			CheckMarker();
		}
		else {
			throw new System.Exception("Scene " + scene + " does not exist in PlaceOrigin");
		}
	}

	private void CheckMarker() {
		if(marker == null) {
			marker = Instantiate(markerPrefab);
			marker.SetActive(false);
			defaultScale = marker.transform.localScale.x;
		}
	}
}
