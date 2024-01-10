using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateCharacterSceneManager : MonoBehaviour
{
    [SerializeField]
    Transform trfPlayer;

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

    private float rotSpeed = 10.0f;

    void MoveSelection(ref List<GameObject> list, ref int num, Text text, string strName, bool b = true)
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
        text.text = strName + (num+1).ToString();
    }

    public void MoveRightHairSelection()
    {
        MoveSelection(ref lHairBase, ref hairNum, hairBaseText, "머리");
    }

    public void MoveLeftHairSelection()
    {
        MoveSelection(ref lHairBase, ref hairNum, hairBaseText, "머리", false);
    }

    public void MoveRightHairFrontSelection()
    {
        MoveSelection(ref lHairFront, ref hairFrontNum, hairFrontText, "앞머리");
    }
    public void MoveLeftHairFrontSelection()
    {
        MoveSelection(ref lHairFront, ref hairFrontNum, hairFrontText, "앞머리", false);
    }

    public void MoveRightFaceSelection()
    {
        MoveSelection(ref lFace, ref faceNum, faceText, "얼굴");
    }
    public void MoveLeftFaceSelection()
    {
        MoveSelection(ref lFace, ref faceNum, faceText, "얼굴", false);
    }
    public void MoveRightAccessorySelection()
    {
        MoveSelection(ref lAccesary, ref accesorryNum, accesoryText, "장신구");
    }
    public void MoveLeftAccessorySelection()
    {
        MoveSelection(ref lAccesary, ref accesorryNum, accesoryText,"장신구",false);
    }

    public void MoveRightSkinSelection()
    {
        //1. 0 1 2 3 4 5, size = 6, 3
        //2. 0  2  4 6 -2, + 6 = 4
        lSkin[skinNum].SetActive(false);
        lSkin[skinNum + 1].SetActive(false);

        if (skinNum == (lSkin.Count - 2)) skinNum -= (lSkin.Count - 2);
        else skinNum += 2;

        lSkin[skinNum].SetActive(true);
        lSkin[skinNum + 1].SetActive(true);
        skinText.text = "스킨" + ((skinNum / 2) + 1).ToString();
    }
    public void MoveLeftSkinSelection()
    {
        lSkin[skinNum].SetActive(false);
        lSkin[skinNum + 1].SetActive(false);

        skinNum -= 2;
        if (skinNum < 0) skinNum += lSkin.Count;

        lSkin[skinNum].SetActive(true);
        lSkin[skinNum + 1].SetActive(true);
        skinText.text = "스킨" + ((skinNum / 2) + 1).ToString();
    }

    IEnumerator RotateCharacter()
    {
        trfPlayer.Rotate(new Vector3(0, 90, 0) * Time.deltaTime);
        yield return null;
    }

    public void StartCorutine(string name)
    {
        StartCoroutine(name);
    }
}
