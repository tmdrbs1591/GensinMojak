using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject damageEffectPrefab;

    private GameObject[] damageText;
    private GameObject[] damageEffect;

    private GameObject[] targetPool;

    void Awake()
    {
        damageText = new GameObject[10];
        damageEffect = new GameObject[10];

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
