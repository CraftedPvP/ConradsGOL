using UnityEngine;

namespace Michael
{
    public class UIManager : Singleton<UIManager>
    {
        AudioSource clickSound;
        public override void Awake()
        {
            base.Awake();
            clickSound = GetComponent<AudioSource>();
        }
        public void PlayClickSound()
        {
            clickSound.Play();
        }
    }
}