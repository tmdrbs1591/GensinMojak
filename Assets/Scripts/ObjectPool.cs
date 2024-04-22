using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectPool : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject damageEffectPrefab;
    public GameObject SkillPrefab;

    private GameObject[] damageText;
    private GameObject[] damageEffect;
    private GameObject[] Skill;

    private GameObject[] targetPool;

    void Awake()
    {
        damageText = new GameObject[100];
        damageEffect =  new GameObject[100];
        Skill =  new GameObject[10];

        Generate();
    }

    void Generate()
    {
        for (int i = 0; i < damageText.Length; i++)
        {
            damageText[i] = Instantiate(damageTextPrefab, transform); // Set parent as ObjectManager
            damageText[i].SetActive(false);
        }
        for (int i = 0; i < damageEffect.Length; i++)
        {
            damageEffect[i] = Instantiate(damageEffectPrefab, transform); // Set parent as ObjectManager
            damageEffect[i].SetActive(false);
        }   
        for (int i = 0; i < Skill.Length; i++)
        {
            Skill[i] = Instantiate(SkillPrefab, transform); // Set parent as ObjectManager
            Skill[i].SetActive(false);
        }
    }

    public GameObject MakeObj(string type, Transform parent = null)
    {
        switch (type)
        {
            case "damageText":
                targetPool = damageText;
                break;
            case "damageEffect":
                targetPool = damageEffect;
                break; 
            case "Skill":
                targetPool = Skill;
                break;
        }

        for (int i = 0; i < targetPool.Length; i++)
        {
            if (!targetPool[i].activeSelf)
            {
                targetPool[i].SetActive(true);
                if (parent != null)
                {
                    targetPool[i].transform.SetParent(parent); // Set parent if specified
                }
                return targetPool[i];
            }
        }
        return null;
    }

    public GameObject[] GetPool(string type)
    {
        return targetPool;
    }
}
