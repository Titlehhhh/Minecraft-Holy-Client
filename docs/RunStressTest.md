# Как запустить стресс-тест?

Перед тем как запустить стресс-тест вам нужно будет написать плагин для него:

1) Скачайте и установите .NET SDK 8 (или вы можете скачать Visual Studio предварительной версии)
2) Создайте проект типа Библиотека классов(`dotnet new classlib --framework "net8.0" -o MyTopBehavior`
3) Добавьте в него ссылку NuGet пакет HolyClient.SDK(`dotnet add package HolyClient.SDK --version 1.0.0-preview.221`)
4) Создайте класс и наследуйтесь от интерфейса HolyClient.Abstractions.StressTest.IStressTestBehavior например:
```
[PluginAuthor("Title")]
[PluginDescription("My TOP STRESS TEST")]
public class Class2 : IStressTestBehavior
{
  public async Task Activate(CompositeDisposable disposables, IEnumerable<IStressTestBot> bots, CancellationToken cancellationToken)
  {
   //TODO
  }
}
```
5) Соберите библиотеку (`dotnet build`)
6) Перейдите в раздел "Менеджер расширений" -> "Сборки" и добавьте сборку, которая должна быть в папке bin вашего проекта.
7) Перейдите во вкладку "Стресс тест" -> "Прокси" и импортирйте свои прокси. Формат "ip:port". Логин и пароль пока не поддерживаются.
8) Перейдите во вкладку "Поведения". Вы там увидите список всех классов из подключеных сборок, которые наследуются от IStressTestBehavior. Выберите нужный и нажмите кнопку "Установить" справа.
9) Запустите стресс-тест!
