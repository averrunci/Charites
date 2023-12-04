﻿// Copyright (C) 2022-2023 Fievus
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.
namespace Charites.Windows.Mvc;

/// <summary>
/// Specifies the parameter of an event handler to inject a value.
/// </summary>
[AttributeUsage(AttributeTargets.Parameter)]
public class EventHandlerParameterAttribute : Attribute;