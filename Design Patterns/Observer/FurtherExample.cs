using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Just example, param sent with event can be a struct, class or anything
 */
//public class PlayerHpMessage
//{
//	public float remainHP;
//}

//public class Player : MonoBehaviour
//{
//	void OnHpChanged()
//	{
//		var message = new PlayerHpMessage();
//		message.remainHP = GetRemainHP();
//		// the PlayerHpMessage will be boxed, become object
//		this.PostEvent(EventID.PlayerHPChanged, message);
//	}
//}

//public class UIDipslayer : MonoBehaviour
//{
//	void OnPlayerHpChanged(object para)
//	{
//		// unbox message from object
//		if (para is PlayerHpMessage)
//		{
//			var message = para as PlayerHpMessage;
//			// ..... bla..bla..
//		}
//	}
//}
