using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Card2 : MonoBehaviour
{
    [Header("Set Dynamically")]
    public string suit; 
    public int rank; 
    public Color color = Color.black; 
    public string colS = "Black";
    
    public List<GameObject> deco2GOs = new List<GameObject>();
    public List<GameObject> pip2GOs = new List<GameObject>();
    public GameObject back2; 
    public Card2Definition def2;
    public SpriteRenderer[] spriteRenderers;
    void Start()
    {
        SetSortOrder(0); 
    }
    public void PopulateSpriteRenderers()
    {
        if (spriteRenderers == null || spriteRenderers.Length == 0)
        {
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        }
    }
    public void SetSortingLayerName(string tSLN)
    {
        PopulateSpriteRenderers();
        foreach (SpriteRenderer tSR in spriteRenderers)
        {
            tSR.sortingLayerName = tSLN;
        }
    }
    public void SetSortOrder(int sOrd)
    { 
        PopulateSpriteRenderers();

        foreach (SpriteRenderer tSR in spriteRenderers)
        {
            if (tSR.gameObject == this.gameObject)
            {
                tSR.sortingOrder = sOrd; 
                continue; 
            }
            switch (tSR.gameObject.name)
            {
                case "back": // if the name is "back"
                             // Set it to the highest layer to cover the other sprites
                    tSR.sortingOrder = sOrd + 2;
                    break;
                case "face": // if the name is "face"
                default: // or if it's anything else
                         // Set it to the middle layer to be above the background
                    tSR.sortingOrder = sOrd + 1;
                    break;
            }
        }
    }
    public bool faceUp2
    {
        get
        {
            return (!back2.activeSelf);
        }
        set
        {
            back2.SetActive(!value);
        }
    }
    virtual public void OnMouseUpAsButton1()
    {
        print(name); 
    }

}
[System.Serializable]
public class Decorator2
{
    public string type; 
    public Vector3 loc; 
    public bool flip = false; 
    public float scale = 1f; 
}
[System.Serializable]
public class Card2Definition
{
    public string face; 
    public int rank; 
    public List<Decorator2> pip2s = new List<Decorator2>(); 
}


