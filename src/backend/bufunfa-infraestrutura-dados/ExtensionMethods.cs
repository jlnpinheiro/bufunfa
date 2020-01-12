using System.Linq;
using System.Reflection;

namespace JNogueira.Bufunfa.Infraestrutura.Dados
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Ordena uma coleção a partir do nome da propriedade do seu tipo
        /// </summary>
        public static IQueryable<T> OrderByProperty<T>(this IQueryable<T> entities, string propertyName, string sortDirection)
        {
            if (!entities.Any() || string.IsNullOrEmpty(propertyName))
                return entities;

            var propertyInfo = typeof(T).GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
                throw new System.Exception($"Não é possível ordernar por \"{propertyName}\", pois a classe \"{typeof(T).Name}\" não possui uma propriedade com esse nome.");

            return string.Equals(sortDirection, "ASC", System.StringComparison.OrdinalIgnoreCase)
                ? entities.AsEnumerable().OrderBy(e => propertyInfo.GetValue(e, null)).AsQueryable()
                : entities.AsEnumerable().OrderByDescending(e => propertyInfo.GetValue(e, null)).AsQueryable();
        }
    }
}
