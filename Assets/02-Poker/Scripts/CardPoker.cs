using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum eCardState1
{
    drawpile,
    tableau,
    target,
    discard
}
public class CardPoker : Card
{ 
    [Header("Set Dynamically: CardProspector")]

public eCardState1 state = eCardState1.drawpile;
    
public List<CardPoker> hiddenBy = new List<CardPoker>();
    
public int layoutID;

public SlotDef slotDef;
}
