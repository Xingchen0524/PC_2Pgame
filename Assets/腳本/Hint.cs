using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint : MonoBehaviour
{
    [Header("提示或說明框")]
    public GameObject Illustrate;
    [Header("文字說明")]
    public GameObject Text;
    [Header("透明碰撞器")]
    public GameObject Transparent;
    private void OnTriggerEnter2D(Collider2D other)
    {
    if (other.CompareTag("Player"))
    {
        Illustrate.SetActive(true);
        Text.SetActive(true);
        Transparent.SetActive(false);
    }
    }

}
