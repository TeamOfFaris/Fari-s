using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class CookwareBox : MonoBehaviour, IDropHandler
{
    public Cookware[] cookwares;

    public void Awake()
    {
        cookwares = this.GetComponentsInChildren<Cookware>();
    }
    
    public void OnDrop(PointerEventData eventData) => DropItemHandler(eventData);

    private void DropItemHandler(PointerEventData eventData)
    {
        if (eventData.pointerDrag.TryGetComponent<CookwareData>(out CookwareData cookwareData)) {
            eventData.pointerDrag.TryGetComponent<DragItem>(out DragItem dragItem);
            GetCookware(cookwareData.cookwareType).Count++;
            dragItem.isReturn = true;
        }
    }

    public Cookware GetCookware(CookwareType cookwareType)
    {
        for (int i = 0; i < cookwares.Count(); ++i) {
            if (cookwares[i].cookwareType == cookwareType) {
                return cookwares[i];
            }
        }
        return null;
    }
}