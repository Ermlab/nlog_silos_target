nlog_silos_target
=================

NLog target for silos logging service

Usage:
1. First we need to add reference to SilosWrapper project (or .dll file) to a project where we want to do some logging.
2. Run Package Manager Console in logging project and type (or download via NuGet):
	install-package NLog
	install-package NLog.Config (to download a sample config file for NLog)
3. In NLog.config set your configuration for logging (sample file included).
4. Use logger in your code:
	NLog.LogManager.GetCurrentClassLogger().Info("Sample log.");
	
