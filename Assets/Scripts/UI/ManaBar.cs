using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    private Image manaBarImage;

    private void Awake()
    {
        manaBarImage = GetComponentInChildren<Image>();
    }

    public void SetMana(float manaNormalized)
    {
        manaBarImage.fillAmount = manaNormalized;
    }
}