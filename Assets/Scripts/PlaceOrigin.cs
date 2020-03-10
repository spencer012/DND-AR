using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARReferencePointManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class PlaceOrigin : MonoBehaviour {

	public static GameObject anchor;
	[HideInInspector]
	public Pose startOriginPose;

	public GameObject confirmUI, rotateUI,
		markerPrefab, marker;

	private ARPlane plane = null;

	private bool start, placed, adjust, done;
	public float height, rotation;

	ARRaycastManager raycastManager;
	ARReferencePointManager refManager;
	ARPlaneManager planeManager;
	void Start() {
		raycastManager = GetComponent<ARRaycastManager>();
		refManager = GetComponent<ARReferencePointManager>();
		planeManager = GetComponent<ARPlaneManager>();

	}

	private List<ARRaycastHit> hits = new List<ARRaycastHit>();
	void Update() {
		if(start && !placed) { //if it is the start and the first phase
			foreach(Touch touch in Input.touches) {
				if(touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended) {
					if(raycastManager.Raycast(touch.position, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinBounds)) {
						if (marker.activeSelf == false) {
							marker.SetActive(true);
						}
						Pose p = new Pose(hits[0].pose.position + (Vector3.up * height / 100), hits[0].pose.rotation); //move marker to new hit
						marker.transform.position = p.position;
						marker.transform.rotation = p.rotation;
						if (touch.phase == TouchPhase.Ended) { //if the lifting of the touch is on a plane go to the confirm screen
							plane = planeManager.GetPlane(hits[0].trackableId);
							startOriginPose = new Pose(hits[0].pose.position + (Vector3.up * height / 100), hits[0].pose.rotation);
							confirmUI.SetActive(true);
							placed = true;

							foreach (ARPlane pl in planeManager.trackables) {
								if (pl != plane) {
									pl.gameObject.SetActive(false);
								}
							}
							planeManager.detectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.None;
						}
					} else {
						marker.SetActive(false);
					}
				}
			}
		}
		else if(start && adjust) { //if it is the start and the second phase
			marker.transform.position = startOriginPose.position + (Vector3.up * height / 100);
			if (done) {
				startOriginPose.position += (Vector3.up * height / 100);
				anchor = refManager.AttachReferencePoint(plane, startOriginPose).gameObject;
				print("Anchor placed");
				MySceneManager.sceneManager.ChangeScene(MySceneManager.MAIN);
				done = adjust = false;
			}
		}
		else if(!start && !placed) { //if it is not the start and the first phase
			foreach (Touch touch in Input.touches) {
				if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Ended) {
					if (raycastManager.Raycast(touch.position, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinBounds)) {
						if (marker.activeSelf == false) {
							marker.SetActive(true);
						}
						Pose p = new Pose(hits[0].pose.position + (Vector3.up * height / 100), hits[0].pose.rotation); //move marker to new hit
						marker.transform.position = p.position;
						marker.transform.rotation = p.rotation;
						if (touch.phase == TouchPhase.Ended) { //if the lifting of the touch is on a plane go to the confirm screen
							plane = planeManager.GetPlane(hits[0].trackableId);
							startOriginPose = new Pose(hits[0].pose.position + (Vector3.up * height / 100), hits[0].pose.rotation);
							confirmUI.SetActive(true);
							placed = true;

							foreach (ARPlane pl in planeManager.trackables) {
								if (pl != plane) {
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
		else if(!start && adjust) { //if it is not the start and the second phase
			//TODO
		}
	}

	public void Confirm() {
		done = true;
		Destroy(marker);
	}

	public void Back(bool starting) { //pressed the back button
		if (starting) {

		} else {

		}
		confirmUI.SetActive(false);
		rotateUI.SetActive(false);
		placed = adjust = false;
		foreach (ARPlane pl in planeManager.trackables) {
			if (pl != plane) {
				pl.gameObject.SetActive(true);
			}
		}
		planeManager.detectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.Horizontal;
	}

	public void HeightSlider(float v) {
		height = v;
	}

	public void RotationSlider(float v) {
		rotation = v;
	}

	public void ChangeScene(int scene) {
		if(scene == MySceneManager.CHOOSINGSTART) { //the place you go to after pressing start in the start menu
			start = true;
			placed = adjust = false;
			confirmUI.SetActive(false);
			rotateUI.SetActive(false);

			foreach (ARPlane pl in planeManager.trackables) {
				if (pl != plane) {
					pl.gameObject.SetActive(true);
				}
			}
			planeManager.detectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.Horizontal;


			marker = Instantiate(markerPrefab);
			marker.SetActive(false);
		}
		else if(scene == MySceneManager.CHOOSING) { // after you press change rot/pos in options menu
			start = placed = false;
			adjust = true;
			confirmUI.SetActive(false);
			rotateUI.SetActive(true);

			if (anchor != null) {
				Destroy(anchor);
			}
			
			marker = Instantiate(markerPrefab);
			marker.SetActive(false);
		}
		else {
			throw new System.Exception("Scene " + scene + " does not exist in PlaceOrigin");
		}
	}
}
