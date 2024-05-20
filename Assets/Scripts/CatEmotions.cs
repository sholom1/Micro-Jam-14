using UnityEngine;

public class CatEmotions : MonoBehaviour
{
    [SerializeField]
    private Sprite boredCat;
    [SerializeField]
    private Sprite excitedCat;
    [SerializeField]
    private Sprite happyCat;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private PawController pawController;

    private float timer;

    private CatMood catState;

    private void Update()
    {
        if (catState == CatMood.bored && pawController.distancesTotal > 0)
        {
            timer = 1;
            SetCatState(CatMood.excited);
            return;
        }
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            switch (catState)
            {
                case CatMood.happy:
                    SetCatState(CatMood.excited);
                    break;
                case CatMood.excited:
                    SetCatState(CatMood.bored);
                    break;
            }
            timer = 1;
        }
    }
    public void SetCatStateExcited()
    {
        SetCatState(CatMood.happy);
    }

    public void SetCatState(CatMood state)
    {
        switch (state)
        {
            case CatMood.bored:
                spriteRenderer.sprite = boredCat;
                break;
            case CatMood.excited:
                spriteRenderer.sprite = excitedCat;
                break;
            case CatMood.happy:
                spriteRenderer.sprite = happyCat;
                break;
        }
        catState = state;
    }
}
[System.Serializable]
public enum CatMood
{
    bored,
    excited,
    happy
}