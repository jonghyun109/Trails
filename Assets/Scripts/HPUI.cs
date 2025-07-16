using UnityEngine;
using UnityEngine.UI;

public class HPUI : MonoBehaviour
{
    public int targetActorNumber; // 1 or 2 등 (Inspector에서 지정)
    public Image[] heartImages;
    public Sprite[] heartSprites;

    private int currentHp = 6;

    void Start()
    {
        UpdateHearts();
    }

    public void TryUpdateHp(int actorNumber, int newHp)
    {
        if (actorNumber != targetActorNumber) return;

        currentHp = newHp;
        UpdateHearts();
    }

    void UpdateHearts()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            int heartHp = Mathf.Clamp(currentHp - (i * 2), 0, 2);
            heartImages[i].sprite = heartSprites[heartHp];
        }
    }
}
