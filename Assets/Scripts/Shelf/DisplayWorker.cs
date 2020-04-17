using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DisplayWorker : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler {
	public RawImage sprite;
	public Text text;
	public RectTransform rectTransform;
	public Image box;
	public Outline outline;
	[HideInInspector]
	public int num;

	[HideInInspector]
	public ShelfManager boss;

	public bool hover = false, selected = false;

	public void OnPointerDown(PointerEventData eventData) {
		if(!hover && boss.canHover) {
			box.color = Color.gray;
			hover = true;
		}
	}

	public void OnPointerUp(PointerEventData eventData) {
		if(hover && boss.canHover) {
			box.color = Color.white;
			hover = false;
			if(boss.canHover) {
				boss.Selected(selected ? -1 : num);
				Selected(!selected);
			}
		}
	}
	
	public void OnPointerEnter(PointerEventData eventData) {
		if(!hover && boss.canHover) {
			box.color = Color.gray;
			hover = true;
		}
	}

	public void OnPointerExit(PointerEventData eventData) {
		if(hover && boss.canHover) {
			box.color = Color.white;
			hover = false;
		}
	}
	public void StopHover() {
		if(hover) {
			box.color = Color.white;
			hover = false;
		}
	}
	public void Selected(bool active) {
		outline.enabled = active;
		selected = active;
		//temp
		Invoke("Selected", 1);
	}
}
