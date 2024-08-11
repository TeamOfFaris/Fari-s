using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Cookware : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Header("=== Reference===")]
    private Image image;
    private Canvas canvas;
    private RectTransform rectTransform;
    private TextMeshProUGUI countText;

    [Header("=== Item Info ===")]
    public CookwareType cookwareType;
    public Sprite cookwareSprite;
    [SerializeField, ReadOnly(true), Range(0, 99)] private int count;
    public int Count
    {
        get { return count; }
        set { 
            count = value; 
            countText.text = $"x{value}";

            if (value == 0) { image.color = new Color(image.color.r, image.color.g, image.color.b, .6f); } 
            else { image.color = new Color(image.color.r, image.color.g, image.color.b, 1f); }
        }
    }

    [Header("=== Drag Info===")]
    public CookwareDrag cookwareDrag;
    public delegate void DragFunction(PointerEventData eventData);
    public DragFunction onBeginDrag;
    public DragFunction onDrag;
    public DragFunction onEndDrag;
    public DragFunction onDrop;

    private void Awake()
    {
        image = this.GetComponent<Image>();
        canvas = this.GetComponentInParent<Canvas>();
        rectTransform = this.GetComponent<RectTransform>();
        countText= this.GetComponentInChildren<TextMeshProUGUI>();

        InvokeProperty();
    }

    private void InvokeProperty()
    {
        Count = Count;
    }

    private void Start()
    {
        onBeginDrag = InstantiateDragItem;
        onDrag = MoveDragItem;
        onEndDrag = DragEndItemHandler;
        onDrop = ReturnItem;
    }

    public void OnBeginDrag(PointerEventData eventData) => onBeginDrag(eventData);
    public void OnDrag(PointerEventData eventData) => onDrag(eventData);
    public void OnEndDrag(PointerEventData eventData) => onEndDrag(eventData);
    public void OnDrop(PointerEventData eventData) => onDrop(eventData);

    private void InstantiateDragItem(PointerEventData eventData)
    {
        if (this.Count <= 0) { eventData.pointerDrag = null; return; }
        
        cookwareDrag.item = new GameObject(this.name + "_Cookware");
        cookwareDrag.item.transform.SetParent(canvas.transform);
        cookwareDrag.itemData = cookwareDrag.item.AddComponent<CookwareData>();
        cookwareDrag.item.AddComponent<IngredientSlot>();
        cookwareDrag.item.AddComponent<DragItem>();
        cookwareDrag.itemImage = cookwareDrag.item.AddComponent<Image>();
        cookwareDrag.itemCanvasGroup = cookwareDrag.item.AddComponent<CanvasGroup>();
        cookwareDrag.itemRectTransform = cookwareDrag.item.GetComponent<RectTransform>();

        cookwareDrag.itemImage.sprite = this.image.sprite;
        cookwareDrag.itemCanvasGroup.alpha = .6f;
        cookwareDrag.itemCanvasGroup.blocksRaycasts = false;
        cookwareDrag.itemRectTransform.position = this.rectTransform.position;
        cookwareDrag.itemRectTransform.rotation = this.rectTransform.rotation;
        cookwareDrag.itemData.cookwareType = cookwareType;
    }

    private void MoveDragItem(PointerEventData eventData)
    {
        cookwareDrag.itemRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    private void DragEndItemHandler(PointerEventData eventData)
    {
        InstallationData installationdata = cookwareDrag.item.GetComponentInParent<InstallationData>();
        if (installationdata == null) {
            Destroy(cookwareDrag.item);
            cookwareDrag = new CookwareDrag();
            return;
        }
        
        cookwareDrag.itemImage.sprite = cookwareSprite;
        cookwareDrag.itemRectTransform.sizeDelta = new Vector2(200, 200);
        cookwareDrag.itemCanvasGroup.alpha = 1f;
        cookwareDrag.itemCanvasGroup.blocksRaycasts = true;
        cookwareDrag = new CookwareDrag();
        --Count;
    }

    private void ReturnItem(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent<CookwareData>(out CookwareData cookwareData)) {
            eventData.pointerDrag.TryGetComponent<DragItem>(out DragItem dragItem);
            this.GetComponentInParent<CookwareBox>().GetCookware(cookwareData.cookwareType).Count++;
            dragItem.isReturn = true;
        }
    }
}