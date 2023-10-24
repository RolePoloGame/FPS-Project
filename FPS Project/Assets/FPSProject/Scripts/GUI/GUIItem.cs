using Assets.FPSProject.Scripts.Core.Items;
using RolePoloGame.Core.Extensions;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIItem : MonoBehaviour
{
    [SerializeField]
    private RawImage icon;
    [SerializeField]
    private TextMeshProUGUI nameTMP;
    [SerializeField]
    private float fadeTime = 1.0f;

    private CanvasGroup CanvasGroup => m_CanvasGroup ??= GetComponent<CanvasGroup>();
    private CanvasGroup m_CanvasGroup;

    public void Set(Item item)
    {
        nameTMP.SetText(item.Name);
        icon.texture = item.Icon;
        icon.color = item.IconColor.WithAlpha(1.0f);
    }

    public void FadeIn() => StartCoroutine(Fade(from: 0.0f, to: 1.0f, time: fadeTime));
    public void FadeOut() => StartCoroutine(Fade(from: 1.0f, to: 0.0f, time: fadeTime));

    private IEnumerator Fade(float from, float to, float time)
    {
        float timer = 0.0f;
        CanvasGroup.alpha = from;
        while (timer < time)
        {
            yield return new WaitForEndOfFrame();
            timer += Time.deltaTime;
            CanvasGroup.alpha = Mathf.Lerp(from, to, timer / time);
        }
        CanvasGroup.alpha = to;
    }
}
