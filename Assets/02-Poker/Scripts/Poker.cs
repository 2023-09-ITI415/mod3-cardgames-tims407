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

    [Header("Set Dynamically")]
    public Deck2 deck2;
    public Layouts layouts;

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

        Card2 c;
        for (int cNum = 0; cNum < deck2.cards.Count; cNum++)
        {  
        c = deck2.cards[cNum];
        c.transform.localPosition = new Vector3((cNum % 13) * 3, cNum / 13 * 4, 0);
        }//shuffle card- how to put them in deck instead of the board?

        layouts = GetComponent<Layouts>(); 
        layouts.ReadLayout(LayoutXML2.text);
    }
}
// ***left from pg. 805***
