using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    [SerializeField] Material backMaterial;
    [SerializeField] Material faceMaterial;
    [SerializeField] GameObject canvas;
    [SerializeField] TMPro.TextMeshProUGUI topLeftText;
    [SerializeField] TMPro.TextMeshProUGUI bottomRightText;

    [SerializeField] Color redColor;
    [SerializeField] Color blackColor;

    /// <summary>
    /// 0: heart
    /// 1: diamond
    /// 2: spade
    /// 3: club
    /// </summary>
    [SerializeField] Sprite[] sprites;

    [SerializeField] Image cardImage;

    private bool revealed;
    private float flipElevation = 0.3f;
    private Renderer render;
    private Card card;

    // Start is called before the first frame update
    void Start()
    {
        render = GetComponent<Renderer>();
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void InitCard(Card card)
    {
        this.card = card;
        cardImage.sprite = sprites[(int)card.color];

        var color = card.color switch
        {
            CardColor.Heart => redColor,
            CardColor.Diamond => redColor,
            CardColor.Spade => blackColor,
            CardColor.Club => blackColor,
            _ => blackColor,
        };
        string numberTxt = GetCardText(card);
        topLeftText.text = numberTxt;
        topLeftText.color = color;
        bottomRightText.text = numberTxt;
        bottomRightText.color = color;
    }

    private static string GetCardText(Card card)
    {
        if (card.number <= 10)
            return card.number.ToString();

        return card.number switch 
        {
            11 => "J",
            12 => "Q",
            13 => "K",
            _ => "?"
        };
    }

    public void Flip()
    {
        revealed = !revealed;

        float targetZrotation = revealed ? 180f : 0f;
        float initialY = transform.position.y;

        StartCoroutine(FlipAnimation(initialY, transform.rotation.z, targetZrotation));
    }

    private IEnumerator FlipAnimation(float initialY, float startingZ, float targetZrotation)
    {
        float time = 0;
        float duration = 0.4f;
        bool materialChanged = false;

        while (time < duration)
        {
            var t = time / duration;
            SetCardLerpElevation(initialY, t);
            SetCardLerpRotation(startingZ, targetZrotation, t);

            yield return new WaitForFixedUpdate();
            time += Time.deltaTime;

            materialChanged = EnsureMaterialChangedWhenHalfRotated(materialChanged, t);
        }

        SetCardLerpRotation(targetZrotation, targetZrotation, 1);
        SetCardLerpElevation(initialY, 1);
    }

    private bool EnsureMaterialChangedWhenHalfRotated(bool materialChanged, float t)
    {
        if (!materialChanged && t > 0.5)
        {
            var mat = revealed ? faceMaterial : backMaterial;
            render.material = mat;
            materialChanged = true;
            canvas.SetActive(revealed);
        }

        return materialChanged;
    }

    private void SetCardLerpElevation(float initialY, float t)
    {
        var pos = transform.position;
        pos.y = initialY + Mathf.Sin(t * Mathf.PI) * flipElevation;
        transform.position = pos;
    }

    private void SetCardLerpRotation(float startingZ, float targetZrotation, float t)
    {
        Vector3 angles = transform.rotation.eulerAngles;
        angles.z = Mathf.Lerp(startingZ, targetZrotation, t);
        transform.rotation = Quaternion.Euler(angles);
    }
}
