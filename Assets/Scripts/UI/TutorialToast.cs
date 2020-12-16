using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialToast : MonoBehaviour
{


    public GameObject root;
    public TextMeshProUGUI text;
    public List<string> hints;

    private float displayTime = 0f;
    private int index;
    private bool visible;
    private GroceryListMenu m_GroceryList;

    void Start()
    {
        visible = SettingsData.Instance.hintsEnabled;
        root.SetActive(visible);
        if (visible)
        {
            m_GroceryList = FindObjectOfType<GroceryListMenu>();
            for (int i = 0; i < hints.Count; i++)
            {
                hints[i] = hints[i].Replace("<br>", "\n"); // Stupid newlines fix
            }
            index = 0;
            text.text = hints[index++];
        }
    }


    void Update()
    {
        if (visible)
        {
            displayTime += Time.deltaTime;
            if (displayTime >= 10f)
            {
                if (index >= hints.Count)
                {
                    visible = false;
                    root.SetActive(false);
                    m_GroceryList.visible = true;
                }
                else
                {
                    displayTime = 0f;
                    text.text = hints[index++];
                }
            }
        }
    }
}
