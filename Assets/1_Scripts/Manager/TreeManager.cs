using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    [SerializeField] GameObject[] trees;
    public static TreeManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
       // DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        FindTrees();
    }
    
    void FindTrees()
    {
        GameObject gbrTrees = GameObject.Find("Trees");
        trees = new GameObject[gbrTrees.transform.childCount];

        int cnt = 0;
        foreach (Transform tree in gbrTrees.transform)
        {
            trees[cnt] = tree.gameObject;
            cnt++;
        }
    }

    //public void CutedTree(float respawnTime)
    //{
    //    for(int i = 0; i < trees.Length; i++)
    //    {
    //        if (trees[i].activeSelf) return;

    //        // tree가 베어졌을 경우
    //        StartCoroutine(Respawn(i, respawnTime));
    //        break;
    //    }
    //}
    //IEnumerator Respawn(int treeNum, float second)
    //{
    //    trees[treeNum].SetActive(false);

    //    yield return new WaitForSeconds(second);
    //    Debug.Log("respawn 재작동");
    //    gameObject.SetActive(true);
    //}
}
