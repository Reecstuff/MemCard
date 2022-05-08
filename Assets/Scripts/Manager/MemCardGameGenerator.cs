using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Random;

/// <summary>
/// Generate a card field out of a list
/// every card exists twice
/// </summary>
public class MemCardGameGenerator : MonoBehaviour
{
    [Serializable]
    public struct MemCard
    {
        public string DescText;
        public Sprite Img;
    }

    [SerializeField]
    [Tooltip("Overall list of memory cards")]
    List<MemCard> MemCards = new List<MemCard>();

    [SerializeField]
    ObjectFieldGenerator ObjectField;

    [Tooltip("Set to 0 to generate a new seed every time")]
    public int RandomSeed = 0;

    List<FlipCard> CardList = new List<FlipCard>();

    private void Awake()
    {
        if (!ObjectField)
        {
            Debug.LogError("No ObjectFieldGenerator selected!");
            return;
        }

        // Turn off SelfGeneration of ObjectFieldGenerator
        ObjectField.CanGenerateSelf = false;
        
        // Check for invalid MemoryCardField
        if (ObjectField.ObjectList.Count > 0 && ObjectField.ObjectList.Count / 2 != MemCards.Count)
        {
            ObjectField.Clear();
        }
        if (ObjectField.FieldSizeX != MemCards.Count || ObjectField.FieldSizeY != MemCards.Count)
        {
            float i = Mathf.Sqrt(MemCards.Count * 2);

            ObjectField.FieldSizeX = Mathf.FloorToInt(i);
            ObjectField.FieldSizeY = Mathf.CeilToInt(i);
        }

        ObjectField.StartGeneration();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (RandomSeed == 0)
            RandomSeed = Range(-9999, 9999);

        InitState(RandomSeed);

        // Get List of all Cards
        GetCardList();

        // Change Cards Values
        CreateMemoryCards();
    }

    void GetCardList()
    {
        foreach (GameObject go in ObjectField.ObjectList)
        {
            FlipCard card = go.GetComponentInChildren<FlipCard>();
            if (card)
            {
                CardList.Add(card);
            }
        }
    }

    void CreateMemoryCards()
    {
        int rn = Range(0, CardList.Count);
        
        for (int i = 0; i < CardList.Count; i++)
        {

            if (i % MemCards.Count == 0)
            {
                ShuffleCards();
            }

            CardList[(i + rn) % CardList.Count].BackText.SetText(MemCards[i % MemCards.Count].DescText);
            CardList[(i + rn) % CardList.Count].BackImage.sprite = MemCards[i % MemCards.Count].Img;
            
        }
    }

    void ShuffleCards()
    {
        int rn = 0;

        for (int i = 0; i < MemCards.Count; i++)
        {
            rn = Range(0, MemCards.Count);
            MemCards.Add(MemCards[rn]);
            MemCards.RemoveAt(rn);
        }
    }
}
