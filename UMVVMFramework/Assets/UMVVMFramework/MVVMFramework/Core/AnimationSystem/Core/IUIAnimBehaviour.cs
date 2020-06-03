using System;

public interface IUIAnimBehaviour
{
    bool IsFinished { get; set; }
    bool IsPlaying { get; set; }

    void PlayIntro(Action callback);
    void PlayOutro(Action callback);
    void FinishSequence(Action callback);
    void ResetAnim();
}
