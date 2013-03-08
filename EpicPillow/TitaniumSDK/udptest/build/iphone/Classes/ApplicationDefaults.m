/**
 * Appcelerator Titanium Mobile
 * Copyright (c) 2009-2013 by Appcelerator, Inc. All Rights Reserved.
 * Licensed under the terms of the Apache Public License
 * Please see the LICENSE included with this distribution for details.
 *
 * WARNING: This is generated code. Do not modify. Your changes *will* be lost.
 */

#import <Foundation/Foundation.h>
#import "TiUtils.h"
#import "ApplicationDefaults.h"

@implementation ApplicationDefaults

+ (NSMutableDictionary*) copyDefaults
{
	NSMutableDictionary * _property = [[NSMutableDictionary alloc] init];
	
	[_property setObject:[TiUtils stringValue:@"d4OCLSl9Unh3OafpGSiE6fAPw9I3q74q"] forKey:@"acs-oauth-secret-production"];
	[_property setObject:[TiUtils stringValue:@"1vRDhfY8Onjowt9GOL9pBUxepfJgW2Mo"] forKey:@"acs-oauth-key-production"];
	[_property setObject:[TiUtils stringValue:@"AZIOv5fLHIVY9fZO9IfxyQTQ9lXpHnRW"] forKey:@"acs-api-key-production"];
	[_property setObject:[TiUtils stringValue:@"LKrkw5XpedVuLqVPWaf4b2Dqx164hSUa"] forKey:@"acs-oauth-secret-development"];
	[_property setObject:[TiUtils stringValue:@"NDrjAjc8juRG1kJJ3lNroXJlamFOB11R"] forKey:@"acs-oauth-key-development"];
	[_property setObject:[TiUtils stringValue:@"1yhOJAmMrMRKRfpZDnUqyVrbqxsP7KfS"] forKey:@"acs-api-key-development"];
	[_property setObject:[TiUtils stringValue:@"system"] forKey:@"ti.ui.defaultunit"];
	return _property;
}

@end