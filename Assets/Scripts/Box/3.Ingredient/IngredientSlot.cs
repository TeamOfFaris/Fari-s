using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IngredientSlot : MonoBehaviour, IDropHandler
{
    private Transform content;

    private void Awake()
    {
        Transform[] childComponents = GetComponentsInChildren<Transform>();
        foreach (Transform transform in childComponents)
        {
            switch(transform.name) {
                case "Content":
                    content = transform;
                    break;
            }
        }
    }
    
    public void OnDrop(PointerEventData eventData) => DropItemHandler(eventData);

    private void DropItemHandler(PointerEventData eventData)
    {
        // 1개의 재료
        if (this.transform.childCount > 0) return;

        if (eventData.pointerDrag.TryGetComponent<Ingredient>(out Ingredient ingredient)) {
            if (CheckAllow(ingredient.ingredientDrag.itemData.ingredientType)) {
                ingredient.ingredientDrag.item.transform.SetParent(this.transform);
                ingredient.ingredientDrag.itemRectTransform.anchoredPosition = Vector2.zero;
            }
        }
        else if (eventData.pointerDrag.TryGetComponent<IngredientData>(out IngredientData ingredientData)) {
            if (CheckAllow(ingredientData.ingredientType)) {
                eventData.pointerDrag.TryGetComponent<DragItem>(out DragItem dragItem);
                this.TryGetComponent<CookwareData>(out CookwareData cookwareData);
                if (CheckTransforming(cookwareData.cookwareType)) {
                    dragItem.itemDrag.iamge.sprite = IngredientData.GetIngredientSprite(cookwareData.cookwareType, ingredientData.ingredientSprite);
                }
                dragItem.itemDrag.itemRectTransform.transform.SetParent(this.transform);
                dragItem.itemDrag.itemRectTransform.anchoredPosition = Vector2.zero;
                dragItem.isDrop = true;
            }
        }
    }

    // 이동 가능 확인
    private bool CheckAllow(IngredientType ingredientType)
    {
        this.TryGetComponent<CookwareData>(out CookwareData cookwareData);

        return cookwareData.allowcookwareTypes.Exists(x => x == ingredientType);
    }

    // 스프라이트가 바뀌는 기구 확인
    private bool CheckTransforming(CookwareType cookwareType)
    {
        if (Enum.IsDefined(typeof(TransformingCookwareType), Enum.GetName(typeof(CookwareType), cookwareType))) return true;
        else return false;
    }
}
