using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ItemDrag
{
    public GameObject item;
    public Image iamge;
    public RectTransform itemRectTransform;
    public CanvasGroup itemCanvasGroup;
}

public enum ItemType
{
    Installation,
    Cookware,
    Ingredient
}

public class ItemData : MonoBehaviour
{
    public ItemType itemType;
}