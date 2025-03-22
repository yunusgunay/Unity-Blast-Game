using DG.Tweening;
using UnityEngine;

// Shake Animation
public static class ShakeAnimation {
    public static Tween ApplyShakeAnimation(Transform target, float shakeDuration, float shakeStrength){
        return target.DOShakePosition(shakeDuration, shakeStrength);
    }
}