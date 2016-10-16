using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {


    [SerializeField]
    RectTransform healthBarFill;
    [SerializeField]
    RectTransform staminaBarFill;
    [SerializeField]
    private PlayerController controller;
 //   private Player player;
    public void SetController(PlayerController controller)
    {
        this.controller = controller;
    }
  /*  public void SetPlayer(Player player)
    {
        this.player = player;
    }
*/


    void Update()
    {
        SetStaminaBar(controller.StaminaAmount / controller.MaxStamina);
       // SetHealthBar(player.CurrentHealth / (float)player.MaxHealth);

    }

    void SetHealthBar(float percentage)
    {
        healthBarFill.GetComponent<Image>().fillAmount = percentage;
    }
    void SetStaminaBar(float percentage)
    {
        staminaBarFill.GetComponent<Image>().fillAmount = percentage;
    }


}
