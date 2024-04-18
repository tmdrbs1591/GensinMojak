using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActive : MonoBehaviour
{


    void OnEnable()
    {
        Invoke("SetActives", 1f);
    }


    void Update()
    {

    }

    void SetActives()
    {
        gameObject.SetActive(false);
    }
}
