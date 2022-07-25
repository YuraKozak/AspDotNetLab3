using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using AspDotNetLab3.Sources;

namespace AspDotNetLab3.Extensions
{
    public static class TextConfigurationExtensions
    {
        public static IConfigurationBuilder AddTextFile(
            this IConfigurationBuilder builder, string path)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("Шлях до файлу не вказаний");
            }
            var source = new TextConfigurationSource(path);
            builder.Add(source);
            return builder;
        }
    }
}
