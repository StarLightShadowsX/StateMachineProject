﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class _ExtensionMethods
{
	public static bool Toggle(this ref bool boolean) => boolean = !boolean;
	public static bool IsTrue(this bool boolean) => boolean == true;
	public static bool IsFalse(this bool boolean) => boolean == false;
	public static bool True(this ref bool boolean) => boolean = true;
	public static bool False(this ref bool boolean) => boolean = false;

	public static int Int(this bool boolean) => boolean ? 1 : 0;
	public static bool Bool(this int integral) => integral > 0;

	public static Color SetRed(this ref Color color, float set) => new Color(set, color.g, color.b, color.a);
	public static Color ChangeRed(this ref Color color, float change) => new Color(color.r + change, color.g, color.b, color.a);
	public static Color SetBlue(this ref Color color, float set) => new Color(color.r, set, color.b, color.a);
	public static Color ChangeBlue(this ref Color color, float change) => new Color(color.r, color.g + change,  color.b, color.a);
	public static Color SetGreen(this ref Color color, float set) => new Color(color.r, color.g, set, color.a);
	public static Color ChangeGreen(this ref Color color, float change) => new Color(color.r, color.g, color.b + change, color.a);
	public static Color SetAlpha(this ref Color color, float set) => new Color(color.r, color.g, color.b, set);
	public static Color ChangeAlpha(this ref Color color, float change) => new Color(color.r, color.g, color.b, color.a + change);


}

public static class _EasierMathExtensions
{
	public static float P(this float F) => Mathf.Pow(F, 2);
	public static float P(this float F, int power) => Mathf.Pow(F, power);
	public static float SQRT(this float F) => Mathf.Sqrt(F);
	public static float Sin(this float F) => Mathf.Sin(F);
	public static float Cos(this float F) => Mathf.Cos(F);
	public static float Tan(this float F) => Mathf.Tan(F);
	public static float ASin(this float F) => Mathf.Asin(F);
	public static float ACos(this float F) => Mathf.Acos(F);
	public static float ATan(this float F) => Mathf.Atan(F);

	public static float Clamp(this float value, float min, float max) => (value < min) ? min : (value > max) ? max : value;
	public static float Min(this float value, float min) => (value < min) ? min : value;
	public static float Max(this float value, float max) => (value > max) ? max : value;

	public static int Int(this float value) => (int)value;
	public static float Float(this int value) => (float)value;
	public static int Floor(this float value) => Mathf.FloorToInt(value);
	public static int Ceil(this float value) => Mathf.CeilToInt(value);

	public static int Sign(this float value) => (int)Mathf.Sign(value);
	public static float Abs(this float value) => Mathf.Abs(value);
	public static float Repeat(this float value, float length) => Mathf.Repeat(value, length);


	public static float Randomize(this float value, float min, float max) { value = UnityEngine.Random.Range(min, max); return value; }
	public static int Randomize(this int value, int min, int max) { value = UnityEngine.Random.Range(min, max); return value; }

	public static float RandomTo(this float value, float min = 0) => UnityEngine.Random.Range(min, value);
	public static int RandomTo(this int value, int min = 0) => UnityEngine.Random.Range(min, value);

	public static float RandomBetween(this Vector2 input) => UnityEngine.Random.Range(input.x, input.y);
	public static int RandomBetween(this Vector2Int input) => UnityEngine.Random.Range(input.x, input.y);

	public static bool RandomChance(this float input) => UnityEngine.Random.Range(0f, 1f) >= input;

    public static float MoveTowards(this float current, float rate, float target)
    {
		return current == target
			? target
			: target > current
				? current + rate >= target
					? target
					: current + rate
				: current - rate <= target
						? target
						: current - rate;
    }
    public static float MoveUp(this float current, float amount, float limit)
    {
        return current == limit
            ? limit
            : current < limit
				? limit - current <= amount 
					? limit 
					: current + amount
			: throw new System.Exception("You are trying to move a float upwards to something below it.");
    }
    public static float MoveDown(this float current, float amount, float limit)
    {
        return current == limit
            ? limit
            : current > limit
				? current - limit <= amount 
					? limit 
					: current - amount
			: throw new System.Exception("You are trying to move a float downwards to something above it.");
    }

