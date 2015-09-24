using UnityEngine;
using System.Collections;
using System;

public class GameSettings : MonoBehaviour  {

    [Serializable]
    public class Player
    {
        public float initialAacceleration = 0.2f;
        public float initialDeceleration = 1.2f;
        public float jumpDeceleration = 0.6f;
      //  public float maxSpeed = 2.2f;
        public float speedJump = 1;
    }

    public Player player;
    public float LaneSeparation = 2.1f;

    public int stars_for_tournament_2;
    public int stars_for_tournament_3;
    public int stars_for_tournament_4;
}
