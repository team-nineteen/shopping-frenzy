using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroceryListMenu : MonoBehaviour
{

    public bool visible {get; set;}
    public int speed = 600;
    private float invisibleX;
    private float visibleX;
    void Start()
    {
        visible = !SettingsData.Instance.hintsEnabled;
        visibleX = transform.localPosition.x;
        invisibleX = visibleX + 600;
    }

    void Update()
    {
        float lx = transform.localPosition.x;
        float amount = speed * Time.deltaTime;
        if (!visible && lx < invisibleX)
            transform.localPosition = new Vector3(lx + amount, transform.localPosition.y, transform.localPosition.z);
        else if (visible && lx > visibleX)
            transform.localPosition = new Vector3(lx - amount, transform.localPosition.y, transform.localPosition.z);
    }
}
