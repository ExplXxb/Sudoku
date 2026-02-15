//
//  LPMRewardedAd.h
//  iOSBridge
//

#import <Foundation/Foundation.h>

#ifdef __cplusplus
extern "C" {
#endif

void *LPMRewardedAdCreateWithConfig(const char *adUnitId, void *configRef);
void LPMRewardedAdSetDelegate(void *rewardedAdRef, void *delegateRef);

void LPMRewardedAdLoadAd(void *rewardedAdRef);
void LPMRewardedAdShowAd(void *rewardedAdRef, const char *placementName);
bool LPMRewardedAdIsAdReady(void *rewardedAdRef);

bool LPMRewardedAdIsPlacementCapped(const char *placementName);

const char *LPMRewardedAdAdId(void *rewardedAdRef);

void *LPMRewardedAdGetReward(void *rewardedAdRef, const char *placement);
const char *LPMRewardedAdRewardGetName(void *rewardRef);
int LPMRewardedAdRewardGetAmount(void *rewardRef);

// Config
void *LPMRewardedAdCreateConfigBuilder();
void LPMRewardedAdConfigBuilderSetBidFloor(void *builderRef, double bidFloor);
void *LPMRewardedAdConfigBuilderBuild(void *builderRef);

#ifdef __cplusplus
}
#endif
