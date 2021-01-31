using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SubTextManager : MonoBehaviour
{
    [SerializeField] DialogInfo startDialog;
    CanvasGroup group;
    Text myText;

    private IEnumerator Start()
    {
        EventManager.instance.OnDialog.AddListener(StartDialog);
        group = GetComponent<CanvasGroup>();
        myText = GetComponentInChildren<Text>();
        group.DOFade(0, 0f);

        yield return new WaitForSeconds(1);
        EventManager.instance.OnDialog.Invoke(startDialog);
    }

    IEnumerator End(float delay)
    {
        yield return new WaitForSeconds(delay);
        group.DOFade(0, 0.5f);
    }

    void StartDialog(DialogInfo info)
    {
        group.DOFade(1, 0.5f);
        myText.text = info.subText;
        StartCoroutine(End(info.clip.length));
    }
}
