using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatText : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private Text text;

    void Start()
    {
        StartCoroutine(FadeOut());
    }

    void Update()
    {
        Move();
    }

    public void Move()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    public IEnumerator FadeOut()
    {
        var startAlpha = text.color.a;
        var rate = 1.0f / lifeTime;
        var progress = 0.0f;
        while (progress < 1.0)
        {
            var tmp = text.color;
            tmp.a = Mathf.Lerp(startAlpha, 0, progress);
            text.color = tmp;
            progress += rate * Time.deltaTime;
            yield return null;
        }
        
        Destroy(gameObject);
    }
}
