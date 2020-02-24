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

	public GameObject confirmUI;

	private bool start, placed, adjust;
	public float height = 1f;

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
		if(start && !placed) {
			foreach(Touch touch in Input.touches) {
				if(touch.phase == TouchPhase.Began) {
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
						print("Origin placed");
						confirmUI.SetActive(true);
					}
				}
			}
		}
		else if(start && adjust) {

		}
		else if(!start && !placed) {

		}
		else if(!start && adjust) {

		}
	}

	public void BackB() {

	}

	public void HeightSlider(float v) {

	}

	public void ChangeScene(int scene) {
		if(scene == MySceneManager.CHOOSINGSTART) {
			start = true;
		}
		else if(scene == MySceneManager.CHOOSING) {

		}
		else {
			throw new System.Exception("Scene " + scene + " does not exist in PlaceOrigin");
		}
	}
}
