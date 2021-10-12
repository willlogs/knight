using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitMouseUI : MonoBehaviour
{
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            gameObject.SetActive(false);
        }
    }
}
