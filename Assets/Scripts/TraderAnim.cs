using DG.Tweening;
using System.Collections;
using UnityEngine;

public class TraderAnim : MonoBehaviour
{
    public ParticleSystem ParticleSystem;
    public LoopType LoopType;
    public float size;
    public Ease Ease;
    private void Start()
    {
        transform.DOScaleY(size,0.5f).SetEase(Ease).SetLoops(-1,LoopType);
    }

    public void StartParticle()
    {
        ParticleSystem.Play();
    }
    public void StopParticle()
    {
        ParticleSystem.Stop();
    }

}
