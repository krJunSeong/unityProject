using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] Item[] items;
    Dictionary<string, Item> dicItems;

    public static ItemManager Instance { get; private set; }
    void Awake()
    {
        Init();
    }
    void Start()
    {
        
    }
    void Update()  
    {
        
    }

    void Init()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // DontDestroyOnLoad(gameObject);

        dicItems = new Dictionary<string, Item>();
        for(int i = 0; i < items.Length; i++)
        {
            dicItems.Add(items[i].name, items[i]);
            // CornSeed, CarrotSeed
        }
    }

    public void GiveToItem(string _name, int _amount)
    {
        GameManager.Instance.GiveItemToPlayer(dicItems[_name], _amount);
    }
}
