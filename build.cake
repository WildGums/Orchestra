var projectName = "Orchestra";
var projectsToPackage = new [] { "Orchestra.Core", "Orchestra.Shell.MahApps", "Orchestra.Shell.Ribbon.Fluent", "Orchestra.Shell.Ribbon.Microsoft", "Orchestra.Shell.TaskRunner" };
var company = "WildGums";
var startYear = 2010;
var defaultRepositoryUrl = string.Format("https://github.com/{0}/{1}", company, projectName);

#l "./deployment/cake/variables.cake"
#l "./deployment/cake/tasks.cake"
