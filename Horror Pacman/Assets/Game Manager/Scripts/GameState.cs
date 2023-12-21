using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * stores that houses the current state of the game.
 */

public class GameState : MonoBehaviour
{
    public enum PlayState { WON, LOST, PLAYING }
    public static PlayState currentPlayState = PlayState.PLAYING;
}
