﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Runtime.CompilerServices;
using Avalonia.Markup.Xaml;
using Avalonia.Utilities;

namespace HolyClient.Localization;

public class TrViewModel : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return new TrViewModelData();
    }
}

public class TrViewModelData : DynamicObject, INotifyPropertyChanged
{
    private readonly List<string> textIdsList = new();

    public TrViewModelData()
    {
        WeakEventHandlerManager.Subscribe<Loc, CurrentLanguageChangedEventArgs, TrData>(Loc.Instance,
            nameof(Loc.CurrentLanguageChanged), CurrentLanguageChanged);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    ~TrViewModelData()
    {
        WeakEventHandlerManager.Unsubscribe<CurrentLanguageChangedEventArgs, TrData>(Loc.Instance,
            nameof(Loc.CurrentLanguageChanged), CurrentLanguageChanged);
    }

    private void CurrentLanguageChanged(object sender, CurrentLanguageChangedEventArgs args)
    {
        textIdsList.ForEach(NotifyPropertyChanged);
    }

    public virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public override bool TryGetMember(GetMemberBinder binder, out object result)
    {
        if (!textIdsList.Contains(binder.Name))
            textIdsList.Add(binder.Name);

        result = Loc.Tr(binder.Name);

        return true;
    }
}