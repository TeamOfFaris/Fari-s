using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("=== Reference===")]
    public ItemDrag itemDrag;
    private Canvas canvas;

    public delegate void DragFunction(PointerEventData eventData);
    public DragFunction onBeginDrag;
    public DragFunction onDrag;
    public DragFunction onEndDrag;

    private Transform previousParent;
    public bool isDrop = false;
    public bool isReturn = false;

    private void Awake()
    {
        canvas = this.GetComponentInParent<Canvas>();
    }

    private void Start()
    {
        itemDrag = new ItemDrag();
        itemDrag.item = this.gameObject;
        itemDrag.iamge = this.transform.GetComponent<Image>();
        itemDrag.itemRectTransform = this.transform.GetComponent<RectTransform>();
        itemDrag.itemCanvasGroup = this.transform.GetComponent<CanvasGroup>();

        onBeginDrag = UndoSetting;
        onDrag = MoveDragItem;
        onEndDrag = RedoItem;
        onEndDrag += RemoveItem;
        onEndDrag += DragReset;
    }

    public void OnBeginDrag(PointerEventData eventData) => onBeginDrag(eventData);
    public void OnDrag(PointerEventData eventData) => onDrag(eventData);
    public void OnEndDrag(PointerEventData eventData) => onEndDrag(eventData);

    private void UndoSetting(PointerEventData eventData)
    {
        itemDrag.itemCanvasGroup.blocksRaycasts = false;

        previousParent = transform.transform.parent;

        this.transform.SetParent(canvas.transform);
        this.transform.SetAsLastSibling();
    }

    private void MoveDragItem(PointerEventData eventData)
    {
        itemDrag.itemRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    private void RedoItem(PointerEventData eventData)
    {
        if (isDrop) return;

        transform.SetParent(previousParent);
        itemDrag.itemRectTransform.position = previousParent.GetComponent<RectTransform>().position;
    }

    private void DragReset(PointerEventData eventData)
    {
        itemDrag.itemCanvasGroup.blocksRaycasts = true;
        isDrop = false;
        isReturn = false;
    }

    private void RemoveItem(PointerEventData eventData)
    {
        if (isReturn) { Destroy(this.gameObject); return; }
    }
}