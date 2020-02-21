using Microsoft.AspNetCore.Mvc;
using System;

namespace JNogueira.Bufunfa.Web
{
    public static partial class ExtensionMethods
    {
        /// <summary>
        /// Generates a fully qualified URL to the specified content by using the specified content path.
        /// </summary>
        /// <param name="url">The URL helper.</param>
        /// <param name="contentPath">The content path.</param>
        /// <returns>The absolute URL.</returns>
        public static string AbsoluteContent(this IUrlHelper url, string contentPath)
        {
            var request = url.ActionContext.HttpContext.Request;
            return new Uri(new Uri(request.Scheme + "://" + request.Host.Value), url.Content(contentPath)).ToString();
        }
    }

}
