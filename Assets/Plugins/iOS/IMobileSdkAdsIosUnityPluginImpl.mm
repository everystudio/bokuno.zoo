//
//  ImobileSdkAdsIosUnityPluginImpl.mm
//
//  Copyright 2014 i-mobile Co.Ltd. All rights reserved.
//

#import "ImobileSdkAdsIosUnityPluginImpl.h"

#ifdef __cplusplus
extern "C" {
#endif
    void UnitySendMessage(const char* obj, const char* method, const char* msg);
#ifdef __cplusplus
}
#endif


@interface ImobileSdkAdsIosUnityPluginImpl ()

@end


@implementation ImobileSdkAdsIosUnityPluginImpl

static const NSMutableSet *gameObjectNames = [NSMutableSet set];
static const NSMutableDictionary *adViewIdDictionary = [NSMutableDictionary dictionary];

extern UIViewController *UnityGetGLViewController();

// ----------------------------------------
#pragma mark - Call from inner C++
// ----------------------------------------

- (void)addObserver:(const char*)gameObjectName {
    [gameObjectNames addObject:[NSString stringWithUTF8String:gameObjectName]];
}

- (void)removeObserver:(const char*)gameObjectName {
    [gameObjectNames removeObject:[NSString stringWithUTF8String:gameObjectName]];
}

- (void)registerWithPublisherID:(const char*)publisherId MediaID:(const char*)mediaId SoptID:(const char*)soptId {
    [ImobileSdkAds registerWithPublisherID:[NSString stringWithUTF8String:publisherId]
                                   MediaID:[NSString stringWithUTF8String:mediaId]
                                    SpotID:[NSString stringWithUTF8String:soptId]];
    
    [ImobileSdkAds setSpotDelegate:[NSString stringWithUTF8String:soptId] delegate:self];
}

- (void)start {
    [ImobileSdkAds start];
}

- (void)stop {
    [ImobileSdkAds stop];
}

- (bool)startBySpotID:(const char*)spotId {
    return [ImobileSdkAds startBySpotID:[NSString stringWithUTF8String:spotId]];
}

- (bool)stopBySpotID:(const char*)spotId {
    return [ImobileSdkAds stopBySpotID:[NSString stringWithUTF8String:spotId]];
}

- (bool)showBySpotID:(const char*)spotId {
    return [ImobileSdkAds showBySpotID:[NSString stringWithUTF8String:spotId]];
}

- (bool)showWithParamStr:(const char*)paramStr {
    
    // parse paramStr
    NSArray *argument = [[NSString stringWithCString:paramStr encoding:NSUTF8StringEncoding] componentsSeparatedByString:@":"];
    NSString *publisherId = [argument objectAtIndex:0];
    NSString *mediaId = [argument objectAtIndex:1];
    NSString *spotId = [argument objectAtIndex:2];
    int left = [[argument objectAtIndex:3] intValue];
    int top = [[argument objectAtIndex:4] intValue];
    int width = [[argument objectAtIndex:5] intValue];
    int height = [[argument objectAtIndex:6] intValue];
    int iconViewLayoutWidth = [[argument objectAtIndex:7] intValue];
    int iconSize = [[argument objectAtIndex:8] intValue];
    bool iconTitleEnable = [[argument objectAtIndex:9] boolValue];
    int iconTitleFontSize = [[argument objectAtIndex:10] intValue];
    NSString *iconTitleFontColor = [argument objectAtIndex:11];
    int iconTitleOffset = [[argument objectAtIndex:12] intValue];
    bool iconTitleShadowEnable = [[argument objectAtIndex:13] boolValue];
    NSString *iconTitleShadowColor = [argument objectAtIndex:14];
    int iconTitleShadowDx = [[argument objectAtIndex:15] intValue];
    int iconTitleShadowDy = [[argument objectAtIndex:16] intValue];
    bool isIcon = [[argument objectAtIndex:17] boolValue];
    bool sizeAdjust = [[argument objectAtIndex:18] boolValue];
    int adViewId = [[argument objectAtIndex:19] intValue];
    
    // resister
    [self registerWithPublisherID:publisherId.UTF8String MediaID:mediaId.UTF8String SoptID:spotId.UTF8String];
    
    // start
    [self startBySpotID:spotId.UTF8String];
    
    // show
    UIView *adContainerView = [[UIView alloc] initWithFrame:CGRectMake(left, top, width, height)];
    [adViewIdDictionary setObject:adContainerView forKey:[NSString stringWithFormat:@"%d", adViewId]];
    [UnityGetGLViewController().view addSubview:adContainerView];
    
    return [ImobileSdkAds showBySpotID:spotId View:adContainerView SizeAdjust:sizeAdjust];
}

