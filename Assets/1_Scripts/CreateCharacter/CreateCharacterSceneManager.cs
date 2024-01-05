using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharacterSceneManager : MonoBehaviour
{
    [SerializeField]
    List<GameObject> lHairBase;

    [SerializeField]
    Text hairBaseText;

    [SerializeField]
    List<GameObject> lHairFront;

    [SerializeField]
    Text hairFrontText;

    [SerializeField]
    List<GameObject> lFace;

    [SerializeField]
    Text faceText;

    [SerializeField]
    List<GameObject> lAccesary;

    [SerializeField]
    Text accesoryText;

    [SerializeField]
    List<GameObject> lSkin;

    [SerializeField]
    Text skinText;

    private int hairNum = 0, hairFrontNum = 0, faceNum = 0,
                accesorryNum = 0, skinNum = 0;

    void MoveSelection(ref List<GameObject> list, ref int num, Text text, bool b = true)
    {
        if (b)
        {
            list[num++].SetActive(false);
            num %= list.Count;
        }
        else
        {
            list[num--].SetActive(false);
            if (num < 0) 
                num += list.Count;
        }

        list[num].SetActive(true);
        text.text = list[num].name;
    }

    public void MoveRightHairSelection()
    {
        MoveSelection(ref lHairBase, ref hairNum, hairBaseText);
    }

    public void MoveLeftHairSelection()
    {
        MoveSelection(ref lHairBase, ref hairNum, hairBaseText, false);
    }

    public void MoveRightHairFrontSelection()
    {
        MoveSelection(ref lHairFront, ref hairFrontNum, hairFrontText);
    }
    public void MoveLeftHairFrontSelection()
    {
        MoveSelection(ref lHairFront, ref hairFrontNum, hairFrontText, false);
    }

    public void MoveRightFaceSelection()
    {
        MoveSelection(ref lFace, ref faceNum, faceText);
    }
    public void MoveLeftFaceSelection()
    {
        MoveSelection(ref lFace, ref faceNum, faceText, false);
    }
    public void MoveRightAccessorySelection()
    {
        MoveSelection(ref lAccesary, ref accesorryNum, accesoryText);
    }
    public void MoveLeftAccessorySelection()
    {
        MoveSelection(ref lAccesary, ref accesorryNum, accesoryText,false);
    }

    public void MoveRightSkinSelection()
    {
        //1. 0 1 2 3 4 5, size = 6, 3
        //2. 0  2  4 6 -2, + 6 = 4
        lSkin[skinNum].SetActive(false);
        lSkin[skinNum+1].SetActive(false);

        skinNum += 2;
        if (skinNum > lSkin.Count) skinNum %= lSkin.Count;

        lSkin[skinNum].SetActive(true);
        lSkin[skinNum + 1].SetActive(true);
        skinText.text = lSkin[skinNum].name;
    }
    public void MoveLeftSkinSelection()
    {
        lSkin[skinNum].SetActive(false);
        lSkin[skinNum + 1].SetActive(false);

        skinNum -= 2;
        if (skinNum < 0) skinNum += lSkin.Count;

        lSkin[skinNum].SetActive(true);
        lSkin[skinNum + 1].SetActive(true);
        skinText.text = lSkin[skinNum].name;
    }
}
