using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARReferencePointManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class PlaceOrigin : MonoBehaviour {

	public static GameObject origin;
	Pose startOriginPose;

	public GameObject confirmUI,
		markerPrefab, marker;

	private bool start, placed, adjust;
	public float height = 1f;

	ARRaycastManager raycastManager;
	ARReferencePointManager refManager;
	ARPlaneManager planeManager;
	void Start() {
		raycastManager = GetComponent<ARRaycastManager>();
		refManager = GetComponent<ARReferencePointManager>();
		planeManager = GetComponent<ARPlaneManager>();

		marker = Instantiate(markerPrefab);
		marker.SetActive(false);
	}

	private List<ARRaycastHit> hits = new List<ARRaycastHit>();
	void Update() {
		if(start && !placed) {
			foreach(Touch touch in Input.touches) {
				if(touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved) {
					if(raycastManager.Raycast(touch.position, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinBounds)) {
						Pose p = new Pose(hits[0].pose.position + (Vector3.up * height / 100), hits[0].pose.rotation);
						marker.transform.position = p.position;
						marker.transform.rotation = p.rotation;
					}
				} else if(touch.phase == TouchPhase.Ended) {
					if(raycastManager.Raycast(touch.position, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinBounds)) {
						ARPlane plane = planeManager.GetPlane(hits[0].trackableId);
						Pose p = new Pose(hits[0].pose.position + (Vector3.up * height / 100), hits[0].pose.rotation);
						origin = refManager.AttachReferencePoint(plane, p).gameObject;
						placed = true;
						planeManager.detectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.None;
						foreach(ARPlane pl in planeManager.trackables) {
							if(pl != plane) {
								pl.gameObject.SetActive(false);
							}
						}
						startOriginPose = new Pose(origin.transform.position, origin.transform.rotation);
						print("Origin placed");
						confirmUI.SetActive(true);
					}
				}
			}
		}
		else if(start && adjust) {
			origin.transform.position = startOriginPose.position + (Vector3.up * height / 100);
		}
		else if(!start && !placed) {

		}
		else if(!start && adjust) {

		}
	}

	public void HeightSlider(float v) {
		print(v);
	}

	public void ChangeScene(int scene) {
		if(scene == MySceneManager.CHOOSINGSTART) {
			start = true;
			placed = false;
			adjust = false;
			confirmUI.SetActive(false);
		}
		else if(scene == MySceneManager.CHOOSING) {
			start = placed = adjust = false;
			confirmUI.SetActive(false);
		}
		else {
			throw new System.Exception("Scene " + scene + " does not exist in PlaceOrigin");
		}
	}
}