- (void)setAdOrientation:(ImobileSdkAdsAdOrientation)orientation {
    [ImobileSdkAds setAdOrientation:orientation];
}

- (void)setAdView:(int)adViewId visible:(bool)visible {
    UIView *adContainerView = [adViewIdDictionary objectForKey:[NSString stringWithFormat:@"%d", adViewId]];
    adContainerView.hidden = !visible;
}

- (void)setLegacyIosSdkMode:(bool)isLegacyMode {
    [ImobileSdkAds setLegacyIosSdkMode:isLegacyMode];
}

- (int)getScreenWidth:(bool)isPortrait {
    CGSize screenSize = UnityGetGLViewController().view.frame.size;
    if (isPortrait) {
        return MIN(screenSize.width, screenSize.height);
    } else {
        return MAX(screenSize.width, screenSize.height);
    }
}

- (int)getScreenHeight:(bool)isPortrait {
    CGSize screenSize = UnityGetGLViewController().view.frame.size;
    if (isPortrait) {
        return MAX(screenSize.width, screenSize.height);
    } else {
        return MIN(screenSize.width, screenSize.height);
    }
}

- (void)setTestMode:(bool)testMode {
    [ImobileSdkAds setTestMode:testMode];
}

// ----------------------------------------
#pragma mark Call from ImobileSdkAds
// ----------------------------------------

- (void)imobileSdkAdsSpot:(NSString *)spotId didReadyWithValue:(ImobileSdkAdsReadyResult)value {
    NSString *msg = [NSString stringWithFormat:@"%@", spotId];
    
    for (NSString *gameObjectName in gameObjectNames ) {
        UnitySendMessage([gameObjectName UTF8String],
                         [@"imobileSdkAdsSpotDidReady" UTF8String],
                         [msg UTF8String]);
    }
}

- (void)imobileSdkAdsSpot:(NSString *)spotId didFailWithValue:(ImobileSdkAdsFailResult)value {
    NSString *msg = [NSString stringWithFormat:@"%@,%d", spotId,value];
    
    for (NSString *gameObjectName in gameObjectNames ) {
        UnitySendMessage([gameObjectName UTF8String],
                         [@"imobileSdkAdsSpotDidFail" UTF8String],
                         [msg UTF8String]);
    }
}

- (void)imobileSdkAdsSpotIsNotReady:(NSString *)spotId {
    NSString *msg = [NSString stringWithFormat:@"%@", spotId];
    
    for (NSString *gameObjectName in gameObjectNames ) {
        UnitySendMessage([gameObjectName UTF8String],
                         [@"imobileSdkAdsSpotIsNotReady" UTF8String],
                         [msg UTF8String]);
    }
}

- (void)imobileSdkAdsSpotDidClick:(NSString *)spotId {
    NSString *msg = [NSString stringWithFormat:@"%@", spotId];
    
    for (NSString *gameObjectName in gameObjectNames ) {
        UnitySendMessage([gameObjectName UTF8String],
                         [@"imobileSdkAdsSpotDidClick" UTF8String],
                         [msg UTF8String]);
    }
}

- (void)imobileSdkAdsSpotDidClose:(NSString *)spotId {
    NSString *msg = [NSString stringWithFormat:@"%@", spotId];
    
    for (NSString *gameObjectName in gameObjectNames ) {
        UnitySendMessage([gameObjectName UTF8String],
                         [@"imobileSdkAdsSpotDidClose" UTF8String],
                         [msg UTF8String]);
    }
}

- (void)imobileSdkAdsSpotDidShow:(NSString *)spotId {
    NSString *msg = [NSString stringWithFormat:@"%@", spotId];
    
    for (NSString *gameObjectName in gameObjectNames ) {
        UnitySendMessage([gameObjectName UTF8String],
                         [@"imobileSdkAdsSpotDidShow" UTF8String],
                         [msg UTF8String]);
    }
}

