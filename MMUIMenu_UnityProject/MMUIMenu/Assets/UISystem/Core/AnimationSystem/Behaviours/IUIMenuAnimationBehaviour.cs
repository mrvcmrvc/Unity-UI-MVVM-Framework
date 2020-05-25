using System;

public interface IUIMenuAnimationBehaviour
{
    bool IsFinished { get; set; }
    bool IsPlaying { get; set; }

    void PlayIntro(Action callback);
    void PlayOutro(Action callback);
}
