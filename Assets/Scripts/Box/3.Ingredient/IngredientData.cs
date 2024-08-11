using UnityEngine;
using UnityEngine.UI;
using System;

/// <summary>
/// [ 드래그 재료 참조용 ]
/// </summary>
[Serializable]
public class IngredientDrag
{
    public GameObject item;
    public RectTransform itemRectTransform;
    public Image itemImage;
    public CanvasGroup itemCanvasGroup;
    public IngredientData itemData;
}

/// <summary>
/// [ 기구별 재료 이미지 ]
/// </summary>
[Serializable]
public struct IngredientSprite
{
    public Sprite defulat;
    public Sprite cuttingBoard;
}

/// <summary>
/// [ 재료 타입 ]
/// </summary>
public enum IngredientType
{
    Tomato
}

public class IngredientData : ItemData
{
    public IngredientType ingredientType;
    public IngredientSprite ingredientSprite;

    public static Sprite GetIngredientSprite(CookwareType cookwareType, IngredientSprite ingredientSprite)
    {
        switch (cookwareType) {
            case CookwareType.Cuttingboard :
                return ingredientSprite.cuttingBoard;
            default :
                return ingredientSprite.defulat;
        }
    }
}