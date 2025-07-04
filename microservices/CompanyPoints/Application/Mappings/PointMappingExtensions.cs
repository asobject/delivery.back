using Domain.DTOs;
using Domain.Entities;

namespace Application.Mappings
{
    public static class PointMappingExtensions
    {
        /// <summary>
        /// Преобразует доменную сущность CompanyPoint в DTO объекта CompanyPointDTO.
        /// </summary>
        /// <param name="point">Исходная сущность точки компании.</param>
        /// <returns>DTO объекта с данными из доменной сущности.</returns>
        public static CompanyPointDTO ToDTO(this CompanyPoint point)
        {

            var dto = new CompanyPointDTO
            {
                Id = point.Id,
                Name = point.Name,
                PointStatus = point.PointStatus,
                Coordinates = point.Coordinates,
                PointType = point.PointType,
                Address = point.Address,
                Country = point.Locality?.Province?.Country?.Name,
                Province = point.Locality?.Province?.Name,
                Locality = point.Locality?.Name
            };
            return dto;
        }

        /// <summary>
        /// Преобразует коллекцию доменных сущностей CompanyPoint в коллекцию DTO объектов.
        /// </summary>
        /// <param name="points">Коллекция исходных сущностей точек компании.</param>
        /// <returns>Коллекция DTO объектов.</returns>
        public static IEnumerable<CompanyPointDTO> ToDTOs(this IEnumerable<CompanyPoint> points)
        {
            return [.. points.Select(item => item.ToDTO())];
        }
    }
}