    public static float Recast(this float input, float fromA, float fromB, float toA, float toB) => Mathf.Lerp(toA, toB, Mathf.InverseLerp(fromA, fromB, input));
    public static float Recast(this float input, Vector2 from, Vector2 to) => Mathf.Lerp(to.x, to.y, Mathf.InverseLerp(from.x, from.y, input));
    public static float Recast(this float input, float fromA, float fromB, AnimationCurve to) => to.Evaluate(Mathf.InverseLerp(fromA, fromB, input));

}

public static class _MonoBehaviorHelpers
{
	public static void LateAwake(this MonoBehaviour m, BasicDelegate result) => m.StartCoroutine(LateWakeENUM(result));
	
	static IEnumerator LateWakeENUM(BasicDelegate result)
	{
		yield return new WaitForEndOfFrame();
		result();
	}

	public static bool Unloading(this MonoBehaviour m) => m.gameObject.scene.isLoaded;

	public static void SafeDestroyers(this MonoBehaviour m, BasicDelegate SafeDestroy, BasicDelegate UnloadDestroy)
	{
		if (!m.gameObject.scene.isLoaded) SafeDestroy();
		else UnloadDestroy();
	}

	public static void Set(this Transform T, Vector3? pos = null, Vector3? rot = null, Vector3? scale = null, Transform parent = null)
	{
		if(pos != null) T.localPosition = pos.Value;
		if(rot != null) T.localEulerAngles = rot.Value;
		if(scale != null) T.localPosition = scale.Value;
		if (parent != null) T.parent = parent;
	}

	public static GameObject NewGameObject(this Object O, string name = "NewGameObject", Vector3? pos = null, Quaternion? rot = null, Vector3? scale = null, Transform parent = null, params System.Type[] additions)
	{
		GameObject result = new(name, additions);

		if (parent != null) result.transform.parent = parent;
		if (pos != null) result.transform.localPosition = pos.Value;
		if (rot != null) result.transform.localRotation = rot.Value;
		if (scale != null) result.transform.localPosition = scale.Value;

		return result;
	}

	public static void Reset(this Transform transform, bool position = true, bool rotation = true, bool scale = true)
	{
		if (position) transform.localPosition = Vector3.zero;
		if (rotation) transform.localRotation = Quaternion.identity;
		if (scale) transform.localScale = Vector3.one;
	}

	public static T Random<T>(this T[] array) => array[UnityEngine.Random.Range(0, array.Length)];
	public static T Random<T>(this List<T> array) => array[UnityEngine.Random.Range(0, array.Count)];
	public static void RemoveAtLast<T>(this List<T> array, int i = 1) => array.Remove(array[^i]);

	public static T GetOrAddComponent<T>(this Component O) where T : Component
	{
		O.gameObject.TryGetComponent(out T V);
		V ??= O.gameObject.AddComponent<T>();
		return V;
	}
	public static T GetOrAddComponent<T>(this GameObject O) where T : Component
	{
		O.TryGetComponent(out T V);
		V ??= O.AddComponent<T>();
		return V;
	}

    public static void SetPositionAndRotation(this Transform target, Transform influence) => target.SetPositionAndRotation(influence.position, influence.rotation);

    public static List<T> GetComponentsRecursive<T>(this GameObject gameObject) where T : Component
    {
        int length = gameObject.transform.childCount;
        List<T> components = new List<T>(length + 1);
        T comp = gameObject.transform.GetComponent<T>();
        if (comp != null) components.Add(comp);
        for (int i = 0; i < length; i++)
        {
            comp = gameObject.transform.GetChild(i).GetComponent<T>();
            if (comp != null) components.Add(comp);
        }
        return components;
    }

    public static List<T> GetComponentsInDirectChildren<T>(this GameObject gameObject) where T : Component
    {
        int length = gameObject.transform.childCount;
        List<T> components = new List<T>(length);
        for (int i = 0; i < length; i++)
        {
            T comp = gameObject.transform.GetChild(i).GetComponent<T>();
            if (comp != null) components.Add(comp);
        }
        return components;
    }
}

public delegate void BasicDelegate();