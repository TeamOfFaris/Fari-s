using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

/// <summary>
/// [ 설치형 타입 ]
/// </summary>
public enum InstallationType
{
    Floor,
    GasStove,
    CoffeeMachine,
}

public class FloorAllow
{
    static public List<CookwareType> cookwareTypes = new List<CookwareType>()
    {
        CookwareType.Cuttingboard,
        CookwareType.FryingPan,
        CookwareType.SaucePan,
    };
}

public class GasStoveAllow
{
    static public List<CookwareType> cookwareTypes = new List<CookwareType>()
    {
        CookwareType.FryingPan,
        CookwareType.SaucePan,
    };
}

public class CoffeeMachineAllow
{
    static public List<CookwareType> cookwareTypes = new List<CookwareType>() { };
}


public class InstallationData : ItemData
{
    public InstallationType installationType;
    [ReadOnly] public List<CookwareType> allowcookwareTypes;

    private void Awake()
    {
        allowcookwareTypes = new List<CookwareType>();

        switch(installationType) {
            case InstallationType.Floor :
                allowcookwareTypes = FloorAllow.cookwareTypes;
                break;
            case InstallationType.GasStove :
                allowcookwareTypes = GasStoveAllow.cookwareTypes;
                break;
            case InstallationType.CoffeeMachine :
                allowcookwareTypes = CoffeeMachineAllow.cookwareTypes;
                break;
        }
    }
}