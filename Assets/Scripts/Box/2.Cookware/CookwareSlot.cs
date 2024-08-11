using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CookwareSlot : MonoBehaviour, IDropHandler
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
        // 1개의 기구
        if (content.transform.childCount > 0) return;

        if (eventData.pointerDrag.TryGetComponent<Cookware>(out Cookware cookware)) {
            if (CheckAllow(cookware.cookwareDrag.itemData.cookwareType)) {
                cookware.cookwareDrag.item.transform.SetParent(content.transform);
                cookware.cookwareDrag.itemRectTransform.anchoredPosition = Vector2.zero;
            }
        }
        else if (eventData.pointerDrag.TryGetComponent<CookwareData>(out CookwareData cookwareData)) {
            if (CheckAllow(cookwareData.cookwareType)) {
                eventData.pointerDrag.TryGetComponent<DragItem>(out DragItem dragItem);
                dragItem.itemDrag.itemRectTransform.transform.SetParent(content.transform);
                dragItem.itemDrag.itemRectTransform.anchoredPosition = Vector2.zero;
                dragItem.isDrop = true;
            }
        }
    }

    private bool CheckAllow(CookwareType cookwareType)
    {
        this.TryGetComponent<InstallationData>(out InstallationData installationData);

        return installationData.allowcookwareTypes.Exists(x => x == cookwareType);
    }
}