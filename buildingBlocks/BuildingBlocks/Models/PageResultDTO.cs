﻿namespace BuildingBlocks.Models;

public class PagedResultDTO<T>
{
    /// <summary>
    /// Данные текущей страницы.
    /// </summary>
    public IEnumerable<T> Data { get; set; } = [];

    /// <summary>
    /// Общее количество записей, удовлетворяющих условию.
    /// </summary>
    public int TotalRecords { get; set; }
}