package com.ironsource.unity.androidbridge;

import com.unity3d.mediation.LevelPlayAdSize;
import com.unity3d.player.UnityPlayer;

public class BannerUtils {
   public static LevelPlayAdSize getAdaptiveAdSize(int customWidth) {
      return LevelPlayAdSize.createAdaptiveAdSize(UnityPlayer.currentActivity, customWidth);
   }
}
