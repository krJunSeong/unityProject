using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [SerializeField] ItemData[] itemDatas;
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
        CheatKey();
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
        for(int i = 0; i < itemDatas.Length; i++)
        {
            if (itemDatas[i] == null) continue;

            if (itemDatas[i].type == ItemData.ItemType.SEED)
            {
                Seed seed = new Seed((SeedData)itemDatas[i]);
                dicItems.Add(itemDatas[i].itemName, seed);
            }
            else
            {
                Item item = new Item(itemDatas[i]);
                dicItems.Add(itemDatas[i].itemName, item);
            }
            // CornSeed, CarrotSeed
        }
    }

    public void GiveToItem(string _name, int _amount)
    {
        GameManager.Instance.GiveItemToPlayer(dicItems[_name], _amount);
    }

    void CheatKey()
    {
        if (Input.GetKeyDown(KeyCode.F1)) GiveToItem(dicItems["CarrotSeed"].itemName, 1);
        if (Input.GetKeyDown(KeyCode.F2)) GiveToItem(dicItems["CornSeed"].itemName, 1);
    }
}
