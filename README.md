# Orchestra

Orchestra is a composable shell built on top of [Catel](http://Catel.Codeplex.com).

Orchestra consists of a main shell including an SDK with services that allows developers to communicate with the shell. Developers can write their own modules which implement the actual functionality of their application.

These modules can communicate with the shell, but also with each other through the services that are offered out of the box.

## Benefits to users:

- Framework developed with best practices in mind which will allow you to deploy robust applications
- Allows you to focus on the business logic so your application will be completed a lot sooner
- Built on [Catel](http://Catel.Codeplex.com) and all that goes with it giving you a head start on your project
- Nuget packages released regularly:
    - [Orchestra.Library](http://nuget.org/packages/Orchestra.Library)
    - [Orchestra.Shell](http://nuget.org/packages/Orchestra.Shell)
- Visual Studio templates to get started quickly
- Open source (MS-PL license), so you are free to use it in commercial applications

## Plugin architecture

Orchestra uses a plugin architecture with some already well know open source libraries already implemented:

- [AvalonDock](http://avalondock.codeplex.com/) (docking library)
- [Catel](http://Catel.Codeplex.com) (MVVM framework plut a lot more)
- [Fluent Ribbon](http://fluent.codeplex.com/)
- [Prism](http://compositewpf.codeplex.com/)
- [Nancy](http://www.nancyfx.org) (Lightweight Web Framework for .NET)
- [OxyPlot](http://oxyplot.codeplex.com/) (Cross platform plotting library - Work in progress)

It is easy to add extra modules by following the examples.

## Roadmap

- Automatic plugin update infrastructure
- Text editor module
- Csv editor module

## Support

You can ask for support on our mailing list: https://groups.google.com/forum/#!forum/orchestradev

## Contribute

Everyone is encouraged to contribute, either by:

- Submitting pull requests
- Documentation
- Blogs and tutorials

## Other similar projects

- [Gemini](https://github.com/tgjones/gemini "Gemini")
- [Wide](https://github.com/chandramouleswaran/Wide/ "Wide")


