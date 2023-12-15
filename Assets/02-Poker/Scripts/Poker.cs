using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  
using UnityEngine.UI; 
public class Poker : MonoBehaviour
{
    static public Poker S;
    [Header("Set in Inspector")]
    public TextAsset DeckXML2;
    public TextAsset LayoutXML2;
    public float xOffset = 3;
    public float yOffset = -2.5f;
    public Vector3 layoutsCenter;


    [Header("Set Dynamically")]
    public Deck2 deck2;
    public Layouts layouts;
    public List<CardPoker> drawPile;
    public Transform layoutsAnchor;
    public CardPoker target;
    public List<CardPoker> tableau;
    public List<CardPoker> discardPile;

    void Awake()
    {
        S = this; 
    }
    void Start()
    {
        deck2 = GetComponent<Deck2>(); 
        deck2.InitDeck2(DeckXML2.text);
        Deck2.Shuffle(ref deck2.cards);
        layouts = GetComponent<Layouts>();
        layouts.ReadLayout(LayoutXML2.text);
        drawPile = ConvertListCardsToListCardPokers(deck2.cards);
        LayoutsGame();



       //Card2 c;
        //for (int cNum = 0; cNum < deck2.cards.Count; cNum++)
        //{  
        //c = deck2.cards[cNum];
        //c.transform.localPosition = new Vector3((cNum % 13) * 3, cNum / 13 * 4, 0);
        //}//shuffle card- how to put them in deck instead of the board?

        layouts = GetComponent<Layouts>(); 
        layouts.ReadLayout(LayoutXML2.text);
    }
    List<CardPoker> ConvertListCardsToListCardPokers(List<Card2>lCO)
    {
        List<CardPoker> lCS = new List<CardPoker>();
        CardPoker tCP;
        foreach (Card2 tCD in lCO)
        {
            tCP = tCD as CardPoker; // a
            lCS.Add(tCP);
        }
        return (lCS);
    }
    CardPoker Draw()
    {
        CardPoker cr = drawPile[0]; 
        drawPile.RemoveAt(0); 
        return (cr); 
    }
    void LayoutsGame()
    {
        if (layoutsAnchor == null)
        {
            GameObject tGO = new GameObject("_LayoutsAnchor");
            layoutsAnchor = tGO.transform;
            layoutsAnchor.transform.position = layoutsCenter;
        }
        CardPoker ck;

        foreach (SlotDef2 tSO in layouts.slotDefs2)
        {
            ck = Draw();
            ck.faceUp2 = tSO.faceUp; 
            ck.transform.parent = layoutsAnchor; 
                                                 
            ck.transform.localPosition = new Vector3(
            layouts.multiplier.x * tSO.x,
            layouts.multiplier.y * tSO.y,
            -tSO.layerID);
            
            ck.layoutID = tSO.id;
            ck.slotDef2 = tSO;
            
            ck.state = eCardState1.tableau;
            tableau.Add(ck);
        }
    }
        }
// *** pg. 805***
