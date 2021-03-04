using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StaminaBar : MonoBehaviour
{
    public Slider staminaBar;

    private double maxStamina = 100; // Default Value for maximum stamina
    private double currentStamina;

    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine regen;

    public static StaminaBar instance;

    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start() {
        currentStamina = maxStamina;
        staminaBar.maxValue = (float)maxStamina;
        staminaBar.value = (float)maxStamina;
    }

    public void UseStamina(double amount) {
        if (currentStamina - amount >= 0) {
            currentStamina -= amount;
            staminaBar.value = (float)currentStamina;

            if (regen != null) {
                StopCoroutine(regen);
            }

            regen = StartCoroutine(RegenStamina());
        } else {
            Debug.Log("Not enought stamina");
        }
    }

    public bool HaveStamina() {
        if (currentStamina > 0.1) {
            return true;
        }
        return false;
    }

    private IEnumerator RegenStamina() {
        yield return new WaitForSeconds(3);

        while (currentStamina < maxStamina) {
            currentStamina += maxStamina / 100;
            staminaBar.value = (float)currentStamina;
            yield return regenTick;
        }
        regen = null;
    }

    // Update is called once per frame
    void Update() {
        
    }
}
