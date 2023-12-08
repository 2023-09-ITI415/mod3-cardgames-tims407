using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotDef2
{
    public float x;
    public float y;
    public bool faceUp = false;
    public string layerName = "Default";
    public int layerID = 0;
    public int id;
    public List<int> hiddenBy = new List<int>();
    public string type = "slot";
    public Vector2 stagger;
}
public class Layouts : MonoBehaviour
{
    public PT_XMLReader xmlr2; 
    public PT_XMLHashtable xml2; 
    public Vector2 multiplier;
    public List<SlotDef2> slotDefs2;
    public SlotDef2 drawPile;
    public SlotDef2 discardPile;
    public string[] sortingLayerNames = new string[] { "Discard", "Draw" };
    public void ReadLayout(string xmlText2)
    {
        xmlr2 = new PT_XMLReader();
        xmlr2.Parse(xmlText2);
        xml2 = xmlr2.xml["xml"][0]; 
        multiplier.x = float.Parse(xml2["multiplier"][0].att("x"));
        multiplier.y = float.Parse(xml2["multiplier"][0].att("y"));

        SlotDef2 tSO;

        PT_XMLHashList slotsX = xml2["slot"];
        for (int i = 0; i < slotsX.Count; i++)
        {
            tSO = new SlotDef2();
            if (slotsX[i].HasAtt("type"))
            {
                tSO.type = slotsX[i].att("type");
            }
            else
            {
                tSO.type = "slot";
            }
            tSO.x = float.Parse(slotsX[i].att("x"));
            tSO.y = float.Parse(slotsX[i].att("y"));
            tSO.layerID = int.Parse(slotsX[i].att("layer"));

            tSO.layerName = sortingLayerNames[tSO.layerID];
            switch (tSO.type)
            {
                case "drawpile":
                    tSO.stagger.x = float.Parse(slotsX[i].att("xstagger"));
                    drawPile = tSO;
                    break;
                case "discardpile":
                    discardPile = tSO;
                    break;
            }
        }
    }
}
