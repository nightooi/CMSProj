using CMSProj.DataLayer.UrlServices;

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
namespace CMSProj.DataLayer.PageServices
{
    public interface IContentExcavationService<T>
    {
        public T RetrieveContent(ContentDatabase.Model.Page page);
        public Task<T> RetrieveContentAsync(ContentDatabase.Model.Page page, CancellationToken token);

    }
}
