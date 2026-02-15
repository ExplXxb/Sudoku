//
//  LPMInitializer.h
//  iOSBridge
//
// Copyright © 2024 IronSource. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <IronSource/LPMInitRequestBuilder.h>
#import <IronSource/LPMImpressionDataDelegate.h>
#import <IronSource/LevelPlay.h>
#import <IronSource/LPMSegment.h>
#import <IronSource/ISConfigurations.h>

typedef void (*InitSuccessCallback)(const char *config);
typedef void (*InitFailureCallback)(const char *error);
typedef void (*ImpressionSuccessCallback)(const char *impressionData);

@interface LPMInitializer : NSObject <LPMImpressionDataDelegate>
@property (assign) InitSuccessCallback initDidSucceedCallback;
@property (assign) InitFailureCallback initDidFailCallback;
@property (assign) ImpressionSuccessCallback impressionDidSucceedCallback;
+ (instancetype)sharedInstance;
- (void)LPMInitialize:(NSString *)appKey userId:(NSString *)userId successCallback:(InitSuccessCallback)successCallback failureCallback:(InitFailureCallback)failureCallback impressionSuccessCallback:(ImpressionSuccessCallback)impressionSuccessCallback;
- (void)setPluginData:(NSString *)pluginType pluginVersion:(NSString *)pluginVersion pluginFrameworkVersion:(NSString *)pluginFrameworkVersion;
- (void)LPMSetPauseGame:(BOOL) pauseGame;
- (BOOL)isUnityPauseGame;
@end
