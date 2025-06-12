using UnityEngine;

public static class SharedPrefManager
{

	public static void SaveData<T>(string key, T value)
	{
		if (value is string stringValue)
		{
			PlayerPrefs.SetString(key, stringValue);
		}
		else if (value is int intValue)
		{
			PlayerPrefs.SetInt(key, intValue);
		}
		else
		{
			Debug.LogError("Unsupported type for PlayerPrefs.");
			return;
		}
		PlayerPrefs.Save();
	}

	public static T GetData<T>(string key, T defaultValue = default)
	{
		object value;

		if (typeof(T) == typeof(string))
		{
			value = PlayerPrefs.GetString(key, defaultValue as string);
		}
		else if (typeof(T) == typeof(int))
		{
			value = PlayerPrefs.GetInt(key, defaultValue is int i ? i : 0);
		}
		else
		{
			Debug.LogError("Unsupported type for PlayerPrefs.");
			return default;
		}

		return (T)value;
	}



}
