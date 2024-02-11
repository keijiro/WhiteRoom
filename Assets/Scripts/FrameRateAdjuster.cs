using UnityEngine;
using UnityEngine.LowLevel;
using System.Linq;

#if UNITY_EDITOR

[UnityEditor.InitializeOnLoad]
sealed class FrameRateAdjuster
{
    static FrameRateAdjuster()
      => InsertPlayerLoopSystem();

    static void UpdateTargetFrameRate()
      => Application.targetFrameRate = Time.captureDeltaTime != 0 ? -1 : 30;

    static void InsertPlayerLoopSystem()
    {
        var system = new PlayerLoopSystem()
          { type = typeof(FrameRateAdjuster),
            updateDelegate = UpdateTargetFrameRate };

        var playerLoop = PlayerLoop.GetCurrentPlayerLoop();

        for (var i = 0; i < playerLoop.subSystemList.Length; i++)
        {
            ref var phase = ref playerLoop.subSystemList[i];
            if (phase.type == typeof(UnityEngine.PlayerLoop.EarlyUpdate))
            {
                phase.subSystemList =
                  phase.subSystemList.Concat(new[]{system}).ToArray();
                break;
            }
        }

        PlayerLoop.SetPlayerLoop(playerLoop);
    }
}

#endif
