using UnityEngine;
using UnityEngine.UI;

public class HeroCell : MonoBehaviour
{
    public CellInfo info;
    void Start()
    {
        transform.GetChild(0).GetComponent<Image>().sprite = info.sprite;
    }

    void Awake() => GetComponent<Button>().onClick.AddListener(HeroCellClick);

    private void HeroCellClick() => HeroInfoController.instance.onHeroCellPress(info);

    public GameObject SetInfo(CellInfo t_info)
    {
        info = t_info;
        return gameObject;
    }
}
