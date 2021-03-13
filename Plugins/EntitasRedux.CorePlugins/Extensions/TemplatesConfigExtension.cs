using System.IO;
using System.Linq;
using EntitasRedux.CorePlugins.Configs;

namespace EntitasRedux.CorePlugins.Extensions
{
	public static class TemplatesConfigExtension
	{
		internal static string FindTemplate(this TemplatesConfig config, string fileName)
		{
			foreach (var dir in config.Templates)
			{
				var template = Directory
					.GetFiles(dir, fileName, SearchOption.TopDirectoryOnly)
					.FirstOrDefault();

				if (template != null)
				{
					return template;
				}
			}

			return null;
		}
	}
}
