#!/usr/bin/env dotnet-script
#r "nuget: Npgsql, 8.0.0"

using Npgsql;
using System;

var connectionString = "Host=aws-1-sa-east-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.ncudmcyardxdxdyuzkmc;Password=gutaneguinha;SSL Mode=Require;Trust Server Certificate=true;Timeout=30";

Console.WriteLine("Testando conexão com Supabase...");
Console.WriteLine($"Connection String: {connectionString.Replace("gutaneguinha", "***")}");

try
{
    using var conn = new NpgsqlConnection(connectionString);
    conn.Open();
    Console.WriteLine("✅ CONEXÃO ESTABELECIDA COM SUCESSO!");
    
    using var cmd = new NpgsqlCommand("SELECT version()", conn);
    var version = cmd.ExecuteScalar();
    Console.WriteLine($"PostgreSQL Version: {version}");
    
    using var cmd2 = new NpgsqlCommand("SELECT COUNT(*) FROM \"Users\"", conn);
    var count = cmd2.ExecuteScalar();
    Console.WriteLine($"Usuários na tabela: {count}");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ ERRO: {ex.Message}");
    Console.WriteLine($"Tipo: {ex.GetType().Name}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner: {ex.InnerException.Message}");
    }
}
