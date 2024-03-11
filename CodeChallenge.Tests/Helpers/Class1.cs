//Console.WriteLine($"Environment {env.EnvironmentName}");
//if (env.IsDevelopment())
//{
//    Console.WriteLine($"Environment {env.EnvironmentName}");

//    services.AddSingleton<EmployeeContext>(_ =>
//    {
//        var options = new DbContextOptionsBuilder<EmployeeContext>()
//            .UseInMemoryDatabase("InMemoryEmployeeTestDb")
//            .Options;
//        var context = new EmployeeContext(options);

//        new EmployeeDataSeeder(context).Seed().Wait();

//        return context;
//    });
//}