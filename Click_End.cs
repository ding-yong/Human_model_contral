using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Click_End : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Button btn = this.GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    private void OnClick()
    {
        Debug.Log("完成标定");
        GameObject.Find("Hand_Rig/L_Hand_Rig").GetComponent<ComHand_L>().Find_MaxMin_Stop();
    }
}