// ----------------------------------------
#pragma mark - Call from Unity
// ----------------------------------------

#ifdef __cplusplus
extern "C" {
#endif
    static const ImobileSdkAdsIosUnityPluginImpl *unityPlugin = NULL;
    
    void imobileAddObserver_(const char* gameObjectName) {
        if (!unityPlugin) {
            unityPlugin = [[ImobileSdkAdsIosUnityPluginImpl alloc] init];
        }
        [unityPlugin addObserver:gameObjectName];
    }
    
    void imobileRemoveObserver_(const char* gameObjectName) {
        if (!unityPlugin) {
            unityPlugin = [[ImobileSdkAdsIosUnityPluginImpl alloc] init];
        }
        [unityPlugin removeObserver:gameObjectName];
    }
    
    void imobileRegisterWithPublisherID_(const char* publisherId, const char* mediaId, const char* soptId) {
        if (!unityPlugin) {
            unityPlugin = [[ImobileSdkAdsIosUnityPluginImpl alloc] init];
        }
        [unityPlugin registerWithPublisherID:publisherId
                                     MediaID:mediaId
                                      SoptID:soptId];
    }
    
    void imobileStart_() {
        if (!unityPlugin) {
            unityPlugin = [[ImobileSdkAdsIosUnityPluginImpl alloc] init];
        }
        [unityPlugin start];
    }
    
    void imobileStop_() {
        if (!unityPlugin) {
            unityPlugin = [[ImobileSdkAdsIosUnityPluginImpl alloc] init];
        }
        [unityPlugin stop];
    }
    
    bool imobileStartBySpotID_(const char* spotId){
        if (!unityPlugin) {
            unityPlugin = [[ImobileSdkAdsIosUnityPluginImpl alloc] init];
        }
        return [unityPlugin startBySpotID:spotId];
    }
    
    bool imobileStopBySpotID_(const char* spotId) {
        if (!unityPlugin) {
            unityPlugin = [[ImobileSdkAdsIosUnityPluginImpl alloc] init];
        }
        return [unityPlugin stopBySpotID:spotId];
    }
    
    bool imobileShowBySpotID_(const char* spotId) {
        if (!unityPlugin) {
            unityPlugin = [[ImobileSdkAdsIosUnityPluginImpl alloc] init];
        }
        return [unityPlugin showBySpotID:spotId];
    }
    
    bool imobileShowBySpotIDWithPositionAndIconParams_(const char* paramStr)
    {
        if (!unityPlugin) {
            unityPlugin = [[ImobileSdkAdsIosUnityPluginImpl alloc] init];
        }
        return [unityPlugin showWithParamStr:paramStr];
    }
    
    void imobileSetAdOrientation_(ImobileSdkAdsAdOrientation orientation) {
        if (!unityPlugin) {
            unityPlugin = [[ImobileSdkAdsIosUnityPluginImpl alloc] init];
        }
        [unityPlugin setAdOrientation:orientation];
    }
    
    void imobileSetVisibility_(int adViewId, bool visible) {
        if (!unityPlugin) {
            unityPlugin = [[ImobileSdkAdsIosUnityPluginImpl alloc] init];
        }
        [unityPlugin setAdView:adViewId visible:visible];
    }
    
    void imobileSetLegacyIosSdkMode_(bool isLegacyMode) {
        if (!unityPlugin) {
            unityPlugin = [[ImobileSdkAdsIosUnityPluginImpl alloc] init];
        }
        [unityPlugin setLegacyIosSdkMode:isLegacyMode];
    }
    
    int getScreenWidth_(bool isPortrait) {
        if (!unityPlugin) {
            unityPlugin = [[ImobileSdkAdsIosUnityPluginImpl alloc] init];
        }
        return [unityPlugin getScreenWidth:isPortrait];
    }
    
    int getScreenHeight_(bool isPortrait) {
        if (!unityPlugin) {
            unityPlugin = [[ImobileSdkAdsIosUnityPluginImpl alloc] init];
        }
        return [unityPlugin getScreenHeight:isPortrait];
    }
    
    void setTestMode_(bool testMode) {
        if (!unityPlugin) {
            unityPlugin = [[ImobileSdkAdsIosUnityPluginImpl alloc] init];
        }
        return [unityPlugin setTestMode:testMode];
    }
    
#ifdef __cplusplus
}
#endif

@end