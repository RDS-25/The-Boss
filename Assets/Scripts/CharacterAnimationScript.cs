using UnityEngine;

public class CharacterAnimationScript : MonoBehaviour
{
    public Collider WeaponCoilder;

    public void EnableCoilder(){
        WeaponCoilder.enabled = true;
    }

    public void DisableCoilder(){
        WeaponCoilder.enabled = false;
    }


}
