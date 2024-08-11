using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Trash : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData) => DropItemHandler(eventData);

    private void DropItemHandler(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent<CookwareData>(out CookwareData cookwareData)) {
            for (int i = 0; i < cookwareData.transform.childCount; ++i) {
                Destroy(cookwareData.transform.GetChild(i).gameObject);
            }
        }
        if (eventData.pointerDrag.TryGetComponent<IngredientData>(out IngredientData ingredientData)) {
            Destroy(ingredientData.gameObject);
        }
    }
}