using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Installation : MonoBehaviour
{
    [Header("=== Reference===")]
    private Canvas canvas;
    private RectTransform rectTransform;

    [Header("=== Item Info ===")]
    public InstallationType installationType;

    private void Awake()
    {
        canvas = this.GetComponentInParent<Canvas>();
        this.TryGetComponent<RectTransform>(out rectTransform);
    }
}
