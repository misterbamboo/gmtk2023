using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform dealerCardSpawnPoint;
    [SerializeField] private Transform[] playersPositions;
    [SerializeField] private Transform dealerPosition;

    private int currentIndex;

    private bool distributeOrFlip = true;
    private CardScript lastDistributeCard;

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (distributeOrFlip)
            {
                DistributeCard();
            }
            else
            {
                FLipCard();
            }
            distributeOrFlip = !distributeOrFlip;
        }
    }

    private void FLipCard()
    {
        lastDistributeCard.Flip();
    }

    private void DistributeCard()
    {
        var cardNumber = UnityEngine.Random.Range(1, 13 + 1);
        var cardColor = UnityEngine.Random.Range(0, 4);
        var card = new Card((CardColor)cardColor, cardNumber);

        var cardObject = Instantiate(cardPrefab, dealerCardSpawnPoint.position, dealerCardSpawnPoint.rotation);
        lastDistributeCard = cardObject.GetComponent<CardScript>();
        lastDistributeCard.InitCard(card);

        Transform targetPos = GetTargetCardPos();
        AnimateToPos(cardObject, targetPos);

        MoveNextPlayer();
    }

    private void MoveNextPlayer()
    {
        currentIndex++;
        if (currentIndex > playersPositions.Length)
        {
            currentIndex = 0;
        }
    }

    private Transform GetTargetCardPos()
    {
        Transform targetPos;
        if (playersPositions.Length == currentIndex)
        {
            targetPos = dealerPosition;
        }
        else
        {
            targetPos = playersPositions[currentIndex];
        }

        return targetPos;
    }

    private void AnimateToPos(GameObject cardObject, Transform targetPos)
    {
        StartCoroutine(AnimateCardToPosition(cardObject, cardObject.transform, targetPos));
    }

    private IEnumerator AnimateCardToPosition(GameObject cardObject, Transform startingPos, Transform targetPos)
    {
        float time = 0;
        while (time < 1)
        {
            var pos = Vector3.Lerp(startingPos.position, targetPos.position, time);
            var rot = Quaternion.Lerp(startingPos.rotation, targetPos.rotation, time);
            cardObject.transform.position = pos;
            cardObject.transform.rotation = rot;
            yield return new WaitForFixedUpdate();
            time += Time.deltaTime;
        }
    }
}
