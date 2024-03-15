using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    public void ControlPlayerPrefs()
    {
        resetPlayerPrefs();
        
        /*
        PlayerPrefs.SetInt("하츠네 미쿠의 포토 카드", 0);
        PlayerPrefs.SetInt("돛새치", 2);
        PlayerPrefs.SetInt("청휘석", 1200);
        PlayerPrefs.SetInt("참치 초밥", 0);
        PlayerPrefs.SetInt("계란 초밥", 0);
        PlayerPrefs.SetInt("장어 초밥", 0);
        PlayerPrefs.SetInt("아보카도 캘리포니아 롤", 0);
        PlayerPrefs.SetInt("새우 초밥", 0);
        */
    }

    private void resetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
}
