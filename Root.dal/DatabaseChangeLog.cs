using LinqToDB.Mapping;

namespace Root.dal;

/// <summary>
/// Зафиксированные изменения в БД.
/// </summary>
public sealed record DatabaseChangeLog
{
    /// <summary>
    /// Идентификатор изменения
    /// </summary>
    public required string Id { get; init; }

    /// <summary>
    /// Автор
    /// </summary>
    public required string Author { get; init; }

    /// <summary>
    /// Файл скрипта
    /// </summary>
    public required string FileName { get; init; }

    /// <summary>
    /// Дада исполнения
    /// </summary>
    public required DateTime DateExecuted { get; init; }
}

/// <summary>
/// Маппинг модели <see cref="DatabaseChangeLog"/> для таблицы "databasechangelog"
/// </summary>
public static class DatabaseChangeLogConfig
{
    /// <summary>
    /// Маппинг
    /// </summary>
    public static void Mapping(this EntityMappingBuilder<DatabaseChangeLog> builder)
    {
        builder.HasTableName("databasechangelog")
               .HasSchemaName("public")
               .HasIdentity(x => x.Id)
               .HasPrimaryKey(x => x.Id)
               .Property(x => x.Id).HasColumnName("id")
               .Property(x => x.Author).HasColumnName("author")
               .Property(x => x.FileName).HasColumnName("filename")
               .Property(x => x.DateExecuted).HasColumnName("dateexecuted");
    }
}