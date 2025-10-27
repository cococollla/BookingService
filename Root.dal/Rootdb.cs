using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;

namespace Root.dal;

/// <summary>
/// Контекст базы данных linq2db для root api.
/// </summary>
public class Rootdb : DataConnection
{
    /// <summary>
    /// Конструктор
    /// </summary>
    public Rootdb(DataOptions<Rootdb> options) : base(options.Options)
    {
    }

    /// <summary>
    /// Сформировать схему бд.
    /// </summary>
    /// <returns></returns>
    public static MappingSchema Schema()
    {
        var myFluentMappings = new MappingSchema();
        var builder = new FluentMappingBuilder(myFluentMappings);

        builder
            .Entity<DatabaseChangeLog>()
            .Mapping();

        builder.Build();

        return myFluentMappings;
    }

    /// <summary>
    /// Логи изменений
    /// </summary>
    public ITable<DatabaseChangeLog> DatabaseChangeLogs => this.GetTable<DatabaseChangeLog>();
}