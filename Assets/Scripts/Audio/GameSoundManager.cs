using UnityEngine;

//the purpose of this script is to have all the audioClips play from one centralized script.
//any scripts that need to play a sound will have a delegate event that this script will listen to
//this script will then play the specific sound for that event when the delegate is invoked
public class GameSoundManager : MonoBehaviour
{  
    private AudioManager audioManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = FindFirstObjectByType<AudioManager>();
        // subscribe to all events that need to be listened to in order to play sounds
        //level sounds
        GameLogic2.OnRespawnPlayer += RespawnSound;
        GameLogic2.OnPlayerWon += VictorySound;
        
        //frog sounds
        GameLogic2.OnPlayerTouchSpike += DeathSound;
        // .OnFrogEat += FrogEating;
        GameLogic2.OnHitWater += WaterSound;
        GameLogic2.OnPlayerCollisionSound += NormalCollisionSound;
        PlayerController.OnJump += JumpSound;
        GraplinMovement.OnGrapple += GrappleSound;
        GraplinMovement.OnGrappleHit += GrappleHitSound;
        frogCroaking.OnFrogCroaking += FrogCroakSound;
    }

    //below add all event methods that will play specific sounds
    private void NormalCollisionSound()
    {
        Debug.Log("Sound Played");
        audioManager.Play("frog_collision");
    }

    private void FrogCroakSound()
    {
        audioManager.Play("frog_croak");
    }
    
    //frog eating/collecting item not yet implemented, no event yet
    private void FrogEating()
    {
        Debug.Log("Sound Played");
        audioManager.Play("frog_eating");
    }
    private void WaterSound()
    {
        Debug.Log("Sound Played");
        audioManager.Play("frog_splash");
    }
    
    private void DeathSound()
    {
        Debug.Log("Sound Played");
        audioManager.Play("frog_death");
    }

    private void RespawnSound()
    {
        Debug.Log("Sound Played");
        audioManager.Play("frog_respawn");
    }

    private void JumpSound()
    {
        Debug.Log("Sound Played");
        audioManager.Play("frog_jump");
    }

    private void GrappleSound()
    {
        Debug.Log("Sound Played");
        audioManager.Play("frog_grapple");
    }

    private void GrappleHitSound()
    {
        Debug.Log("Sound Played");
        audioManager.Play("frog_grapple_hit");
    }
    
    private void VictorySound()
    {
        Debug.Log("Sound Played");
        audioManager.Play("victory");
    }



    private void OnDestroy()
    {
        //unsubscribe to all events on destroy
        GameLogic2.OnHitWater -= WaterSound;
        GameLogic2.OnPlayerWon -= VictorySound;
        GameLogic2.OnPlayerDead -= DeathSound;
        
    }
}
