using UnityEngine;

namespace ShadcnUI.Core.Animation;

/// <summary>
/// Simple animation engine for smooth transitions.
/// </summary>
public sealed class AnimationEngine : IAnimationEngine
{
    public float DeltaTime => Time.deltaTime;

    public void Animate(ref float value, float target, float speed)
    {
        value = Mathf.MoveTowards(value, target, speed * DeltaTime);
    }

    public float SmoothStep(float t)
    {
        return Mathf.SmoothStep(0f, 1f, Mathf.Clamp01(t));
    }

    public float Lerp(float a, float b, float t)
    {
        return Mathf.Lerp(a, b, Mathf.Clamp01(t));
    }

    public Color LerpColor(Color a, Color b, float t)
    {
        return Color.Lerp(a, b, Mathf.Clamp01(t));
    }

    public Vector2 LerpVector(Vector2 a, Vector2 b, float t)
    {
        return Vector2.Lerp(a, b, Mathf.Clamp01(t));
    }

    public float Spring(float target, float current, float velocity, ref float currentVelocity, float strength, float damping)
    {
        var force = (target - current) * strength;
        currentVelocity += force * DeltaTime;
        currentVelocity *= Mathf.Max(0f, 1f - damping * DeltaTime);
        return current + currentVelocity * DeltaTime;
    }

    public float EaseOutCubic(float t)
    {
        var tt = Mathf.Clamp01(t);
        return 1f - Mathf.Pow(1f - tt, 3f);
    }

    public float EaseInOutCubic(float t)
    {
        var tt = Mathf.Clamp01(t);
        return tt < 0.5f ? 4f * tt * tt * tt : 1f - Mathf.Pow(-2f * tt + 2f, 3f) / 2f;
    }
}
