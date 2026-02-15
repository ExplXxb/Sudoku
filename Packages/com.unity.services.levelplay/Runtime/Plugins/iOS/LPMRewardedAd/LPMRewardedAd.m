//
//  LPMRewardedAd.m
//  iOSBridge
//

#import "LPMRewardedAd.h"
#import "LPMRewardedAdCallbacksWrapper.h"
#import "LPMUtilities.h"
#import <IronSource/LPMRewardedAd.h>
#import <IronSource/LPMRewardedAdConfig.h>
#import <IronSource/LPMRewardedAdConfigBuilder.h>
#import <IronSource/LPMReward.h>
#import <UIKit/UIKit.h>

#ifdef __cplusplus
extern "C" {
#endif

    void *LPMRewardedAdCreateWithConfig(const char *adUnitId, void *configRef) {
        NSString *adUnitIdStr = [LPMUtilities getStringFromCString:adUnitId];
        LPMRewardedAdConfig *config = (__bridge LPMRewardedAdConfig *)configRef;
        LPMRewardedAd *rewardedAd = [[LPMRewardedAd alloc] initWithAdUnitId:adUnitIdStr config:config];

        return (__bridge_retained void *)rewardedAd;
    }

    void LPMRewardedAdSetDelegate(void *rewardedAdRef, void *delegateRef) {
        LPMRewardedAd *rewardedAd = (__bridge LPMRewardedAd *)rewardedAdRef;
        LPMRewardedAdCallbacksWrapper *delegate = (__bridge LPMRewardedAdCallbacksWrapper *)delegateRef;
        [rewardedAd setDelegate:delegate];
    }

    void LPMRewardedAdLoadAd(void *rewardedAdRef) {
        LPMRewardedAd *rewardedAd = (__bridge LPMRewardedAd *)rewardedAdRef;
        [rewardedAd loadAd];
    }

    void LPMRewardedAdShowAd(void *rewardedAdRef, const char *placementName) {
        LPMRewardedAd *rewardedAd = (__bridge LPMRewardedAd *)rewardedAdRef;
        NSString *placementNameString = placementName ? [LPMUtilities getStringFromCString:placementName] : nil;
        [rewardedAd showAdWithViewController:[UIApplication sharedApplication].keyWindow.rootViewController placementName:placementNameString];
    }

    bool LPMRewardedAdIsAdReady(void *rewardedAdRef) {
        LPMRewardedAd *rewardedAd = (__bridge LPMRewardedAd *)rewardedAdRef;
        return [rewardedAd isAdReady];
    }

    bool LPMRewardedAdIsPlacementCapped(const char *placementName) {
        return [LPMRewardedAd isPlacementCapped:[LPMUtilities getStringFromCString:placementName]];
    }

    const char *LPMRewardedAdAdId(void *rewardedAdRef) {
        LPMRewardedAd *rewardedAd = (__bridge LPMRewardedAd *)rewardedAdRef;
        return strdup([[rewardedAd adId] UTF8String]);
    }

    void *LPMRewardedAdGetReward(void *rewardedAdRef, const char *placement) {
        if (rewardedAdRef == NULL) {
            return NULL;
        }
        LPMRewardedAd *rewardedAd = (__bridge LPMRewardedAd *)rewardedAdRef;
        NSString *placementString = placement ? [LPMUtilities getStringFromCString:placement] : nil;
        LPMReward *reward = [rewardedAd getRewardWithPlacementName:placementString];
        if (reward == nil) {
            return NULL;
        }
        return (__bridge void *)reward;
    }

    const char *LPMRewardedAdRewardGetName(void *rewardRef) {
        if (rewardRef == NULL) {
            return NULL;
        }
        LPMReward *reward = (__bridge LPMReward *)rewardRef;
        NSString *name = [reward name];
        if (name == nil) {
            return NULL;
        }
        return strdup([name UTF8String]);
    }

    int LPMRewardedAdRewardGetAmount(void *rewardRef) {
        if (rewardRef == NULL) {
            return 0;
        }
        LPMReward *reward = (__bridge LPMReward *)rewardRef;
        return (int)[reward amount];
    }

    // config
    void *LPMRewardedAdCreateConfigBuilder() {
        LPMRewardedAdConfigBuilder *builder = [[LPMRewardedAdConfigBuilder alloc] init];

        return (__bridge_retained void *)builder;
    }

    void LPMRewardedAdConfigBuilderSetBidFloor(void *builderRef, double bidFloor) {
        LPMRewardedAdConfigBuilder *builder = (__bridge LPMRewardedAdConfigBuilder *)builderRef;
        NSNumber *bidFloorNum = [NSNumber numberWithDouble:bidFloor];
        [builder setWithBidFloor:bidFloorNum];
    }

    void *LPMRewardedAdConfigBuilderBuild(void *builderRef) {
        LPMRewardedAdConfigBuilder *builder = (__bridge LPMRewardedAdConfigBuilder *)builderRef;
        LPMRewardedAdConfig *config = [builder build];

        return (__bridge_retained void *)config;
    }

#ifdef __cplusplus
}
#endif
