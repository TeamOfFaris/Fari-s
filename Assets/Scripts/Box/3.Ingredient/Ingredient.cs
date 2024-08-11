using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class Ingredient : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("=== Reference===")]
    private Image image;
    private Canvas canvas;
    private RectTransform rectTransform;
    private TextMeshProUGUI countText;
    
    [Header("=== Item Info ===")]
    public IngredientType ingredientType;
    public IngredientSprite ingredientSprite;
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
    public IngredientDrag ingredientDrag;
    public delegate void DragFunction(PointerEventData eventData);
    public DragFunction onBeginDrag;
    public DragFunction onDrag;
    public DragFunction onEndDrag;
    
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
    }
    
    public void OnBeginDrag(PointerEventData eventData) => onBeginDrag(eventData);
    public void OnDrag(PointerEventData eventData) => onDrag(eventData);
    public void OnEndDrag(PointerEventData eventData) => onEndDrag(eventData);

    private void InstantiateDragItem(PointerEventData eventData)
    {
        if (this.Count <= 0) { eventData.pointerDrag = null; return; }
        
        ingredientDrag.item = new GameObject(this.name + "_Ingredient");
        ingredientDrag.item.transform.SetParent(canvas.transform);
        ingredientDrag.itemData = ingredientDrag.item.AddComponent<IngredientData>();
        ingredientDrag.item.AddComponent<DragItem>();
        ingredientDrag.itemImage = ingredientDrag.item.AddComponent<Image>();
        ingredientDrag.itemCanvasGroup = ingredientDrag.item.AddComponent<CanvasGroup>();
        ingredientDrag.itemRectTransform = ingredientDrag.item.GetComponent<RectTransform>();

        ingredientDrag.itemImage.sprite = this.image.sprite;
        ingredientDrag.itemCanvasGroup.alpha = .6f;
        ingredientDrag.itemCanvasGroup.blocksRaycasts = false;
        ingredientDrag.itemRectTransform.position = this.rectTransform.position;
        ingredientDrag.itemRectTransform.rotation = this.rectTransform.rotation;
        ingredientDrag.itemData.ingredientType = this.ingredientType;
        ingredientDrag.itemData.ingredientSprite = this.ingredientSprite;
    }

    private void MoveDragItem(PointerEventData eventData)
    {
        ingredientDrag.itemRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    private void DragEndItemHandler(PointerEventData eventData)
    {
        CookwareData cookwareData = ingredientDrag.item.GetComponentInParent<CookwareData>();
        if (cookwareData == null) {
            Destroy(ingredientDrag.item);
            ingredientDrag = new IngredientDrag();
            return;
        }
        
        ingredientDrag.itemImage.sprite = IngredientData.GetIngredientSprite(cookwareData.cookwareType, ingredientSprite);
        ingredientDrag.itemCanvasGroup.alpha = 1f;
        ingredientDrag.itemCanvasGroup.blocksRaycasts = true;
        ingredientDrag = new IngredientDrag();
        --Count;
    }
}