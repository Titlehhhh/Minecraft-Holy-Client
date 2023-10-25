using DynamicData;
using HolyClient.Abstractions.StressTest;
using HolyClient.SDK.Attributes;
using System;
using System.Reflection;

namespace HolyClient.Contracts.Services;

public class StressTestBehaviorInfo
{
	public string? Title { get; }
	public string? Description { get; }
	public string? Author { get; }

	private Type type;

	public StressTestBehaviorInfo(Type type)
	{
		if (typeof(IStressTestBehavior).IsAssignableFrom(type))
			throw new ArgumentException($"Тип не является производным от {nameof(IStressTestBehavior)}", nameof(type)); ;

		if (type.IsAbstract)
			throw new ArgumentException("Тип является абстрактным", nameof(type));

		this.type = type;

		var nameAttrib = type.GetCustomAttribute<PluginTitleAttribute>();

		if (nameAttrib is null)
		{
			Title = type.FullName;
		}
		else
		{
			Title = nameAttrib.Title;
		}

		var descriptionAttrib = type.GetCustomAttribute<PluginDescriptionAttribute>();

		if (descriptionAttrib is null)
		{
			Description = descriptionAttrib.Description;
		}
		else
		{
			Description = null;
		}

		var authorAttrib = type.GetCustomAttribute<PluginAuthorAttribute>();

		if (authorAttrib is null)
		{
			Author = null;
		}
		else
		{

			Author = authorAttrib.Author;
		}




	}

	public IStressTestBehavior Create()
	{
		var obj = Activator.CreateInstance(type);

		if (obj is IStressTestBehavior behavior)
		{
			return behavior;
		}
		throw new Exception();

	}

}
public interface IStressTestPluginService
{
	ISourceList<StressTestBehaviorInfo> LoadedPlugins { get; }

	bool AddPath();

	void RemovePath();

}

