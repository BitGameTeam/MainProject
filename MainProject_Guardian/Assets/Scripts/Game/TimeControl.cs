using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    #region ui_변수
    [SerializeField]
    GameObject pauseBtnUI;
    [SerializeField]
    GameObject resumeBtnUI;
    #endregion

    bool isPause = false;

    public void PauseOrResumeGame()
    {
        if(isPause == true)
        {
            pauseBtnUI.SetActive(true);
            //resumeBtnUI.SetActive(false);
            Time.timeScale = 1f;
            isPause = false;
            return;
        }
        if(isPause == false)
        {
            pauseBtnUI.SetActive(false);
            //resumeBtnUI.SetActive(true);
            Time.timeScale = 0f;
            isPause = true;
            return;
        }
    }
}
