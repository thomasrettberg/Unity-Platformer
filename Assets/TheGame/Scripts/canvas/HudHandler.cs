using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudHandler : MonoBehaviour {

    [SerializeField] Image healthBar;

	private void Update () {
        PlayerBehaviour player = FindObjectOfType<PlayerBehaviour>();
        if (player != null)
        {
            healthBar.fillAmount = player.GetHealth();
        }
	}
}
