using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    [SerializeField] Slider BS;
    [SerializeField] GameObject winObject;
    [SerializeField] Image winPanel;
    
    public void Win()
    {
        if (BS.value <= 0)
        {
            winObject.SetActive(true);
            StartCoroutine(Paneltransparency());
        }
    }

    public IEnumerator Paneltransparency()
    {
        Color color = winPanel.color;
        float elapsed = 0f;

        while (elapsed < 2)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsed / 2);
            winPanel.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        winPanel.color = new Color(color.r, color.g, color.b, 1f);

    }




}
