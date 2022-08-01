using UnityEngine;


public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private AudioSource combatSwingSound;
    [SerializeField] private AudioSource playerHurtSound;
    [SerializeField] private AudioSource clickSound;

    public void PlayCombatSound()
    {
        if (combatSwingSound == null) return;

        combatSwingSound.Stop();
        combatSwingSound.Play();
    }

    public void PlayPlayerHurtSound()
    {
        if (playerHurtSound == null) return;

        playerHurtSound.Stop();
        playerHurtSound.Play();
    }

    public void PlayClickSound()
    {
        if (clickSound == null) return;

        clickSound.Stop();
        clickSound.Play();
    }
}