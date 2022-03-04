using System;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    private new RectTransform transform;
    private TextMeshPro textMesh;

    private bool isAnimating = false;
    private int startingFontSize = 10;
    private float moveSpeed = 2.5f;
    private float shrinkSpeed = 4.0f;
    private float fadeSpeed = 0.8f;


    private void Awake()
    {
        transform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(!isAnimating)
            return;

        transform.position += Vector3.up * (moveSpeed * Time.deltaTime);
        textMesh.fontSize -= shrinkSpeed * Time.deltaTime;
        textMesh.color -= Color.black * (fadeSpeed * Time.deltaTime);

        if (textMesh.color.a <= 0)
            Destroy(gameObject);
    }

    public static void SpawnNewDamageNumber(int damage, Actor target)
    {
        GameObject gameObject = new GameObject("DamageNumber", typeof(RectTransform));
        DamageNumber damageNumber = gameObject.AddComponent<DamageNumber>();
        damageNumber.textMesh = gameObject.AddComponent<TextMeshPro>();

        damageNumber.Setup(damage, target);
    }

    private void Setup(int damage, Actor target)
    {
        textMesh.text = damage.ToString();
        textMesh.alignment = TextAlignmentOptions.Midline;
        textMesh.fontSize = startingFontSize;

        transform.position = target.GetDamageNumberPosition;

        isAnimating = true;
    }
}