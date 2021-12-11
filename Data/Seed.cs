using Microsoft.EntityFrameworkCore;
using server.Entities;
using server.Enums;

namespace server.Data;

public class Seed
{
    private static Random random = new();

    public static string RandomString(int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz";
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static bool RandomBool()
    {
        return random.NextDouble() >= 0.5;
    }

    public static DateTime? RandomDate()
    {
        DateTime start = new DateTime(2020, 1, 1);
        int range = (DateTime.Today - start).Days;           
        return RandomBool() ? start.AddDays(random.Next(range)) : null;
    }

    public static Job RandomJob()
    {
        var title = RandomString(random.Next(50));
        var description = RandomString(random.Next(250));
        return new Job(title, description, RandomDate());
    }
    public async static Task InitializeDatabase(IApplicationBuilder app)
    {
        const int samples = 1_000_000;

        using var scope = app.ApplicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();

        context.Database.EnsureDeleted();
        context.Database.Migrate();

        var jobs = (from i in Enumerable.Range(0, samples) select RandomJob()).ToList();
        var logs = (from i in Enumerable.Range(0, samples) select new Log(JobStatus.New, jobs[i].JobId)).ToList();

        jobs.ForEach(job => context.Jobs.Add(job));
        logs.ForEach(log => context.Logs.Add(log));

        await context.SaveChangesAsync();
    }
}