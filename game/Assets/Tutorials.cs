using UnityEngine;
using System.Collections;

public class Tutorials : MonoBehaviour {

    public GameObject panel;

    public GameObject tutorial_walls;
    public GameObject tutorial_trampolin;
    public GameObject tutorial_run;
    public GameObject tutorial_mud;
    public GameObject tutorial_hurdles;

    public enum panels
    {
        WALLS,
        TRAMPOLIN,
        RUN,
        MUD,
        HURDLES
    }

	void Start () {
        panel.SetActive(false);
        Events.OnTutorialOn += OnTutorialOn;
	}
    void OnDestroy()
    {
        Events.OnTutorialOn -= OnTutorialOn;
    }
    void SetOff()
    {
        tutorial_walls.SetActive(false);
        tutorial_trampolin.SetActive(false);
        tutorial_run.SetActive(false);
        tutorial_mud.SetActive(false);
        tutorial_hurdles.SetActive(false);
    }    
    void OnTutorialOn(panels _panel)
    {
        SetOff();
        switch (_panel)
        {
            case panels.HURDLES: 
                if (!Data.Instance.userData.ready_tutorial_hurdles) 
                {
                    tutorial_hurdles.SetActive(true); SetOn();
                    StartCoroutine(PlayCoroutine(tutorial_hurdles.GetComponent<Animation>(), "hurdles", false));
                    Data.Instance.userData.ready_tutorial_hurdles = true;
                }
                break;
            case panels.MUD:
                if (!Data.Instance.userData.ready_tutorial_mud) 
                {
                    tutorial_mud.SetActive(true); SetOn();
                    StartCoroutine(PlayCoroutine(tutorial_mud.GetComponent<Animation>(), "hurdles", false));
                    Data.Instance.userData.ready_tutorial_mud = true;
                } 
                break;
            case panels.RUN:
                if (!Data.Instance.userData.ready_tutorial_run) 
                {
                    tutorial_run.SetActive(true); SetOn();
                    StartCoroutine(PlayCoroutine(tutorial_run.GetComponent<Animation>(), "run", false));
                    Data.Instance.userData.ready_tutorial_run = true;
                } 
                break;
            case panels.TRAMPOLIN:
                if (!Data.Instance.userData.ready_tutorial_trampolin) 
                {
                    tutorial_trampolin.SetActive(true); SetOn();
                    StartCoroutine(PlayCoroutine(tutorial_trampolin.GetComponent<Animation>(), "hurdles", false));
                    Data.Instance.userData.ready_tutorial_trampolin = true;
                } 
                break;
            case panels.WALLS:
                if (!Data.Instance.userData.ready_tutorial_walls) 
                {
                    tutorial_walls.SetActive(true); SetOn();
                    StartCoroutine(PlayCoroutine(tutorial_walls.GetComponent<Animation>(), "walls", false));
                    Data.Instance.userData.ready_tutorial_walls = true;
                } 
                break;
        }
    }
    void SetOn()
    {
        Time.timeScale = 0;
        panel.SetActive(true);
    }
    public void Close()
    {
        print("Close");
        panel.SetActive(false);
        Time.timeScale = 1;
    }

    public IEnumerator PlayCoroutine(Animation animation, string clipName, bool useTimeScale)
    {
        if (!useTimeScale)
        {
            AnimationState _currState = animation[clipName];
            bool isPlaying = true;
            float _progressTime = 0F;
            float _timeAtLastFrame = 0F;
            float _timeAtCurrentFrame = 0F;
            float deltaTime = 0F;

            animation.Play(clipName);

            _timeAtLastFrame = Time.realtimeSinceStartup;

            while (isPlaying)
            {
                _timeAtCurrentFrame = Time.realtimeSinceStartup;
                deltaTime = _timeAtCurrentFrame - _timeAtLastFrame;
                _timeAtLastFrame = _timeAtCurrentFrame;

                _progressTime += deltaTime;
                _currState.normalizedTime = _progressTime / _currState.length;
                animation.Sample();

                if (_progressTime >= _currState.length)
                {
                    if (_currState.wrapMode != WrapMode.Loop)
                    {
                        isPlaying = false;
                    }
                    else
                    {
                        //Debug.Log(&quot;Loop anim, continue.&quot;);
                        _progressTime = 0.0f;
                    }
                }

                yield return new WaitForEndOfFrame();
            }
            yield return null;
        }
        else
        {
            animation.Play(clipName);
        }
    }
}
