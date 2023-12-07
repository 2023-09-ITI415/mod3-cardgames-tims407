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
    public List<SlotDef2> slotDefs;
    public SlotDef2 drawPile;
    public SlotDef2 discardPile;
    public string[] sortingLayerNames = new string[] { "Discard", "Draw" };
    public void ReadLayout(string xmlText)
    {
        xmlr2 = new PT_XMLReader();
        xmlr2.Parse(xmlText);
        xml2 = xmlr2.xml["xml"][0]; 
        multiplier.x = float.Parse(xml2["multiplier"][0].att("x"));
        multiplier.y = float.Parse(xml2["multiplier"][0].att("y"));

        SlotDef2 tSD;

        PT_XMLHashList slotsX = xml2["slot"];
        for (int i = 0; i < slotsX.Count; i++)
        {
            tSD = new SlotDef2();
            if (slotsX[i].HasAtt("type"))
            {
                tSD.type = slotsX[i].att("type");
            }
            else
            {
                tSD.type = "slot";
            }
            tSD.x = float.Parse(slotsX[i].att("x"));
            tSD.y = float.Parse(slotsX[i].att("y"));
            tSD.layerID = int.Parse(slotsX[i].att("layer"));

            tSD.layerName = sortingLayerNames[tSD.layerID];
            switch (tSD.type)
            {
                case "drawpile":
                    tSD.stagger.x = float.Parse(slotsX[i].att("xstagger"));
                    drawPile = tSD;
                    break;
                case "discardpile":
                    discardPile = tSD;
                    break;
            }
        }
    }
}
