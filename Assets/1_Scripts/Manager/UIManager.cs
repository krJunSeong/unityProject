using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    [SerializeField] Stack<GameObject> stkCancelUies; // 후입선출, 
    [SerializeField] GameObject[] uies;               // 특정상황 때 지울 Ui들
    Dictionary<string, GameObject> dicUies;           // 컨트롤용 ui

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        dicUies = new Dictionary<string, GameObject>();
        GameObject inventory = GameObject.Find("pan_Inventroy");
        inventory.GetComponent<CanvasGroup>().alpha = 0;
        inventory.GetComponent<CanvasGroup>().interactable = false;

        dicUies.Add("Inventory", inventory);
        stkCancelUies = new Stack<GameObject>();
    }
    public bool CloesUies()
    {
        foreach (var g in uies)
        {
            if (g == null) continue;
            g.SetActive(false);
        }
        foreach(var g in stkCancelUies)
        {
            CanvasGroup group = g.GetComponent<CanvasGroup>();
            group.alpha = 0;
            group.interactable = false;
        }
        stkCancelUies.Clear();
        return true;
    }

    public bool OpenUies()
    {
        foreach (var g in uies) g.SetActive(true);
        foreach (var g in stkCancelUies)
        {
            CanvasGroup group = g.GetComponent<CanvasGroup>();
            group.alpha = 1;
            group.interactable = true;
        }
        stkCancelUies.Clear();
        return true;
    }

    public void AddUies(GameObject obj)
    {
        stkCancelUies.Push(obj);
    }
    public void CloseUi()
    {
        if (stkCancelUies.Count < 1) { Debug.Log("Ui stack count 0"); return; }
        GameObject obj = stkCancelUies.Pop();
        CanvasGroup group = obj.GetComponent<CanvasGroup>(); // UI active false 하는 함수.
        group.alpha = 0;
        group.interactable = false;
    }

    private void Update()
    {
        UiKeyOpen();
        
    }

    void UiKeyOpen()
    {
        if (Input.GetButtonDown("Cancel")) CloseUi();
        if (Input.GetButtonDown("Inventory")) DicUiControl("Inventory");
    }

    void DicUiControl(string key)
    {
        Debug.Log("111");
        CanvasGroup group = dicUies[key].GetComponent<CanvasGroup>();
        group.interactable = !group.interactable;
        group.alpha = group.alpha > 0 ? 0 : 1;

        if (group.alpha > 0) stkCancelUies.Push(dicUies[key]);
    }
}