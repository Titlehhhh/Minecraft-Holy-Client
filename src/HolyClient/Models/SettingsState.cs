﻿using HolyClient.Localization;
using MessagePack;

namespace HolyClient.Models;

[MessagePackObject(true)]
public class SettingsState
{
    public string Language { get; set; } = Loc.Instance.CurrentLanguage;
}