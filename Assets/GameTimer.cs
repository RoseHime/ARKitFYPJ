using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour {

    public float f_gameTimer_sec;
    public int f_gameTimer_min;
    public Text sec_timerText;
    public Text min_timerText;

	// Use this for initialization
	void Start () {
        sec_timerText = gameObject.transform.GetChild(0).GetComponent<Text>();
        min_timerText = gameObject.transform.GetChild(1).GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Pivot").transform.GetChild(0).gameObject.activeSelf == true)
        {
            //Do the timer count
            f_gameTimer_sec += Time.deltaTime;

            //Change per sec
            sec_timerText.text = f_gameTimer_sec.ToString("00");
            min_timerText.text = f_gameTimer_min.ToString("00");
            if (f_gameTimer_sec >= 59)
            {
                f_gameTimer_sec = 0;
                f_gameTimer_min++;
            }
        }
    }
}
