using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

/// <summary>
/// [ 드래그 기구 참조용 ]
/// </summary>
[Serializable]
public class CookwareDrag
{
    public GameObject item;
    public RectTransform itemRectTransform;
    public Image itemImage;
    public CanvasGroup itemCanvasGroup;
    public CookwareData itemData;
}

/// <summary>
/// [ 조리기구 타입 ]
/// </summary>
public enum CookwareType
{
    Cuttingboard,
    FryingPan,
    SaucePan,
}

/// <summary>
/// [ 스프라이트 변경되는 조리기구 타입 ]
/// </summary>
public enum TransformingCookwareType
{
    Cuttingboard,
}

public class CuttingboardAllow
{
    static public List<IngredientType> cookwareTypes = new List<IngredientType>()
    {
        IngredientType.Tomato,
    };
}

public class FryingPanAllow
{
    static public List<IngredientType> cookwareTypes = new List<IngredientType>()
    {
        IngredientType.Tomato,
    };
}

public class SaucePanAllow
{
    static public List<IngredientType> cookwareTypes = new List<IngredientType>()
    {
        IngredientType.Tomato,
    };
}


public class CookwareData : ItemData
{
    public CookwareType cookwareType;
    [ReadOnly] public List<IngredientType> allowcookwareTypes;

    private void Awake()
    {
        allowcookwareTypes = new List<IngredientType>();

        switch(cookwareType) {
            case CookwareType.Cuttingboard :
                allowcookwareTypes = CuttingboardAllow.cookwareTypes;
                break;
            case CookwareType.FryingPan :
                allowcookwareTypes = FryingPanAllow.cookwareTypes;
                break;
            case CookwareType.SaucePan :
                allowcookwareTypes = SaucePanAllow.cookwareTypes;
                break;
        }
    }
}