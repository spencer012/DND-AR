using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapNode : MonoBehaviour {
	public static MapSelector parent;

	public Toggle toggle;
	public RawImage image;
	public int index;

	public void Alert() {
		parent.Selected(index);
	}
}
