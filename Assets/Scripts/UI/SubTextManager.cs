using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SubTextManager : MonoBehaviour
{
    [SerializeField] DialogInfo startDialog;
    [SerializeField] Text dialogText;
    [SerializeField] Text doorText;
    CanvasGroup group;

    private IEnumerator Start()
    {
        EventManager.instance.onDialog.AddListener(StartDialog);
        EventManager.instance.onDoorUnlocked.AddListener(UnlockDoor);
        group = GetComponent<CanvasGroup>();
        group.DOFade(0, 0f);
        doorText.DOFade(0, 0f);

        yield return new WaitForSeconds(1);
        EventManager.instance.onDialog.Invoke(startDialog);
    }

    IEnumerator End(float delay)
    {
        yield return new WaitForSeconds(delay);
        group.DOFade(0, 0.5f);
    }

    void StartDialog(DialogInfo info)
    {
        group.DOFade(1, 0.5f);
        dialogText.text = info.subText;
        StartCoroutine(End(info.clip.length));
    }

    IEnumerator EndDoor()
    {
        yield return new WaitForSeconds(5);
        doorText.DOFade(0, 0.5f);
    }

    void UnlockDoor()
    {
        doorText.DOFade(1, 0.5f);
        StartCoroutine(EndDoor());
    }
}
