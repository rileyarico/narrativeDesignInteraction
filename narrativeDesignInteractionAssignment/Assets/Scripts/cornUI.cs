using TMPro;
using UnityEngine;

public class cornUI : MonoBehaviour
{
    public TextMeshProUGUI coinAmtText;

    private void Awake()
    {
        GameController.instance.setUI(this);
    }

    public void addCorn(int corn)
    {
        coinAmtText.text = corn.ToString();
    }
}
