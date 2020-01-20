using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowControl : MonoBehaviour
{
    private Canvas canvas;
    [SerializeField]
    private Canvas[] otherCanvases;
    [SerializeField]
    private int sortingOrder = 1;
    public void SeletWindowBar()
    {
        canvas = this.gameObject.GetComponent<Canvas>();
        for (int i = 0; i < otherCanvases.Length; i++)
        {
            if (otherCanvases[i] != canvas)
                otherCanvases[i].sortingOrder = 1;
        }

        
        canvas.sortingOrder = sortingOrder;
    }
    public void CloseWindow()
    {
        this.gameObject.SetActive(false);
    }

}
