using UnityEngine;
using System.Collections;
using System;

public class AnimationExtensions : MonoBehaviour {

    public void Play(Animation anim, string clipName, bool useTimeScale, Action onComplete)
    {
        StartCoroutine(PlayCoroutine(anim, clipName, false, onComplete));
    }
	 public IEnumerator PlayCoroutine( Animation animation, string clipName, bool useTimeScale, Action onComplete )
      {
          if(!useTimeScale)
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
            animation.Sample ();
  
            if (_progressTime >= _currState.length) 
            {
                if(_currState.wrapMode != WrapMode.Loop)
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
                if(onComplete != null)
                {
                onComplete();
                } 
            }
            else
            {
            animation.Play(clipName);
            }
      }
}
