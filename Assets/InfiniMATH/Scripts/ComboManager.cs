using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Ververg
{
    public class ComboManager : Singleton<ComboManager>
    {

        public float comboTime = 1.0f;
        public Image comboBar;
        public Text comboText;
        public AudioClip comboClip;
        private Animator comboAnim;
        private AudioSource audioSource;

        private float lastTime;
        private int comboMultiplier = 0;
        private int BestCombo = 0;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            lastTime = Time.time;
            comboAnim = comboText.GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = comboClip;
        }

        void Update()
        {
            if(LevelManager.instance.isBuildingLevel())
            {
                lastTime = Time.time;
            }

            if (Time.time - lastTime < comboTime)
            {
                comboBar.fillAmount = 1f - (Time.time - lastTime) / comboTime;
            }
        }

        public void SetLastTime(float time)
        {
            lastTime = time;
        }

        public void ComboCheck()
        {
            if (Time.time - lastTime < comboTime)
            {
                comboMultiplier++;
                lastTime = Time.time;

                if(comboMultiplier > 1)
                {
                    comboText.text = "Combo x" + comboMultiplier;
                    comboAnim.SetTrigger("Combo");
                    audioSource.Play();
                }

                if(comboMultiplier > BestCombo)
                {
                    BestCombo = comboMultiplier;
                }

                if(BestCombo > StatsManager.Instance.GetStats(StatsType.BestCombo))
                {
                    StatsManager.Instance.SaveStats(StatsType.BestCombo, BestCombo);
                }
            }
            else
            {
                comboMultiplier = 1;
                lastTime = Time.time;
            }
        }

        public void SetCombo(int amount)
        {
            comboMultiplier = amount;
        }

        public void ResetCombo() {
            comboMultiplier = 0;
            BestCombo = 0;
        }

        public int GetBestCombo()
        {
            return BestCombo;
        }
    }
}