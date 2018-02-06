using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour {

    private float f_milicounter;
    private int i_gameTimer_sec;
    private int i_gameTimer_min;
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
            f_milicounter += Time.deltaTime;

            //Change per sec
            sec_timerText.text = i_gameTimer_sec.ToString("00");
            min_timerText.text = i_gameTimer_min.ToString("00");

            if (f_milicounter > 1)
            {
                f_milicounter = 0;
                i_gameTimer_sec++;
                if (i_gameTimer_sec > 59)
                {
                    i_gameTimer_min++;
                    i_gameTimer_sec = 0;
                }
            }
        }
    }
}
