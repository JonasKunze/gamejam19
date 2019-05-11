using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public Canvas canvas;
    private static Shop instance;

    public static void CloseShopCanvas()
    {
        Cursor.visible = false;
        Instance().canvas.gameObject.SetActive(false);
    }

    public static void OpenShopCanvas()
    {
        Cursor.visible = true;
        Instance().canvas.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(instance == null);
        instance = this;
    }

    public static Shop Instance()
    {
        return instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
