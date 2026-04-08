namespace Orchestra.Tests;

using System.Linq;
using System.Windows.Input;
using Catel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using CommandManager = Catel.MVVM.CommandManager;
using InputGesture = Catel.Windows.Input.InputGesture;

[TestFixture]
public class CommandManagerExtensionsFacts
{
    [TestCase(Key.A, ModifierKeys.Control, true)]
    [TestCase(Key.A, ModifierKeys.Shift, false)]
    [TestCase(Key.A, ModifierKeys.None, false)]
    [TestCase(Key.B, ModifierKeys.Control, false)]
    public void The_FindCommandsByGesture_Method(Key key, ModifierKeys modifierKeys, bool expectedToBeAvailable)
    {
        var serviceCollection = ServiceCollectionHelper.CreateServiceCollection();

        using var serviceProvider = serviceCollection.BuildServiceProvider();

        var commandManager = new CommandManager(NullLogger<CommandManager>.Instance, serviceProvider);

        commandManager.CreateCommand("CtrlA", new InputGesture(Key.A, ModifierKeys.Control));

        var inputGesture = new InputGesture(key, modifierKeys);
        var existingCommands = commandManager.FindCommandsByGesture(inputGesture);

        Assert.That(existingCommands.Any(), Is.EqualTo(expectedToBeAvailable));
    }
}
