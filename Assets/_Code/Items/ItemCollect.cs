using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollect : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float dissolveAmount;
    public bool collisionPlayerCar;
    public IEnumerator DissolveSprite(bool _status)
    {
        collisionPlayerCar = _status;
        yield return null;
        while(collisionPlayerCar)
        {
            yield return new WaitForSeconds(0.001f);
            if (dissolveAmount < 1)
            {
                dissolveAmount = Mathf.Clamp01(dissolveAmount + Time.deltaTime);
                spriteRenderer.material.SetFloat("_DissolveAmount", dissolveAmount);
            }
            else
            {
                collisionPlayerCar = false;
                this.gameObject.SetActive(false);
            }
        }
    }
}
