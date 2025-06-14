using Newtonsoft.Json;
using System;
using System.Text;
using UnityEngine;

public static class TokenDecoder
{
	public static JWTToken Decode(string token)
	{
		var parts = token.Split('.');
		if (parts.Length != 3)
		{
			Debug.LogError("Invalid JWT format.");
			return null;
		}

		var payload = parts[1];

		// Fix Base64 padding
		int padLength = (4 - (payload.Length % 4)) % 4;
		payload = payload.PadRight(payload.Length + padLength, '=');
		payload = payload.Replace('-', '+').Replace('_', '/');

		try
		{
			var payloadBytes = Convert.FromBase64String(payload);
			var payloadJson = Encoding.UTF8.GetString(payloadBytes);

			var tokenData = JsonConvert.DeserializeObject<JWTToken>(payloadJson);
			return tokenData;
		}
		catch (Exception e)
		{
			Debug.LogError("JWT decoding failed: " + e.Message);
			return null;
		}
	}

	[Serializable]
	public class JWTToken
	{
		[JsonProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")]
		public string NameIdentifier;

		[JsonProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")]
		public string Name;

		public string StudentId;


		public override string ToString()
		{
			return $"NameIdentifier: {NameIdentifier}, Name: {Name}, StudentId: {StudentId}";
		}

	}
}
