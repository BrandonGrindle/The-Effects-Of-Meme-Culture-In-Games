using System;

public class CustomEvents
{
    public event Action onFishCollected;

    public void FishCollected()
    {
        if (onFishCollected != null)
        {
            onFishCollected();
        }
    }

    public event Action onSkeletonKilled;

    public void SkeletonKilled() 
    { 
        if (onSkeletonKilled != null)
        {
            onSkeletonKilled();
        }
    }

    public event Action onDance;

    public void Dance()
    {
        if (onDance != null)
        {
            onDance();
        }
    }
}
