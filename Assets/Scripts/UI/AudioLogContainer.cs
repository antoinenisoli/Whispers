using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class AudioLogContainer : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Image portrait;
    Vector3 basePos;
    RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        EventManager.instance.OnPlayLog.AddListener(Translation);
        basePos = rectTransform.position;
    }

    IEnumerator End(AudioProfile profile)
    {
        yield return new WaitForSeconds(profile.myDialog.length);
        rectTransform.DOLocalMove(basePos, 0.5f);
    }

    public void Translation(AudioProfile profile)
    {
        StartCoroutine(End(profile));
        portrait.sprite = profile.portrait;
        rectTransform.DOMove(target.position, 0.5f);
    }
}
