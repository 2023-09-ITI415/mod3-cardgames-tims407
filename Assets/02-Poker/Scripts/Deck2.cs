using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Deck2 : MonoBehaviour
{
    [Header("Set in Inspector")]
    public bool startFaceUp = false;
    public Sprite suitClub;
    public Sprite suitDiamond;
    public Sprite suitHeart;
    public Sprite suitSpade;
    public Sprite[] faceSprites;
    public Sprite[] rankSprites;
    public Sprite cardBack;
    public Sprite cardFront;
    public GameObject prefabCards;
    public GameObject prefabSprites;

    [Header("Set Dynamically")]
    public PT_XMLReader xmlr2;
    public List<string> cardNames;
    public List<Card2> cards;
    public List<Decorator2> decorators;
    public List<Card2Definition> cardDefs;
    public Transform deckAnchor2;
    public Dictionary<string, Sprite> dictSuits;

    public void InitDeck2(string DeckXML2Text)
    {
        if (GameObject.Find("_Deck2") == null)
        {
            GameObject anchorGO = new GameObject("_Deck2");
            deckAnchor2 = anchorGO.transform;
        }
        dictSuits = new Dictionary<string, Sprite>() {
            { "C", suitClub },
            { "D", suitDiamond },
            { "H", suitHeart },
            { "S", suitSpade }
        };
        ReadDeck2(DeckXML2Text);
        MakeCards2();
    }
    public void ReadDeck2(string DeckXML2Text)
    {
        xmlr2 = new PT_XMLReader(); 
        xmlr2.Parse(DeckXML2Text); 
    
        string s = "xml2[0] decorator2[0] ";
        s += "type=" + xmlr2.xml["xml2"][0]["decorator2"][0].att("type");
        s += " x=" + xmlr2.xml["xml2"][0]["decorator2"][0].att("x");
        s += " y=" + xmlr2.xml["xml2"][0]["decorator2"][0].att("y");
        s += " scale=" + xmlr2.xml["xml2"][0]["decorator2"][0].att("scale");

        decorators = new List<Decorator2>(); 
        PT_XMLHashList xDecos = xmlr2.xml["xml2"][0]["decorator2"];
        Decorator2 deco;
        for (int i = 0; i < xDecos.Count; i++)
        {
            deco = new Decorator2(); 
            deco.type = xDecos[i].att("type");
            deco.flip = (xDecos[i].att("flip") == "1"); 
            deco.scale = float.Parse(xDecos[i].att("scale"));
            deco.loc.x = float.Parse(xDecos[i].att("x"));
            deco.loc.y = float.Parse(xDecos[i].att("y"));
            deco.loc.z = float.Parse(xDecos[i].att("z"));
            decorators.Add(deco);
        }
        cardDefs = new List<Card2Definition>(); 
        PT_XMLHashList xCardDefs2 = xmlr2.xml["xml2"][0]["card2"];
        for (int i = 0; i < xCardDefs2.Count; i++)
        {
            Card2Definition cDef = new Card2Definition();
            cDef.rank = int.Parse(xCardDefs2[i].att("rank"));
            PT_XMLHashList xPip2s = xCardDefs2[i]["pip2"];
            if (xPip2s != null)
            {
                for (int j = 0; j < xPip2s.Count; j++)
                {
                    deco = new Decorator2();

                    deco.type = "pip2";
                    deco.flip = (xPip2s[j].att("flip") == "1");
                    deco.loc.x = float.Parse(xPip2s[j].att("x"));
                    deco.loc.y = float.Parse(xPip2s[j].att("y"));
                    deco.loc.z = float.Parse(xPip2s[j].att("z"));
                    if (xPip2s[j].HasAtt("scale"))
                    {
                        deco.scale = float.Parse(xPip2s[j].att("scale"));
                    }
                    cDef.pip2s.Add(deco);
                }
            }
        
            if (xCardDefs2[i].HasAtt("face"))
            {
                cDef.face = xCardDefs2[i].att("face"); 
            }
            cardDefs.Add(cDef);
        }

    }
    public Card2Definition GetCardDefinitionByRank(int rnk)
    {
        foreach (Card2Definition cd2 in cardDefs)
        {
            if (cd2.rank == rnk)
            {
                return (cd2);
            }
        }
        return (null);
    }
    public void MakeCards2()
    {
        cardNames = new List<string>();
        string[] letters = new string[] { "C", "D", "H", "S" };
        foreach (string s in letters)
        {
            for (int i = 0; i < 13; i++)
            {
                cardNames.Add(s + (i + 1));
            }
        }
        cards = new List<Card2>();
        for (int i = 0; i < cardNames.Count; i++)
        {
            cards.Add(MakeCard2(i));
        }
    }
    private Card2 MakeCard2(int cNum)
    {
        GameObject cgo2 = Instantiate(prefabCards) as GameObject;
        cgo2.transform.parent = deckAnchor2;
        Card2 card2 = cgo2.GetComponent<Card2>(); 
        cgo2.transform.localPosition = new Vector3((cNum % 13) * 3, cNum / 13 * 4, 0);
        card2.name = cardNames[cNum];
        card2.suit = card2.name[0].ToString();
        card2.rank = int.Parse(card2.name.Substring(1));
        if (card2.suit == "D" || card2.suit == "H")
        {
            card2.colS = "Red";
            card2.color = Color.red;
        }
        card2.def2 = GetCardDefinitionByRank(card2.rank);
        AddDecorators(card2);
        AddPips(card2);
        AddFace(card2);
        AddBack(card2);
        return card2;
    }
    private Sprite _tSp = null;
    private GameObject _tGO = null;
    private SpriteRenderer _tSR = null;
    private void AddDecorators(Card2 card2)
    { 
        foreach (Decorator2 deco in decorators)
        {
            if (deco.type == "suit")
            {
                _tGO = Instantiate(prefabSprites) as GameObject;
                _tSR = _tGO.GetComponent<SpriteRenderer>();
                _tSR.sprite = dictSuits[card2.suit];
            }
            else
            {
                _tGO = Instantiate(prefabSprites) as GameObject;
                _tSR = _tGO.GetComponent<SpriteRenderer>();
                _tSp = rankSprites[card2.rank];
                _tSR.sprite = _tSp;
                _tSR.color = card2.color;
            }
            _tSR.sortingOrder = 1;
            _tGO.transform.SetParent(card2.transform);
            _tGO.transform.localPosition = deco.loc;
            if (deco.flip)
            {
                _tGO.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            if (deco.scale != 1)
            {
                _tGO.transform.localScale = Vector3.one * deco.scale;
            }
            _tGO.name = deco.type;
            card2.deco2GOs.Add(_tGO);
        }
    }
    private void AddPips(Card2 card2)
    {
        foreach (Decorator2 pip2 in card2.def2.pip2s)
        {
            _tGO = Instantiate(prefabSprites) as GameObject;
            _tGO.transform.SetParent(card2.transform);
            _tGO.transform.localPosition = pip2.loc;
            if (pip2.flip)
            {
                _tGO.transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            if (pip2.scale != 1)
            {
                _tGO.transform.localScale = Vector3.one * pip2.scale;
            }
            _tGO.name = "pip2";
            _tSR = _tGO.GetComponent<SpriteRenderer>();
            _tSR.sprite = dictSuits[card2.suit];
            _tSR.sortingOrder = 1;
            card2.pip2GOs.Add(_tGO);
        }
    }
    private void AddFace(Card2 card2)
    {
        if (card2.def2.face == "")
        {
            return; 
        }
        _tGO = Instantiate(prefabSprites) as GameObject;
        _tSR = _tGO.GetComponent<SpriteRenderer>();
        _tSp = GetFace(card2.def2.face + card2.suit);
        _tSR.sprite = _tSp; 
        _tSR.sortingOrder = 1; 
        _tGO.transform.SetParent(card2.transform);
        _tGO.transform.localPosition = Vector3.zero;
        _tGO.name = "face";
    }
    private Sprite GetFace(string faceS)
    {
        foreach (Sprite _tSP in faceSprites)
        {
            if (_tSP.name == faceS)
            {
                return (_tSP);
            }
        }
        return (null);
    }
    private void AddBack(Card2 card2)
    {
        _tGO = Instantiate(prefabSprites) as GameObject;
        _tSR = _tGO.GetComponent<SpriteRenderer>();
        _tSR.sprite = cardBack;
        _tGO.transform.SetParent(card2.transform);
        _tGO.transform.localPosition = Vector3.zero;
        _tSR.sortingOrder = 2;
        _tGO.name = "back2";
        card2.back2 = _tGO;
        card2.faceUp2 = startFaceUp;
    }
    static public void Shuffle(ref List<Card2> oCards)
    { 
        List<Card2> tCards = new List<Card2>();
        int ndx; 
        tCards = new List<Card2>();
        while (oCards.Count > 0)
        {
            ndx = Random.Range(0, oCards.Count);
            tCards.Add(oCards[ndx]);
            oCards.RemoveAt(ndx);
        }
        oCards = tCards;
    }


}
